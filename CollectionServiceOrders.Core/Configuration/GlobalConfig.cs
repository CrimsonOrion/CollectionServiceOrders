namespace CollectionServiceOrders.Core.Configuration;
public static class GlobalConfig
{
    public static EmailSettingsConfigurationModel EmailConfig { get; set; }
    public static DepositInfoConfigurationModel DepositInfoConfiguration { get; set; }
    public static SignalChargeFeesConfigurationModel SignalChargeFeesConfiguration { get; set; }
    public static TemplateFileLocationConfigurationModel TemplateFileLocationConfiguration { get; set; }
}