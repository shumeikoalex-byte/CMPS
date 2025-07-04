﻿CREATE TYPE [dbo].[DocumentLineTypeList] AS TABLE (
	[DocumentIndex]			INT,
	[LineType]				NVARCHAR (255),
	[SortKey]				TINYINT DEFAULT(1),
	[BaseLineId]			INT,
	[ScalingFactor]			FLOAT,
	[Memo]					NVARCHAR (255),

	[OperationId1]			INT,
	[AccountId1]			NVARCHAR (255),
	[AgentId1]				INT,
	[ResourceId1]			INT,
	[Direction1]			SMALLINT,
	[Amount1]				MONEY,
	[Value1]				VTYPE,
	[NoteId1]				NVARCHAR (255),
	[Reference1]			NVARCHAR (255),
	[RelatedReference1]		NVARCHAR (255),
	[RelatedAgentId1]		INT,
	[RelatedResourceId1]	INT,
	[RelatedAmount1]		MONEY,

	[OperationId2]			INT,
	[AccountId2]			NVARCHAR (255),
	[AgentId2]				INT,
	[ResourceId2]			INT,
	[Direction2]			SMALLINT,
	[Amount2]				MONEY,
	[Value2]				VTYPE,
	[NoteId2]				NVARCHAR (255),
	[Reference2]			NVARCHAR (255),
	[RelatedReference2]		NVARCHAR (255),
	[RelatedAgentId2]		INT,
	[RelatedResourceId2]	INT,
	[RelatedAmount2]		MONEY,

	[OperationId3]			INT,
	[AccountId3]			NVARCHAR (255),
	[AgentId3]				INT,
	[ResourceId3]			INT,
	[Direction3]			SMALLINT,
	[Amount3]				MONEY,
	[Value3]				VTYPE,
	[NoteId3]				NVARCHAR (255),
	[Reference3]			NVARCHAR (255),
	[RelatedReference3]		NVARCHAR (255),
	[RelatedAgentId3]		INT,
	[RelatedResourceId3]	INT,
	[RelatedAmount3]		MONEY,

	[OperationId4]			INT,
	[AccountId4]			NVARCHAR (255),
	[AgentId4]				INT,
	[ResourceId4]			INT,
	[Direction4]			SMALLINT,
	[Amount4]				MONEY,
	[Value4]				VTYPE,
	[NoteId4]				NVARCHAR (255),
	[Reference4]			NVARCHAR (255),
	[RelatedReference4]		NVARCHAR (255),
	[RelatedAgentId4]		INT,
	[RelatedResourceId4]	INT,
	[RelatedAmount4]		MONEY,

	[EntityState]		NVARCHAR (255)		NOT NULL DEFAULT(N'Inserted'),
	PRIMARY KEY ([DocumentIndex], [LineType]),
	CHECK ([EntityState] IN (N'Unchanged', N'Inserted', N'Updated', N'Deleted'))
);
