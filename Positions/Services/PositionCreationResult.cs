using Contracts;

namespace Positions.Services;

public class PositionCreationResult
{
    public required bool IsSuccess { get; init; }

    public string? StatusMessage { get; init; }

    public Position Result { get; init; }

    public static PositionCreationResult Error(string message)
    {
        return new PositionCreationResult
        {
            IsSuccess = false,
            StatusMessage = message
        };
    }

    public static PositionCreationResult Success(Position result, string? message = null)
    {
        return new PositionCreationResult
        {
            IsSuccess = true,
            Result = result,
            StatusMessage = message
        };
    }
}