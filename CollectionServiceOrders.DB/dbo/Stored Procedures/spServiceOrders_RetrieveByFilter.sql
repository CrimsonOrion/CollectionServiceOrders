-----------------------------------------------------------
-- Author:      Jim Lynch
-- Date:        10/20/2021
-- Description: Retrieves all service orders based on selected filter
-----------------------------------------------------------

CREATE PROCEDURE [dbo].[spServiceOrders_RetrieveByFilter]
	@Filter varchar(50)
AS

 IF @Filter = 'All'
             BEGIN
               SELECT ID, FSR, ServiceOrderNumber, SignalNumber, RouteNumber, Cycle, Rate, MeterNumber, LocationNumber, AccountNumber, SubAccountNumber, ServiceNumber, AccountName, AccountAddress1, AccountAddress2, AccountAddress3, AccountZip, Address, AddressStreetNumber, CutoffCreatorUserID, ExistingDeposit, AdditionalDeposit, TotalDeposit, GuarantorAccountNumber, GuarantorSubAccountNumber, GuarantorAmount, AddedCharges, LetterPrinted, ServiceOrderDate, Completed, DateCompleted, Reading, SignalChange, PaymentReceived, KeyAccountString, OnlinePayment, BankDraft
	           FROM ServiceOrders
			   WHERE (Archived = 0 OR Archived IS NULL)
			   ORDER BY ServiceOrderNumber, AccountNumber, SubAccountNumber, ServiceNumber
             END
 ELSE IF @Filter = 'Completed'
              BEGIN
               SELECT ID, FSR, ServiceOrderNumber, SignalNumber, RouteNumber, Cycle, Rate, MeterNumber, LocationNumber, AccountNumber, SubAccountNumber, ServiceNumber, AccountName, AccountAddress1, AccountAddress2, AccountAddress3, AccountZip, Address, AddressStreetNumber, CutoffCreatorUserID, ExistingDeposit, AdditionalDeposit, TotalDeposit, GuarantorAccountNumber, GuarantorSubAccountNumber, GuarantorAmount, AddedCharges, LetterPrinted, ServiceOrderDate, Completed, DateCompleted, Reading, SignalChange, PaymentReceived, KeyAccountString, OnlinePayment, BankDraft
                FROM ServiceOrders
                WHERE Completed = 1 AND (Archived = 0 OR Archived IS NULL)
				ORDER BY ServiceOrderNumber, AccountNumber, SubAccountNumber, ServiceNumber
              END
 ELSE IF @Filter = 'Not Completed'
              BEGIN     
               SELECT ID, FSR, ServiceOrderNumber, SignalNumber, RouteNumber, Cycle, Rate, MeterNumber, LocationNumber, AccountNumber, SubAccountNumber, ServiceNumber, AccountName, AccountAddress1, AccountAddress2, AccountAddress3, AccountZip, Address, AddressStreetNumber, CutoffCreatorUserID, ExistingDeposit, AdditionalDeposit, TotalDeposit, GuarantorAccountNumber, GuarantorSubAccountNumber, GuarantorAmount, AddedCharges, LetterPrinted, ServiceOrderDate, Completed, DateCompleted, Reading, SignalChange, PaymentReceived, KeyAccountString, OnlinePayment, BankDraft
	            FROM ServiceOrders
                WHERE Completed = 0 AND (Archived = 0 OR Archived IS NULL)
				ORDER BY ServiceOrderNumber, AccountNumber, SubAccountNumber, ServiceNumber
               END
 ELSE IF @Filter = 'Disconnect'
              BEGIN     
               SELECT ID, FSR, ServiceOrderNumber, SignalNumber, RouteNumber, Cycle, Rate, MeterNumber, LocationNumber, AccountNumber, SubAccountNumber, ServiceNumber, AccountName, AccountAddress1, AccountAddress2, AccountAddress3, AccountZip, Address, AddressStreetNumber, CutoffCreatorUserID, ExistingDeposit, AdditionalDeposit, TotalDeposit, GuarantorAccountNumber, GuarantorSubAccountNumber, GuarantorAmount, AddedCharges, LetterPrinted, ServiceOrderDate, Completed, DateCompleted, Reading, SignalChange, PaymentReceived, KeyAccountString, OnlinePayment, BankDraft
	            FROM ServiceOrders
                WHERE SignalNumber IN ('2', '2B') AND (Archived = 0 OR Archived IS NULL)
				ORDER BY ServiceOrderNumber, AccountNumber, SubAccountNumber, ServiceNumber
               END    
 ELSE IF @Filter = 'FieldCollection'
              BEGIN     
               SELECT ID, FSR, ServiceOrderNumber, SignalNumber, RouteNumber, Cycle, Rate, MeterNumber, LocationNumber, AccountNumber, SubAccountNumber, ServiceNumber, AccountName, AccountAddress1, AccountAddress2, AccountAddress3, AccountZip, Address, AddressStreetNumber, CutoffCreatorUserID, ExistingDeposit, AdditionalDeposit, TotalDeposit, GuarantorAccountNumber, GuarantorSubAccountNumber, GuarantorAmount, AddedCharges, LetterPrinted, ServiceOrderDate, Completed, DateCompleted, Reading, SignalChange, PaymentReceived, KeyAccountString, OnlinePayment, BankDraft
	            FROM ServiceOrders
                WHERE SignalNumber IN ('9', '9B') AND (Archived = 0 OR Archived IS NULL)
				ORDER BY ServiceOrderNumber, AccountNumber, SubAccountNumber, ServiceNumber
               END   
ELSE IF @Filter = 'AdditionalFees'
              BEGIN     
               SELECT ID, FSR, ServiceOrderNumber, SignalNumber, RouteNumber, Cycle, Rate, MeterNumber, LocationNumber, AccountNumber, SubAccountNumber, ServiceNumber, AccountName, AccountAddress1, AccountAddress2, AccountAddress3, AccountZip, Address, AddressStreetNumber, CutoffCreatorUserID, ExistingDeposit, AdditionalDeposit, TotalDeposit, GuarantorAccountNumber, GuarantorSubAccountNumber, GuarantorAmount, AddedCharges, LetterPrinted, ServiceOrderDate, Completed, DateCompleted, Reading, SignalChange, PaymentReceived, KeyAccountString, OnlinePayment, BankDraft
	            FROM ServiceOrders
                WHERE (AddedCharges > 0) AND (Archived = 0 OR Archived IS NULL)
				ORDER BY ServiceOrderNumber, AccountNumber, SubAccountNumber, ServiceNumber
               END    
ELSE IF @Filter = 'AdditionalDeposit'
              BEGIN     
               SELECT ID, FSR, ServiceOrderNumber, SignalNumber, RouteNumber, Cycle, Rate, MeterNumber, LocationNumber, AccountNumber, SubAccountNumber, ServiceNumber, AccountName, AccountAddress1, AccountAddress2, AccountAddress3, AccountZip, Address, AddressStreetNumber, CutoffCreatorUserID, ExistingDeposit, AdditionalDeposit, TotalDeposit, GuarantorAccountNumber, GuarantorSubAccountNumber, GuarantorAmount, AddedCharges, LetterPrinted, ServiceOrderDate, Completed, DateCompleted, Reading, SignalChange, PaymentReceived, KeyAccountString, OnlinePayment, BankDraft
	            FROM ServiceOrders
                WHERE SignalNumber IN ('4','OT4','4B','OT4B')
                    AND (AdditionalDeposit > 0) AND (Archived = 0 OR Archived IS NULL)
				ORDER BY ServiceOrderNumber, AccountNumber, SubAccountNumber, ServiceNumber
               END    
ELSE 
              BEGIN     
               SELECT NULL
	            FROM ServiceOrders
                WHERE 1 = 2 
               END
