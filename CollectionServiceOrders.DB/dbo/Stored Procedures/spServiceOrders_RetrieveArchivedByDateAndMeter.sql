-----------------------------------------------------------
-- Author:      Jim Lynch
-- Date:        10/20/2021
-- Description: Retrieves Archived SO
-----------------------------------------------------------

CREATE PROCEDURE [dbo].[spServiceOrders_RetrieveArchivedByDateAndMeter]
    @MeterNumber int,
    @ServiceOrderDate datetime2(7)
AS
IF @MeterNumber = 0
    BEGIN
        SELECT ID, FSR, ServiceOrderNumber, SignalNumber, RouteNumber, Cycle, Rate, MeterNumber, LocationNumber, AccountNumber, SubAccountNumber, ServiceNumber, AccountName, AccountAddress1, AccountAddress2, AccountAddress3, AccountZip, Address, AddressStreetNumber, CutoffCreatorUserID, ExistingDeposit, AdditionalDeposit, TotalDeposit, GuarantorAccountNumber, GuarantorSubAccountNumber, GuarantorAmount, AddedCharges, LetterPrinted, ServiceOrderDate, Completed, DateCompleted, Reading, SignalChange, PaymentReceived, KeyAccountString, OnlinePayment, BankDraft
        FROM ServiceOrders
        WHERE Archived = 1 AND ServiceOrderDate >= @ServiceOrderDate
        ORDER BY ServiceOrderNumber, AccountNumber, SubAccountNumber, ServiceNumber
    END
ELSE
    BEGIN
        SELECT ID, FSR, ServiceOrderNumber, SignalNumber, RouteNumber, Cycle, Rate, MeterNumber, LocationNumber, AccountNumber, SubAccountNumber, ServiceNumber, AccountName, AccountAddress1, AccountAddress2, AccountAddress3, AccountZip, Address, AddressStreetNumber, CutoffCreatorUserID, ExistingDeposit, AdditionalDeposit, TotalDeposit, GuarantorAccountNumber, GuarantorSubAccountNumber, GuarantorAmount, AddedCharges, LetterPrinted, ServiceOrderDate, Completed, DateCompleted, Reading, SignalChange, PaymentReceived, KeyAccountString, OnlinePayment, BankDraft
        FROM ServiceOrders
        WHERE Archived = 1 AND ServiceOrderDate >= @ServiceOrderDate AND MeterNumber = @MeterNumber
        ORDER BY ServiceOrderNumber, AccountNumber, SubAccountNumber, ServiceNumber
    END