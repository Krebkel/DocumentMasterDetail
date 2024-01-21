// Функция добавления строки в таблицу документов
let selectedInvoiceId = 0;

function createDocumentTableRow(invoiceId, number, date, sum, note) {
    const invoiceIdAttribute = invoiceId ? `data-invoice-id="${invoiceId}"` : '';
    const selectButtonStyleAttribute = invoiceId ? '' : `style="display: none;"`;

    return `<tr ${invoiceIdAttribute}>
        <th class="invoice-number" contenteditable="true" scope="row">${number}</th>
        <td class="invoice-date" contenteditable="false">
            <input type='date' class="form-control" value="${date}">
        </td>
        <td class="invoice-sum" contenteditable="false">${sum}</td>
        <td class="invoice-note" contenteditable="true">${note}</td>
        <td>
            <span class="delete-button table-remove edit">
                <button type="button" class="table-remove btn btn-danger btn-rounded btn-sm my-0">Удалить</button>
            </span>
            <span class="save-button table-save edit">
                <button type="button" class="table-save btn btn-success btn-rounded btn-sm my-0">Сохранить</button>
            </span>
            <span class="select-button table-select edit" ${selectButtonStyleAttribute}>
                <button type="button" class="table-select btn btn-info btn-rounded btn-sm my-0">Выбрать</button>
            </span>
        </td>
    </tr>`;
}

// Функция добавления строки в таблицу позиций документа
function createPositionTableRow(invoiceId, positionId, number, name, sum) {
    const invoiceIdAttribute = invoiceId ? `data-invoice-id="${invoiceId}"` : '';
    const positionIdAttribute = positionId ? `data-position-id="${positionId}"` : '';

    return `<tr ${invoiceIdAttribute} ${positionIdAttribute}>
        <th class="position-number" contenteditable="true" scope="row">${number}</th>
        <td class="position-name" contenteditable="true">${name}</td>
        <td class="position-sum" contenteditable="true">
            <input type='number' class="form-control" value="${sum}">
        </td>
        <td>
            <span class="delete-button table-remove edit">
                <button type="button" class="table-remove btn btn-danger btn-rounded btn-sm my-0">Удалить</button>
            </span>
            <span class="save-button table-save edit">
                <button type="button" class="table-save btn btn-success btn-rounded btn-sm my-0">Сохранить</button>
            </span>
        </td>
    </tr>`;
}

// вызывается, когда страница загрузилась
$(document).ready(function () {
    // Прячем кнопку добавления документа, пока не загрузим документы с сервера
    $(".invoice-table .table-add").hide();

    // Таблицу позиций тоже прячем
    $(".position-table").hide();

    // При загрузке страницы отправляем GET запрос на сервер
    $.ajax({
        type: "GET",
        url: "http://localhost/api/invoices",
        dataType: "json",
        success: function (response) {
            const invoices = Array.isArray(response) ? response : [response];
            invoices.forEach(function (invoice) {                
                const formattedDate = new Date(invoice.date).toISOString().split('T')[0];
                const newRow= createDocumentTableRow(invoice.id, invoice.number, formattedDate, invoice.sum, invoice.note);
                $(".invoice-table tbody").append(newRow);
            });

            // Если доки загрузились, покажем кнопку добавления нового дока
            $(".invoice-table .table-add").show();
        },
        error: function (error) {
            console.error("Error fetching invoices:", error);
        }
    });
});

// обработчик событий нажатия на кнопку "добавить документ"
$(".invoice-table .table-add").on("click", () => {
    $(".invoice-table tbody").append(createDocumentTableRow(null, "введите номер", null, 0, "введите примечание"));
});

// обработчик событий нажатия на кнопку "удалить документ"
$(".invoice-table tbody").on("click", ".delete-button", function () {
    const currentRow = $(this).closest("tr");
    const invoiceId = currentRow.attr("data-invoice-id");
    if (invoiceId) {
        $.ajax({
            type: "DELETE",
            url: `http://localhost/api/invoices/${invoiceId}`,
            success: function () {
                // Удаляем строку только если сервак ответил 200
                currentRow.remove();
                
                if(invoiceId === selectedInvoiceId)
                    $(".position-table").hide();
            },
            error: function (error) {
                console.error("Error deleting invoice:", error);
            }
        });
    } else {
        // Если у строки еще нет идентификатора инвойса, просто ее удаляем (строка еще не была сохранена на сервере)
        currentRow.remove();
    }
});

// обработчик событий нажатия на кнопку "сохранить документ"
$(".invoice-table tbody").on("click", ".save-button", function () {
    const currentRow = $(this).closest("tr");
    const selectButton = currentRow.find('.select-button.table-select.edit');

    const invoiceId = currentRow.attr("data-invoice-id");
    const number = currentRow.find(".invoice-number").text().trim();
    if(!number) {
        alert(`Заполните номер!`);
        return;
    }
    const dateInputValue = currentRow.find(".invoice-date input").val();
    if(!dateInputValue) {
        alert(`Заполните дату!`);
        return;
    }
    const date= new Date(dateInputValue).toISOString();
    const note = currentRow.find(".invoice-note").text().trim();

    const data = {
        Number: number,
        Date: date,
        Note: note
    };

    const requestType = invoiceId ? "PUT" : "POST";
    const url = invoiceId ? `http://localhost/api/invoices/${invoiceId}` : "http://localhost/api/invoices";

    jQuery.ajax({
        type: requestType,
        url: url,
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            if (response && response.id) {
                currentRow.attr("data-invoice-id", response.id);
                selectButton.show();
            } else {
                alert(`Ошибка! С сервера вернулись невалидные данные: нет идентификатора документа`);
            }
            alert(`Документ успешно ${requestType === "PUT" ? "обновлен" : "создан"}`);
            
        },
        error: function (error) {
            const json = JSON.parse(error.responseText);
            console.error(`Error ${requestType.toLowerCase()}ing invoice:`, json);
            alert(`Ошибка: ${json.statusMessage}`);
        }
    });
});

// обработчик событий нажатия на кнопку "выбрать документ"
$(".invoice-table tbody").on("click", ".select-button", function () {
    const currentRow = $(this).closest("tr");
    selectedInvoiceId = currentRow.attr("data-invoice-id");
    
    // Отправка запроса на сервер с id выбранного документа
    $.ajax({
        type: "GET",
        url: `http://localhost/api/positions/${selectedInvoiceId}`,
        dataType: "json",
        success: function (response) {
            const positions = Array.isArray(response) ? response : [response];
            $(".position-table tbody").empty(); // Очищаем текущие позиции в таблице

            // Добавляем новые позиции в таблицу позиций
            positions.forEach(function (position) {
                const newRow = createPositionTableRow(position.invoice.id, position.id, 
                    position.number, position.name, position.value);
                $(".position-table tbody").append(newRow);
            });

            // Показываем таблицу позиций
            $(".position-table").show();
        },
        error: function (error) {
            console.error("Error fetching positions:", error);
        }
    });
});

// ДАЛЬШЕ МЕТОДЫ ТАБЛИЦЫ ПОЗИЦИЙ --------------------------------------------------

// обработчик событий нажатия на кнопку "добавить позицию"
$(".position-table .table-add").on("click", () => {
    $(".position-table tbody").append(createPositionTableRow(selectedInvoiceId, null, 
        "введите номер", "введите имя", 0));
});

// обработчик событий нажатия на кнопку "сохранить позицию"
$(".position-table tbody").on("click", ".save-button", function () {
    const currentRow = $(this).closest("tr");
    const positionId = currentRow.attr("data-position-id");
    const number = currentRow.find(".position-number").text().trim();
    if(!number) {
        alert(`Заполните номер!`);
        return;
    }
    const name = currentRow.find(".position-name").text().trim();
    if(!name) {
        alert(`Заполните наименование!`);
        return;
    }
    
    const sum = parseInt(currentRow.find(".position-sum input").val(), 10);
    const data = {
        number: number,
        name: name,
        sum: sum
    };

    const requestType = positionId ? "PUT" : "POST";
    const url = positionId 
        ? `http://localhost/api/positions/${positionId}`
        : `http://localhost/api/positions/invoice/${selectedInvoiceId}`;

    jQuery.ajax({
        type: requestType,
        url: url,
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            if (response && response.id) {
                currentRow.attr("data-position-id", response.id);
            } else {
                alert(`Ошибка! С сервера вернулись невалидные данные: нет идентификатора позиции`);
            }
            updateInvoiceSum(selectedInvoiceId);
            alert(`Позиция успешно ${requestType === "PUT" ? "обновлена" : "создана"}`);
        },
        error: function (error) {
            const json = JSON.parse(error.responseText);
            console.error(`Error ${requestType.toLowerCase()}ing invoice:`, json);
            alert(`Ошибка: ${json.statusMessage}`);
        }
    });
});

// обработчик событий нажатия на кнопку "удалить документ"
$(".position-table tbody").on("click", ".delete-button", function () {
    const currentRow = $(this).closest("tr");
    const positionId = currentRow.attr("data-position-id");
    if (positionId) {
        $.ajax({
            type: "DELETE",
            url: `http://localhost/api/positions/${positionId}`,
            success: function () {
                updateInvoiceSum(selectedInvoiceId);
                // Удаляем строку только если сервак ответил 200
                currentRow.remove();
            },
            error: function (error) {
                console.error("Error deleting position:", error);
            }
        });
    } else {
        // Если у строки еще нет идентификатора инвойса, просто ее удаляем (строка еще не была сохранена на сервере)
        currentRow.remove();
    }
});

function updateInvoiceSum(invoiceId) {
    // Отправка запроса на сервер для получения обновленной суммы инвойса
    $.ajax({
        type: "GET",
        url: `http://localhost/api/invoices/${invoiceId}`,
        dataType: "json",
        success: function (response) {
            const sum = response.sum;

            // Находим соответствующую строку в таблице инвойсов и обновляем сумму
            const invoiceRow = $(`.invoice-table tbody tr[data-invoice-id="${invoiceId}"]`);
            invoiceRow.find(".invoice-sum").text(sum);
        },
        error: function (error) {
            console.error("Error updating invoice sum:", error);
        }
    });
}