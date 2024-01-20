import {showUpdateInvoiceForm} from "./GetModule";
import {fetchInvoices} from "./UtilsModule";

export async function deletePosition(positionId) {
    try {
        const response = await fetch(`http://localhost/api/positions/${positionId}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            console.error(`Ошибка при удалении позиции ${positionId}: ${response.statusText}`);
            return;
        }

        showUpdateInvoiceForm(parseInt(document.getElementById('updateButton').getAttribute('data-invoice-id'), 10));
    } catch (error) {
        console.error(`Ошибка при удалении позиции ${positionId}:`, error.message);
    }
}

export function removePositionRow(rowNumber) {
    const positionsTableBody = document.getElementById('positionsTable').getElementsByTagName('tbody')[0];
    positionsTableBody.deleteRow(rowNumber);
}

export async function deleteInvoiceByNumber(number) {
    const response = await fetch(`http://localhost/api/invoices/${encodeURIComponent(number)}`, {
        method: 'DELETE'
    });

    if (!response.ok) {
        console.error('Error deleting invoice:', response.statusText);
        return false;
    }

    fetchInvoices();  // После удаления обновляем таблицу документов
    return true;
}

export function removePositionRowToUpdateForm(rowNumber) {
    const positionsTableBody = document.getElementById('updateInvoiceFormPositionsTable').getElementsByTagName('tbody')[0];
    positionsTableBody.deleteRow(rowNumber);
}