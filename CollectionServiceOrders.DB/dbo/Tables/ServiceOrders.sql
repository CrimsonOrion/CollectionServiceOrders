﻿CREATE TABLE [dbo].[ServiceOrders](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[FSR] [varchar](50) NULL,
	[ServiceOrderNumber] [int] NOT NULL,
	[SignalNumber] [varchar](50) NULL,
	[RouteNumber] [int] NULL,
	[Cycle] [int] NULL,
	[Rate] [varchar](5) NULL,
	[MeterNumber] [int] NULL,
	[LocationNumber] [varchar](50) NULL,
	[AccountNumber] [int] NULL,
	[SubAccountNumber] [int] NULL,
	[ServiceNumber] [int] NOT NULL,
	[AccountName] [varchar](100) NULL,
	[AccountAddress1] [varchar](100) NULL,
	[AccountAddress2] [varchar](100) NULL,
	[AccountAddress3] [varchar](100) NULL,
	[AccountZip] [varchar](20) NULL,
	[Address] [varchar](100) NULL,
	[AddressStreetNumber] [int] NULL,
	[CutoffCreatorUserID] [varchar](50) NOT NULL,
	[ExistingDeposit] [money] NULL,
	[AdditionalDeposit] [money] NULL,
	[TotalDeposit] [money] NULL,
	[GuarantorAccountNumber] [int] NULL,
	[GuarantorSubAccountNumber] [int] NULL,
	[GuarantorAmount] [money] NULL,
	[AddedCharges] [money] NULL,
	[LetterPrinted] [bit] NULL,
	[ServiceOrderDate] [datetime2](7) NULL,
	[Completed] [bit] NULL,
	[DateCompleted] [datetime2](7) NULL,
	[Archived] [bit] NULL,
	[DateArchived] [datetime2](7) NULL,
	[Reading] [int] NULL,
	[SignalChange] [varchar](255) NULL,
	[PaymentReceived] [bit] NULL, 
    [KeyAccountString] [varchar](10) NULL, 
	[OnlinePayment] [bit] NULL,
    [BankDraft] [bit] NULL);
	GO

	CREATE UNIQUE INDEX idx_serviceOrderNumberServiceNumber
	ON [dbo].[ServiceOrders] (ServiceOrderNumber, ServiceNumber);
	GO