namespace CollectionServiceOrders.Core;
public class CurrencyFormatProvider : IFormatProvider
{
    public object GetFormat(Type formatType) => "c";
}