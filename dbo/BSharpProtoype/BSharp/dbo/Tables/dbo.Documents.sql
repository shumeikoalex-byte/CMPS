﻿CREATE TABLE [dbo].[Documents] (
--	This table for all business documents that are routed for requisition, authorization, completion, and posting.
--	Its scope is 
	[Id]									INT PRIMARY KEY,
	-- Common to all document types
	[DocumentTypeId]						NVARCHAR (255)	NOT NULL,	
	[SerialNumber]							INT				NOT NULL,	-- auto generated, copied to paper if needed.
	[DocumentDate]							DATE			NOT NULL DEFAULT CONVERT (DATE, SYSDATETIME()),
	[State]									NVARCHAR (255)	NOT NULL DEFAULT N'Draft', -- N'Void', N'Requested', N'Rejected', N'Authorized', N'Failed', N'Completed', N'Declined', N'Posted'
	
	-- For a source socument, Evidence type == Authentication. Else source document, Attachment, trust
	[EvidenceTypeId]						NVARCHAR(255)		NOT NULL,
	-- When evidence type = source document
	[VoucherBookletId]						INT, -- each range might be dedicated for a special purpose
	[VoucherNumericReference]				INT, -- must fall between RangeStarts and RangeEnds of the booklet
	-- when evidence type = attachment or evidence type = source document (take snapshot of it)
	[BlobName]								NVARCHAR(255),		-- for attachments including videos, images, and audio messages
	-- Dynamic properties defined by document type specification
	[DocumentLookup1Id]						INT, -- e.g., cash machine serial in the case of a sale
	[DocumentLookup2Id]						INT,
	[DocumentLookup3Id]						INT,
	[DocumentText1]							NVARCHAR (255),
	[DocumentText2]							NVARCHAR (255),
	-- Additional properties to simplify data entry. No report should be based on them!!!
	[Memo]									NVARCHAR (255),
	[MemoIsCommon]							BIT				DEFAULT 1,
	[CustomerAccountId]						INT,
	[CustomerAccountIsCommon]				BIT				DEFAULT 1,
	[SupplierAccountId]						INT, 
	[SupplierAccountIsCommon]				BIT				DEFAULT 1,
	[EmployeeAccountId]						INT, 
	[EmployeeAccountIsCommon]				BIT				DEFAULT 1,
	[CurrencyId]							INT, 
	[CurrencyIsCommon]						BIT				DEFAULT 1,
	-- For non cash items, as cash is usually one line only.
	-- We are using "Stock", which includes "Farm/livestock" and any other non-cash custody.
	[SourceStockAccountId]					INT, 
	[SourceStockAccountIdIsCommon]			BIT				DEFAULT 1,
	[DestinationStockAccountId]				INT, 
	[DestinationStockAccountIdIsCommon]		BIT				DEFAULT 1,
	[InvoiceReference]						NVARCHAR (255),
	[InvoiceReferenceIsCommon]				BIT				DEFAULT 1,
	-- Transaction specific, to record the acquisition or loss of goods and services
	-- Orders that are not negotiables, are assumed to happen, and hence are journalized, even we are verifying it later.
	
	[Frequency]								NVARCHAR (255)	NOT NULL DEFAULT (N'OneTime'), -- an easy way to define a recurrent document
	[Repetitions]							INT				NOT NULL DEFAULT 0, -- time unit is function of frequency
	[EndDate] AS (
					CASE 
						WHEN [Frequency] = N'OneTime' THEN [DocumentDate]
						WHEN [Frequency] = N'Daily' THEN DATEADD(DAY, [Repetitions], [DocumentDate])
						WHEN [Frequency] = N'Weekly' THEN DATEADD(WEEK, [Repetitions], [DocumentDate])
						WHEN [Frequency] = N'Monthly' THEN DATEADD(MONTH, [Repetitions], [DocumentDate])
						WHEN [Frequency] = N'Quarterly' THEN DATEADD(QUARTER, [Repetitions], [DocumentDate])
						WHEN [Frequency] = N'Yearly' THEN DATEADD(YEAR, [Repetitions], [DocumentDate])
					END
	) PERSISTED,
	-- Request specific: purchase requisition, payment requesition, production request, maintenance request
	[NeededBy]								DATE, -- 
	-- Offer expiry date can be put on the generated template (expires in two weeks from above date)
	[CreatedAt]								DATETIMEOFFSET(7)	NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	[CreatedById]							INT	NOT NULL DEFAULT CONVERT(INT, SESSION_CONTEXT(N'UserId')),
	[ModifiedAt]							DATETIMEOFFSET(7)	NOT NULL DEFAULT SYSDATETIMEOFFSET(), 
	[ModifiedById]							INT	NOT NULL DEFAULT CONVERT(INT, SESSION_CONTEXT(N'UserId')),
	-- If the company is in Alofi, and the server is hosted in Apia, the server time will be one day behind
	-- So, the user will not be able to enter transactions unless DocumentDate is allowed 1d future 
	
	CONSTRAINT [CK_Documents__Duration] CHECK ([Frequency] IN (N'OneTime', N'Daily', N'Weekly', N'Monthly', N'Quarterly', N'Yearly')),
	CONSTRAINT [FK_Documents__DocumentTypeId] FOREIGN KEY ([DocumentTypeId]) REFERENCES [dbo].[DocumentTypes] ([Id]) ON UPDATE CASCADE, 
	CONSTRAINT [CK_Documents__DocumentDate] CHECK ([DocumentDate] < DATEADD(DAY, 1, GETDATE())) ,
	CONSTRAINT [CK_Documents__State] CHECK ([State] IN (N'Void', N'Requested', N'Rejected', N'Authorized', N'Failed', N'Completed', N'Invalid', N'Posted')),
	CONSTRAINT [CK_Documents__EvidenceTypeId] CHECK ([EvidenceTypeId] IN (N'Authentication', N'SourceDocument', N'Attachment', N'Trust')),
	CONSTRAINT [FK_Documents__CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[LocalUsers] ([Id]),
	CONSTRAINT [FK_Documents__ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[LocalUsers] ([Id])
 );
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Documents__VoucherBooklet_VoucherNumericReference]
  ON [dbo].[Documents]([VoucherBookletId], [VoucherNumericReference])
  WHERE [VoucherNumericReference] IS NOT NULL;
GO