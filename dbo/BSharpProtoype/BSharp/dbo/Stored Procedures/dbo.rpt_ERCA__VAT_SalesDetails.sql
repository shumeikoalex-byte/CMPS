﻿CREATE PROCEDURE [dbo].[rpt_ERCA__VAT_SalesDetails] -- used for online submission
	@fromDate Date = '01.01.2000', 
	@toDate Date = '01.01.2100'
AS
BEGIN
	SELECT
		J.Id, A.TaxIdentificationNumber AS TIN, J.AdditionalReference AS MRC,
		J.ExternalReference AS RCPT_NUM, J.DocumentDate As RCPT_Date,  J.Quantity,
		J.RelatedMoneyAmount As Price, N'' AS COM_CODE, ResourceType As COM_DETAIL,
		R.[Name] As [Description]
	FROM dbo.[fi_JournalDetails](@fromDate, @toDate) J
	LEFT JOIN dbo.Resources R ON J.RelatedResourceId = R.Id 
	LEFT JOIN dbo.Agents A ON J.[RelatedAccountId] = A.Id
	WHERE IfrsAccountId = N'CurrentValueAddedTaxPayables'
	AND J.Direction = -1
END
GO;