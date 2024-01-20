import {convertMoscowTimeToUtc, fetchInvoices, validateNumberInput} from './UtilsModule';
import { removePositionRow, removePositionRowToUpdateForm } from './DeleteModule';
import {showAddInvoiceForm} from "./GetModule";

export async function addInvoice(newInvoiceData) {
    const response = await fetch('http://localhost/api/invoices', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newInvoiceData)
    });

    if (!response.ok) {
        console.error('Ошибка добавления документа:', response.statusText);
        return null;
    }

    return await response.json();
}

// Добавление новой позиции на бек
export async function addPosition(newPositionData) {
    const response = await fetch('http://localhost/api/positions', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            Name: newPositionData.name,
            Quantity: newPositionData.quantity,
            Value: newPositionData.value,
            InvoiceId: newPositionData.invoiceId
        })
    });

    if (!response.ok) {
        console.error('Ошибка добавления позиции:', response.statusText);
        return null;
    }

    return await response.json();
}

export function addPositionRow() {
    
    let positionRowCounter = 0
    const positionsTableBody = document.getElementById('invoiceFormPositionsTable').getElementsByTagName('tbody')[0];

    // Проверяем, существует ли positionsTableBody
    if (!positionsTableBody) {
        console.error('Не удалось найти tbody для таблицы позиций.');
        return;
    }

    const newRow = positionsTableBody.insertRow();

    // Добавляем ячейки для каждого поля позиции
    newRow.innerHTML = `
            <td><input type="text" name="positionName_${positionRowCounter}" required></td>
            <td><input type="number" name="positionQuantity_${positionRowCounter}" oninput="validateNumberInput(this)" required></td>
            <td><input type="number" name="positionPrice_${positionRowCounter}" oninput="validateNumberInput(this)" required></td>
            <td><button type="button" onclick="removePositionRow(${positionRowCounter})">Удалить</button></td>
        `;

    positionRowCounter++;
}

export function addPositionRowToUpdateForm() {
    const positionsTableBody = document.getElementById('updateInvoiceFormPositionsTable').getElementsByTagName('tbody')[0];

    // Проверяем, существует ли positionsTableBody
    if (!positionsTableBody) {
        console.error('Не удалось найти tbody для таблицы позиций.');
        return;
    }

    const newRow = positionsTableBody.insertRow();

    // Добавляем ячейки для каждого поля позиции
    newRow.innerHTML = `
        <td><input type="text" name="updatePositionName_${positionRowCounter}" required></td>
        <td><input type="number" name="updatePositionQuantity_${positionRowCounter}" oninput="validateNumberInput(this)" required></td>
        <td><input type="number" name="updatePositionPrice_${positionRowCounter}" oninput="validateNumberInput(this)" required></td>
        <td><button type="button" onclick="removePositionRowToUpdateForm(${positionRowCounter})">Удалить</button></td>
    `;

    positionRowCounter++;
}

export async function addNewInvoice() {
    const number = document.getElementById('number').value;
    const localDate = new Date(document.getElementById('date').value);
    const note = document.getElementById('note').value;

    const newInvoiceData = {
        number: number,
        date: convertMoscowTimeToUtc(localDate),
        note: note
    };

    const addedInvoice = await addInvoice(newInvoiceData);

    if (addedInvoice) {
        const positionsContainer = document.getElementById('invoiceFormPositionsTable');
        const positionRows = positionsContainer.querySelectorAll('tbody tr');

        if (positionRows.length > 0) {
            // Обрабатываем каждую строку позиции
            for (let i = 0; i < positionRows.length; i++) {
                const positionRow = positionRows[i];
                const positionName = positionRow.querySelector('input[name^="positionName"]').value;
                const positionQuantity = positionRow.querySelector('input[name^="positionQuantity"]').value;
                const positionPrice = positionRow.querySelector('input[name^="positionPrice"]').value;

                // Создаем данные позиции
                const positionData = {
                    name: positionName,
                    quantity: positionQuantity,
                    value: positionPrice,
                    invoiceId: addedInvoice.result.id
                };

                // Добавляем данные позиции в массив
                const addedPosition = await addPosition(positionData);
                console.log(addedPosition);
            }
        }
        // Очищаем форму и контейнер строк после успешного добавления
        document.getElementById('invoiceForm').reset();
        document.getElementById('invoiceFormPositionsTable').getElementsByTagName('tbody')[0].innerHTML = '';
        showAddInvoiceForm();

        // Обновляем таблицу документов
        fetchInvoices();
    }
}