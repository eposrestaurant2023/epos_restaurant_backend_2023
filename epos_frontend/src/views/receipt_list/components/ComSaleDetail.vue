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
                            <tr v-if="sale.doc.phone_number">
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
                                <td class="pb-2 text-right">{{ sale.name }}</td>
                            </tr>
                            <tr>
                                <td>Date</td>
                                <td>:</td>
                                <td class="pb-2 text-right">{{ sale.doc.posting_date }}</td>
                            </tr>
                            <tr >
                                <td>Branch</td>
                                <td>:</td>
                                <td class="pb-2 text-right">{{ sale.doc.business_branch }}</td>
                            </tr>
                            <tr >
                                <td>Stock Location</td>
                                <td>:</td>
                                <td class="pb-2 text-right">{{ sale.doc.stock_location }}</td>
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
                                <th class="text-right" style="color: white">Amount</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(p, index) in saleProducts" :key="index">
                                <th>{{ index + 1}}</th>
                                <th class="text-center">
                                    <div class="w-10 h-10 overflow-hidden rounded-full bg-gray-200">
                                        <img :src="p.product_photo" />
                                    </div>
                                </th>
                                <td>{{ p.product_code }} - {{ p.product_name }}</td>
                                <th>{{ p.unit }}</th>
                                <th>{{ p.quantity }}</th> 
                                <th class="text-right"><CurrencyFormat :value="p.price"/></th>
                                <th class="text-right"><CurrencyFormat :value="p.amount"/></th>
                            </tr>
                        </tbody>
                    </v-table>   
                    <div>
                        <table class="ml-auto">
                            <tr v-if="sale.doc.total_quantity !=0">
                                <td class="pb-2">Total Quantity</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right">{{ sale.doc.total_quantity }}</td>
                            </tr>
                            <tr>
                                <td class="pb-2">Sub Total</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.doc.sub_total"/></td>
                            </tr>  
                            <tr v-if="sale.doc.product_discount">
                                <td class="pb-2">
                                    <span v-if="sale.doc.sale_discount">Product</span>
                                    <span> Discount</span>
                                </td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.doc.product_discount"/></td>
                            </tr>
                            <tr v-if="sale.doc.sale_discount">
                                <td class="pb-2">
                                    <span v-if="sale.doc.product_discount">Sale</span>
                                    <span>Discount</span> ({{sale.doc.discount}}%)</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.doc.sale_discount"/></td>
                            </tr>
                            <tr v-if="sale.doc.total_discount != sale.doc.product_discount && sale.doc.total_discount != sale.doc.sale_discount">
                                <td class="pb-2">Total Discount</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.doc.total_discount"/></td>
                            </tr>
                            <tr v-if="sale.doc.tax_1_amount ">
                                <td class="pb-2">{{ setting.tax_1_name }} ({{ sale.doc.tax_1_rate }}%)</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.doc.tax_1_amount"/></td>
                            </tr>
                            <tr v-if="sale.doc.tax_2_amount " >
                                <td class="pb-2">{{ setting.tax_2_name }} ({{ sale.doc.tax_2_rate }}%)</td>
                                <td class="pb-2 px-2 px-2">:</td>   
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.doc.tax_2_amount"/></td>
                            </tr>
                            <tr v-if="sale.doc.tax_3_amount ">
                                <td class="pb-2">{{ setting.tax_3_name }} ({{ sale.doc.tax_3_rate }}%)</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.doc.tax_3_amount"/></td>
                            </tr>
                            <tr v-if="sale.doc.total_tax !=0 && sale.doc.total_tax > 1">
                                <td class="pb-2">Total Tax</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.doc.total_tax"/></td>
                            </tr>
                            <tr>
                                <td class="pb-2">Grand Total</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.doc.grand_total"/></td>
                            </tr>
                            <tr v-for="d in sale.doc.payment" :key="d.name">
                                <td class="pb-2">Paid by {{ d.payment_type }}</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :currency="d.currency" :value="d.input_amount"/></td>
                            </tr>
                            <tr v-if="sale.doc.payment.length > 1">
                                <td class="pb-2">Total Paid</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.doc.total_paid"/></td>
                            </tr>                   
                            <tr v-if="sale.doc.balance !=0">
                                <td class="pb-2">Balance</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.doc.balance"/></td>
                            </tr>
                            <tr v-if="sale.doc.changed_amount !=0">
                                <td class="pb-2">Changed Amount</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.doc.changed_amount"/></td>
                            </tr>
                        </table>
                    </div>
                </v-card>
            </v-card-text>
        </v-card>
    </v-dialog>
   
</template>
  
<script setup>

import {  createDocumentResource,ref,inject, computed } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import ComPrintButton from '@/components/ComPrintButton.vue';
import { printPreviewDialog } from '@/utils/dialog';
import Enumerable from 'linq';
const gv = inject("$gv")
const props = defineProps({
    params: {
        type: Object,
        required: true,
    },
})
const emit = defineEmits(["resolve","reject"])
const setting = computed(()=>{
    return JSON.parse(localStorage.getItem('setting'))
})

 
const open = ref(true);


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
            setting:gv.setting.pos_setting,
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
const saleProducts = computed(() => {
    return Enumerable.from(sale.doc.sale_products).groupBy(
        "{product_code:$.product_code,portion:$.portion,modifiers:$.modifiers,product_name:$.product_name,product_name_kh:$.product_name_kh,unit:$.unit,discount:$.discount,discount_type:$.discount_type,product_photo:$.product_photo,is_free:$.is_free,price:$.price}",
        "{quantity:$.quantity, amount: $.amount}",
        "{product_code:$.product_code,product_name:$.product_name,product_name_kh:$.product_name_kh,modifiers_price:$.modifiers_price,portion:$.portion,modifiers:$.modifiers,unit:$.unit,discount:$.discount,discount_type:$.discount_type,product_photo:$.product_photo,price:$.price,is_free:$.is_free, quantity: $$.sum('$.quantity'), amount: $$.sum('$.amount')}",
        "$.product_code+','+$.unit+','+$.quantity+','+$.is_free+','+$.portion+','+$.modifiers+',' + $.discount + ',' + $.discount_type + ',' + $.price + ',' + $.modifiers_price"
        ).toArray()
})
</script>

