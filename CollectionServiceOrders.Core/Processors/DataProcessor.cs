namespace CollectionServiceOrders.Core.Processors;
public class DataProcessor : IDataProcessor
{
    private readonly ISqlDataAccess _sqlDataAccess;
    private readonly IOdbcDataAccess _odbcDataAccess;
    private readonly string _sqlConnString = new SqlConnectionStringBuilder { DataSource = "SqlServer19", InitialCatalog = "CollectionServiceOrderDB", IntegratedSecurity = true, TrustServerCertificate = true }.ConnectionString;
    private readonly string _odbcConnString = "Driver={IBM i Access ODBC Driver};System=AS400;TRANSLATE=1;SIGNON=4;SSL=1";

    #region Constructor
    public DataProcessor(ISqlDataAccess sqlDataAccess, IOdbcDataAccess odbcDataAccess)
    {
        _sqlDataAccess = sqlDataAccess;
        _odbcDataAccess = odbcDataAccess;
    }
    #endregion

    #region SQL Data Access

    public bool TestSqlConnection() => _sqlDataAccess.TestConnection(_sqlConnString);

    #region Service Orders

    public async Task<int> CreateNewServiceOrderAsync(ServiceOrderModel model)
    {
        var storedProcedure = "dbo.spServiceOrders_CreateNew";
        Dictionary<string, object> parameters = new()
        {
            { "@FSR", model.FSR.Trim() },
            { "@ServiceOrderNumber", model.ServiceOrderNumber },
            { "@SignalNumber", model.SignalNumber },
            { "@RouteNumber", model.RouteNumber },
            { "@Cycle", model.Cycle },
            { "@Rate", model.Rate },
            { "@MeterNumber", model.MeterNumber },
            { "@LocationNumber", model.LocationNumber },
            { "@AccountNumber", model.AccountNumber },
            { "@SubAccountNumber", model.SubAccountNumber },
            { "@ServiceNumber", model.ServiceNumber },
            { "@AccountName", model.AccountName },
            { "@AccountAddress1", model.AccountAddress1 },
            { "@AccountAddress2", model.AccountAddress2 },
            { "@AccountAddress3", model.AccountAddress3 },
            { "@AccountZip", model.AccountZip },
            { "@Address", model.Address },
            { "@AddressStreetNumber", model.AddressStreetNumber },
            { "@CutoffCreatorUserID", Environment.UserName },
            { "@ExistingDeposit", model.ExistingDeposit },
            { "@AdditionalDeposit", model.AdditionalDeposit },
            { "@TotalDeposit", model.TotalDeposit },
            { "@GuarantorAccountNumber", model.GuarantorAccountNumber },
            { "@GuarantorSubAccountNumber", model.GuarantorSubAccountNumber },
            { "@GuarantorAmount", model.GuarantorAmount },
            { "@AddedCharges", model.AddedCharges },
            { "@LetterPrinted", model.LetterPrinted },
            { "@ServiceOrderDate", model.ServiceOrderDate },
            { "@Completed", model.IsCompleted },
            { "@DateCompleted", model.IsCompleted == false ? null : model.DateCompleted },
            { "@Reading", model.Reading },
            { "@SignalChange", model.SignalChange },
            { "@PaymentReceived", model.IsPaymentReceived },
            { "@KeyAccountString", model.KeyAccountString },
            { "@OnlinePayment", model.IsOnlinePayment },
            { "@BankDraft", model.IsBankDraft }
        };

        var returnValue = await _sqlDataAccess.PostDataAsync(storedProcedure, _sqlConnString, parameters, true);
        return returnValue;
    }

    public async Task<IEnumerable<ServiceOrderModel>> RetrieveArchivedServiceOrdersByDateAndMeterAsync(int meterNumber, DateTime serviceOrderDate)
    {
        var storedProcedure = "dbo.spServiceOrders_RetrieveArchivedByDateAndMeter";
        Dictionary<string, object> parameters = new()
        {
            { "@MeterNumber", meterNumber },
            { "@ServiceOrderDate", serviceOrderDate }
        };

        IEnumerable<ServiceOrderModel> list = await _sqlDataAccess.GetDataAsync<ServiceOrderModel, IDictionary<string, object>>(storedProcedure, _sqlConnString, parameters, true);
        return list;
    }

    public async Task<IEnumerable<ServiceOrderModel>> RetrieveServiceOrdersByFilterAsync(string filter)
    {
        var storedProcedure = "dbo.spServiceOrders_RetrieveByFilter";
        Dictionary<string, object> parameters = new()
        {
            { "@Filter", filter ?? "All" }
        };
        IEnumerable<ServiceOrderModel> list = await _sqlDataAccess.GetDataAsync<ServiceOrderModel, IDictionary<string, object>>(storedProcedure, _sqlConnString, parameters, true);
        return list;
    }

    public async Task<ServiceOrderModel> RetrieveServiceOrdersBySONumberAndServiceAsync(ServiceOrderModel model)
    {
        var storedProcedure = "dbo.spServiceOrders_RetrieveBySONumberAndService";
        Dictionary<string, object> parameters = new()
        {
            { "@ServiceOrderNumber", model.ServiceOrderNumber },
            { "@ServiceNumber", model.ServiceNumber }
        };

        IEnumerable<ServiceOrderModel> output = await _sqlDataAccess.GetDataAsync<ServiceOrderModel, IDictionary<string, object>>(storedProcedure, _sqlConnString, parameters, true);
        return output.FirstOrDefault();
    }

    public async Task<int> UpdateArchiveFlagAllAsync(string userId)
    {
        var storedProcedure = "dbo.spServiceOrders_UpdateArchiveAll";
        Dictionary<string, object> parameters = new()
        {
            { "@SignalChange", $"Mass Archived by {userId} on {DateTime.Now}" }
        };
        var returnValue = await _sqlDataAccess.PutDataAsync(storedProcedure, _sqlConnString, parameters, true);
        return returnValue;
    }

    public async Task<int> UpdateArchiveFlagByIDAsync(ServiceOrderModel model)
    {
        var storedProcedure = "dbo.spServiceOrders_UpdateArchiveFlagByID";
        Dictionary<string, object> parameters = new()
        {
            { "@ID", model.ID },
            { "@Archived", model.Archived },
            { "@SignalChange", model.SignalChange },
        };

        var returnValue = await _sqlDataAccess.PutDataAsync(storedProcedure, _sqlConnString, parameters, true);
        return returnValue;
    }

    public async Task<int> UpdateCompletedByIDAsync(ServiceOrderModel model)
    {
        var storedProcedure = "dbo.spServiceOrders_UpdateCompletedFlagByID";
        Dictionary<string, object> parameters = new()
        {
            { "@ID", model.ID },
            { "@Completed", model.IsCompleted },
            { "@DateCompleted", model.IsCompleted == false ? null : model.DateCompleted },
        };
        var returnValue = await _sqlDataAccess.PutDataAsync(storedProcedure, _sqlConnString, parameters, true);
        return returnValue;
    }

    public async Task<int> UpdateServiceOrderByIDAsync(ServiceOrderModel model)
    {
        var storedProcedure = "dbo.spServiceOrders_UpdateByID";
        Dictionary<string, object> parameters = new()
        {
            { "@ID", model.ID },
            { "@FSR", model.FSR },
            { "@ServiceOrderNumber", model.ServiceOrderNumber },
            { "@SignalNumber", model.SignalNumber },
            { "@RouteNumber", model.RouteNumber },
            { "@Cycle", model.Cycle },
            { "@Rate", model.Rate },
            { "@MeterNumber", model.MeterNumber },
            { "@LocationNumber", model.LocationNumber },
            { "@AccountNumber", model.AccountNumber },
            { "@SubAccountNumber", model.SubAccountNumber },
            { "@ServiceNumber", model.ServiceNumber },
            { "@AccountName", model.AccountName },
            { "@AccountAddress1", model.AccountAddress1 },
            { "@AccountAddress2", model.AccountAddress2 },
            { "@AccountAddress3", model.AccountAddress3 },
            { "@AccountZip", model.AccountZip },
            { "@Address", model.Address },
            { "@AddressStreetNumber", model.AddressStreetNumber },
            { "@CutoffCreatorUserID", model.CutoffCreatorUserID },
            { "@ExistingDeposit", model.ExistingDeposit },
            { "@AdditionalDeposit", model.AdditionalDeposit },
            { "@TotalDeposit", model.TotalDeposit },
            { "@GuarantorAccountNumber", model.GuarantorAccountNumber },
            { "@GuarantorSubAccountNumber", model.GuarantorSubAccountNumber },
            { "@GuarantorAmount", model.GuarantorAmount },
            { "@AddedCharges", model.AddedCharges },
            { "@LetterPrinted", model.LetterPrinted },
            { "@ServiceOrderDate", model.ServiceOrderDate },
            { "@Completed", model.IsCompleted },
            { "@DateCompleted", model.IsCompleted == false ? null : model.DateCompleted },
            { "@Archived", model.Archived },
            { "@DateArchived", model.Archived == false ? null : model.DateArchived },
            { "@Reading", model.Reading },
            { "@SignalChange", model.SignalChange },
            { "@PaymentReceived", model.IsPaymentReceived },
            { "@KeyAccountString", model.KeyAccountString },
            { "@OnlinePayment", model.IsOnlinePayment },
            { "@BankDraft", model.IsBankDraft }
        };
        var returnValue = await _sqlDataAccess.PutDataAsync(storedProcedure, _sqlConnString, parameters, true);
        return returnValue;
    }

    #endregion

    #region User Settings

    public async Task<IEnumerable<UserSettingsModel>> RetrieveAllUserSettingsAsync()
    {
        var storedProcedure = "dbo.spUserSettings_RetrieveAll";
        IEnumerable<UserSettingsModel> output = await _sqlDataAccess.GetDataAsync<UserSettingsModel>(storedProcedure, _sqlConnString, true);
        return output;
    }

    public UserSettingsModel RetrieveUserSettingsByUsername(UserSettingsModel model)
    {
        var storedProcedure = "dbo.spUserSettings_RetrieveByUserID";
        Dictionary<string, object> parameters = new()
        {
            { "@UserID", model.UserID },
            { "@Role", model.Role },
            { "@FirstName", model.FirstName },
            { "@LastName", model.LastName },
            { "@Filter", model.Filter },
            { "@FontSize", model.FontSize },
            { "@ShowColumnSONumber", model.ShowColumnSONumber },
            { "@ShowColumnSignalNumber", model.ShowColumnSignalNumber },
            { "@ShowColumnFSR", model.ShowColumnFSR },
            { "@ShowColumnMeterNumber", model.ShowColumnMeterNumber },
            { "@ShowColumnReading", model.ShowColumnReading },
            { "@ShowColumnRate", model.ShowColumnRate },
            { "@ShowColumnRouteNumber", model.ShowColumnRouteNumber },
            { "@ShowColumnLocationNumber", model.ShowColumnLocationNumber },
            { "@ShowColumnAddress", model.ShowColumnAddress },
            { "@ShowColumnAccountNumber", model.ShowColumnAccountNumber },
            { "@ShowColumnExistingDeposit", model.ShowColumnExistingDeposit },
            { "@ShowColumnAdditionalDeposit", model.ShowColumnAdditionalDeposit },
            { "@ShowColumnAddedCharges", model.ShowColumnAddedCharges },
            { "@ShowColumnSignalChange", model.ShowColumnSignalChange },
            { "@ShowColumnLetterPrinted", model.ShowColumnLetterPrinted },
            { "@SortColumn", model.SortColumn },
            { "@SortAscending", model.SortAscending },
            { "@RefreshInterval", model.RefreshInterval },
            { "@EmailAddress", model.EmailAddress },
            { "@ColorCoding", model.ColorCoding },
            { "@ThemeSyncMode", model.ThemeSyncMode },
            { "@AppTheme", model.AppTheme },
            { "@AccentColor", model.AccentColor },
            { "@KeyAccountColor", model.KeyAccountColor },
            { "@CompletedColor", model.CompletedColor },
            { "@GoodSignalColor", model.GoodSignalColor },
            { "@PaymentReceivedColor", model.PaymentReceivedColor },
            { "@OnlinePaymentColor", model.OnlinePaymentColor },
            { "@BankDraftColor", model.BankDraftColor }
        };

        UserSettingsModel output = _sqlDataAccess.GetData<UserSettingsModel, IDictionary<string, object>>(storedProcedure, _sqlConnString, parameters, true).FirstOrDefault();

        return output ?? model;
    }

    public async Task<UserSettingsModel> RetrieveUserSettingsByUsernameAsync(UserSettingsModel model) => await Task.Run(() => RetrieveUserSettingsByUsername(model));

    public async Task<int> UpdateUserSettingsByIDAsync(UserSettingsModel model)
    {
        var storedProcedure = "dbo.spUserSettings_UpdateByUserID";
        Dictionary<string, object> parameters = new()
        {
            { "@UserID", model.UserID },
            { "@Role", model.Role },
            { "@FirstName", model.FirstName },
            { "@LastName", model.LastName },
            { "@Filter", model.Filter },
            { "@FontSize", model.FontSize },
            { "@ShowColumnSONumber", model.ShowColumnSONumber },
            { "@ShowColumnSignalNumber", model.ShowColumnSignalNumber },
            { "@ShowColumnFSR", model.ShowColumnFSR },
            { "@ShowColumnMeterNumber", model.ShowColumnMeterNumber },
            { "@ShowColumnReading", model.ShowColumnReading },
            { "@ShowColumnRate", model.ShowColumnRate },
            { "@ShowColumnRouteNumber", model.ShowColumnRouteNumber },
            { "@ShowColumnLocationNumber", model.ShowColumnLocationNumber },
            { "@ShowColumnAddress", model.ShowColumnAddress },
            { "@ShowColumnAccountNumber", model.ShowColumnAccountNumber },
            { "@ShowColumnExistingDeposit", model.ShowColumnExistingDeposit },
            { "@ShowColumnAdditionalDeposit", model.ShowColumnAdditionalDeposit },
            { "@ShowColumnAddedCharges", model.ShowColumnAddedCharges },
            { "@ShowColumnSignalChange", model.ShowColumnSignalChange },
            { "@ShowColumnLetterPrinted", model.ShowColumnLetterPrinted },
            { "@SortColumn", model.SortColumn },
            { "@SortAscending", model.SortAscending },
            { "@RefreshInterval", model.RefreshInterval },
            { "@EmailAddress", model.EmailAddress },
            { "@ColorCoding", model.ColorCoding },
            { "@ThemeSyncMode", model.ThemeSyncMode },
            { "@AppTheme", model.AppTheme },
            { "@AccentColor", model.AccentColor },
            { "@KeyAccountColor", model.KeyAccountColor },
            { "@CompletedColor", model.CompletedColor },
            { "@GoodSignalColor", model.GoodSignalColor },
            { "@PaymentReceivedColor", model.PaymentReceivedColor },
            { "@OnlinePaymentColor", model.OnlinePaymentColor },
            { "@BankDraftColor", model.BankDraftColor }
        };
        var returnValue = await _sqlDataAccess.PutDataAsync(storedProcedure, _sqlConnString, parameters, true);
        return returnValue;
    }

    #endregion

    #endregion

    #region Odbc Data Access

    public bool TestOdbcConnection() => _odbcDataAccess.TestConnection(_odbcConnString);

    #region Service Orders

    public async Task<ServiceOrderModel> DetermineDelinquentAmountAsync(ServiceOrderModel model)
    {
        var nonbudgetQuery = $"" +
            $"SELECT x3pe30 + x3pe60 + x3pe90 + x3pe09 AS Amount " +
            $"FROM database.tblax03 " +
            $"WHERE x3acct = {model.AccountNumber} AND x3sub = {model.SubAccountNumber}";

        var budgetQuery = $"" +
            $"SELECT x7bdue - x7buda AS Amount " +
            $"FROM database.tblax07 " +
            $"WHERE x7acct = {model.AccountNumber} AND x7sub = {model.SubAccountNumber} AND x7serv = 1 AND x7rsp = ''";

        var query = model.BudgetAccount ? budgetQuery : nonbudgetQuery;

        IEnumerable<decimal> output = await _odbcDataAccess.GetDataAsync<decimal>(query, _odbcConnString);
        model.DelinquentAmount = output.Any() ? output.FirstOrDefault() : 0m;

        return model;
    }

    public async Task<IEnumerable<ServiceOrderModel>> RetrieveCSOsByDateAsync(DateTime collectionDate)
    {
        IEnumerable<ServiceOrderModel> models = await RetrieveCSOsByDateQueryAsync(collectionDate);

        foreach (ServiceOrderModel model in models)
        {
            _ = await ProcessDepositQueriesAsync(model);
        }

        return models;
    }

    public async Task<ServiceOrderModel> RetrieveCSOBySONOAsync(ServiceOrderModel model)
    {
        model = await RetrieveCSOBySONOQueryAsync(model);

        if (model != null)
        {
            model = await ProcessDepositQueriesAsync(model);
        }

        return model;
    }

    public async Task<ServiceOrderModel> RetrieveDepositRecordsAsync(ServiceOrderModel model)
    {
        var query = $"" +
            $"SELECT xadamt, xaadja, xadarf, xaddat, xadint + xaiadj + xaincr AS InterestTotal " +
            $"FROM database.tblax10 " +
            $"WHERE xaacct = {model.AccountNumber} AND xasub = {model.SubAccountNumber} AND xaserv = {model.ServiceNumber} AND xadamt + xaadja - xadarf > 0";

        IEnumerable<DepositRecordModel> list = await _odbcDataAccess.GetDataAsync<DepositRecordModel>(query, _odbcConnString);

        model.DepositRecords = list.ToList();

        return model;
    }

    public async Task<ServiceOrderModel> RetrievePhoneCodesAsync(ServiceOrderModel model)
    {
        var query = $"" +
            $"SELECT DISTINCT hccode " +
            $"FROM database.tblhistc " +
            $"WHERE hccode > '' AND hccode <> 'ECE' AND hcacct = {model.AccountNumber} AND hcsub = {model.SubAccountNumber} AND hcdate >= '{model.ServiceOrderDate.ToShortDateString()}'";

        IEnumerable<string> output = await _odbcDataAccess.GetDataAsync<string>(query, _odbcConnString);

        var codes = "";
        foreach (var code in output)
        {
            codes += $"{code} ";
        }

        model.PhoneCodes = codes.Trim();

        return model;
    }

    public async Task<PaymentRecordModel> UpdatePaymentReceivedAsync(ServiceOrderModel model)
    {
        var query = $"" +
            $"SELECT msin50, mobatc " +
            $"FROM database.tblmstr " +
            $"INNER JOIN ( " +
            $"  SELECT moacct, mosub, moserv, mobatc " +
            $"  FROM database.tblmotr " +
            $"	WHERE moacct = {model.AccountNumber} AND mosub = {model.SubAccountNumber} AND moserv = {model.ServiceNumber} " +
            $"	ORDER BY modate DESC, motime DESC " +
            $"	LIMIT 1 " +
            $") ON msacct = moacct AND mssub = mosub AND msserv = moserv";

        PaymentRecordModel output = (await _odbcDataAccess.GetDataAsync<PaymentRecordModel>(query, _odbcConnString)).FirstOrDefault();
        return output;
    }

    #endregion

    #region Private Methods

    private async Task<ServiceOrderModel> ProcessDepositQueriesAsync(ServiceOrderModel model)
    {
        model = await ProcessLastTwelveMonthUsageOnLocationQueryAsync(model);
        model = await ProcessExistingTotalDepositQueryAsync(model);
        model = await ProcessGuarantorAmountQueryAsync(model);

        return model;
    }

    private async Task<ServiceOrderModel> ProcessExistingTotalDepositQueryAsync(ServiceOrderModel model)
    {
        var existingDeposit = 0m;
        var totalDeposit = model.TotalDeposit;
        var query = $"" +
            $"SELECT SUM(xadamt + xaadja + xadarf) AS EXISTING_DEPOSIT " +
            $"FROM database.tblax10 " +
            $"WHERE xaacct = {model.AccountNumber} AND xasub = {model.SubAccountNumber} AND xaserv IN ({model.ServiceNumber})";

        List<dynamic> deposits = new(await _odbcDataAccess.GetDataAsync<dynamic>(query, _odbcConnString));

        if (deposits[0].EXISTING_DEPOSIT is not null)
        {
            for (var record = 0; record < deposits.Count; record++)
            {
                existingDeposit += decimal.TryParse((deposits[record].EXISTING_DEPOSIT).ToString(), out decimal result) ? result : 0m;
            }
        }

        existingDeposit = Math.Round(existingDeposit);

        if (totalDeposit - existingDeposit > 0)
        {
            model.AdditionalDeposit = Math.Round(totalDeposit - existingDeposit);
        }
        else
        {
            model.AdditionalDeposit = 0m;
            totalDeposit = existingDeposit;
        }

        model.ExistingDeposit = existingDeposit;
        model.TotalDeposit = totalDeposit;
        return model;
    }

    private async Task<ServiceOrderModel> ProcessGuarantorAmountQueryAsync(ServiceOrderModel model)
    {
        var query = $"" +
            $"SELECT grnacct, grnsub, grnexdate, grnamt " +
            $"FROM database.tbl8grntr " +
            $"WHERE grngtacct = {model.AccountNumber} AND grngtsub = {model.SubAccountNumber} AND grnexdate >='{DateTime.Today.ToShortDateString()}'";

        dynamic output = (await _odbcDataAccess.GetDataAsync<dynamic>(query, _odbcConnString)).FirstOrDefault();

        if (output != null)
        {
            model.GuarantorAccountNumber = (int)output.GRNACCT;
            model.GuarantorSubAccountNumber = (int)output.GRNSUB;
            var guarAmount = (decimal)output.GRNAMT;
            var totalDeposit = model.TotalDeposit;
            var existingDeposit = model.ExistingDeposit;
            var temp = totalDeposit - existingDeposit - guarAmount;
            var additionalDeposit = 0m;
            if (temp > 0)
            {
                additionalDeposit = Math.Round(temp);
                totalDeposit = Math.Round(existingDeposit + temp);
            }
            else
            {
                totalDeposit = Math.Round(existingDeposit);
            }
            model.GuarantorAmount = guarAmount;
            model.TotalDeposit = totalDeposit;
            model.AdditionalDeposit = additionalDeposit;
        }

        return model;
    }

    private async Task<ServiceOrderModel> ProcessLastTwelveMonthUsageOnLocationQueryAsync(ServiceOrderModel model)
    {
        var minimumBill = model.ServiceNumber == 1 ? GlobalConfig.DepositInfoConfiguration.MinimumBillElectric : GlobalConfig.DepositInfoConfiguration.MinimumBillWater;

        var query = $"" +
            $"SELECT a.hiloc, a.hiserv, a.hidays, a.himmyy, b.hiamt, (a.hieng + a.hipca + a.hifca + a.hisl + a.hitax) AS Total " +
            $"FROM database.libhist7 a " +
            $"INNER JOIN (" +
            $"  SELECT SUM(hiamt) AS hiamt, himmyy, hiloc, hiserv " +
            $"  FROM database.libhist7 " +
            $"  GROUP BY himmyy, hiloc, hiserv " +
            $") b ON a.hiloc = b.hiloc AND a.hiserv = b.hiserv AND a.himmyy = b.himmyy " +
            $"WHERE a.hitype IN ('1','4') AND a.hiserv IN ({model.ServiceNumber}) AND a.hiloc = '{model.LocationNumber.Trim()}' AND (a.hieng + a.hipca + a.hifca + a.hisl + a.hitax) >= {minimumBill} " +
            $"ORDER BY a.himmyy DESC, a.hidays DESC " +
            $"LIMIT 12";

        IEnumerable<dynamic> list = await _odbcDataAccess.GetDataAsync<dynamic>(query, _odbcConnString);

        var totalDeposit = 0m;
        if (list.Any())
        {
            totalDeposit = (decimal)list.Max(_ => _.TOTAL);// Get Highest Bill
            var daysOfService = (int)list.Where(_ => _.TOTAL == totalDeposit).Select(_ => _.HIDAYS).First();// Get Days of Service on that bill

            var YYMM = (int)list.Where(_ => _.TOTAL == totalDeposit).Select(_ => _.HIMMYY).First();// Don't really need the month, but incase you do...
            var hiamt = (decimal)list.Where(_ => _.HIMMYY == YYMM).Select(_ => _.HIAMT).First();// Don't really need the HIAMY, but incase you do...
            var totalDepositWithHIAMT = totalDeposit + hiamt;

            totalDeposit = Math.Round(totalDeposit / daysOfService, 2, MidpointRounding.AwayFromZero);// Divide bill by days of service to get a per day amount rounded to the nearest cent.
            totalDeposit = Math.Round(totalDeposit * 30m * 2.5m, 0, MidpointRounding.AwayFromZero);// Use that amount and multiply by 30 (days of service) and then by 2.5 rounded to the nearest dollar.
        }

        var minDeposit = model.ServiceNumber == 1
            ? GlobalConfig.DepositInfoConfiguration.MinimumDepositElectric
            : GlobalConfig.DepositInfoConfiguration.MinimumDepositWater;
        if (totalDeposit < minDeposit)
        {
            totalDeposit = minDeposit;
        }

        model.TotalDeposit = totalDeposit;

        return model;
    }

    private static bool ProcessPaymentReceived(string paymentReceivedCode)
    {
        // R = Reg. coll.; S = Special coll.; B = Both
        var paymentReceived = paymentReceivedCode switch
        {
            "R" => false,
            "S" => false,
            "B" => false,
            _ => true
        };

        return paymentReceived;
    }

    private async Task<IEnumerable<ServiceOrderModel>> RetrieveCSOsByDateQueryAsync(DateTime collectionDate)
    {
        var query = $"" +
            $"SELECT sosono AS ServiceOrderNumber, msbook AS RouteNumber, bkcycl AS Cycle, msrate AS Rate, s2metr AS MeterNumberS2, lomete AS MeterNumberLO, soloc AS LocationNumber, soacct AS AccountNumber, sosub AS SubAccountNumber, soserv AS ServiceNumber, sosadd AS Address, sosadn AS AddressStreetNumber, soedte AS ServiceOrderDate, msin50 AS PaymentReceivedCode, msin07 AS BudgetAccountCode, prename AS PreName, fname AS FirstName, mname AS MiddleName, lname AS LastName, postname AS PostName, delivery1 AS AccountAddress1, delivery2 AS AccountAddress2, city as City, state AS State, zip AS AccountZip, nain16 AS KeyAccountString, xbacct AS BankDraftAcct " +
            $"FROM database.tblso " +
            $"INNER JOIN database.tblmstr ON msacct = soacct AND mssub = sosub " +
            $"INNER JOIN database.tblnamst ON naacct = soacct AND nasub = sosub " +
            $"INNER JOIN database.tblbk ON msbook = bkbook " +
            $"INNER JOIN database.tblloct ON loloc = soloc " +
            $"INNER JOIN database.tblnamcnt ON ncacct = soacct AND ncsub = sosub " +
            $"LEFT OUTER JOIN database.tbladdress ON nckeyid = adkeyid " +
            $"LEFT OUTER JOIN database.tblax11 ON xbacct = soacct AND xbsub = sosub " +
            $"LEFT OUTER JOIN (" +
            $"  SELECT s2sono, s2metr, s2serv " +
            $"  FROM database.tbls2ax " +
            $"  WHERE s2mtyp = '1'" +
            $") ON s2sono = sosono AND s2serv = soserv " +
            $"WHERE socode = '*CL' AND soedte = '{collectionDate.ToShortDateString()}' " +
            $"ORDER BY sosono ASC";

        IEnumerable<ServiceOrderModel> output = await _odbcDataAccess.GetDataAsync<ServiceOrderModel>(query, _odbcConnString);

        foreach (ServiceOrderModel so in output)
        {
            so.MeterNumber = so.MeterNumberS2 == 0 ? so.MeterNumberLO : so.MeterNumberS2;
            so.AccountName = so.AccountNameFormatted;
            so.AccountAddress3 = so.AccountAddress3Formatted;
        }

        return output;
    }

    private async Task<ServiceOrderModel> RetrieveCSOBySONOQueryAsync(ServiceOrderModel model)
    {
        var serviceType = model.ServiceType switch
        {
            null => model.ServiceNumber.ToString(),
            "Electric" => "1",
            _ => "2,3"
        };

        var query = $"" +
            $"SELECT sosono AS ServiceOrderNumber, msbook AS RouteNumber, bkcycl AS Cycle, msrate AS Rate, s2metr AS MeterNumberS2, lomete AS MeterNumberLO, soloc AS LocationNumber, soacct AS AccountNumber, sosub AS SubAccountNumber, soserv AS ServiceNumber, sosadd AS Address, sosadn AS AddressStreetNumber, soedte AS ServiceOrderDate, msin50 AS PaymentReceivedCode, msin07 AS BudgetAccountCode, prename AS PreName, fname AS FirstName, mname AS MiddleName, lname AS LastName, postname AS PostName, delivery1 AS AccountAddress1, delivery2 AS AccountAddress2, city as City, state AS State, zip AS AccountZip, nain16 AS KeyAccountString, xbacct AS BankDraftAcct " +
            $"FROM database.tblso " +
            $"INNER JOIN database.tblmstr ON msacct = soacct AND mssub = sosub " +
            $"INNER JOIN database.tblnamst ON naacct = soacct AND nasub = sosub " +
            $"INNER JOIN database.tblbk ON msbook = bkbook " +
            $"INNER JOIN database.tblloct ON loloc = soloc " +
            $"INNER JOIN database.tblnamcnt ON ncacct = soacct AND ncsub = sosub " +
            $"LEFT OUTER JOIN database.tbladdress ON nckeyid = adkeyid " +
            $"LEFT OUTER JOIN database.tblax11 ON xbacct = soacct AND xbsub = sosub " +
            $"LEFT OUTER JOIN (" +
            $"  SELECT s2sono, s2metr, s2serv " +
            $"  FROM database.tbls2ax " +
            $"  WHERE s2mtyp = '1'" +
            $") ON s2sono = sosono AND s2serv = soserv " +
            $"WHERE sosono = {model.ServiceOrderNumber} AND soserv IN ({serviceType})";

        model = (await _odbcDataAccess.GetDataAsync<ServiceOrderModel>(query, _odbcConnString)).FirstOrDefault();

        if (model != null && serviceType == "1")
        {
            model.ServiceType = "Electric";
        }
        else if (model != null)
        {
            model.ServiceType = "Water";
        }

        model.MeterNumber = model.MeterNumberS2 == 0 ? model.MeterNumberLO : model.MeterNumberS2;
        model.AccountName = model.AccountNameFormatted;
        model.AccountAddress3 = model.AccountAddress3Formatted;

        return model;
    }

    #endregion

    #endregion
}