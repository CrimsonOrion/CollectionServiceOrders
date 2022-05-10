using System.DirectoryServices.AccountManagement;

namespace CollectionServiceOrders.Core.ErrorHandler;
public class ErrorHandler : IErrorHandler
{
    private readonly string _name;
    private readonly string _emailAddress;
    private readonly string _adminEmail;
    private readonly string _smtpServer;
    private readonly IEmailer _emailer;
    private readonly ICustomLogger _logger;

    #region Constructor

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Will only be run on Windows machines, for now")]
    public ErrorHandler(IEmailer emailer, ICustomLogger logger)
    {
        UserPrincipal userInfo = UserPrincipal.Current;
        _name = userInfo.GivenName + " " + userInfo.Surname;
        _emailAddress = userInfo.EmailAddress;
        _adminEmail = GlobalConfig.EmailConfig.AdminEmail;
        _smtpServer = GlobalConfig.EmailConfig.SmtpServer;
        _emailer = emailer;
        _logger = logger;
    }

    #endregion

    #region Methods

    #region Public

    public async Task<string> ReportErrorAsync(Exception ex)
    {
        var errorMessage = $"Error as follows: {ex.Message}\r\nInner Exception:{ex.InnerException}";
        try
        {
            SendResponse result = await _emailer.SetOptions(
                new AddressModel(_emailAddress, _name),
                new List<AddressModel>() { new AddressModel(_adminEmail) },
                "Error in CSO 21",
                errorMessage + $"\r\nStackTrace:\r\n{ex.StackTrace}",
                _smtpServer
                ).SendEmailAsync();
            _logger.LogError(ex, errorMessage);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Problem sending mail: {e.Message}\r\n{e.InnerException}");
        }

        return errorMessage;
    }

    #endregion

    #endregion
}