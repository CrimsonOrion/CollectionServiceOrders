namespace CollectionServiceOrders.Core.Models;
public class ServiceOrderModel
{
    // = Field in SQL table ServiceOrders
    public int ID { get; set; } //
    public string FSR { get; set; } = string.Empty; //
    public int ServiceOrderNumber { get; set; } //
    public string SignalNumber { get; set; } = string.Empty; //
    public int RouteNumber { get; set; } //
    public int Cycle { get; set; } //
    public string Rate { get; set; } //
    public int MeterNumberS2 { get; set; }
    public int MeterNumberLO { get; set; }
    public int MeterNumber { get; set; } //
    public string LocationNumber { get; set; } //
    public int AccountNumber { get; set; } //
    public int SubAccountNumber { get; set; } //
    public int ServiceNumber { get; set; } //
    public string AccountName { get; set; } //
    public string AccountAddress1 { get; set; } //
    public string AccountAddress2 { get; set; } //
    public string AccountAddress3 { get; set; } //
    public string AccountZip { get; set; } //
    public string Address { get; set; } //
    public int AddressStreetNumber { get; set; } //
    public string CutoffCreatorUserID { get; set; } //
    public decimal ExistingDeposit { get; set; } //
    public decimal AdditionalDeposit { get; set; } //
    public decimal TotalDeposit { get; set; } //
    public int? GuarantorAccountNumber { get; set; } //
    public int? GuarantorSubAccountNumber { get; set; } //
    public decimal? GuarantorAmount { get; set; } //
    public decimal AddedCharges //
    {
        get
        {
            if (!SignalNumber.Equals(string.Empty))
            {
                return GlobalConfig.SignalChargeFeesConfiguration.SignalNumbers.FirstOrDefault(_ => _.SignalNumber == SignalNumber).Fee;
            }
            else
            {
                return 0m;
            }
        }
    }

    public bool LetterPrinted { get; set; } //
    public DateTime ServiceOrderDate { get; set; } //
    public bool Completed { get; set; } //
    public DateTime? DateCompleted { get; set; } //
    public bool Archived { get; set; } = false; //
    public DateTime? DateArchived { get; set; } //
    public int Reading { get; set; } //
    public string SignalChange { get; set; } //
    public bool PaymentReceived { get; set; }//
    public string KeyAccountString { get; set; } //
    public int BankDraftAcct { get; set; } = 0;
    public bool OnlinePayment { get; set; } //
    public int BankDraft { get; set; } //
    public string LocationFormatted => LocationNumber.Length > 1 ? $"{LocationNumber}" : "";
    public string AddressFormatted => $"{AddressStreetNumber} {Address}";
    public string AccountFormatted => $"{AccountNumber}-{SubAccountNumber.ToString().PadLeft(3, '0')}";
    public string GuarantorAccountFormatted => GuarantorAccountNumber is null or 0 ? "N/A" : $"{GuarantorAccountNumber}-{GuarantorSubAccountNumber.ToString().PadLeft(3, '0')}";
    public string ExistingDepositFormatted => $"{ExistingDeposit:c}";
    public string AdditionalDepositFormatted => $"{AdditionalDeposit:c}";
    public string TotalDepositFormatted => $"{TotalDeposit:c}";
    public string AddedChargesFormatted => $"{AddedCharges:c}";
    public string GuarantorAmountFormatted => GuarantorAccountNumber is null or 0 ? "N/A" : $"{Convert.ToString(GuarantorAmount, new CurrencyFormatProvider())}";
    public bool IsCompleted => Completed;
    public bool IsLetterPrinted => LetterPrinted;
    public bool IsArchived => Archived;
    public bool IsPaymentReceived => PaymentReceived;
    public bool IsOnlinePayment => OnlinePayment;
    public bool IsSignalNumberGood => SignalNumber != null && (SignalNumber.Contains('4') || SignalNumber.Contains('7') || SignalNumber.Contains('1'));
    public bool IsKeyAccount => KeyAccountString?.Trim() == "Y";
    public bool IsBankDraft => BankDraftAcct > 0 || BankDraft == 1;
    public string ServiceType { get; set; }
    public string PaymentReceivedCode { get; private set; }

    public List<DepositRecordModel> DepositRecords { get; set; } = new();
    public string PhoneCodes { get; set; }
    public double MinimumDeposit { get; set; }
    public double MinimumAdditionalDeposit { get; } = Convert.ToDouble(GlobalConfig.DepositInfoConfiguration.MinimumAdditionalDeposit);

    public bool BudgetAccount => BudgetAccountCode == "Y".ToUpper();
    public string BudgetAccountCode { get; set; }
    public decimal DelinquentAmount { get; set; }
    public string DelinquentAmountFormatted => $"{DelinquentAmount:c}";

    // From address file
    public string PreName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string PostName { get; set; }
    public string Attention { get; set; }
    public string AccountNameFormatted
    {
        get
        {
            var name = string.IsNullOrEmpty(LastName) ? "" : LastName.Trim();
            name += string.IsNullOrEmpty(PostName) ? "," : $" {PostName.Trim()},";
            name += string.IsNullOrEmpty(PreName) ? "" : $" {PreName.Trim()}";
            name += string.IsNullOrEmpty(FirstName) ? "" : $" {FirstName.Trim()}";
            name += string.IsNullOrEmpty(MiddleName) ? "" : $" {MiddleName.Trim()}";

            return name;
        }
    }
    public string City { get; set; }
    public string State { get; set; }
    public string AccountAddress3Formatted => $"{City.Trim()} {State.Trim()}";
}