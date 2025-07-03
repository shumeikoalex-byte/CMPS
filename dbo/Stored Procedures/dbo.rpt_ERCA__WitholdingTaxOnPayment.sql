﻿CREATE PROCEDURE [dbo].[rpt_ERCA__WitholdingTaxOnPayment]
	@fromDate Date = '01.01.2000', 
	@toDate Date = '01.01.2100'
AS 
BEGIN
	SELECT
		A.TaxIdentificationNumber As [Withholdee TIN],
		A.[Name] As [Organization/Person Name],
		A.[RegisteredAddress] As [Withholdee Address], 
		J.[Memo] As [Withholding Type],
		J.[RelatedMoneyAmount] As [Taxable Amount], 
		J.[MoneyAmount] As [Tax Withheld], 
		J.[ExternalReference] As [Receipt Number], 
		J.DocumentDate As [Receipt Date],
		J.[DocumentLineId] -- for navigation
	FROM [dbo].[fi_JournalDetails](@fromDate, @toDate) J
	LEFT JOIN [dbo].[Agents] A ON J.[RelatedAccountId] = A.Id
	WHERE J.[IfrsAccountId] = N'CurrentWithholdingTaxPayable'
	AND J.Direction = -1;
END;