namespace CollectionServiceOrders.Core.Configuration;

public class SignalChargeFeesConfigurationModel
{
    public List<SignalNumberModel> SignalNumbers { get; set; }
}

public class SignalNumberModel
{
    public string SignalNumber { get; set; }
    public string Description { get; set; }
    public string FeeString { get; set; }
    public decimal Fee => Convert.ToDecimal(FeeString);
}