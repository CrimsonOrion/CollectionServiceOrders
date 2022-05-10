
namespace CollectionServiceOrders.Core.Processors;

public interface IDataProcessor
{
    Task<int> CreateNewServiceOrderAsync(ServiceOrderModel model);
    Task<ServiceOrderModel> DetermineDelinquentAmountAsync(ServiceOrderModel model);
    Task<IEnumerable<UserSettingsModel>> RetrieveAllUserSettingsAsync();
    Task<IEnumerable<ServiceOrderModel>> RetrieveArchivedServiceOrdersByDateAndMeterAsync(int meterNumber, DateTime serviceOrderDate);
    Task<ServiceOrderModel> RetrieveCSOBySONOAsync(ServiceOrderModel model);
    Task<IEnumerable<ServiceOrderModel>> RetrieveCSOsByDateAsync(DateTime collectionDate);
    Task<ServiceOrderModel> RetrieveDepositRecordsAsync(ServiceOrderModel model);
    Task<ServiceOrderModel> RetrievePhoneCodesAsync(ServiceOrderModel model);
    Task<IEnumerable<ServiceOrderModel>> RetrieveServiceOrdersByFilterAsync(string filter);
    Task<ServiceOrderModel> RetrieveServiceOrdersBySONumberAndServiceAsync(ServiceOrderModel model);
    UserSettingsModel RetrieveUserSettingsByUsername(UserSettingsModel model);
    Task<UserSettingsModel> RetrieveUserSettingsByUsernameAsync(UserSettingsModel model);
    bool TestOdbcConnection();
    bool TestSqlConnection();
    Task<int> UpdateArchiveFlagAllAsync(string userId);
    Task<int> UpdateArchiveFlagByIDAsync(ServiceOrderModel model);
    Task<int> UpdateCompletedByIDAsync(ServiceOrderModel model);
    Task<PaymentRecordModel> UpdatePaymentReceivedAsync(ServiceOrderModel model);
    Task<int> UpdateServiceOrderByIDAsync(ServiceOrderModel model);
    Task<int> UpdateUserSettingsByIDAsync(UserSettingsModel model);
}