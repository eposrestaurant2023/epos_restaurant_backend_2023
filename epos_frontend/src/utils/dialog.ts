import { createPromiseDialog } from "vue-promise-dialogs"
import ComPopup from '@/views/ComPopup.vue';
import ComPopup2 from '@/views/ComPopup2.vue';
import ComPrintPreview from '@/components/ComPrintPreview.vue'
import SaleDetail from '@/views/receipt_list/components/ComSaleDetail.vue';
import CustomerDetail from '@/views/customer/CustomerDetail.vue';
import ComConfirm from '@/components/ComConfirm.vue';
import ComAuthorize from '@/components/ComAuthorize.vue';
import ComNote from '@/components/ComNote.vue';
import ComModelKeyboard from '@/components/form/ComModalKeyboard.vue';
import ComSelectSaleOrder from '@/views/sale/components/ComSelectSaleOrder.vue';
import ComSaleProductNoteModal from '@/views/sale/components/ComSaleProductNoteModal.vue';
import ComAddModifier from '@/views/sale/components/ComAddModifier.vue';
import ComAddCustomer from '@/views/customer/ComAddCustomer.vue';
import ComConfirmBackToTableLayout from '@/views/sale/components/ComConfirmBackToTableLayout.vue';
import ComSearchCustomer from '@/views/sale/components/ComSearchCustomer.vue';
import ComScanCustomerCode from '@/views/sale/components/ComScanCustomerCode.vue';
import ComSaleProductDiscountModal from '@/views/sale/components/ComSaleProductDiscountModal.vue';
import ComPayment from '@/views/sale/components/ComPayment.vue';
import ComOpenMenu from '@/views/sale/components/ComOpenMenu.vue';
import ComViewBillModel from '@/views/sale/components/ComViewBillModel.vue';
import ComSmallViewSaleProductListModel from '@/views/sale/components/mobile_screen/ComSmallViewSaleProductListModel.vue';
import ComSmallCurrencyPrefineModel from '@/views/sale/components/mobile_screen/ComSmallCurrencyPrefineModel.vue';
import ComChangeTable from '@/views/sale/components/ComChangeTable.vue';
import ComChangePriceRuleModal from '@/views/sale/components/ComChangePriceRuleModal.vue';
import ComChangePOSMenuModal from '@/views/sale/components/ComChangePOSMenuModal.vue';
import ComSearchSaleModal from '@/views/sale/components/ComSearchSaleModal.vue'
import ComChangeSaleTypeModal from '@/views/sale/components/ComChangeSaleTypeModal.vue'
import ComAddCashDrawerModal from '@/views/cash_drawer/components/ComAddCashDrawerModal.vue'
import ComChangeTableSelectSaleOrder from '@/views/sale/components/ComChangeTableSelectSaleOrder.vue'

interface params {
    doctype?:String,
    name?:String,
    text?: String,
    title?: String,
    type?: String,
    report?:String, 
    value?:String,
    print:{
            type:Boolean,
            default:0
        },
    data:Object,//use in comSelectSaleOrder
    table:Object, //use in comSelectSaleOrder
    permissionCode:String
}

export  const comPopupDialog = createPromiseDialog<params, object>(ComPopup);
export  const comPopup2Dialog = createPromiseDialog<params, object>(ComPopup2);
export  const saleDetailDialog = createPromiseDialog<params, object>(SaleDetail);
export  const printPreviewDialog = createPromiseDialog<params, object>(ComPrintPreview);
export  const confirm = createPromiseDialog<params, object>(ComConfirm);
export  const confirmDialog = createPromiseDialog<params, object>(ComConfirm);
export  const keyboardDialog = createPromiseDialog<params, object>(ComModelKeyboard);
export  const selectSaleOrderDialog = createPromiseDialog<params, object>(ComSelectSaleOrder);
export  const addModifierDialog = createPromiseDialog<params, object>(ComAddModifier);
export  const customerDetailDialog = createPromiseDialog<params, object>(CustomerDetail);
export  const addCustomerDialog = createPromiseDialog<params, object>(ComAddCustomer);
export  const confirmBackToTableLayout = createPromiseDialog<params, object>(ComConfirmBackToTableLayout);
export  const searchCustomerDialog = createPromiseDialog<params, object>(ComSearchCustomer);
export  const saleProductNoteModalDialog = createPromiseDialog<params, object>(ComSaleProductNoteModal);
export  const scanCustomerCodeDialog= createPromiseDialog<params, object>(ComScanCustomerCode);
export  const authorizeDialog= createPromiseDialog<params, object>(ComAuthorize);
export  const noteDialog= createPromiseDialog<params, object>(ComNote);
export  const saleProductDiscountDialog = createPromiseDialog<params, object>(ComSaleProductDiscountModal);
export  const paymentDialog = createPromiseDialog<params, object>(ComPayment);
export  const openMenuDialog = createPromiseDialog<params, object>(ComOpenMenu);
export  const viewBillModelModel = createPromiseDialog<params, object>(ComViewBillModel);
export  const smallViewSaleProductListModel = createPromiseDialog<params, object>(ComSmallViewSaleProductListModel);
export  const smallCurrencyPrefineModel = createPromiseDialog<params, object>(ComSmallCurrencyPrefineModel);
export  const changeTableDialog = createPromiseDialog<params, object>(ComChangeTable);
export  const changePriceRuleDialog = createPromiseDialog<params, object>(ComChangePriceRuleModal);
export  const searchSaleDialog = createPromiseDialog<params, object>(ComSearchSaleModal);
export  const changeSaleTypeModalDialog = createPromiseDialog<params, object>(ComChangeSaleTypeModal);
export  const addCashDrawerModalDialog = createPromiseDialog<params, object>(ComAddCashDrawerModal);
export  const changeTableSelectSaleOrderDialog = createPromiseDialog<params, object>(ComChangeTableSelectSaleOrder);
export  const changePOSMenuDialog = createPromiseDialog<params, object>(ComChangePOSMenuModal);

