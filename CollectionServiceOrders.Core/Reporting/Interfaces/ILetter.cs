

namespace CollectionServiceOrders.Core.Reporting;

public interface ILetter
{
    void CreateAdditionalDepositLetter(ServiceOrderModel serviceOrder, bool makeWordVisible);
    void CreateAdditionalDepositLetters(List<ServiceOrderModel> serviceOrders, bool makeWordVisible);
}