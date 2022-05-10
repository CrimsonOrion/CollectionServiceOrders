namespace CollectionServiceOrders.Core.Configuration;

public class EmailSettingsConfigurationModel
{
    public string SmtpServer { get; set; }
    public string AdminEmail { get; set; }
    public string AdminText { get; set; }
}