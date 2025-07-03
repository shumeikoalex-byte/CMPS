﻿CREATE TYPE [dbo].[AgentList] AS TABLE (
	[Index]						INT				IDENTITY(0, 1),
	[Id]						INT NOT NULL DEFAULT 0,
	[IsActive]					BIT					NOT NULL DEFAULT 1, -- 0 means the person is dead or the organization is close
	[Name]						NVARCHAR (255)		NOT NULL,
	[Name2]						NVARCHAR (255),
	[Name3]						NVARCHAR (255),
	[Code]						NVARCHAR (255),
	[SystemCode]				NVARCHAR (255), -- some used are anoymous, self, parent
--	Common
	[AgentType]					NVARCHAR (255),  -- 'Individual', 'Machine', 'Organization' Organization includes Dept, Team
	[IsRelated]					BIT					NOT NULL DEFAULT 0,
	[TaxIdentificationNumber]	NVARCHAR (255),
	[IsLocal]					BIT,
	[Citizenship]				NCHAR(2),		-- ISO 3166-1 Alpha-2 code
	[Facebook]					NVARCHAR (255),				
	[Instagram]					NVARCHAR (255),				
	[Twitter]					NVARCHAR (255),
	[PreferredContactChannel1]	INT,			-- e.g., Mobile
	[PreferredContactAddress1]	NVARCHAR (255),  -- e.g., +251 94 123 4567
	[PreferredContactChannel2]	INT,			-- e.g., email
	[PreferredContactAddress2]	NVARCHAR (255),	-- e.g., info@contoso.com
--	Individuals only
--	--	Personal
	[BirthDateTime]				DATETIME2 (7),
	[TitleId]					INT,			-- LKT
	[Gender]					INT,			-- ISO/IEC 5218. 0=unknown, 1=Male, 2=Female, 9=N/A
	[ResidentialAddress]		NVARCHAR (1024), -- in the country language
	[ImageId]					INT,
--	--	Social
	[MaritalStatus]				INT,			-- LKT
	[NumberOfChildren]			TINYINT,
	[Religion]					NCHAR (1),		-- (?) I=Islam, C=Christianity, X=Others -- , J=Judaism, H=Hinduism, B=Buddhism
	[Race]						INT,			-- LKT
	[TribeId]					INT,			-- LKT
	[RegionId]					INT,			-- LKT
--	--	Academic
	[EducationLevelId]			INT,			-- LKT
	[EducationSublevelId]		INT,			-- ===
--	--	Financial
	[BankId]					INT,			-- LKT
	[BankAccountNumber]			NVARCHAR (255),					
--	Organizations only
--	Organization type is defined by the government entity responsible for this organization. For instance, banks
--	are all handled by the central bank. Charities are handled by a different body, and so on.
	[OrganizationType]			INT,			-- UDL General/Bank/Insurance/Charity/NGO/TaxOrg/Diplomatic
	[WebSite]					NVARCHAR (255),
	[ContactPerson]				NVARCHAR (255),
	[RegisteredAddress]			NVARCHAR (1024),
	[OwnershipType]				NVARCHAR (255), -- Investment/Shareholder/SisterCompany/Other(Default) -- We Own shares in them, they own share in us, ...
	[OwnershipPercent]			DECIMAL	DEFAULT 0, -- If investment, how much the entity owns in this agent. If shareholder, how much he owns in the entity

	[EntityState]				NVARCHAR (255)	NOT NULL DEFAULT(N'Inserted'),
	PRIMARY KEY ([Index]),
	INDEX IX_AgentList__Code ([Code]),
	CHECK ([EntityState] IN (N'Unchanged', N'Inserted', N'Updated', N'Deleted'))
);