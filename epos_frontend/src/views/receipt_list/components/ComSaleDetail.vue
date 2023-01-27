<template>
    <v-dialog v-model="open" fullscreen>
        <v-card>
            <ComToolbar @onClose="onClick"   :isMoreMenu="true" >
                <template #title>
                    Sale # : {{ sale.name }}
                </template>
                <template #action>
                    <ComPrintButton doctype="Sale" @onPrint="onPrint"/>
                </template>
                <template #more_menu>
                    <v-list density="compact">
                        <v-list-item>
                            <template v-slot:prepend>
                                <v-icon color="error">mdi-delete</v-icon>
                            </template>
                            <v-list-item-title>Delete</v-list-item-title>
                        </v-list-item>
                    </v-list>
                </template>
            </ComToolbar>
            <v-card-text v-if="sale.doc">
                <v-card max-width="960" class="mx-auto my-0 pa-4">
                    <div class="float-sm-left">
                        <table class="tbl-list">
                            <tr >
                                <td>Customer Code</td>
                                <td>:</td>
                                <td>{{ sale.doc.customer }}</td>
                            </tr>
                            <tr >
                                <td>Customer Name</td>
                                <td>:</td>
                                <td>{{ sale.doc.customer_name }}</td>
                            </tr>
                            <tr v-if="sale.doc.phone_number > 0">
                                <td>Phone Number</td>
                                <td>:</td>
                                <td>{{ sale.doc.phone_number }}</td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table class="tbl-list ml-auto tbl-list-right">
                            <tr>
                                <td>Sale #</td>
                                <td>:</td>
                                <td>{{ sale.name }}</td>
                            </tr>
                            <tr>
                                <td>Date</td>
                                <td>:</td>
                                <td>{{ sale.doc.posting_date }}</td>
                            </tr>
                            <tr >
                                <td>Branch</td>
                                <td>:</td>
                                <td>{{ sale.doc.business_branch }}</td>
                            </tr>
                            <tr >
                                <td>Stock Location</td>
                                <td>:</td>
                                <td>{{ sale.doc.stock_location }}</td>
                            </tr>
                        </table>
                    </div>
                    <v-table class="bg">
                        <thead class="bg-blue-400">
                            <tr>
                                <th style="color: white">No</th>
                                <th style="color: white">Image</th>
                                <th style="color: white">Description</th>
                                <th style="color: white">Unit</th>
                                <th style="color: white">QTY</th>
                                <th class="text-right" style="color: white">Price</th>
                                <!-- <th class="text-right" style="color: white">Discount</th> -->
                                <th class="text-right" style="color: white">Amount</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="p in sale.doc.sale_products">
                                <th>{{ p.idx }}</th>
                                <th class="text-center">
                                    <div class="w-10 h-10 overflow-hidden rounded-full bg-gray-200">
                                        <img :src="p.product_photo" />
                                    </div>
                                </th>
                                <td>{{ p.product_code }} - {{ p.product_name }}</td>
                                <th>{{ p.unit }}</th>
                                <th>{{ p.quantity }}</th> 
                                <th class="text-right"><CurrencyFormat :value="p.price"/></th>
                                <!-- <th class="text-right"><CurrencyFormat :value="sale.doc.total_discount"/></th> -->
                                <th class="text-right"><CurrencyFormat :value="p.amount"/></th>
                            </tr>
                        </tbody>
                    </v-table>   
                    <div>
                        <table class="ml-auto">
                            <tr v-if="sale.doc.total_quantity !=0">
                                <td>Total Quantity</td>
                                <td>:</td>
                                <td>{{ sale.doc.total_quantity }}</td>
                            </tr>
                            <tr>
                                <td>Sub Total</td>
                                <td>:</td>
                                <td><CurrencyFormat :value="sale.doc.sub_total"/></td>
                            </tr>  
                            <tr v-if="sale.doc.product_discount > 0">
                                <td>
                                    <span v-if="sale.doc.sale_discount > 0 ">Product</span>
                                    <span> Discount</span>
                                </td>
                                <td>:</td>
                                <td><CurrencyFormat :value="sale.doc.product_discount"/></td>
                            </tr>
                            <tr v-if="sale.doc.sale_discount > 0">
                                <td>
                                    <span v-if="sale.doc.product_discount > 0 ">Sale</span>
                                    <span>Discount</span>({{sale.doc.discount}}%)</td>
                                <td>:</td>
                                <td><CurrencyFormat :value="sale.doc.sale_discount"/></td>
                            </tr>
                            <tr v-if="sale.doc.total_discount != sale.doc.product_discount && sale.doc.total_discount != sale.doc.sale_discount">
                                <td>Total Discount</td>
                                <td>:</td>
                                <td><CurrencyFormat :value="sale.doc.total_discount"/></td>
                            </tr>
                            <tr v-if="sale.doc.tax_1_amount > 0">
                                <td>Service Charge({{ sale.doc.tax_1_rate }}%)</td>
                                <td>:</td>
                                <td><CurrencyFormat :value="sale.doc.tax_1_amount"/></td>
                            </tr>
                            <tr v-if="sale.doc.tax_2_amount > 0" >
                                <td>P/L Tax({{ sale.doc.tax_2_rate }}%)</td>
                                <td>:</td>   
                                <td><CurrencyFormat :value="sale.doc.tax_2_amount"/></td>
                            </tr>
                            <tr v-if="sale.doc.tax_3_amount > 0">
                                <td>VAT({{ sale.doc.tax_3_rate }}%)</td>
                                <td>:</td>
                                <td><CurrencyFormat :value="sale.doc.tax_3_amount"/></td>
                            </tr>
                            <tr v-if="sale.doc.total_tax !=0 && sale.doc.total_tax > 1">
                                <td>Total Tax</td>
                                <td>:</td>
                                <td><CurrencyFormat :value="sale.doc.total_tax"/></td>
                            </tr>
                            <tr>
                                <td>Grand Total</td>
                                <td>:</td>
                                <td><CurrencyFormat :value="sale.doc.grand_total"/></td>
                            </tr>
                            <tr v-for="d in sale.doc.payment" :key="d.name">
                                <td>Paid by {{ d.payment_type }}</td>
                                <td>:</td>
                                <td><CurrencyFormat :currency="d.currency" :value="d.input_amount"/></td>
                            </tr>
                            <tr v-if="sale.doc.payment.length > 1">
                                <td>Total Paid</td>
                                <td>:</td>
                                <td><CurrencyFormat :value="sale.doc.total_paid"/></td>
                            </tr>                   
                            <tr v-if="sale.doc.balance !=0">
                                <td>Balance</td>
                                <td>:</td>
                                <td><CurrencyFormat :value="sale.doc.balance"/></td>
                            </tr>
                            <tr v-if="sale.doc.changed_amount !=0">
                                <td>Changed Amount</td>
                                <td>:</td>
                                <td><CurrencyFormat :value="sale.doc.changed_amount"/></td>
                            </tr>
                        </table>
                    </div>
                </v-card>
            </v-card-text>
        </v-card>
    </v-dialog>
   
</template>
  
<script setup>

import { useStore, createDocumentResource,ref } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import ComPrintButton from '@/components/ComPrintButton.vue';
import { printPreviewDialog } from '@/utils/dialog';

const props = defineProps({
    params: {
        type: Object,
        required: true,
    },
})
const emit = defineEmits(["resolve","reject"])

const store = useStore();

 
const open = ref(true);

const setting = JSON.parse(localStorage.getItem("setting"))

let sale = createDocumentResource({
    url: 'frappe.client.get',
    doctype: 'Sale',
    name: props.params.name,
    auto: true
})
 
function onClick() {
    emit('reject',false);
}

async function onPrint(r) {
   
    if(r.pos_receipt_file_name &&  localStorage.getItem("is_window") ){
        let data = {
            action:"print_receipt",
            print_setting:r,
            setting:store.state.setting.pos_setting,
            sale:sale.doc
        }
        window.chrome.webview.postMessage(JSON.stringify(data));
    }else {
    
        await printPreviewDialog({ 
            title: "Sale #: "  + sale.doc.name,
            doctype: "Sale", 
            name:sale.doc.name,
            "report": r.name,
            print:true
        });
      
 
    }

}
</script>

