﻿BEGIN -- Cleanup & Declarations
	DECLARE @IfrsDisclosureDetailsDTO [IfrsDisclosureDetailList];
END
INSERT INTO @IfrsDisclosureDetailsDTO
([IfrsDisclosureId],[Value]) Values
-- Ifrs values
(N'NameOfReportingEntityOrOtherMeansOfIdentification', N'Banan IT, plc'),
(N'DomicileOfEntity', N'ET'),
(N'LegalFormOfEntity', N'PrivateLimitedCompany'),
(N'CountryOfIncorporation', N'ET'),
(N'AddressOfRegisteredOfficeOfEntity', N'Addis Abab, Bole Subcity, Woreda 6, House 316/3/203A'),
(N'PrincipalPlaceOfBusiness', N'Markan GH, Girgi, Addis Ababa'),
(N'DescriptionOfNatureOfEntitysOperationsAndPrincipalActivities', N'Software design, development and implementation'),
(N'NameOfParentEntity', N'BIOSS'),
(N'NameOfUltimateParentOfGroup', N'BIOSS');
-- Non Ifrs values
--(N'TaxIdentificationNumber', N'123456789'),
--(N'FunctionalCurrencyCode', N'ETB');

EXEC [dbo].[api_IfrsDisclosureDetails__Save]
	@Entities = @IfrsDisclosureDetailsDTO,
	@ValidationErrorsJson = @ValidationErrorsJson OUTPUT

IF @ValidationErrorsJson IS NOT NULL 
BEGIN
	Print 'Place: Inserting IfrsConcepts'
	GOTO Err_Label;
END
UPDATE @IfrsDisclosureDetailsDTO SET [IsDirty] = 0;
UPDATE @IfrsDisclosureDetailsDTO
SET ValidSince = N'2018.08.01', [IsDirty] = 1
WHERE [Index] IN (2, 3, 6);

UPDATE @IfrsDisclosureDetailsDTO
SET ValidSince = N'2018.09.15', [IsDirty] = 1
WHERE [Index] IN (5);

DELETE @IfrsDisclosureDetailsDTO WHERE [IsDirty] = 0

INSERT INTO @IfrsDisclosureDetailsDTO ([IfrsDisclosureId],[Value], [ValidSince]) Values
(N'AddressOfRegisteredOfficeOfEntity', N'Addis Abab, N/S/L, Woreda:01, House:New', N'2018.08.01');

EXEC [dbo].[api_IfrsDisclosureDetails__Save]
	@Entities = @IfrsDisclosureDetailsDTO,
	@ValidationErrorsJson = @ValidationErrorsJson OUTPUT

IF @DebugIfrsConcepts = 1
	EXEC rpt_Ifrs @fromDate = '2018.07.01', @toDate = '2019.06.30';

IF @ValidationErrorsJson IS NOT NULL 
BEGIN
	Print 'Place: Updating IfrsConcepts'
	GOTO Err_Label;
END

--IF @DebugSettings = 1
	SELECT
		IDD.IfrsDisclosureId,  IDD.[Value], IDD.ValidSince,
		LUC.[Name] AS CreatedBy, IDD.CreatedAt, LUM.[Name] AS ModifiedBy, IDD.ModifiedAt
	FROM [dbo].[IfrsDisclosureDetails] IDD
	JOIN dbo.LocalUsers LUC ON IDD.CreatedById = LUC.Id
	JOIN dbo.LocalUsers LUM ON IDD.ModifiedById = LUM.Id
	WHERE IsDeleted = 0;
	;