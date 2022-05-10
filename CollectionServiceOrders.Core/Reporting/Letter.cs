using System.Diagnostics;

namespace CollectionServiceOrders.Core.Reporting;
public class Letter : ILetter
{
    #region Constructor

    public Letter() => ComponentInfo.SetLicense("FREE-LIMITED-KEY");

    #endregion

    #region Methods

    #region Public

    // Unused. Here for reference of multi-page documents.
    public void CreateAdditionalDepositLetters(List<ServiceOrderModel> serviceOrders, bool makeWordVisible)
    {
        FileInfo fileInfo = new($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\{DateTime.Today:yyyy-MM-dd} Additional Deposit Letters.docx");

        var template = File.Exists(GlobalConfig.TemplateFileLocationConfiguration.LetterheadBwDfs) ? GlobalConfig.TemplateFileLocationConfiguration.LetterheadBwDfs : GlobalConfig.TemplateFileLocationConfiguration.LetterheadBwHardcode;

        DocumentModel document = DocumentModel.Load(template);
        Block paragraph = document.Sections[0].Blocks[0];
        _ = document.Sections[0].Blocks.Remove(paragraph);
        document.DefaultCharacterFormat.FontName = "Times New Roman";
        document.DefaultCharacterFormat.Size = 12;
        var letterDate = DateTime.Today.ToLongDateString();
        var pageNum = 1;
        foreach (ServiceOrderModel so in serviceOrders)
        {
            var nameLine = so.AccountName.Trim().ToUpper();
            var accountLine = $"Account #: {so.AccountFormatted}";

            Paragraph body = new(document);

            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new Run(document, letterDate));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new Run(document, nameLine));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new Run(document, so.AccountAddress1.Trim().ToUpper()));
            if (so.AccountAddress2.Trim().Length > 0)
            {
                body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                body.Inlines.Add(new Run(document, so.AccountAddress2.Trim().ToUpper()));
            }
            if (so.AccountAddress3.Trim().Length > 0)
            {
                body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                body.Inlines.Add(new Run(document, so.AccountAddress3.Trim().ToUpper()));
            }
            body.Inlines.Add(new Run(document, " " + so.AccountZip.ToUpper()));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new Run(document, accountLine));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new Run(document, "" +
                "Dear Member:"));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new Run(document, $"" +
                $"Your electric service was recently disconnected for non-payment, and a security " +
                $"deposit of {so.AdditionalDepositFormatted} was charged to your account when your service was reconnected. " +
                $"The charge will show on your next billing and is due and payable by the date " +
                $"indicated on the bill."));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new Run(document, $"" +
                $"A service disconnected for non-payment is required to pay a security deposit equal " +
                $"to 2.5 times the highest estimated power bill for that location."));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new Run(document, $"" +
                $"Your total security deposit of {so.TotalDepositFormatted} will be refunded with interest after maintaining a " +
                $"good credit rating for a period of thirty-six consecutive months, or upon termination " +
                $"of service."));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new Run(document, "" +
                "If you have any questions, feel free to call our Customer Service Department."));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new Run(document, "" +
                "Sincerely,"));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new Run(document, "" +
                "Carteret-Craven Electric Cooperative"));

            if (pageNum < serviceOrders.Count)
            {
                body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.PageBreak));
                pageNum++;
            }

            document.Sections[0].Blocks.Add(body);
        }

        document.Save(fileInfo.FullName);
        if (makeWordVisible)
        {
            ProcessStartInfo processStartInfo = new() { FileName = $"\"{fileInfo.FullName}\"", UseShellExecute = true };
            _ = Process.Start(processStartInfo);
        }
    }

    public void CreateAdditionalDepositLetter(ServiceOrderModel serviceOrder, bool makeWordVisible)
    {
        var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Deposit Letters");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        FileInfo fileInfo = new(Path.Combine(folderPath, $"{serviceOrder.AccountFormatted} {serviceOrder.AccountName} {DateTime.Today:yyyy-MM-dd}.docx"));

        var template = File.Exists(GlobalConfig.TemplateFileLocationConfiguration.LetterheadBwDfs) ? GlobalConfig.TemplateFileLocationConfiguration.LetterheadBwDfs : GlobalConfig.TemplateFileLocationConfiguration.LetterheadBwHardcode;

        DocumentModel document = DocumentModel.Load(template);
        Block paragraph = document.Sections[0].Blocks[0];
        _ = document.Sections[0].Blocks.Remove(paragraph);
        document.DefaultCharacterFormat.FontName = "Times New Roman";
        document.DefaultCharacterFormat.Size = 12;
        var letterDate = DateTime.Today.ToLongDateString();

        var nameLine = serviceOrder.AccountName.Trim().ToUpper();
        var accountLine = $"Account #: {serviceOrder.AccountFormatted}";

        Paragraph body = new(document);

        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new Run(document, letterDate));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new Run(document, nameLine));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new Run(document, serviceOrder.AccountAddress1.Trim().ToUpper()));
        if (serviceOrder.AccountAddress2.Trim().Length > 0)
        {
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new Run(document, serviceOrder.AccountAddress2.Trim().ToUpper()));
        }
        if (serviceOrder.AccountAddress3.Trim().Length > 0)
        {
            body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
            body.Inlines.Add(new Run(document, serviceOrder.AccountAddress3.Trim().ToUpper()));
        }
        body.Inlines.Add(new Run(document, " " + serviceOrder.AccountZip.ToUpper()));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new Run(document, accountLine));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new Run(document, "" +
            "Dear Member:"));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new Run(document, $"" +
            $"Your electric service was recently disconnected for non-payment, and a security " +
            $"deposit of {serviceOrder.AdditionalDepositFormatted} was charged to your account when your service was reconnected. " +
            $"The charge will show on your next billing and is due and payable by the date " +
            $"indicated on the bill."));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new Run(document, $"" +
            $"A service disconnected for non-payment is required to pay a security deposit equal " +
            $"to 2.5 times the highest estimated power bill for that location."));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new Run(document, $"" +
            $"Your total security deposit of {serviceOrder.TotalDepositFormatted} will be refunded with interest after maintaining a " +
            $"good credit rating for a period of thirty-six consecutive months, or upon termination " +
            $"of service."));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new Run(document, "" +
            "If you have any questions, feel free to call our Customer Service Department."));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new Run(document, "" +
            "Sincerely,"));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
        body.Inlines.Add(new Run(document, "" +
            "Carteret-Craven Electric Cooperative"));

        document.Sections[0].Blocks.Add(body);

        document.Save(fileInfo.FullName);
        if (makeWordVisible)
        {
            ProcessStartInfo processStartInfo = new() { FileName = $"\"{fileInfo.FullName}\"", UseShellExecute = true };
            _ = Process.Start(processStartInfo);
        }
    }

    #endregion

    #endregion
}