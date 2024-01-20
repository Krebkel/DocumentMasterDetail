import {findInvoiceById, getPositionsByInvoiceId, showUpdateInvoiceForm} from "./GetModule";
import {convertMoscowTimeToUtc, fetchInvoices, hideUpdateInvoiceForm} from "./UtilsModule";

export async function updatePosition(positionId) {
    try {
        const positionsTableBody = document.getElementById('updateInvoiceFormPositionsTable').getElementsByTagName('tbody')[0];
        const positionRow = Array.from(positionsTableBody.getElementsByTagName('tr')).find(row => row.contains(event.target) || row === event.target.parentNode);
        const positionName = positionRow.cells[0].textContent.trim();
        const positionQuantity = positionRow.cells[1].textContent.trim();
        const positionValue = positionRow.cells[2].textContent.trim();

        // Проверка на пустые значения или некорректные данные
        if (!positionName || !positionQuantity || !positionValue) {
            console.error('Пожалуйста, заполните все поля для обновления позиции.');
            return;
        }

        const positionData = {
            Name: positionName,
            Quantity: positionQuantity,
            Value: positionValue
        };

        const response = await fetch(`http://localhost/api/positions/${positionId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(positionData)
        });

        if (!response.ok) {
            console.error(`Ошибка при обновлении позиции ${positionId}: ${response.statusText}`);
            return;
        }

        // Обновите форму обновления документа после обновления позиции
        showUpdateInvoiceForm(parseInt(document.getElementById('updateButton').getAttribute('data-invoice-id'), 10));
    } catch (error) {
        console.error(`Ошибка при обновлении позиции ${positionId}:`, error.message);
    }
    fetchInvoices();
}

export async function updateInvoice() {
    const invoiceId = parseInt(document.getElementById('updateButton').getAttribute('data-invoice-id'), 10);

    // Fetch the current invoice data
    const positions = await getPositionsByInvoiceId(invoiceId);
    const invoiceToUpdate = findInvoiceById(invoiceId, positions);

    if (!invoiceToUpdate) {
        console.error('Invoice not found.');
        return;
    }

    const updateNumber = document.getElementById('updateNumber').value;
    const updateDate = new Date(document.getElementById('updateDate').value);
    const updateNote = document.getElementById('updateNote').value;

    // Set the date value
    document.getElementById('updateDate').valueAsDate = new Date(invoiceToUpdate.date);

    const updateData = {
        Id: invoiceId,
        Number: updateNumber,
        Date: convertMoscowTimeToUtc(updateDate),
        Note: updateNote,
        Positions: []
    };

    const positionsTableRows = document.getElementById('updateInvoiceFormPositionsTable').getElementsByTagName('tbody')[0].getElementsByTagName('tr');

    for (let i = 0; i < positionsTableRows.length; i++) {
        const positionRow = positionsTableRows[i];
        const positionName = positionRow.querySelector('input[name^="updatePositionName"]').value;
        const positionQuantity = positionRow.querySelector('input[name^="updatePositionQuantity"]').value;
        const positionPrice = positionRow.querySelector('input[name^="updatePositionPrice"]').value;

        // Создаем объект позиции и добавляем его в массив Positions
        const positionData = {
            Name: positionName,
            Quantity: positionQuantity,
            Price: positionPrice
        };

        updateData.Positions.push(positionData);
    }

    try {
        const response = await fetch(`http://localhost/api/invoices/${invoiceId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updateData)
        });

        if (!response.ok) {
            console.error('Ошибка при обновлении документа:', response.statusText);
            return;
        }

        hideUpdateInvoiceForm();
        fetchInvoices();
    } catch (error) {
        console.error('Ошибка при обновлении документа:', error.message);
    }
}