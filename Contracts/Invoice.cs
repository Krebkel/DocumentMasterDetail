using System;
using System.Collections.Generic;

namespace Contracts;

/// <summary>
/// Документ Master
/// </summary>
public class Invoice : DatabaseEntity
{
    /// <summary>
    /// Номер документа
    /// </summary>
    public required string Number { get; set; }

    /// <summary>
    /// Дата проведения
    /// </summary>
    public required DateTimeOffset Date { get; set; }

    /// <summary>
    /// Сумма по документу
    /// </summary>
    public required decimal TotalAmount { get; set; }

    /// <summary>
    /// Примечание
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Позиции документа
    /// </summary>
    public required List<Position> Positions { get; set; }
}