-----------------------------------------------------------
-- Author:      Jim Lynch
-- Date:        10/20/2021
-- Description: Updates user settings by user id.
-----------------------------------------------------------

CREATE PROCEDURE [dbo].[spUserSettings_UpdateByUserID]
    @UserID varchar(50),
	@Role varchar(50),
    @FirstName varchar(50),
    @LastName varchar(50),
    @Filter varchar(50),
    @FontSize int,
    @ShowColumnSONumber bit,
    @ShowColumnSignalNumber bit,
    @ShowColumnFSR bit,
	@ShowColumnMeterNumber bit,
	@ShowColumnReading bit,
	@ShowColumnRate bit,
    @ShowColumnRouteNumber bit,
	@ShowColumnLocationNumber bit,
	@ShowColumnAddress bit,
	@ShowColumnAccountNumber bit,
	@ShowColumnExistingDeposit bit,
    @ShowColumnAdditionalDeposit bit,
	@ShowColumnAddedCharges bit,
	@ShowColumnSignalChange bit,
    @ShowColumnLetterPrinted bit,
	@SortColumn varchar(50),
    @SortAscending bit,
    @RefreshInterval int,
    @EmailAddress varchar(100),
    @ColorCoding bit,
	@ThemeSyncMode varchar(50),
	@AppTheme varchar(50),
	@AccentColor varchar(50),
	@KeyAccountColor varchar(50),
    @CompletedColor varchar(50),
    @GoodSignalColor varchar(50),
    @PaymentReceivedColor varchar(50),
    @OnlinePaymentColor varchar(50),
    @BankDraftColor varchar(50)
AS

BEGIN
	UPDATE
		[dbo].[UserSettings]
	SET
		Role = @Role,
		FirstName = @FirstName,
		LastName = @LastName,
		Filter = @Filter,
		FontSize = @FontSize,
		ShowColumnSONumber = @ShowColumnSONumber,
		ShowColumnSignalNumber = @ShowColumnSignalNumber,
		ShowColumnFSR = @ShowColumnFSR,
		ShowColumnMeterNumber = @ShowColumnMeterNumber,
		ShowColumnReading = @ShowColumnReading,
		ShowColumnRate = @ShowColumnRate,
		ShowColumnRouteNumber = @ShowColumnRouteNumber,
		ShowColumnLocationNumber = @ShowColumnLocationNumber,
		ShowColumnAddress = @ShowColumnAddress,
		ShowColumnAccountNumber = @ShowColumnAccountNumber,
		ShowColumnExistingDeposit = @ShowColumnExistingDeposit,
		ShowColumnAdditionalDeposit = @ShowColumnAdditionalDeposit,
		ShowColumnAddedCharges = @ShowColumnAddedCharges,
		ShowColumnSignalChange = @ShowColumnSignalChange,
		ShowColumnLetterPrinted = @ShowColumnLetterPrinted,
		SortColumn = @SortColumn,
		SortAscending = @SortAscending,
		RefreshInterval = @RefreshInterval,
		EmailAddress = @EmailAddress,
		ColorCoding = @ColorCoding,
		ThemeSyncMode = @ThemeSyncMode,
		AppTheme = @AppTheme,
		AccentColor = @AccentColor,
		KeyAccountColor = @KeyAccountColor,
		CompletedColor = @CompletedColor,
		GoodSignalColor = @GoodSignalColor,
		PaymentReceivedColor = @PaymentReceivedColor,
		OnlinePaymentColor = @OnlinePaymentColor,
		BankDraftColor = @BankDraftColor
	WHERE
		UserID = @UserID;
END