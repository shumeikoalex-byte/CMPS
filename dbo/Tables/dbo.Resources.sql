﻿CREATE TABLE [dbo].[Resources] (
	[Id]						INT PRIMARY KEY,
/*
	Money,
	Intangible [rights,..]
	Material/Good [RM, WIP, FG, TM]
	PPE (leases, investments ?)
	Biological
	Lease services
	Employee Job
	general services
*/
	[ResourceType]				NVARCHAR (255)		NOT NULL,
	[Name]						NVARCHAR (255)		NOT NULL,
	[Name2]						NVARCHAR (255),
	[Name3]						NVARCHAR (255),
	[IsActive]					BIT					NOT NULL DEFAULT 1,
-- IsFungible = 0 <=> ResourceInstance is REQUIRED in table TransactionEntries when Document in Completed state
-- HasInstance, IsIdentifiable, IsSpecifiable
	[IsFungible]				BIT					NOT NULL DEFAULT 0,
-- IsBatch = 1 <=> BatchNumber is REQUIRED in table TransactionEntries when Document in Completed state
-- HasBatch, IsTrackable, 
	[IsBatch]					BIT					NOT NULL DEFAULT 0,
	[ValueMeasure]				NVARCHAR (255)		NOT NULL, -- Currency, Mass, Volumne, Area, Length, Count, Time, 
	[UnitId]					INT	NOT NULL,
	[CurrencyId]				INT,	-- the unit If the resource has a financial meaure assigned to it.
	[UnitPrice]					DECIMAL,		-- if not null, it specifies the Cost per Unit
	[MassUnitId]				INT,-- the unit If the resource has a mass measure assigned to it.
	[UnitMass]					DECIMAL,		-- if not null, it specifies the conversion rate Mass/Count
	[VolumeUnitId]				INT,-- FK, Table Units
	[UnitVolume]				DECIMAL,		-- if not null, it specifies the conversion rate Volume/Count
	[AreaUnitId]				INT,-- FK, Table Units
	[UnitArea]					DECIMAL,		-- if not null, it specifies the conversion rate Area/Count
	[LengthUnitId]				INT,-- FK, Table Units
	[UnitLength]				DECIMAL,		-- if not null, it specifies the conversion rate Length/Count
	[TimeUnitId]				INT,-- FK, Table Units
	[UnitTime]					DECIMAL,		-- if not null, it specifies the conversion rate Time/Count
	[CountUnitId]				INT,-- FK, Table Units
	[Code]						NVARCHAR (255),
 -- functional currency, common stock, basic, allowance, overtime/types, 
	[SystemCode]				NVARCHAR (255),
	[Memo]						NVARCHAR (2048), -- description
	[CustomsReference]			NVARCHAR (255), -- how it is referred to by Customs
	[UniversalProductCode]		NVARCHAR (255), -- for barcode readers
	[PreferredSupplierId]		INT,-- FK, Table Agents, specially for purchasing
--	Useful for smart posting
	[ExpenseAccountId]			INT,
	[RevenueAccountId]			INT,
	-- The following properties are user-defined, used for reporting
	-- Examples for Steel finished goods are: Thickness and width. For cars: make and model.
	[ProductCategoryId]			INT,
	[ResourceLookup1Id]			INT,			-- UDL 
	[ResourceLookup2Id]			INT,			-- UDL 
	[ResourceLookup3Id]			INT,			-- UDL 
	[ResourceLookup4Id]			INT,			-- UDL 
	[CreatedAt]					DATETIMEOFFSET(7)	NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	[CreatedById]				INT	NOT NULL DEFAULT CONVERT(INT, SESSION_CONTEXT(N'UserId')),
	[ModifiedAt]				DATETIMEOFFSET(7)	NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	[ModifiedById]				INT	NOT NULL DEFAULT CONVERT(INT, SESSION_CONTEXT(N'UserId')),
	CONSTRAINT [FK_Resources__MassUnitId] FOREIGN KEY ([MassUnitId]) REFERENCES [dbo].[MeasurementUnits] ([Id]),
	CONSTRAINT [FK_Resources__VolumeUnitId] FOREIGN KEY ([VolumeUnitId]) REFERENCES [dbo].[MeasurementUnits] ([Id]),
	-- repeat for all lookups
	CONSTRAINT [FK_Resources__ProductCategoryId] FOREIGN KEY ([ProductCategoryId]) REFERENCES [dbo].[ProductCategories] ([Id]),
	CONSTRAINT [FK_Resources__ResourceLookup1Id] FOREIGN KEY ([ResourceLookup1Id]) REFERENCES [dbo].[ResourceLookup1s] ([Id]),
);
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Resources__Name]
  ON [dbo].[Resources]([Name]);
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Resources__Name2]
  ON [dbo].[Resources]([Name2]) WHERE [Name2] IS NOT NULL;
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Resources__Name3]
  ON [dbo].[Resources]([Name3]) WHERE [Name3] IS NOT NULL;
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Resources__Code]
  ON [dbo].[Resources]([Code]) WHERE [Code] IS NOT NULL;
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Resources__SystemCode]
  ON [dbo].[Resources]([SystemCode]) WHERE [SystemCode] IS NOT NULL;
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Resources__UnversalProductCode]
  ON [dbo].[Resources]([UniversalProductCode]) WHERE [UniversalProductCode] IS NOT NULL;
GO

ALTER TABLE [dbo].[Resources] ADD CONSTRAINT [CK_Resources__UnitId] CHECK (
	[UnitId] = (
		CASE
			WHEN [ValueMeasure] = N'Currency' THEN [CurrencyId]
			WHEN [ValueMeasure] = N'Mass' THEN [MassUnitId]
			WHEN [ValueMeasure] = N'Volume' THEN [VolumeUnitId]
			WHEN [ValueMeasure] = N'Area' THEN [AreaUnitId]
			WHEN [ValueMeasure] = N'Length' THEN [LengthUnitId]
			WHEN [ValueMeasure] = N'Count' THEN [CountUnitId]
		END
	)
);
GO