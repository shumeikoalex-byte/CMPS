﻿CREATE PROCEDURE [dbo].[rpt_ERCA__EmploymentIncomeTax]
	@fromDate Date = '01.01.2000', 
	@toDate Date = '01.01.2100'
AS
BEGIN
	SELECT
		A.[TaxIdentificationNumber] As [Employee TIN],
		A.[Name] As [Employee Full Name],
		J.[RelatedMoneyAmount] As [Taxable Income], 
		J.[MoneyAmount] As [Tax Withheld]
	FROM [dbo].[fi_JournalDetails](@fromDate, @toDate) J
	LEFT JOIN [dbo].[Agents] A ON J.[RelatedAccountId] = A.Id
	WHERE J.[IfrsAccountId] = N'CurrentEmployeeIncomeTaxPayable'
	AND J.Direction = -1;
END;
GO;