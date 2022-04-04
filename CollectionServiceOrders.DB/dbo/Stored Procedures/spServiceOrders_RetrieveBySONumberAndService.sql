-----------------------------------------------------------
-- Author:      Jim Lynch
-- Date:        10/20/2021
-- Description: Retrieves service orders based on 
--              service order number and service
-----------------------------------------------------------

CREATE PROCEDURE [dbo].[spServiceOrders_RetrieveBySONumberAndService] 
	@ServiceOrderNumber int,
    @ServiceNumber int
AS
BEGIN
	SELECT ID, FSR, ServiceOrderNumber, SignalNumber, RouteNumber, Cycle, Rate, MeterNumber, LocationNumber, AccountNumber, SubAccountNumber, ServiceNumber, AccountName, AccountAddress1, AccountAddress2, AccountAddress3, AccountZip, Address, AddressStreetNumber, CutoffCreatorUserID, ExistingDeposit, AdditionalDeposit, TotalDeposit, GuarantorAccountNumber, GuarantorSubAccountNumber, GuarantorAmount, LetterPrinted, ServiceOrderDate, Completed, DateCompleted, Archived, DateArchived, Reading, SignalChange, PaymentReceived, KeyAccountString, OnlinePayment, BankDraft
	FROM ServiceOrders
	WHERE ServiceOrderNumber = @ServiceOrderNumber AND ServiceNumber IN (@ServiceNumber)
END