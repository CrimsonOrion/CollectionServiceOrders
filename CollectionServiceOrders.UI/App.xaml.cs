using System.IO;
using System.Windows;

namespace CollectionServiceOrders.UI;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override Window CreateShell() => Container.Resolve<MainWindow>();

    protected override void OnStartup(StartupEventArgs e) => base.OnStartup(e);//ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncAll;//ThemeManager.Current.SyncTheme();

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // Set up the custom logger to go to Desktop
        ICustomLogger logger = new CustomLogger(
            new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CCEMC Collection Service Orders Errors.log")),
            true,
            LogLevel.Error);

        // Set up the configuration using the appSettings.json file.
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json", false, true)
            .Build();

        GlobalConfig.EmailConfig = new()
        {
            SmtpServer = configuration["Email Settings:Smtp Server"] ?? "",
            AdminEmail = configuration["Email Settings:Admin Email"] ?? "",
            AdminText = configuration["Email Settings:Admin Text"] ?? ""
        };

        GlobalConfig.DepositInfoConfiguration = new()
        {
            MinimumDepositElectric = decimal.TryParse(configuration["Deposit Info:Minimum Deposit Electric"], out var d) ? d : 0m,
            MinimumDepositWater = decimal.TryParse(configuration["Deposit Info:Minimum Deposit Water"], out d) ? d : 0m,
            MinimumBillElectric = decimal.TryParse(configuration["Deposit Info:Minimum Bill Electric"], out d) ? d : 0m,
            MinimumBillWater = decimal.TryParse(configuration["Deposit Info:Minimum Bill Water"], out d) ? d : 0m,
            MinimumAdditionalDeposit = decimal.TryParse(configuration["Deposit Info:Minimum Additional Deposit"], out d) ? d : 0m
        };

        GlobalConfig.SignalChargeFeesConfiguration = new()
        {
            SignalNumbers = new()
        };

        foreach (IConfigurationSection item in configuration.GetSection("Signal Charge Fees").GetChildren())
        {
            SignalNumberModel signalNumber = new()
            {
                SignalNumber = item.Key,
                FeeString = item.Value
            };
            signalNumber.Description = signalNumber.SignalNumber switch
            {
                "1" => $"{signalNumber.SignalNumber} - ERROR-RECON ASAP",
                "1B" => $"{signalNumber.SignalNumber} - ERROR BKT-RECON ASAP",
                "2" => $"{signalNumber.SignalNumber} - CUT",
                "2B" => $"{signalNumber.SignalNumber} - BKT TRK CUT",
                "3" => $"{signalNumber.SignalNumber} - TRIP CHARGE",
                "3B" => $"{signalNumber.SignalNumber} - BKT TRIP",
                "4" => $"{signalNumber.SignalNumber} - RECONNECT",
                "4B" => $"{signalNumber.SignalNumber} - BKT RECON",
                "OT4" => $"{signalNumber.SignalNumber} - OT RECON",
                "OT4B" => $"{signalNumber.SignalNumber} - BTK OT RECON",
                "7" => $"{signalNumber.SignalNumber} - KILL C/O",
                "9" => $"{signalNumber.SignalNumber} - FIELD CONNECT",
                "9B" => $"{signalNumber.SignalNumber} - BKT COLLECT",
                _ => $"{signalNumber.SignalNumber} - ???"
            };
            GlobalConfig.SignalChargeFeesConfiguration.SignalNumbers.Add(signalNumber);
        }

        GlobalConfig.TemplateFileLocationConfiguration = new()
        {
            LetterheadBwDfs = configuration["Template File Locations:Letterhead-BW-DFS"] ?? "",
            LetterheadBwHardcode = configuration["Template File Locations:Letterhead-BW-Hardcode"] ?? ""
        };

        _ = containerRegistry
            .RegisterInstance(logger)

            .RegisterInstance<IDialogCoordinator>(new DialogCoordinator())

            .RegisterScoped<ISqlDataAccess, MsSqlDataAccess>()
            .RegisterScoped<IOdbcDataAccess, OdbcDataAccess>()
            .RegisterScoped<IEmailer, FluentEmailerMailKit>()
            .RegisterScoped<IErrorHandler, ErrorHandler>()
            .RegisterScoped<IDataProcessor, DataProcessor>()
            .RegisterScoped<IWorksheet, Worksheet>()
            .RegisterScoped<ILetter, Letter>()
            ;
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) => _ = moduleCatalog
        .AddModule<AboutModule>()
        .AddModule<CutoffScreenModule>()
        .AddModule<AdditionalDepositModule>()
        .AddModule<AdhocReportModule>()
        .AddModule<AddNewCutoffModule>()
        .AddModule<SearchForCutoffModule>()
        .AddModule<ImportDataFromDaffronModule>()
        .AddModule<AdministrationModule>()
        ;
}