-----------------------------------------------------------
-- Author:      Jim Lynch
-- Date:        10/20/2021
-- Description: Adds a service order record to SqlDB
-----------------------------------------------------------

CREATE PROCEDURE  [dbo].[spServiceOrders_CreateNew]
	@FSR varchar(50), 
	@ServiceOrderNumber int, 
	@SignalNumber varchar(50), 
	@RouteNumber int ,
    @Cycle int,
	@Rate varchar(50), 
	@MeterNumber int,
	@LocationNumber varchar(50),
	@AccountNumber int,
    @SubAccountNumber int,
	@ServiceNumber int,
    @AccountName varchar(100),
    @AccountAddress1 varchar(100),
    @AccountAddress2 varchar(100),
    @AccountAddress3 varchar(100),
    @AccountZip varchar(20),
	@Address varchar(100),
    @AddressStreetNumber int,
    @CutoffCreatorUserID varchar(50),
	@ExistingDeposit money,
	@AdditionalDeposit money,
    @TotalDeposit money,
    @GuarantorAccountNumber int,
    @GuarantorSubAccountNumber int,
    @GuarantorAmount money,
	@AddedCharges money,
	@LetterPrinted bit,
    @ServiceOrderDate datetime2(7),
	@Completed bit,
	@DateCompleted datetime2(7),
    @Reading int,
    @SignalChange varchar(255),
	@PaymentReceived bit,
    @KeyAccountString varchar(10),
    @OnlinePayment bit,
    @BankDraft bit
    
AS

   BEGIN
       INSERT INTO ServiceOrders (FSR, ServiceOrderNumber, SignalNumber, RouteNumber, Cycle, Rate, MeterNumber, LocationNumber, AccountNumber, SubAccountNumber, ServiceNumber, AccountName, AccountAddress1, AccountAddress2, AccountAddress3, AccountZip, Address, AddressStreetNumber, CutoffCreatorUserID, ExistingDeposit, AdditionalDeposit, TotalDeposit, GuarantorAccountNumber, GuarantorSubAccountNumber, GuarantorAmount, AddedCharges, LetterPrinted, ServiceOrderDate, Completed, DateCompleted, Reading, SignalChange, PaymentReceived, KeyAccountString, OnlinePayment, BankDraft)
       VALUES  (@FSR, @ServiceOrderNumber, @SignalNumber, @RouteNumber, @Cycle, @Rate, @MeterNumber, @LocationNumber, @AccountNumber, @SubAccountNumber, @ServiceNumber, @AccountName, @AccountAddress1, @AccountAddress2, @AccountAddress3, @AccountZip, @Address, @AddressStreetNumber, @CutoffCreatorUserID, @ExistingDeposit, @AdditionalDeposit, @TotalDeposit, @GuarantorAccountNumber, @GuarantorSubAccountNumber, @GuarantorAmount, @AddedCharges, @LetterPrinted, @ServiceOrderDate, @Completed, @DateCompleted, @Reading, @SignalChange, @PaymentReceived, @KeyAccountString, @OnlinePayment, @BankDraft)
   END