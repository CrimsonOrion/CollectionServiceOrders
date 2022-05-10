
namespace CollectionServiceOrders.Core.Reporting;

public interface IWorksheet
{
    void CreateAdditionalDepositWorksheet(List<ServiceOrderModel> serviceOrders, bool isExcelVisible = true, CancellationToken token = default);
    void ExportArchivedCutoffs(List<ServiceOrderModel> serviceOrders, bool isExcelVisible = true, CancellationToken token = default);
    void ExportCurrentCutoffs(List<ServiceOrderModel> serviceOrders, bool isExcelVisible = true, CancellationToken token = default);
}