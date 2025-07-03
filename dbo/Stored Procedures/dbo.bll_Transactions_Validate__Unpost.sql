﻿CREATE PROCEDURE [dbo].[bll_Transactions_Validate__Unpost]
	@Entities [dbo].[IndexedUuidList] READONLY,
	@ValidationErrorsJson NVARCHAR(MAX) OUTPUT
AS
SET NOCOUNT ON;
	DECLARE @ValidationErrors [dbo].[ValidationErrorList];

	DECLARE @ArchiveDate DATETIMEOFFSET(7) = (SELECT ArchiveDate FROM dbo.Settings);

	-- Cannot unpost if the period is closed	
	INSERT INTO @ValidationErrors([Key], [ErrorName], [Argument0], [Argument1])
	SELECT
		'[' + CAST(FE.[Index] AS NVARCHAR (255)) + '].[DocumentDate]',
		N'Error_TheDocumentDate0FallsBefore1ArchiveDate',
		BE.[DocumentDate],
		@ArchiveDate
	FROM @Entities FE
	JOIN [dbo].[Documents] BE ON FE.[Id] = BE.[Id]
	WHERE (BE.[DocumentDate] < @ArchiveDate)

	-- Cannot unpost if not the user who posted

	-- No inactive account
	INSERT INTO @ValidationErrors([Key], [ErrorName], [Argument0])
	SELECT
		'[' + CAST(FE.[Index] AS NVARCHAR (255)) + '].TransactionEntries[' +
			CAST(E.[Id] AS NVARCHAR (255)) + '].AccountId',
		N'Error_Account0IsInactive',
		A.[Name]
	FROM @Entities FE
	JOIN dbo.[DocumentLineEntries] E ON FE.[Id] = E.[DocumentLineId]
	JOIN dbo.[Accounts] A ON E.[AccountId] = A.[Id]
	WHERE (A.IsActive = 0);

	-- No inactive custody
	-- No inactive resource

	SELECT @ValidationErrorsJson = (SELECT * FROM @ValidationErrors	FOR JSON PATH);