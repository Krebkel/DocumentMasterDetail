using Contracts;

namespace Invoices.Services;

public class InvoiceCreationResult
{
    public required bool IsSuccess { get; init; }
    
    public string? StatusMessage { get; init; }

    public Invoice Result { get; init; }

    public static InvoiceCreationResult Error(string message)
    {
        return new InvoiceCreationResult
        {
            IsSuccess = false,
            StatusMessage = message
        };
    }
    
    public static InvoiceCreationResult Success(Invoice result, string? message = null)
    {
        return new InvoiceCreationResult
        {
            IsSuccess = true,
            Result = result,
            StatusMessage = message
        };
    }
}

public class InvoiceUpdateResult
{
    public required bool IsSuccess { get; init; }
    
    public string? StatusMessage { get; init; }

    public Invoice Result { get; init; }

    public static InvoiceUpdateResult Error(string message)
    {
        return new InvoiceUpdateResult
        {
            IsSuccess = false,
            StatusMessage = message
        };
    }
    
    public static InvoiceUpdateResult Success(Invoice result, string? message = null)
    {
        return new InvoiceUpdateResult
        {
            IsSuccess = true,
            Result = result,
            StatusMessage = message
        };
    }
}