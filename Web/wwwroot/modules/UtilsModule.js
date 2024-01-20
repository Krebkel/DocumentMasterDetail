import {showPositions, showUpdateInvoiceForm} from "./GetModule";
import {deleteInvoiceByNumber} from "./DeleteModule";

export function validateNumberInput(input) {
    input.addEventListener('keydown', function (event) {
        const keyCode = event.keyCode;

        if ((keyCode < 48 || keyCode > 57) && keyCode !== 190 && keyCode !== 8 && keyCode !== 46) {
            event.preventDefault();
        }

        if (keyCode === 190 && input.value.includes('.')) {
            event.preventDefault();
        }
    });
}

export function convertMoscowTimeToUtc(localDate) {
    const moscowTimezoneOffset = new Date().getTimezoneOffset(); // Получаем текущее смещение времени в минутах
    const utcDate = new Date(localDate.getTime()     - moscowTimezoneOffset * 60000);
    return utcDate.toISOString();
}

export function createStyledButton(text, backgroundColor, hoverColor) {
    const button = document.createElement('button');
    button.textContent = text;
    button.style.backgroundColor = backgroundColor;
    button.style.color = 'white';
    button.style.border = 'none';
    button.style.cursor = 'pointer';

    button.addEventListener('mouseenter', () => {
        button.style.backgroundColor = hoverColor;
    });

    button.addEventListener('mouseleave', () => {
        button.style.backgroundColor = backgroundColor;
    });

    return button;
}

export async function calculateTotalAmount(invoiceId, tableId) {
    const response = await fetch(`http://localhost/api/positions/${invoiceId}`);
    const positions = await response.json();

    let totalAmount = 0;

    positions.forEach(position => {
        totalAmount += position.value * position.quantity;
    });

    const rows = document.getElementById(tableId).getElementsByTagName('tbody')[0].getElementsByTagName('tr');
    for (let i = 0; i < rows.length; i++) {
        const row = rows[i];
        const dataInvoiceId = row.getAttribute('data-invoice-id');

        if (dataInvoiceId && parseInt(dataInvoiceId) === invoiceId) {
            const cells = row.getElementsByTagName('td');
            cells[2].textContent = totalAmount.toFixed(2);
            break;
        }
    }
}

export function fetchInvoices() {
    fetch('http://localhost/api/invoices')
        .then(response => response.json())
        .then(data => {
            const invoicesTableBody = document.getElementById('invoicesTable').getElementsByTagName('tbody')[0];
            invoicesTableBody.innerHTML = '';

            data.forEach(invoice => {
                const moscowDate = new Date(invoice.date).toLocaleString('ru-RU', { timeZone: 'Europe/Moscow' });

                const row = invoicesTableBody.insertRow();
                row.innerHTML = `<td>${invoice.number}</td><td>${moscowDate}</td><td>${invoice.totalAmount}</td><td>${invoice.note}</td>`;

                const actionsCell = row.insertCell();

                row.setAttribute('data-invoice-id', invoice.id);

                const updateButton = createStyledButton('Изменить', '#4596d1', '#025ea1');
                updateButton.addEventListener('click', () => showUpdateInvoiceForm(invoice.id));
                actionsCell.appendChild(updateButton);

                const deleteButton = createStyledButton('Удалить', '#4596d1', '#025ea1');
                deleteButton.addEventListener('click', () => {
                    const invoiceNumber = invoice.number;
                    deleteInvoiceByNumber(invoiceNumber);
                });
                actionsCell.appendChild(deleteButton);

                row.addEventListener('click', () => showPositions(invoice.id));

                calculateTotalAmount(invoice.id,"invoicesTable");
            });
        })
        .catch(error => console.error('Ошибка загрузки документов:', error));
}

export function hideUpdateInvoiceForm() {
    document.getElementById('updateInvoiceFormDiv').style.display = 'none';
}

export function hideAddInvoiceForm() {
    document.getElementById('addInvoiceForm').style.display = 'none';
}