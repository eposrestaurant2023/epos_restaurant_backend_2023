
<template>
    <ComModal  :fullscreen="mobile" @onClose="onClose" width="1200px" :hideOkButton="true" :hideCloseButton="true">
        <template #title>
            Table No: {{ params.table.tbl_no }}
        </template>
        <template #content>
            <ComLoadingDialog v-if="isLoading" />
            <ComPlaceholder :is-not-empty="params.data.length > 0">
                <v-row class="!-m-1">
                    <v-col class="!p-0" cols="12" md="6" v-for="(s, index) in params.data" :key="index">
                        <ComSaleListItem :sale="s" @click="openOrder(s)" />
                    </v-col>
                </v-row>
            </ComPlaceholder>
        </template>
        <template #action>
            <ComSelectSaleOrderAction
                :isDesktop="isDesktop"
                :is-bill-requested="isDesktop && params.data.filter(r =>r.sale_status == 'Bill Requested' ).length > 0"
                @onClose="onClose"
                @onNewOrder="onNewOrder"
                @onQuickPay="onQuickPay"
                @onPrintAllBill="onPrintAllBill"
                @onCancelPrintBill="onCancelPrintBill"
                />
        </template>
    </ComModal>
    
</template>
<script setup>
import { inject, ref, useRouter, confirmDialog,  createDocumentResource ,createResource,smallViewSaleProductListModel } from '@/plugin'
import { useDisplay } from 'vuetify'
import ComSaleListItem from './ComSaleListItem.vue';
import ComLoadingDialog from '@/components/ComLoadingDialog.vue';
import { createToaster } from "@meforma/vue-toaster";
import ComSelectSaleOrderAction from './ComSelectSaleOrderAction.vue';

const isLoading = ref(false);
const { mobile } = useDisplay()
const sale = inject("$sale")
const gv = inject("$gv")
const tableLayout = inject("$tableLayout")
const router = useRouter()
const toaster = createToaster({ position: "top" });
const isDesktop = localStorage.getItem('is_window');
const emit = defineEmits(["resolve"])
const props = defineProps({
    params: {
        type: Object,
        require: true
    }
})


async function onPrintAllBill(r) {


    if (r.pos_receipt_file_name == null) {
        toaster.warning("This receipt doest not have POS receipt file");
        return;
    }
    if (props.params.data.filter(r => r.sale_status == "Submitted").length == 0) {
        toaster.warning("All receipt are printed");
        return;
    }

    if (await confirmDialog({ title: "Print All Receipt", text: "Are you sure you want to print all receipt?" })) {



        let promises = [];
        isLoading.value = true;
        props.params.data.filter(r => r.sale_status == "Submitted").forEach(async (d) => {
            promises.push(PrintReceipt(d, r));
        });



        Promise.all(promises).then(() => {

            toaster.success("All receipts has been sent to printer successfully");
            tableLayout.getSaleList();
            isLoading.value = false;

            emit('resolve', true);

        })
    }

}

async function onQuickPay(isPrint=true) {
     if (props.params.data.filter(r => r.sale_status == "Submitted" || r.sale_status == "Bill Requested" ).length == 0) {
        toaster.warning("There is no bill to close.");
        return;
    }

    if (await confirmDialog({ title: "Quick Receipt", text: "Are you sure you want to process payment all receipt?" })) {
        isLoading.value = true;
        const promises = [];
        props.params.data.filter(r => r.sale_status == "Submitted" || r.sale_status == "Bill Requested").forEach(async (d) => {
              promises.push(submitQuickPay(d,isPrint));
            
        });

        Promise.all(promises).then(() => {

            toaster.success("All receipts has been close successfully");
            tableLayout.getSaleList();
            isLoading.value = false;

            emit('resolve', true);

        })

        
    }
}

async function submitQuickPay(d,isPrint){

    const resource = createDocumentResource({
        doctype: "Sale",
        name: d.name,

    });
    await resource.get.fetch().then(async (v)=>{
        const payment = [];
        payment.push({
            payment_type: sale.setting?.default_payment_type,
            input_amount: v.grand_total,
            amount: v.grand_total
        })
         
		await resource.setValue.submit({
            payment:payment,
            docstatus:1,
            sale_status:'Closed'
        }).then((doc)=>{
            d.sale_status = "Closed";
            d.sale_status_color = sale.setting.sale_status.find(r => r.name == 'Closed').background_color;

            const data = {
                    action: "print_receipt",
                    print_setting:  sale.setting?.default_pos_receipt,
                    setting: sale.setting?.pos_setting,
                    sale: doc
                }
            if (localStorage.getItem("is_window") == "1" && isPrint) {
                window.chrome.webview.postMessage(JSON.stringify(data));
            }
          
        })
        
	})
    


    
}


async function PrintReceipt(d, r) {
    const resource = createResource({
        url: 'frappe.client.get',
        params: {
            doctype: "Sale",
            name: d.name
        },
        async onSuccess(doc) {
            if (doc.sale_products.length > 0) {
                const data = {
                    action: "print_receipt",
                    print_setting: r,
                    setting: sale.setting?.pos_setting,
                    sale: doc
                }

                if (localStorage.getItem("is_window") == "1") {
                    window.chrome.webview.postMessage(JSON.stringify(data));
                }



            }
            d.sale_status = "Bill Requested";




        },
    });

    await resource.fetch().then(async (doc) => {
        if (doc) {

            const updateResource = createResource({
                url: 'epos_restaurant_2023.api.api.update_print_bill_requested',
                params: {
                    name: doc.name
                }
            });
            await updateResource.fetch();
        }
    });


}



async function onCancelPrintBill() {
    if (props.params.data.filter(r => r.sale_status == "Bill Requested").length == 0) {
        toaster.warning("There is no bill printed to cancel.");
        return;
    }

    gv.authorize("cancel_print_bill_required_password", "cancel_print_bill", "cancel_print_bill_required_note", "Cancel Print Bill Note").then((v) => {
        if (v) {
            isLoading.value = true;
            const promises = [];
            props.params.data.filter(r => r.sale_status == "Bill Requested").forEach(async (d) => {
                promises.push(submitCancelPrintBill(d));

            });

            Promise.all(promises).then(() => {
                toaster.success("Cancel print bill successfully");
                tableLayout.getSaleList();
                isLoading.value = false;

               
            });


        }
    })

}


async function submitCancelPrintBill(d){
    const resource = createDocumentResource({
        doctype: "Sale",
        name: d.name,
    });
    await resource.get.fetch().then(async (v)=>{
		await resource.setValue.submit({
            sale_status:'Submitted'
        }).then((data)=>{
            d.sale_status = "Submitted";
            d.sale_status_color = sale.setting.sale_status.find(r => r.name == 'Submitted').background_color;

        });
	})
}

async function openOrder(s) {
    if(mobile.value){
        await sale.LoadSaleData(s.name).then(async (v)=>{
            const result =  await smallViewSaleProductListModel ({title: s.name ? s.name : 'New Sale', data: {from_table: true}});
            
        })
    }else{
        router.push({ name: "AddSale", params: { name: s.name } });
    }
    emit('resolve', false);
}

async function onNewOrder() {
    emit('resolve', { action: "new_sale" });
}
function onClose() {
    emit("resolve", false);
}
</script>