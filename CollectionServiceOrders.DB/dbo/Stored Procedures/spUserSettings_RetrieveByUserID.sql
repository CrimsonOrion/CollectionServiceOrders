-----------------------------------------------------------
-- Author:      Jim Lynch
-- Date:        10/20/2021
-- Description: Retrieves user profile by user id.
--              If it doesn't exist, create a new one.
-----------------------------------------------------------

CREATE PROCEDURE [dbo].[spUserSettings_RetrieveByUserID] 
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
 IF EXISTS (SELECT UserID FROM UserSettings WHERE UserID = @UserID)
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
       WHERE UserID = @UserID
   END
 ELSE
   BEGIN
		INSERT INTO UserSettings (
			UserID, Role, FirstName, LastName, Filter,
			FontSize, ShowColumnSONumber, ShowColumnSignalNumber, ShowColumnFSR, ShowColumnMeterNumber, 
			ShowColumnReading, ShowColumnRate, ShowColumnRouteNumber, ShowColumnLocationNumber, ShowColumnAddress, 
			ShowColumnAccountNumber, ShowColumnExistingDeposit, ShowColumnAdditionalDeposit, ShowColumnAddedCharges, 
			ShowColumnSignalChange, ShowColumnLetterPrinted, SortColumn, SortAscending,
			RefreshInterval, EmailAddress, ColorCoding, ThemeSyncMode, AppTheme, AccentColor,
            KeyAccountColor, CompletedColor, GoodSignalColor, PaymentReceivedColor,
            OnlinePaymentColor, BankDraftColor)
         VALUES (
            @UserID, @Role, @FirstName, @LastName, @Filter,
			@FontSize, @ShowColumnSONumber, @ShowColumnSignalNumber, @ShowColumnFSR, @ShowColumnMeterNumber, 
			@ShowColumnReading, @ShowColumnRate, @ShowColumnRouteNumber, @ShowColumnLocationNumber, @ShowColumnAddress, 
			@ShowColumnAccountNumber, @ShowColumnExistingDeposit, @ShowColumnAdditionalDeposit, @ShowColumnAddedCharges, 
			@ShowColumnSignalChange, @ShowColumnLetterPrinted, @SortColumn, @SortAscending,
			@RefreshInterval, @EmailAddress, @ColorCoding, @ThemeSyncMode, @AppTheme, @AccentColor,
            @KeyAccountColor, @CompletedColor, @GoodSignalColor, @PaymentReceivedColor, 
			@OnlinePaymentColor, @BankDraftColor)
	END
END