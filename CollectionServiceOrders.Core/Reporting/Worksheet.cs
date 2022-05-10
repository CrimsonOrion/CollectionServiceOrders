using System.Diagnostics;

namespace CollectionServiceOrders.Core.Reporting;
public class Worksheet : IWorksheet
{
    #region Constructor

    public Worksheet() => ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

    #endregion

    #region Methods

    #region Public

    public void CreateAdditionalDepositWorksheet(List<ServiceOrderModel> serviceOrders, bool isExcelVisible = true, CancellationToken token = new())
    {
        serviceOrders = serviceOrders.OrderBy(_ => _.AccountFormatted).ThenBy(_ => _.AccountName).ThenBy(_ => _.ServiceOrderNumber).ToList();

        FileInfo fileInfo = new($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\{DateTime.Today:yyyy-MM-dd} Additional Deposit Report.xlsx");
        using ExcelPackage excel = new();
        // Add worksheet
        ExcelWorksheet sheet = excel.Workbook.Worksheets.Add($"Additional Deposits {DateTime.Today.ToShortDateString()}");

        // Populate Header variables
        var reportHeader = $"{DateTime.Today:MM-dd-yyyy} Additional Deposit Report";
        List<string> pageHeader = new() { "Name", "Acct#", "SO No.", "Exist. Dep.", "Exst. Dep. Date", "Interest", "Transfer Amt.", "Guarantor", "Additional Dep.", "Cycle" };

        // Build Excel sheet
        // Insert report headers
        using (ExcelRange range = sheet.Cells[1, 1, 1, 6])
        {
            range.Style.Font.Name = "Arial";
            range.Style.Font.Size = 18;
            range.Style.Font.Bold = true;
            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            range.Merge = true;
            range.Value = reportHeader;
        }

        // Insert page headers
        for (var i = 1; i <= pageHeader.Count; i++)
        {
            using ExcelRange range = sheet.Cells[2, i, 2, i];
            range.Style.Font.Name = "Calibri";
            range.Style.Font.Size = 11;
            range.Style.Font.Bold = true;
            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            range.Value = pageHeader[i - 1];
            range.AutoFitColumns();
        }

        // Insert data
        var row = 3;
        var col = 1;
        var sameInfo = "****";
        foreach (ServiceOrderModel so in serviceOrders)
        {
            // Check if user wants to cancel the report generation
            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }

            col = 1;

            // set variables
            var accountName = so.AccountName;
            var accountNumber = so.AccountFormatted;
            var soNumber = so.ServiceOrderNumber;
            var guarantor = so.GuarantorAccountNumber == 0 ? "----" : so.GuarantorAccountFormatted;
            var additionalDep = so.AdditionalDeposit;
            var existingDep = so.ExistingDeposit;
            var transferAmt = so.ExistingDeposit + so.AdditionalDeposit;
            var cycle = so.Cycle;
            var existingDeposit = 0.0d;
            var interestTotal = 0.0d;

            using (ExcelRange range = sheet.Cells[row, col, row + so.DepositRecords.Count, pageHeader.Count])
            {
                range.Style.Font.Name = "Calibri";
                range.Style.Font.Size = 11;
                range.Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                // Account Number
                sheet.Cells[row, col++].Value = accountNumber;

                // Account Name
                sheet.Cells[row, col++].Value = accountName;

                // SO No.
                sheet.Cells[row, col].Style.Numberformat.Format = "#";
                sheet.Cells[row, col++].Value = soNumber;

                // Exist. Dep.
                sheet.Cells[row, col++].Value = existingDep;

                // Exst. Dep. Date
                sheet.Cells[row, col++].Value = sameInfo;

                // Interest (value input after calculated)
                sheet.Cells[row, col++].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                // Transfer Amt.
                sheet.Cells[row, col++].Value = transferAmt;

                // Guarantor
                sheet.Cells[row, col++].Value = guarantor;

                // Additional Dep.
                sheet.Cells[row, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[row, col++].Value = additionalDep;

                // Cycle
                sheet.Cells[row, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[row, col].Style.Numberformat.Format = "#";
                sheet.Cells[row, col].Value = cycle;

                var startRow = row++;

                if (so.DepositRecords.Any())
                {
                    foreach (DepositRecordModel record in so.DepositRecords)
                    {
                        col = 4;
                        var dep = record.XADAMT + record.XAADJA - record.XADARF;
                        DateTime depDate = record.XADDAT;
                        var interest = record.InterestTotal;
                        existingDeposit += dep;
                        interestTotal += interest;
                        sheet.Cells[row, col++].Value = dep;
                        sheet.Cells[row, col++].Value = depDate.ToShortDateString();
                        sheet.Cells[row, col].Value = interest;

                        col = 9;
                        sheet.Cells[row++, col].Value = sameInfo;
                    }

                    // Interest inserted after calculated.
                    col = 6;
                    sheet.Cells[startRow, col].Style.Font.Italic = true;
                    sheet.Cells[startRow, col].Style.Font.Bold = true;
                    sheet.Cells[startRow, col].Value = interestTotal;
                }
            }

            // Add a border line separating the accounts
            using (ExcelRange range = sheet.Cells[row - 1, 1, row - 1, pageHeader.Count])
            {
                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }
        }

        sheet.Cells.AutoFitColumns();

        excel.SaveAs(fileInfo);
        if (isExcelVisible)
        {
            ProcessStartInfo processStartInfo = new() { FileName = $"\"{fileInfo.FullName}\"", UseShellExecute = true };
            _ = Process.Start(processStartInfo);
        }
    }

    public void ExportArchivedCutoffs(List<ServiceOrderModel> serviceOrders, bool isExcelVisible = true, CancellationToken token = new())
    {
        FileInfo fileInfo = new($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Archived Cutoffs Report.xlsx");
        using (ExcelPackage excel = new())
        {
            // Add worksheet
            ExcelWorksheet sheet = excel.Workbook.Worksheets.Add($"Archived Cutoffs - {DateTime.Now}");

            // Populate Header variables
            var reportHeader = "Archived Cutoffs";
            List<string> pageHeader = new() { "Completed", "FSR", "SONumber", "Signal", "Route", "Letter", "Exst. Dep.", "Add. Dep.", "Meter", "Location", "Account", "Signal Change", "Charges", "Readings" };

            // Build Excel sheet
            // Insert Report Headers
            using (ExcelRange range = sheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Name = "Arial";
                range.Style.Font.Size = 18;
                range.Style.Font.Bold = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                range.Merge = true;
                range.Value = reportHeader;
            }

            // Insert page headers
            for (var i = 1; i <= pageHeader.Count; i++)
            {
                using ExcelRange range = sheet.Cells[2, i, 2, i];
                range.Style.Font.Name = "Calibri";
                range.Style.Font.Size = 12;
                range.Style.Font.Bold = true;
                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Value = pageHeader[i - 1];
                range.AutoFitColumns();
            }

            // Insert data
            var row = 3;
            var col = 1;
            using (ExcelRange range = sheet.Cells[row, col, row + serviceOrders.Count, pageHeader.Count])
            {
                range.Style.Font.Name = "Calibri";
                range.Style.Font.Size = 11;
                range.Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                foreach (ServiceOrderModel so in serviceOrders)
                {
                    // Check if user wants to cancel the report generation
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }

                    var completed = so.Completed ? "Y" : "N";
                    var fsr = so.FSR;
                    var soNumber = so.ServiceOrderNumber.ToString();
                    var signal = so.SignalNumber;
                    var route = so.RouteNumber.ToString();
                    var letter = so.LetterPrinted ? "Y" : "N";
                    var exDep = so.ExistingDeposit;
                    var addDep = so.AdditionalDeposit;
                    var meter = so.MeterNumber.ToString();
                    var loc = so.LocationFormatted;
                    var acct = so.AccountFormatted;
                    var sigChange = so.SignalChange;
                    var charges = so.AddedCharges;
                    var reading = so.Reading;

                    col = 1;
                    sheet.Cells[row, col++].Value = completed;
                    sheet.Cells[row, col++].Value = fsr;
                    sheet.Cells[row, col++].Value = soNumber;
                    sheet.Cells[row, col++].Value = signal;
                    sheet.Cells[row, col++].Value = route;
                    sheet.Cells[row, col++].Value = letter;
                    sheet.Cells[row, col++].Value = exDep;
                    sheet.Cells[row, col++].Value = addDep;
                    sheet.Cells[row, col++].Value = meter;
                    sheet.Cells[row, col++].Value = loc;
                    sheet.Cells[row, col++].Value = acct;
                    sheet.Cells[row, col++].Value = sigChange;
                    sheet.Cells[row, col++].Value = charges;
                    sheet.Cells[row, col].Style.Numberformat.Format = "#";
                    sheet.Cells[row++, col].Value = reading;
                }
            }

            sheet.Cells.AutoFitColumns();

            excel.SaveAs(fileInfo);
        }
        if (isExcelVisible)
        {
            ProcessStartInfo processStartInfo = new() { FileName = $"\"{fileInfo.FullName}\"", UseShellExecute = true };
            _ = Process.Start(processStartInfo);
        }
    }

    public void ExportCurrentCutoffs(List<ServiceOrderModel> serviceOrders, bool isExcelVisible = true, CancellationToken token = new())
    {
        FileInfo fileInfo = new($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Current Cutoffs Report.xlsx");
        using (ExcelPackage excel = new())
        {
            // Add worksheet
            ExcelWorksheet sheet = excel.Workbook.Worksheets.Add($"Current Cutoffs - {DateTime.Now}");

            // Populate Header variables
            var reportHeader = "Current Cutoffs";
            List<string> pageHeader = new() { "Completed", "SONumber", "Signal", "FSR", "Meter", "Readings", "Rate", "Route", "Location", "Address", "Account", "Exist. Dep.", "Add. Dep.", "Charges", "Letter", "Signal Change" };

            // Build Excel sheet
            // Insert Report Headers
            using (ExcelRange range = sheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Name = "Arial";
                range.Style.Font.Size = 18;
                range.Style.Font.Bold = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                range.Merge = true;
                range.Value = reportHeader;
            }

            // Insert page headers
            for (var i = 1; i <= pageHeader.Count; i++)
            {
                using ExcelRange range = sheet.Cells[2, i, 2, i];
                range.Style.Font.Name = "Calibri";
                range.Style.Font.Size = 12;
                range.Style.Font.Bold = true;
                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Value = pageHeader[i - 1];
                range.AutoFitColumns();
            }

            // Insert data
            var row = 3;
            var col = 1;
            using (ExcelRange range = sheet.Cells[row, col, row + serviceOrders.Count, pageHeader.Count])
            {
                range.Style.Font.Name = "Calibri";
                range.Style.Font.Size = 11;
                range.Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                foreach (ServiceOrderModel so in serviceOrders)
                {
                    // Check if user wants to cancel the report generation
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }

                    var completed = so.Completed ? "Y" : "N";
                    var soNumber = so.ServiceOrderNumber.ToString();
                    var signal = so.SignalNumber;
                    var fsr = so.FSR;
                    var meter = so.MeterNumber.ToString();
                    var reading = so.Reading;
                    var rate = so.Rate;
                    var route = so.RouteNumber.ToString();
                    var loc = so.LocationFormatted;
                    var address = so.AddressFormatted;
                    var acct = so.AccountFormatted;
                    var exDep = so.ExistingDeposit;
                    var addDep = so.AdditionalDeposit;
                    var charges = so.AddedCharges;
                    var letter = so.LetterPrinted ? "Y" : "N";
                    var sigChange = so.SignalChange;

                    col = 1;
                    sheet.Cells[row, col++].Value = completed;
                    sheet.Cells[row, col++].Value = soNumber;
                    sheet.Cells[row, col++].Value = signal;
                    sheet.Cells[row, col++].Value = fsr;
                    sheet.Cells[row, col++].Value = meter;
                    sheet.Cells[row, col].Style.Numberformat.Format = "#";
                    sheet.Cells[row, col++].Value = reading;
                    sheet.Cells[row, col++].Value = rate;
                    sheet.Cells[row, col++].Value = route;
                    sheet.Cells[row, col++].Value = loc;
                    sheet.Cells[row, col++].Value = address;
                    sheet.Cells[row, col++].Value = acct;
                    sheet.Cells[row, col++].Value = exDep;
                    sheet.Cells[row, col++].Value = addDep;
                    sheet.Cells[row, col++].Value = charges;
                    sheet.Cells[row, col++].Value = letter;
                    sheet.Cells[row++, col].Value = sigChange;
                }
            }

            sheet.Cells.AutoFitColumns();

            excel.SaveAs(fileInfo);
        }
        if (isExcelVisible)
        {
            ProcessStartInfo processStartInfo = new() { FileName = $"\"{fileInfo.FullName}\"", UseShellExecute = true };
            Process.Start(processStartInfo);
        }
    }

    #endregion

    #endregion
}