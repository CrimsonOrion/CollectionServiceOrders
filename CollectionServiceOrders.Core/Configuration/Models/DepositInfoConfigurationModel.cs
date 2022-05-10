namespace CollectionServiceOrders.Core.Configuration;

public class DepositInfoConfigurationModel
{
    public decimal MinimumDepositElectric { get; set; }
    public decimal MinimumDepositWater { get; set; }
    public decimal MinimumBillElectric { get; set; }
    public decimal MinimumBillWater { get; set; }
    public decimal MinimumAdditionalDeposit { get; set; }
}