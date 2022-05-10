namespace CollectionServiceOrders.Core.Models;
public class DepositRecordModel
{
    public double XADAMT { get; set; } = 0;
    public double XAADJA { get; set; } = 0;
    public double XADARF { get; set; } = 0;
    public DateTime XADDAT { get; set; }
    public double InterestTotal { get; set; } = 0;
}