﻿CREATE FUNCTION [dbo].[fi_Agents] ( -- SELECT * FROM dbo.fi_Agents(N'supploer', 0))
	@AgentRelationType NVARCHAR (255) = NULL,
	@IsActive BIT = NULL
) RETURNS TABLE
AS
RETURN
	SELECT * FROM [dbo].[Agents]
	WHERE [Id] IN (
		SELECT [AgentId]
		FROM dbo.[AgentRelations]
		WHERE (@AgentRelationType IS NULL OR [AgentRelationType] = @AgentRelationType)
		AND (@IsActive IS NULL OR [IsActive] = @IsActive)
	);