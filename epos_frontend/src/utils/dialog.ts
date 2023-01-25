import { createPromiseDialog } from "vue-promise-dialogs"
import ComPopup from '@/views/ComPopup.vue';
import ComPopup2 from '@/views/ComPopup2.vue';
import ComPrintPreview from '@/components/ComPrintPreview.vue'
import SaleDetail from '@/views/receipt_list/components/ComSaleDetail.vue';
import ComConfirm from '@/components/ComConfirm.vue';
import ComInputNumber from '@/components/ComInputNumber.vue';
import ComSelectSaleOrder from '@/views/sale/components/ComSelectSaleOrder.vue';

interface params {
    doctype?:String,
    name?:String,
    text?: String,
    title?: String, 
    report?:String, 
    print:{
            type:Boolean,
            default:0
        },
    data:Object,//use in comSelectSaleOrder
    table:Object //use in comSelectSaleOrder
}

export  const comPopupDialog = createPromiseDialog<params, object>(ComPopup);
export  const comPopup2Dialog = createPromiseDialog<params, object>(ComPopup2);
export  const saleDetailDialog = createPromiseDialog<params, object>(SaleDetail);
export  const printPreviewDialog = createPromiseDialog<params, object>(ComPrintPreview);
export  const confirm = createPromiseDialog<params, object>(ComConfirm);
export  const inputNumberDialog = createPromiseDialog<params, object>(ComInputNumber);
export  const selectSaleOrderDialog = createPromiseDialog<params, object>(ComSelectSaleOrder);

