namespace CollectionServiceOrders.Core.ErrorHandler;

public interface IErrorHandler
{
    Task<string> ReportErrorAsync(Exception ex);
}