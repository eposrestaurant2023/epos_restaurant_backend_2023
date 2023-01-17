<template>
    <v-dialog v-model="open" fullscreen persistent>
        <v-card>
            <ComToolbar @onClose="onClick" :isPrint="true" :isMoreMenu="true" @onPrint="onPrint">
                <template #title>
                    Sale # : {{ sale.name }}
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
                            <tr>
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
                            <tr>
                                <td>Branch</td>
                                <td>:</td>
                                <td>{{ sale.doc.business_branch }}</td>
                            </tr>
                            <tr>
                                <td>Stock Location</td>
                                <td>:</td>
                                <td>{{ sale.doc.stock_location }}</td>
                            </tr>
                        </table>
                    </div>
                    <v-table fixed-header hover>
                        <thead class="bg-gray-200">
                            <tr>
                                <th>No</th>
                                <th>Image</th>
                                <th>Description</th>
                                <th>QTY</th>
                                <th>Unit</th>
                                <th class="text-right">Price</th>
                                <th class="text-right">Amount</th>
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
                                <th>{{ p.quantity }}</th>
                                <th>{{ p.unit }}</th>
                                <th class="text-right">{{ $filter.currency(p.price) }}</th>
                                <th class="text-right">{{ $filter.currency(p.amount) }}</th>
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
                            <tr v-if="sale.doc.sub_total !=0">
                                <td>Sub Total</td>
                                <td>:</td>
                                <td>{{ $filter.currency(sale.doc.sub_total) }}</td>
                            </tr>
                            <tr v-if="sale.doc.discount !=0">
                                <td>Discount({{ sale.doc.discount }}%)</td>
                                <td>:</td>
                                <td>{{ $filter.currency(sale.doc.sale_discount) }}</td>
                            </tr>
                            <tr v-if="sale.doc.sale_discount !=0">
                                <td>Sale Discount</td>
                                <td>:</td>
                                <td>{{ $filter.currency(sale.doc.sale_discount) }}</td>
                            </tr>
                            <tr v-if="sale.doc.total_discount !=0">
                                <td>Total Discount</td>
                                <td>:</td>
                                <td>{{ $filter.currency(sale.doc.total_discount) }}</td>
                            </tr>
                            <tr v-if="sale.doc.tax_1_amount !=0">
                                <td>Service Charge({{ sale.doc.tax_1_rate }}%)</td>
                                <td>:</td>
                                <td>{{ $filter.currency(sale.doc.tax_1_amount) }} </td>
                            </tr>
                            <tr v-if="sale.doc.tax_2_amount !=0">
                                <td>P/L Tax({{ sale.doc.tax_2_rate }}%)</td>
                                <td>:</td>   
                                <td>{{ $filter.currency(sale.doc.tax_2_amount) }} </td>
                            </tr>
                            <tr v-if="sale.doc.tax_3_amount !=0">
                                <td>VAT({{ sale.doc.tax_3_rate }}%)</td>
                                <td>:</td>
                                <td>{{ $filter.currency(sale.doc.tax_3_amount) }} </td>
                            </tr>
                            <tr v-if="sale.doc.total_tax !=0">
                                <td>Total Tax</td>
                                <td>:</td>
                                <td>{{ $filter.currency(sale.doc.total_tax) }}</td>
                            </tr>
                            <tr v-if="sale.doc.grand_total !=0">
                                <td>Grand Total</td>
                                <td>:</td>
                                <td>{{ $filter.currency(sale.doc.grand_total) }}</td>
                            </tr>
                            <tr v-if="sale.doc.total_paid !=0">
                                <td>Total Paid</td>
                                <td>:</td>
                                <td>{{ $filter.currency(sale.doc.total_paid) }}</td>
                            </tr>
                            <tr v-if="sale.doc.balance !=0">
                                <td>Balance</td>
                                <td>:</td>
                                <td>{{ $filter.currency(sale.doc.balance) }}</td>
                            </tr>
                            <tr v-if="sale.doc.changed_amount !=0">
                                <td>Changed Amount</td>
                                <td>:</td>
                                <td>{{ $filter.currency(sale.doc.changed_amount) }}</td>
                            </tr>
                      
                        </table>
                    </div>
                </v-card>
            </v-card-text>
        </v-card>
    </v-dialog>
</template>
  
<script setup>
import { createDocumentResource,ref } from '@/plugin'
import ComToolbar from '../../components/ComToolbar.vue';
import { closeDialog } from 'vue3-promise-dialog'

const props = defineProps({ selected: String})

const open = ref(true);

const setting = JSON.parse(localStorage.getItem("setting"))

let sale = createDocumentResource({
    url: 'frappe.client.get',
    doctype: 'Sale',
    name: props.selected,
    auto: true
})
function onClick() {
    closeDialog(false);
}

function onPrint() {
    if (localStorage.getItem("is_window")) {

        window.chrome.webview.postMessage(JSON.stringify(sale));
    } else {
        alert("print on web")
    }

}
</script>

