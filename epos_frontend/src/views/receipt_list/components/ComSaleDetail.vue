<template>
    <ComModal @onClose="onClick" :isMoreMenu="true" :isShowBarMoreButton="true" :isPrint="true" @onPrint="onPrint" :hideOkButton="true" width="1200px">
        <template #title>
            Sale # : {{ sale.name }}
        </template>
        <template #bar_more_button>
            <v-list density="compact">
                <v-list-item>
                    <template v-slot:prepend>
                        <v-icon color="error">mdi-delete</v-icon>
                    </template>
                    <v-list-item-title>Delete</v-list-item-title>
                </v-list-item>
            </v-list>
        </template>
        <template #content>
            <div v-if="sale.doc && !mobile">
            <div class="float-sm-left">
                <table class="tbl-list">
                    <tr>
                        <td>Customer Code</td>
                        <td class="px-2">:</td>
                        <td>{{ sale.doc.customer }}</td>
                    </tr>
                    <tr>
                        <td>Customer Name</td>
                        <td class="px-2">:</td>
                        <td>{{ sale.doc.customer_name }}</td>
                    </tr>
                    <tr v-if="sale.doc.phone_number">
                        <td>Phone Number</td>
                        <td class="px-2">:</td>
                        <td>{{ sale.doc.phone_number }}</td>
                    </tr>
                </table>
            </div>
            <div>
                <table class="ml-auto tbl-list-right">
                    <tr>
                        <td>Sale #</td>
                        <td class="px-2">:</td>
                        <td class="text-right">{{ sale.name }}</td>
                    </tr>
                    <tr>
                        <td>Date</td>
                        <td class="px-2">:</td>
                        <td class="text-right">{{ sale.doc.posting_date }}</td>
                    </tr>
                    <tr>
                        <td>Branch</td>
                        <td class="px-2">:</td>
                        <td class="text-right">{{ sale.doc.business_branch }}</td>
                    </tr>
                    <tr>
                        <td>Stock Location</td>
                        <td class="px-2">:</td>
                        <td class="text-right">{{ sale.doc.stock_location }}</td>
                    </tr>
                </table>
            </div>
            <v-table class="bg">
                <thead>
                    <tr>
                        <th>No</th>
                        <th>Image</th>
                        <th>Description</th>
                        <th>Unit</th>
                        <th>QTY</th>
                        <th class="text-center">Price</th>
                        <th class="text-right">Amount</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(p, index) in saleProducts" :key="index">
                        <th>{{ index + 1 }}</th>
                        <th class="text-center">
                            <div class="w-10 h-10 overflow-hidden rounded-full bg-gray-200">
                                <img :src="p.product_photo" />
                            </div>
                        </th>
                        <th>{{ p.product_code }} - {{ p.product_name }}</th>
                        <th>{{ p.unit }}</th>
                        <th>{{ p.quantity }}</th>
                        <th class="text-right">
                            <CurrencyFormat :value="p.price" />
                        </th>
                        <th class="text-right">
                            <CurrencyFormat :value="p.amount" />
                        </th>
                    </tr>
                </tbody>
            </v-table>
            <div>
                <table class="ml-auto tbl-list-right">
                    <tr v-if="sale.doc.total_quantity">
                        <td>Total Quantity</td>
                        <td class="px-2">:</td>
                        <td class="text-right">{{ sale.doc.total_quantity }}</td>
                    </tr>
                    <tr>
                        <td>Sub Total</td>
                        <td class="px-2">:</td>
                        <td class="text-right">
                            <CurrencyFormat :value="sale.doc.sub_total" />
                        </td>
                    </tr>
                    <tr v-if="sale.doc.product_discount">
                        <td>
                            <span v-if="sale.doc.sale_discount">Product</span>
                            <span> Discount</span>
                        </td>
                        <td class="px-2">:</td>
                        <td class="text-right">
                            <CurrencyFormat :value="sale.doc.product_discount" />
                        </td>
                    </tr>
                    <tr v-if="sale.doc.sale_discount">
                        <td>
                            <span v-if="sale.doc.product_discount">Sale </span>
                            <span>Discount</span> ({{ sale.doc.discount }}%)
                        </td>
                        <td class="px-2">:</td>
                        <td class="text-right">
                            <CurrencyFormat :value="sale.doc.sale_discount" />
                        </td>
                    </tr>
                    <tr
                        v-if="sale.doc.total_discount != sale.doc.product_discount && sale.doc.total_discount != sale.doc.sale_discount">
                        <td>Total Discount</td>
                        <td class="px-2">:</td>
                        <td class="text-right">
                            <CurrencyFormat :value="sale.doc.total_discount" />
                        </td>
                    </tr>
                    <tr v-if="sale.doc.tax_1_amount">
                        <td>{{ setting.tax_1_name }} ({{ sale.doc.tax_1_rate }}%)</td>
                        <td class="px-2">:</td>
                        <td class="text-right">
                            <CurrencyFormat :value="sale.doc.tax_1_amount" />
                        </td>
                    </tr>
                    <tr v-if="sale.doc.tax_2_amount">
                        <td>{{ setting.tax_2_name }} ({{ sale.doc.tax_2_rate }}%)</td>
                        <td class="px-2">:</td>
                        <td class="text-right">
                            <CurrencyFormat :value="sale.doc.tax_2_amount" />
                        </td>
                    </tr>
                    <tr v-if="sale.doc.tax_3_amount">
                        <td>{{ setting.tax_3_name }} ({{ sale.doc.tax_3_rate }}%)</td>
                        <td class="px-2">:</td>
                        <td class="text-right">
                            <CurrencyFormat :value="sale.doc.tax_3_amount" />
                        </td>
                    </tr>
                    <tr v-if="sale.doc.total_tax && sale.doc.total_tax > 1">
                        <td>Total Tax</td>
                        <td class="px-2">:</td>
                        <td class="text-right">
                            <CurrencyFormat :value="sale.doc.total_tax" />
                        </td>
                    </tr>
                    <tr>
                        <td>Grand Total</td>
                        <td class="px-2">:</td>
                        <td class="text-right">
                            <CurrencyFormat :value="sale.doc.grand_total" />
                        </td>
                    </tr>
                    <tr v-for="d in sale.doc.payment" :key="d.name">
                        <td>Paid by {{ d.payment_type }}</td>
                        <td class="px-2">:</td>
                        <td class="text-right">
                            <CurrencyFormat :currency="d.currency" :value="d.input_amount" />
                        </td>
                    </tr>
                    <tr v-if="sale.doc.payment.length > 1">
                        <td>Total Paid</td>
                        <td class="px-2">:</td>
                        <td class="text-right">
                            <CurrencyFormat :value="sale.doc.total_paid" />
                        </td>
                    </tr>
                    <tr v-if="sale.doc.balance">
                        <td>Balance</td>
                        <td class="px-2">:</td>
                        <td class="text-right">
                            <CurrencyFormat :value="sale.doc.balance" />
                        </td>
                    </tr>
                    <tr v-if="sale.doc.changed_amount">
                        <td>Changed Amount</td>
                        <td class="px-2">:</td>
                        <td class="text-right">
                            <CurrencyFormat :value="sale.doc.changed_amount" />
                        </td>
                    </tr>
                </table>
            </div>
            </div>
            <div v-else>
                <div v-if="sale.doc" class="text-sm">
                    <v-row no-gutters>
                        <v-col>
                            <v-sheet>
                                <table>
                                    <tr>
                                        <td>Code</td>
                                        <td class="px-2">:</td>
                                        <td>{{ sale.doc.customer }}</td>
                                    </tr>
                                    <tr>
                                        <td>Name</td>
                                        <td class="px-2">:</td>
                                        <td>{{ sale.doc.customer_name }}</td>
                                    </tr>
                                    <tr v-if="sale.doc.phone_number">
                                        <td>Phone</td>
                                        <td class="px-2">:</td>
                                        <td>{{ sale.doc.phone_number }}</td>
                                    </tr>
                                </table>
                            </v-sheet>
                        </v-col>
                        <v-col>
                            <v-sheet>
                                <table class="ml-auto">
                                    <tr>
                                        <td>Sale #</td>
                                        <td class="px-2">:</td>
                                        <td class="text-right">{{ sale.doc.name }}</td>
                                    </tr>
                                    <tr>
                                        <td>Date</td>
                                        <td class="px-2">:</td>
                                        <td class="text-right">{{ sale.doc.posting_date }}</td>
                                    </tr>
                                    <tr>
                                        <td>Branch</td>
                                        <td class="px-2">:</td>
                                        <td class="text-right">{{ sale.doc.business_branch }}</td>
                                    </tr>
                                    <tr v-if="sale.doc.phone_number">
                                        <td>Stock</td>
                                        <td class="px-2">:</td>
                                        <td class="text-right">{{ sale.doc.stock_location }}</td>
                                    </tr>
                                </table>
                            </v-sheet>
                        </v-col>
                    </v-row>
                    <v-table class="text-gray-500">

                        <thead class="text-center">
                            <tr>
                                <th>No</th>
                                <th>Image</th>
                                <th>Name</th>
                                <th>QTY</th>
                                <th>Price</th>
                                <th>Amount</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(p, index) in saleProducts" :key="index">
                                <th>{{ index + 1 }}</th>
                                <th class="text-left">
                                    <div class="w-8 h-8 overflow-hidden rounded-full bg-gray-200">
                                        <img :src="p.product_photo" />
                                    </div>
                                </th>
                                <th class="text-left">{{ p.product_name }}</th>
                                <th class="text-center">{{ p.quantity }}</th>
                                <th class="text-left">
                                    <CurrencyFormat :value="p.price" />
                                </th>
                                <th class="text-left">
                                    <CurrencyFormat :value="p.amount" />
                                </th>
                            </tr>
                        </tbody>
                    </v-table>
                    <div>
                        <table class="ml-auto">
                            <tr v-if="sale.doc.total_quantity">
                                <td>Total Quantity</td>
                                <td class="px-2">:</td>
                                <td class="text-right">{{ sale.doc.total_quantity }}</td>
                            </tr>
                            <tr>
                                <td>Sub Total</td>
                                <td class="px-2">:</td>
                                <td class="text-right">
                                    <CurrencyFormat :value="sale.doc.sub_total" />
                                </td>
                            </tr>
                            <tr v-if="sale.doc.product_discount">
                                <td>
                                    <span v-if="sale.doc.sale_discount">Product</span>
                                    <span> Discount</span>
                                </td>
                                <td class="px-2">:</td>
                                <td class="text-right">
                                    <CurrencyFormat :value="sale.doc.product_discount" />
                                </td>
                            </tr>
                            <tr v-if="sale.doc.sale_discount">
                                <td>
                                    <span v-if="sale.doc.product_discount">Sale </span>
                                    <span>Discount</span> ({{ sale.doc.discount }}%)
                                </td>
                                <td class="px-2">:</td>
                                <td class="text-right">
                                    <CurrencyFormat :value="sale.doc.sale_discount" />
                                </td>
                            </tr>
                            <tr
                                v-if="sale.doc.total_discount != sale.doc.product_discount && sale.doc.total_discount != sale.doc.sale_discount">
                                <td>Total Discount</td>
                                <td class="px-2">:</td>
                                <td class="text-right">
                                    <CurrencyFormat :value="sale.doc.total_discount" />
                                </td>
                            </tr>
                            <tr v-if="sale.doc.tax_1_amount">
                                <td>{{ setting.tax_1_name }} ({{ sale.doc.tax_1_rate }}%)</td>
                                <td class="px-2">:</td>
                                <td class="text-right">
                                    <CurrencyFormat :value="sale.doc.tax_1_amount" />
                                </td>
                            </tr>
                            <tr v-if="sale.doc.tax_2_amount">
                                <td>{{ setting.tax_2_name }} ({{ sale.doc.tax_2_rate }}%)</td>
                                <td class="px-2">:</td>
                                <td class="text-right">
                                    <CurrencyFormat :value="sale.doc.tax_2_amount" />
                                </td>
                            </tr>
                            <tr v-if="sale.doc.tax_3_amount">
                                <td>{{ setting.tax_3_name }} ({{ sale.doc.tax_3_rate }}%)</td>
                                <td class="px-2">:</td>
                                <td class="text-right">
                                    <CurrencyFormat :value="sale.doc.tax_3_amount" />
                                </td>
                            </tr>
                            <tr v-if="sale.doc.total_tax && sale.doc.total_tax > 1">
                                <td>Total Tax</td>
                                <td class="px-2">:</td>
                                <td class="text-right">
                                    <CurrencyFormat :value="sale.doc.total_tax" />
                                </td>
                            </tr>
                            <tr>
                                <td>Grand Total</td>
                                <td class="px-2">:</td>
                                <td class="text-right">
                                    <CurrencyFormat :value="sale.doc.grand_total" />
                                </td>
                            </tr>
                            <tr v-for="d in sale.doc.payment" :key="d.name">
                                <td>Paid by {{ d.payment_type }}</td>
                                <td class="px-2">:</td>
                                <td class="text-right">
                                    <CurrencyFormat :currency="d.currency" :value="d.input_amount" />
                                </td>
                            </tr>
                            <tr v-if="sale.doc.payment.length > 1">
                                <td>Total Paid</td>
                                <td class="px-2">:</td>
                                <td class="text-right">
                                    <CurrencyFormat :value="sale.doc.total_paid" />
                                </td>
                            </tr>
                        <tr v-if="sale.doc.balance">
                            <td>Balance</td>
                            <td class="px-2">:</td>
                            <td class="text-right">
                                <CurrencyFormat :value="sale.doc.balance" />
                            </td>
                        </tr>
                        <tr v-if="sale.doc.changed_amount">
                            <td>Changed Amount</td>
                            <td class="px-2">:</td>
                            <td class="text-right">
                                <CurrencyFormat :value="sale.doc.changed_amount" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        </template>
    </ComModal>
</template >

<script setup>

import { createDocumentResource, ref, inject, computed } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import ComPrintButton from '@/components/ComPrintButton.vue';
import { printPreviewDialog } from '@/utils/dialog';
import Enumerable from 'linq';
import { useDisplay } from 'vuetify'
import ComModal from '../../../components/ComModal.vue';

const { mobile } = useDisplay()

const gv = inject("$gv")
const props = defineProps({
    params: {
        type: Object,
        required: true,
    },
})
const emit = defineEmits(["resolve", "reject"])
const setting = computed(() => {
    return JSON.parse(localStorage.getItem('setting'))
})


let open = ref(true);


let sale = createDocumentResource({
    url: 'frappe.client.get',
    doctype: 'Sale',
    name: props.params.name,
    auto: true
})

function onClick() {
    emit('reject', false);
}

async function onPrint(r) {

    if (r.pos_receipt_file_name && localStorage.getItem("is_window")) {
        let data = {
            action: "print_receipt",
            print_setting: r,
            setting: gv.setting.pos_setting,
            sale: sale.doc
        }
        window.chrome.webview.postMessage(JSON.stringify(data));
    } else {

        await printPreviewDialog({
            title: "Sale #: " + sale.doc.name,
            doctype: "Sale",
            name: sale.doc.name,
            "report": r.name,
            print: true
        });

    }

}
const saleProducts = computed(() => {
    return Enumerable.from(sale.doc.sale_products).groupBy(
        "{product_code:$.product_code,portion:$.portion,modifiers:$.modifiers,product_name:$.product_name,product_name_kh:$.product_name_kh,unit:$.unit,discount:$.discount,discount_type:$.discount_type,product_photo:$.product_photo,price:$.price}",
        "{quantity:$.quantity, amount: $.amount}",
        "{product_code:$.product_code,product_name:$.product_name,product_name_kh:$.product_name_kh,modifiers_price:$.modifiers_price,portion:$.portion,modifiers:$.modifiers,unit:$.unit,discount:$.discount,discount_type:$.discount_type,product_photo:$.product_photo,price:$.price, quantity: $$.sum('$.quantity'), amount: $$.sum('$.amount')}",
        "$.product_code+','+$.unit+','+$.quantity+','+$.portion+','+$.modifiers+',' + $.discount + ',' + $.discount_type + ',' + $.price + ',' + $.modifiers_price"
    ).toArray()
})
</script>

