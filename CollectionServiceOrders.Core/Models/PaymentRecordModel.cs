namespace CollectionServiceOrders.Core.Models;
public class PaymentRecordModel
{
    // R = Reg. coll.; S = Special coll.; B = Both
    private readonly List<string> _paymentCodes = new() { "R", "S", "B" };

    // 88881/2 = PP24 Customer eCheck/CC, 88886/7 = Mobile Customer eCheck/CC, 88879/88880 = IVR
    private readonly List<int> _onlinePaymentBatchNumbers = new() { 88879, 88880, 88881, 88882, 88886, 88887 };

    public string MSIN50 { get; set; }
    public int MOBATC { get; set; }
    public bool PaymentReceived => !_paymentCodes.Contains(MSIN50.Trim());
    public bool OnlinePayment => _onlinePaymentBatchNumbers.Contains(MOBATC);
}