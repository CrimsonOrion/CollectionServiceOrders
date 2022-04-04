CREATE PROCEDURE [dbo].[spUserSettings_RetrieveAll]
AS
BEGIN
	SELECT 
		UserID, Role, FirstName, LastName, Filter,
		FontSize, ShowColumnSONumber, ShowColumnSignalNumber, ShowColumnFSR, ShowColumnMeterNumber, 
		ShowColumnReading, ShowColumnRate, ShowColumnRouteNumber, ShowColumnLocationNumber, ShowColumnAddress,
		ShowColumnAccountNumber, ShowColumnExistingDeposit, ShowColumnAdditionalDeposit, ShowColumnAddedCharges, 
		ShowColumnSignalChange, ShowColumnLetterPrinted, SortColumn, SortAscending,
		RefreshInterval, EmailAddress, ColorCoding, ThemeSyncMode, AppTheme, AccentColor,
		KeyAccountColor, CompletedColor, GoodSignalColor, PaymentReceivedColor,
		OnlinePaymentColor, BankDraftColor
	FROM UserSettings
END