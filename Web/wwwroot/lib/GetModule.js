import {hideAddInvoiceForm, hideUpdateInvoiceForm} from './UtilsModule';
import { deletePosition } from './DeleteModule';
import { updatePosition } from './UpdateModule';

export function showUpdateInvoiceForm(id) {

    hideAddInvoiceForm();

    // Получаем позиции для текущего документа
    getPositionsByInvoiceId(id)
        .then(positions => {
            // Заполняем поля формы обновления данными текущего документа (по id)
            const invoiceToUpdate = findInvoiceById(id, positions);
            if (invoiceToUpdate && invoiceToUpdate.positions) {
                const positionsTableBody = document.getElementById('updateInvoiceFormPositionsTable').getElementsByTagName('tbody')[0];
                document.getElementById('updateNumber').value = invoiceToUpdate.number;
                document.getElementById('updateDate').valueAsDate = new Date(invoiceToUpdate.date);
                document.getElementById('updateNote').value = invoiceToUpdate.note;

                // Очищаем таблицу позиций перед добавлением новых
                positionsTableBody.innerHTML = '';

                // Добавляем позиции в таблицу
                invoiceToUpdate.positions.forEach(position => {
                    const newRow = positionsTableBody.insertRow();
                    newRow.innerHTML = `
                            <td contenteditable="true">${position.name}</td>
                            <td contenteditable="true">${position.quantity}</td>
                            <td contenteditable="true">${position.value}</td>
                            <td>
                                <button type="button" onclick="updatePosition(${position.id})">Обновить</button>
                                <button type="button" onclick="deletePosition(${position.id})">Удалить</button>
                            </td>
                        `;
                });

                const updateButton = document.getElementById('updateButton');
                updateButton.setAttribute('data-invoice-id', id);
            }

            document.getElementById('updateInvoiceFormDiv').style.display = 'block';
        })
        .catch(error => console.error(`Ошибка при получении позиций для документа ${id}:`, error));
}

export async function getPositionsByInvoiceId(invoiceId) {
    try {
        const response = await fetch(`http://localhost/api/positions/${invoiceId}`);

        return await response.json();
    } catch (error) {
        console.error(error.message);
        throw error;
    }
}

export function findInvoiceById(id, positions) {
    const invoicesTableBody = document.getElementById('invoicesTable').getElementsByTagName('tbody')[0];
    const rows = invoicesTableBody.getElementsByTagName('tr');

    for (let i = 0; i < rows.length; i++) {
        const row = rows[i];
        const dataInvoiceId = row.getAttribute('data-invoice-id');

        if (dataInvoiceId && parseInt(dataInvoiceId) === id) {
            const cells = row.getElementsByTagName('td');
            const number = cells[0].textContent;
            const date = cells[1].textContent;
            const totalAmount = cells[2].textContent;
            const note = cells[3].textContent;

            return {
                id: id,
                number: number,
                date: date,
                totalAmount: totalAmount,
                note: note,
                positions: positions  // Добавляем позиции к объекту инвойса
            };
        }
    }

    return null;
}

export async function showPositions(invoiceId) {
    fetch(`http://localhost/api/positions/${invoiceId}`)
        .then(response => response.json())
        .then(data => {
            const positionsTableBody = document.getElementById('positionsTable').getElementsByTagName('tbody')[0];
            positionsTableBody.innerHTML = '';

            data.forEach(position => {
                const row = positionsTableBody.insertRow();
                row.innerHTML = `<td>${position.name}</td><td>${position.quantity}</td><td>${position.value}</td>`;
            });
        })
        .catch(error => console.error(`Ошибка загрузки позиций документа ${invoiceId}:`, error));
}

export function showAddInvoiceForm() {
    hideUpdateInvoiceForm();  // Закрыть форму обновления, если она открыта
    const addInvoiceForm = document.getElementById('addInvoiceForm');
    addInvoiceForm.style.display = addInvoiceForm.style.display === 'none' ? 'block' : 'none';
    document.getElementById('invoiceForm').reset();
}