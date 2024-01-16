namespace Contracts;

/// <summary>
/// Ошибка
/// </summary>
public class ErrorLog : DatabaseEntity
{
    /// <summary>
    /// Время ошибки
    /// </summary>
    public DateTime? Date { get; set; }

    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; }
}