namespace ElasticSearchCRUDExampleApi.ElasticSearch;

public class OperationResult
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public OperationStatus Status { get; set; }

    public OperationResult(string message)
    {
        Message = message;
        Success = false;
        Status = OperationStatus.Error;
    }

    public OperationResult(Exception exception)
    {
        Message = exception.ToString();
        Success = false;
        Status = OperationStatus.Error;
    }

    public OperationResult()
    {
        Success = true;
        Status = OperationStatus.Success;
    }

    private OperationResult(OperationStatus status, string? message)
    {
        Success = false;
        Message = message;
        Status = status;
    }

    public static OperationResult NotFoundNothingToUpdate(string message) =>
        new OperationResult(OperationStatus.NotFoundNothingToUpdate, message);
}


public class OperationResult<T>
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public T? Result { get; set; }

    public OperationResult(string message)
    {
        Message = message;
        Success = false;
    }

    public OperationResult(Exception exception)
    {
        Message = exception.ToString();
        Success = false;
    }

    public OperationResult(T result)
    {
        Success = true;
        Result = result;
    }
}


public enum OperationStatus
{
    Success = 0,
    Error = 1,
    NotFoundNothingToUpdate = 2,
}