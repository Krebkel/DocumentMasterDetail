namespace ErrorLogs.Requests;

public class AddErrorLogApiRequest
{
    /// <summary>
    /// Идентификатор лога
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Время ошибки
    /// </summary>
    public required DateTime Date { get; set; }

    /// <summary>
    /// Примечание
    /// </summary>
    public required string Note { get; set; }
}