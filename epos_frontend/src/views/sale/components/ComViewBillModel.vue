<template>
    <ComModal :fullscreen="true" @onClose="onClose" title-ok-button="OK" :hideOkButton="true">
        <template #title>
            <span>Sale # : <span v-if="sale.sale.sale_status=='New'">New</span><span v-else>{{ sale.sale.name }}</span></span>
        </template>
        <template #content>
            <v-container>
                    <v-card style="width: 100%; max-width: 800px; margin: 0 auto;">
                        <v-card-text>
                            <div class="sm:flex sm:justify-between pb-2">
                                <div>
                                    <div class="flex">
                                        <div class="flex-auto">
                                            <div class="flex">
                                                <v-avatar v-if="sale.sale.customer_photo">
                                                    <v-img :src="sale.sale.customer_photo"></v-img>
                                                </v-avatar>
                                                <template v-if="sale.sale.customer_name != undefined">
                                                    <avatar v-if="sale.sale.customer_photo == undefined" :name="sale.sale.customer_name"
                                                        class="mr-4" size="40"></avatar>
                                                </template>
                                                <div class="px-2">
                                                    <div class="font-bold">{{ sale.sale.customer }} - {{ sale.sale.customer_name }}</div> 
                                                    <div class="text-gray-400 text-sm" v-if="sale.sale.phone_number"><v-icon icon="mdi-phone" size="small"></v-icon> {{ sale.sale.phone_number }}</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div>
                                    <ul class="sm:ml-auto flex flex-col p-0 m-0">
                                        <li class="pb-1 flex justify-between items-center">
                                            <div class="mr-16">Sale#</div>
                                            <span class="ml-auto text-red-500 font-bold">
                                                <span v-if="sale.sale.sale_status=='New'">New</span><span v-else>{{ sale.sale.name }}</span>
                                            </span>
                                        </li>
                                        <li class="pb-1 flex justify-between items-center" v-if="setting.table_groups && setting.table_groups.length > 0">
                                            <div class="mr-16">Table#</div>
                                            <span class="ml-auto">{{ sale.sale.tbl_number }}</span>
                                        </li>
                                        <li class="pb-1 flex justify-between items-center">
                                            <div class="mr-16">Date</div>
                                            <span class="ml-auto">{{ sale.sale.posting_date }}</span>
                                        </li>
                                        <li class="flex justify-end">
                                            <div class="ml-auto">
                                                <v-chip variant="elevated" v-if="sale.sale.name!=undefined" :color="sale.sale.sale_status_color" size="x-small">
                                                    {{ sale.sale.sale_status }}
                                                </v-chip>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="pt-4 pb-5">
                                <ComTableView>
                                    <template #header>
                                        <tr>
                                            <th class="!bg-gray-100">No</th>
                                            <th class="text-center !bg-gray-100">Image</th>
                                            <th style="width: unset;" class="text-left !bg-gray-100">Description</th>
                                            <th class="text-center !bg-gray-100">Unit</th>
                                            <th class="text-center !bg-gray-100">QTY</th>
                                            <th class="text-right !bg-gray-100">Price</th>
                                            <th class="text-right !bg-gray-100">Amount</th>
                                        </tr>
                                    </template>
                                    <template #body>
                                        <tr v-for="(p, index) in saleProducts" :key="index">
                                            <td>{{ index + 1 }}</td>
                                            <ComTdImage :photo="p.product_photo" :title="p.product_name"></ComTdImage>
                                            <td>
                                                <div>
                                                    <span class="mr-1">{{ p.product_code }} - {{ p.product_name }}</span>
                                                    <v-chip class="mr-1" size="x-small" color="error" variant="outlined" v-if="p.portion">{{ p.portion }}</v-chip>
                                                    <v-chip class="mr-1" v-if="p.is_free" size="x-small" color="success" variant="outlined">Free</v-chip>
                                                </div>
                                                <div class="text-xs pt-1">
                                                    <div v-if="p.modifiers">
                                                        <span>{{ p.modifiers }} (<CurrencyFormat :value="p.modifiers_price*p.quantity" />)</span>
                                                    </div>
                                                    <div class="text-red-500" v-if="p.discount > 0">
                                                        Discount : 
                                                        <span v-if="p.discount_type == 'Percent'">{{ p.discount }}%</span>
                                                        <CurrencyFormat v-else :value="parseFloat(p.discount)" />
                                                    </div>
                                                </div>
                                            </td>
                                            <td class="text-center">{{ p.unit }}</td>
                                            <td class="text-center">{{ p.quantity }}</td>
                                            <td class="text-right">
                                                <CurrencyFormat :value="p.price" />
                                            </td>
                                            <td class="text-right">
                                                <CurrencyFormat :value="p.amount" />
                                            </td>
                                        </tr>
                                    </template>
                                </ComTableView>
                            </div>
                            <div class="flex justify-end">
                                <ul class="sm:ml-auto flex flex-col m-0 p-3">
                                    <li class="py-1 flex justify-between items-center" v-if="sale.sale.total_quantity > 0">
                                        <div class="mr-16">Total Quantity</div>
                                        <span class="ml-auto">{{ sale.sale.total_quantity }}</span>
                                    </li>
                                    <li class="py-1 flex justify-between items-center" v-if="(sale.sale.total_discount + sale.sale.total_tax) > 0">
                                        <div class="mr-16">Sub Total</div>
                                        <span class="ml-auto"><CurrencyFormat :value="sale.sale.sub_total"/></span>
                                    </li>
                                    <li class="py-1 flex justify-between items-center" v-if="sale.sale.product_discount > 0">
                                        <div class="mr-16">
                                            <span v-if="sale.sale.sale_discount > 0 ">Product</span>
                                            <span> Discount</span>
                                        </div>
                                        <span class="ml-auto"><CurrencyFormat :value="sale.sale.product_discount"/></span>
                                    </li>
                                    <li class="py-1 flex justify-between items-center" v-if="sale.sale.sale_discount > 0">
                                        <div class="mr-16">
                                            <span v-if="sale.sale.product_discount > 0 ">Sale</span>
                                                <span>Discount</span><span>({{sale.sale.discount}}%)</span>
                                        </div>
                                        <span class="ml-auto"><CurrencyFormat :value="sale.sale.sale_discount"/></span>
                                    </li>
                                    <li class="py-1 flex justify-between items-center" v-if="sale.sale.total_discount != sale.sale.product_discount && sale.sale.total_discount != sale.sale.sale_discount">
                                        <div class="mr-16">
                                            <span>Total Discount</span>
                                        </div>
                                        <span class="ml-auto"><CurrencyFormat :value="sale.sale.total_discount"/></span>
                                    </li>
                                    <li class="py-1 flex justify-between items-center" v-if="sale.sale.tax_1_amount > 0">
                                        <div class="mr-16">
                                            <span>{{ setting.tax_1_name }}({{ sale.sale.tax_1_rate }}%)</span>
                                        </div>
                                        <span class="ml-auto"><CurrencyFormat :value="sale.sale.tax_1_amount"/></span>
                                    </li>
                                    <li class="py-1 flex justify-between items-center" v-if="sale.sale.tax_2_amount > 0">
                                        <div class="mr-16">
                                            <span>{{ setting.tax_2_name }}({{ sale.sale.tax_2_rate }}%)</span>
                                        </div>
                                        <span class="ml-auto"><CurrencyFormat :value="sale.sale.tax_2_amount"/></span>
                                    </li>
                                    <li class="py-1 flex justify-between items-center" v-if="sale.sale.tax_3_amount > 0">
                                        <div class="mr-16">
                                            <span>{{ setting.tax_3_name }}({{ sale.sale.tax_3_rate }}%)</span>
                                        </div>
                                        <span class="ml-auto"><CurrencyFormat :value="sale.sale.tax_3_amount"/></span>
                                    </li>
                                    <li class="py-1 flex justify-between items-center" v-if="sale.sale.total_tax !=0 && sale.sale.total_tax > 0">
                                        <div class="mr-16">
                                            <span>Total Tax</span>
                                        </div>
                                        <span class="ml-auto"><CurrencyFormat :value="sale.sale.total_tax"/></span>
                                    </li>
                                    <li class="py-1 flex justify-between items-center font-bold">
                                        <div class="mr-16">
                                            <span>Grand Total</span>
                                        </div>
                                        <span class="ml-auto"><CurrencyFormat :value="sale.sale.grand_total"/></span>
                                    </li>
                                    <li class="py-1 flex justify-between items-center font-bold text-green-600" v-for="d in sale.sale.payment" :key="d.name">
                                        <div class="mr-16">
                                            <span>Paid by {{ d.payment_type }}</span>
                                        </div>
                                        <span class="ml-auto"><CurrencyFormat :currency="d.currency" :value="d.input_amount"/></span>
                                    </li>
                                    <li class="py-1 flex justify-between items-center" v-if="sale.sale.payment.length > 1">
                                        <div class="mr-16">
                                            <span>Total Paid</span>
                                        </div>
                                        <span class="ml-auto"><CurrencyFormat :value="d.total_paid"/></span>
                                    </li>
                                    <li class="py-1 flex justify-between items-center text-red-500" v-if="sale.sale.balance > 0">
                                        <div class="mr-16">
                                            <span>Balance</span>
                                        </div>
                                        <span class="ml-auto  font-bold"><CurrencyFormat :value="sale.sale.balance"/></span>
                                    </li>
                                    <li class="py-1 flex justify-between items-center" v-if="sale.sale.changed_amount > 0">
                                        <div class="mr-16">
                                            <span>Changed Amount</span>
                                        </div>
                                        <span class="ml-auto"><CurrencyFormat :value="sale.sale.changed_amount"/></span>
                                    </li>
                                </ul>
                            </div>
                        </v-card-text>
                    </v-card>
                </v-container>
        </template>
    </ComModal>
</template>
  
<script setup>
import Enumerable from 'linq'
import { ref, defineEmits, inject, computed } from '@/plugin'
const props = defineProps({
    params: {
        type: Object,
        require: true
    }
})

const emit = defineEmits(["resolve", "reject"])
const setting = JSON.parse(localStorage.getItem('setting'))
const sale = inject('$sale')
let open = ref(true);
const saleProducts = computed(() => {
    return Enumerable.from(sale.sale.sale_products).groupBy(
        "{product_code:$.product_code,modifiers_price:$.modifiers_price,portion:$.portion,modifiers:$.modifiers,product_name:$.product_name,product_name_kh:$.product_name_kh,unit:$.unit,discount:$.discount,discount_type:$.discount_type,product_photo:$.product_photo,is_free:$.is_free,price:$.price}",
        "{quantity:$.quantity, amount: $.amount}",
        "{product_code:$.product_code,product_name:$.product_name,product_name_kh:$.product_name_kh,modifiers_price:$.modifiers_price,portion:$.portion,modifiers:$.modifiers,unit:$.unit,discount:$.discount,discount_type:$.discount_type,product_photo:$.product_photo,price:$.price,is_free:$.is_free, quantity: $$.sum('$.quantity'), amount: $$.sum('$.amount')}",
        "$.product_code+','+$.unit+','+$.quantity+','+$.is_free+','+$.portion+','+$.modifiers+',' + $.discount + ',' + $.discount_type + ',' + $.price + ',' + $.modifiers_price"
        ).toArray()
})
function onClose() {
    emit('reject', false);
}

</script>