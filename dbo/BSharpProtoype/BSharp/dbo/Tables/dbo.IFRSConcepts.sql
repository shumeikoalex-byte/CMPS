﻿CREATE TABLE [dbo].[IfrsConcepts] (
	[Id]						NVARCHAR (255) PRIMARY KEY NONCLUSTERED, -- Ifrs Concept
/*
	- Regulatory: Proposed by regulatory entity. We may have different flavors for different countries
	- Amendment: Added for consistency, must be amended in the iXBRL tool. Mainly for members vs non members issue
	- Extension: Added for business logic in B#. Is ignored by the iXBRL tool 
*/
	[IfrsType]					NVARCHAR (255)	DEFAULT (N'Regulatory') NOT NULL, -- N'Amendment', N'Extension', N'Regulatory'
	[IsActive]					BIT					NOT NULL DEFAULT 1,
	[Label]						NVARCHAR (1024)		NOT NULL,
	[Label2]					NVARCHAR (1024),
	[Label3]					NVARCHAR (1024),
	[Documentation]				NVARCHAR (MAX),
	[Documentation2]			NVARCHAR (MAX),
	[Documentation3]			NVARCHAR (MAX),
	[EffectiveDate]				DATETIME2(7)		NOT NULL DEFAULT('0001-01-01 00:00:00'),
	[ExpiryDate]				DATETIME2(7)		NOT NULL DEFAULT('9999-12-31 23:59:59'),
--	
	[CreatedAt]					DATETIMEOFFSET(7)	NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	[CreatedById]				INT	NOT NULL DEFAULT CONVERT(INT, SESSION_CONTEXT(N'UserId')),
	[ModifiedAt]				DATETIMEOFFSET(7)	NOT NULL DEFAULT SYSDATETIMEOFFSET(), 
	[ModifiedById]				INT	NOT NULL DEFAULT CONVERT(INT, SESSION_CONTEXT(N'UserId')),

	CONSTRAINT [CK_IfrsConcepts__IfrsType] CHECK ([IfrsType] IN (N'Amendment', N'Extension', N'Regulatory')),
	CONSTRAINT [FK_IfrsConcepts__CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[LocalUsers] ([Id]),
	CONSTRAINT [FK_IfrsConcepts__ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[LocalUsers] ([Id])
	);