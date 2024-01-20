import { addInvoice, addPosition, addPositionRowToUpdateForm, addNewInvoice } from './lib/AddModule.js';
import { deletePosition, removePositionRowToUpdateForm } from './lib/DeleteModule.js';
import { validateNumberInput, createStyledButton, calculateTotalAmount, fetchInvoices, hideUpdateInvoiceForm, hideAddInvoiceForm} from './lib/UtilsModule.js';
import { showUpdateInvoiceForm, getPositionsByInvoiceId, findInvoiceById, showPositions, showAddInvoiceForm } from './lib/GetModule.js';


fetchInvoices();