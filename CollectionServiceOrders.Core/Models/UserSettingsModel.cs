using System.DirectoryServices.AccountManagement;

namespace CollectionServiceOrders.Core.Models;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Only used on Windows systems, for now")]
public class UserSettingsModel
{
    public string UserID { get; set; } = System.Environment.UserName ?? string.Empty;
    public UserRole Role { get; set; } = UserRole.CSR;
    public string FirstName { get; set; } = UserPrincipal.Current.GivenName ?? string.Empty;
    public string LastName { get; set; } = UserPrincipal.Current.Surname ?? string.Empty;
    public string Filter { get; set; } = "All";
    public int FontSize { get; set; } = 14;
    public bool ShowColumnSONumber { get; set; } = true;
    public bool ShowColumnSignalNumber { get; set; } = true;
    public bool ShowColumnFSR { get; set; } = true;
    public bool ShowColumnMeterNumber { get; set; } = true;
    public bool ShowColumnReading { get; set; } = true;
    public bool ShowColumnRate { get; set; } = true;
    public bool ShowColumnRouteNumber { get; set; } = true;
    public bool ShowColumnLocationNumber { get; set; } = true;
    public bool ShowColumnAddress { get; set; } = true;
    public bool ShowColumnAccountNumber { get; set; } = true;
    public bool ShowColumnExistingDeposit { get; set; } = true;
    public bool ShowColumnAdditionalDeposit { get; set; } = true;
    public bool ShowColumnAddedCharges { get; set; } = true;
    public bool ShowColumnSignalChange { get; set; } = true;
    public bool ShowColumnLetterPrinted { get; set; } = true;
    public string SortColumn { get; set; } = "SO#";
    public bool SortAscending { get; set; } = true;
    public int RefreshInterval { get; set; } = 60;
    public string EmailAddress { get; set; } = UserPrincipal.Current.EmailAddress ?? string.Empty;
    public bool ColorCoding { get; set; } = true;
    public string ThemeSyncMode { get; set; } = "SyncAll";
    public string AppTheme { get; set; } = "";
    public string AccentColor { get; set; } = "";
    public string KeyAccountColor { get; set; } = "DarkSalmon";
    public string CompletedColor { get; set; } = "Gray";
    public string GoodSignalColor { get; set; } = "Fuchsia";
    public string PaymentReceivedColor { get; set; } = "LimeGreen";
    public string OnlinePaymentColor { get; set; } = "Aqua";
    public string BankDraftColor { get; set; } = "MediumPurple";
    public string FullName => $"{FirstName} {LastName}";
}