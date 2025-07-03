﻿CREATE TABLE [dbo].[AgentRelations] (
	[AgentId]					INT	NOT NULL,
	-- for every customer, supplier, and employee account types: sales, purchase and employment
	[AgentRelationType]			NVARCHAR (255)		NOT NULL,
	[AgentSubRelationType]		NVARCHAR (255),		-- Customer:G/S, Student, Distributor; Supplier:G/S; Employee: part-time, full-time;
	[IsActive]					BIT					NOT NULL DEFAULT 1,
	[Code]						NVARCHAR (255), -- location code for storage locations
	[StartDate]					DATETIME2 (7)		DEFAULT (CONVERT (date, SYSDATETIME())),
--	employee-accounts
	[JobTitle]					NVARCHAR (255), -- FK to table Jobs
	[BasicSalary]				MONEY,			-- As of now, typically part of direct labor expenses
	[TransporationAllowance]	MONEY,			-- As of now, typically part of overhead expenses.
	[OvertimeRate]				MONEY,			-- probably better moved to a template table
	[PerDiemRate]				MONEY,			-- probably better moved to a template table
--	supplier-accounts
	[SupplierRating]			INT,			-- user defined list
	[PaymentTerms]				NVARCHAR (255),
	-- extra details PO, LC, etc...

--	customer-accounts
	[CustomerRating]			INT,			-- user defined list
	[ShippingAddress]			NVARCHAR (255), -- default, the full list is in a separate table
	[BillingAddress]			NVARCHAR (255),

	[CreditLine]				MONEY				DEFAULT 0,

	[CreatedAt]					DATETIMEOFFSET(7)	NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	[CreatedById]				INT	NOT NULL DEFAULT CONVERT(INT, SESSION_CONTEXT(N'UserId')),
	[ModifiedAt]				DATETIMEOFFSET(7)	NOT NULL DEFAULT SYSDATETIMEOFFSET(), 
	[ModifiedById]				INT	NOT NULL DEFAULT CONVERT(INT, SESSION_CONTEXT(N'UserId')),
	CONSTRAINT [PK_AgentRelations] PRIMARY KEY NONCLUSTERED ([AgentId], [AgentRelationType]),
	CONSTRAINT [FK_AgentRelations__CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[LocalUsers] ([Id]),
	CONSTRAINT [FK_AgentRelations__ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[LocalUsers] ([Id]),
	CONSTRAINT [CK_AgentRelations__AgentRelationType] CHECK ([AgentRelationType] IN (
			N'investor', N'investment' ,
			N'cash', N'bank', N'customer', N'supplier',
			N'employee', N'debtor', N'creditor', N'custodian', -- custodian for non cash resources
			N'employer'
	)),
/*
	Agent Relation type		UDL (can only have ONE default account per (agent, relation type)
		N'investor'			-- Default
		N'investment'		-- Default, per investment contract
		N'cash'				-- Default, per cash bag
		N'bank'				-- Default, per bank account
		N'customer'			-- Default, per Sales order, per lease out, ...
		N'supplier'			-- Default, per Purchase Order, per LC, per lease in
		N'employee'			-- Default, per Contract
		N'debtor'			-- Default, per Loan
		N'creditor'			-- Default, per borrowing
		N'custodian'		-- Default, per storage location. Use the code to define the location structure
							-- Includes warehouse/aisles/shelves/bins, factory/line/unit, farm/zone/..
		N'employer'			-- Default, per contract
							-- used for companies providing outsourcing services
		)),
	*/
);
