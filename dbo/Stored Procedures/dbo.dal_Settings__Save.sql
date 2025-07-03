﻿CREATE PROCEDURE [dbo].[dal_Settings__Save]
	@Settings [SettingList] READONLY
AS
SET NOCOUNT ON;
	DECLARE @Now DATETIMEOFFSET(7) = SYSDATETIMEOFFSET();
	DECLARE @UserId INT = CONVERT(INT, SESSION_CONTEXT(N'UserId'));

-- Inserts and Updates
	MERGE INTO [dbo].Settings AS t
	USING (
		SELECT [FunctionalCurrencyId], [ArchiveDate], [TenantLanguage2], [TenantLanguage3]
		FROM @Settings 
		WHERE [EntityState] IN (N'Inserted', N'Updated')
	) AS s ON (1=1)
	WHEN MATCHED 
	THEN
		UPDATE SET 
			t.[FunctionalCurrencyId]	= s.[FunctionalCurrencyId],
			t.[ArchiveDate]				= s.[ArchiveDate],
			t.[TenantLanguage2]			= s.[TenantLanguage2],
			t.[TenantLanguage3]			= s.[TenantLanguage3],
			t.ModifiedAt				= @Now,
			t.ModifiedById				= @UserId
	WHEN NOT MATCHED THEN
		INSERT ([FunctionalCurrencyId], [ArchiveDate], [TenantLanguage2], [TenantLanguage3])
		VALUES (s.[FunctionalCurrencyId], s.[ArchiveDate], s.[TenantLanguage2], s.[TenantLanguage3]);	