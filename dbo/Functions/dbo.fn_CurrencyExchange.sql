﻿CREATE FUNCTION [dbo].[fn_CurrencyExchange] (
	@Date DATE = NULL,
	@BaseCurrency char(3),
	@TargetCurrency char(3) = NULL,
	@Amount money
)
RETURNS money
AS
BEGIN
	DECLARE @Result money;

	SELECT @Date = ISNULL(@Date, GETDATE()), @TargetCurrency = ISNULL(@TargetCurrency, [dbo].fn_FunctionalCurrency())

	SELECT @Result = @Amount * ExchangeRate FROM [dbo].ExchangeRatesHistory 
	WHERE [Date] = @Date AND BaseCurrency = @BaseCurrency AND TargetCurrency = @TargetCurrency

	RETURN @Result;
END;