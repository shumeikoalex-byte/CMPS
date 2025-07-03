﻿CREATE PROCEDURE [dbo].[bll_Documents_Validate__Save]
	@Documents [dbo].[DocumentList] READONLY,
	@Entries [dbo].DocumentLineEntryList READONLY,
	@ValidationErrorsJson NVARCHAR(MAX) OUTPUT
AS
SET NOCOUNT ON;
	DECLARE @ValidationErrors [dbo].[ValidationErrorList];
	DECLARE @Now DATETIMEOFFSET(7) = SYSDATETIMEOFFSET();

	-- (SL Check)  Cannot save with a future date, (Settings dependent)
	INSERT INTO @ValidationErrors([Key], [ErrorName], [Argument0])
	SELECT
		'[' + CAST([Index] AS NVARCHAR (255)) + '].DocumentDate',
		N'Error_TheTransactionDate0IsInTheFuture',
		[DocumentDate]
	FROM @Documents
	WHERE ([DocumentDate] > DATEADD(DAY, 1, @Now)) -- More accurately, FE.[DocumentDate] > CONVERT(DATE, SWITCHOFFSET(@Now, user_time_zone)) 

	-- (FE Check) If Resource = functional currency, the value must match the money amount
	INSERT INTO @ValidationErrors([Key], [ErrorName], [Argument0], [Argument1])
	SELECT
		'[' + CAST([DocumentIndex] AS NVARCHAR (255)) + '].TransactionWideLines[' +
			CAST([DocumentLineIndex] AS NVARCHAR (255)) + '].Amount' + CAST([EntryNumber] AS NVARCHAR(255)),
		N'Error_TheAmount0DoesNotMatchTheValue1',
		[MoneyAmount],
		[Value]
	FROM @Entries
	WHERE ([ResourceId] = dbo.fn_FunctionalCurrency())
	AND ([Value] <> [MoneyAmount] )

	-- (FE Check, DB constraint)  Cannot save with a date that lies in the archived period
	INSERT INTO @ValidationErrors([Key], [ErrorName], [Argument0])
	SELECT
		'[' + CAST([Index] AS NVARCHAR (255)) + '].DocumentDate',
		N'Error_TheTransactionDateIsBeforeArchiveDate0',
		(SELECT TOP 1 ArchiveDate FROM dbo.Settings)
	FROM @Documents
	WHERE [DocumentDate] < (SELECT TOP 1 ArchiveDate FROM dbo.Settings) 
	
	-- (FE Check, DB IU trigger) Cannot save a document not in draft state
	INSERT INTO @ValidationErrors([Key], [ErrorName])
	SELECT
		'[' + CAST(FE.[Index] AS NVARCHAR (255)) + '].DocumentState',
		N'Error_CannotOnlySaveADocumentInDraftState'
	FROM @Documents FE
	JOIN [dbo].[Documents] BE ON FE.[Id] = BE.[Id]
	WHERE (BE.[State] <> N'Draft')
	
	-- Note Id is missing when required
	-- TODO: Add the condition that Ifrs Note is enforced
	INSERT INTO @ValidationErrors([Key], [ErrorName])
	SELECT
		'[' + CAST([DocumentIndex] AS NVARCHAR (255)) + '].TransactionWideLines[' +
			CAST([DocumentLineIndex] AS NVARCHAR (255)) + '].IfrsNoteId' + CAST([EntryNumber] AS NVARCHAR(255)),
		N'Error_TheIfrsNoteIsRequired'
	FROM @Entries E
	JOIN dbo.Accounts A ON E.AccountId = A.Id
	WHERE (E.[IfrsNoteId] IS NULL)
	AND A.[IfrsAccountId] IN (
		SELECT [IfrsAccountId] FROM dbo.[IfrsAccountsIfrsNotes]
	);

	-- Invalid Note Id
	INSERT INTO @ValidationErrors([Key], [ErrorName], [Argument0])
	SELECT
		'[' + CAST(E.[DocumentIndex] AS NVARCHAR (255)) + '].TransactionWideLines[' +
			CAST(E.[DocumentLineIndex] AS NVARCHAR (255)) + '].IfrsNoteId' + CAST(E.[EntryNumber] AS NVARCHAR(255)),
		N'Error_TheIfrsNoteIsIncompatibleWithAccountClassification0',
		A.[IfrsAccountId]
	FROM @Entries E
	JOIN dbo.Accounts A ON E.AccountId = A.Id
	LEFT JOIN dbo.[IfrsAccountsIfrsNotes] AN ON A.[IfrsAccountId] = AN.[IfrsAccountId] AND E.Direction = AN.Direction AND E.IfrsNoteId = AN.[IfrsNoteId]
	WHERE (E.[IfrsNoteId] IS NOT NULL)
	AND (AN.[IfrsNoteId] IS NULL);

	-- No expired Ifrs Account
	-- No expired Ifrs Note
	INSERT INTO @ValidationErrors([Key], [ErrorName], [Argument0])
	SELECT
		'[' + CAST(E.[DocumentIndex] AS NVARCHAR (255)) + '].TransactionWideLines[' +
			CAST(E.[DocumentLineIndex] AS NVARCHAR (255)) + '].IfrsNoteId' + CAST(E.[EntryNumber] AS NVARCHAR(255)),
		N'Error_TheIfrsNoteId0HasExpired',
		IC.[Label]
	FROM @Entries E
	JOIN @Documents T ON E.[DocumentIndex] = T.[Index]
	JOIN dbo.[IfrsNotes] N ON E.[IfrsNoteId] = N.Id
	JOIN dbo.[IfrsConcepts] IC ON N.Id = IC.Id
	WHERE (IC.ExpiryDate < T.[DocumentDate]);
	
	-- External Reference is required for selected account and direction, 
	INSERT INTO @ValidationErrors([Key], [ErrorName])
	SELECT
		'[' + CAST(E.[DocumentIndex] AS NVARCHAR (255)) + '].TransactionWideLines[' +
			CAST(E.[DocumentLineIndex] AS NVARCHAR (255)) + '].ExternalReference' + CAST(E.[EntryNumber] AS NVARCHAR(255)),
		N'Error_TheReferenceIsNotSpecified'
	FROM @Entries E
	JOIN dbo.[Accounts] A On E.AccountId = A.Id
	JOIN dbo.[IfrsAccounts] IA ON A.IfrsAccountId = IA.Id
	WHERE (E.ExternalReference IS NULL)
	AND (E.[Direction] = 1 AND IA.[DebitExternalReferenceSetting] = N'Required' OR
		E.[Direction] = -1 AND IA.[CreditExternalReferenceSetting] = N'Required')
	AND (E.[EntityState] IN (N'Inserted', N'Updated'));

	-- Additional Reference is required for selected account and direction, 
	INSERT INTO @ValidationErrors([Key], [ErrorName])
	SELECT
		'[' + CAST(E.[DocumentIndex] AS NVARCHAR (255)) + '].TransactionWideLines[' +
			CAST(E.[DocumentLineIndex] AS NVARCHAR (255)) + '].RelatedReference' + CAST(E.[EntryNumber] AS NVARCHAR(255)),
		N'Error_TheRelatedReferenceIsNotSpecified'
	FROM @Entries E
	JOIN dbo.[Accounts] A On E.AccountId = A.Id
	JOIN dbo.[IfrsAccounts] IA ON A.IfrsAccountId = IA.Id
	WHERE (E.[AdditionalReference] IS NULL)
	AND (E.[Direction] = 1 AND IA.[DebitAdditionalReferenceSetting] = N'Required' OR
		E.[Direction] = -1 AND IA.[CreditAdditionalReferenceSetting] = N'Required')
	AND (E.[EntityState] IN (N'Inserted', N'Updated'));

	-- RelatedAgent is required for selected account and direction, 
	INSERT INTO @ValidationErrors([Key], [ErrorName])
	SELECT
		'[' + CAST(E.[DocumentIndex] AS NVARCHAR (255)) + '].TransactionWideLines[' +
			CAST(E.[DocumentLineIndex] AS NVARCHAR (255)) + '].RelatedAgentId' + CAST(E.[EntryNumber] AS NVARCHAR(255)),
		N'Error_TheRelatedAgentIsNotSpecified'
	FROM @Entries E
	JOIN dbo.[Accounts] A On E.AccountId = A.Id
	JOIN dbo.[IfrsAccounts] IA ON A.IfrsAccountId = IA.Id
	WHERE (E.[RelatedAgentId] IS NULL)
	AND (IA.[RelatedAgentAccountSetting] = N'Required')
	AND (E.[EntityState] IN (N'Inserted', N'Updated'));
	
	-- RelatedResource is required for selected account and direction, 
	INSERT INTO @ValidationErrors([Key], [ErrorName])
	SELECT
		'[' + CAST(E.[DocumentIndex] AS NVARCHAR (255)) + '].TransactionWideLines[' +
			CAST(E.[DocumentLineIndex] AS NVARCHAR (255)) + '].RelatedResourceId' + CAST(E.[EntryNumber] AS NVARCHAR(255)),
		N'Error_TheRelatedResourceIsNotSpecified'
	FROM @Entries E
	JOIN dbo.[Accounts] A On E.AccountId = A.Id
	JOIN dbo.[IfrsAccounts] IA ON A.IfrsAccountId = IA.Id
	WHERE (E.[RelatedResourceId] IS NULL)
	AND (IA.[RelatedResourceSetting] = N'Required')
	AND (E.[EntityState] IN (N'Inserted', N'Updated'));

	SELECT @ValidationErrorsJson = (SELECT * FROM @ValidationErrors	FOR JSON PATH);