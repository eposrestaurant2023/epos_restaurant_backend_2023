<template>
    <v-dialog v-model="open" fullscreen scrollable>
        <v-card>
            <ComToolbar @onClose="onClose">
                <template #title>
                    Sale # : {{ sale.sale.name }}
                </template>
            </ComToolbar>
            <v-card-text>
                <table class="tbl-list">
                    <tr>
                        <td class="pb-2">Sale #</td>
                        <td class="pb-2 px-2">:</td>
                        <td class="pb-2">{{ sale.sale.name }}</td>
                    </tr>
                    <tr>
                        <td class="pb-2">Date</td>
                        <td class="pb-2 px-2">:</td>
                        <td class="pb-2">{{ sale.sale.posting_date }}</td>
                    </tr>
                    <tr>
                        <td class="pb-2">Customer Code</td>
                        <td class="pb-2 px-2">:</td>
                        <td class="pb-2">{{ sale.sale.customer }}</td>
                    </tr>
                    <tr >
                        <td class="pb-2">Customer Name</td>
                        <td class="pb-2 px-2">:</td>
                        <td class="pb-2">{{ sale.sale.customer_name }}</td>
                    </tr>
                    <tr v-if="sale.sale.phone_number > 0">
                        <td class="pb-2">Phone Number</td>
                        <td class="pb-2 px-2">:</td>
                        <td class="pb-2">{{ sale.sale.phone_number }}</td>
                    </tr>
                </table>
                <ComTableView>
                    <template #header>
                        <tr>
                            <th>No</th>
                            <th class="text-center">Image</th>
                            <th style="width: unset;" class="text-left">Description</th>
                            <th class="text-center">Unit</th>
                            <th class="text-center">QTY</th>
                            <th class="text-right">Price</th>
                            <th class="text-right">Amount</th>
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
                <div> 
                        <table class="ml-auto">
        
                            <tr  v-if="sale.sale.total_quantity !=0">
                                <td class="pb-2">Total Quantity</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right">{{ sale.sale.total_quantity }}</td>
                            </tr>
                            <tr >
                                <td class="pb-2">Sub Total</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.sale.sub_total"/></td>
                            </tr>  
                            <tr  v-if="sale.sale.product_discount > 0">
                                <td class="pb-2">
                                    <span v-if="sale.sale.sale_discount > 0 ">Product</span>
                                    <span> Discount</span>
                                </td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.sale.product_discount"/></td>
                            </tr>
                            <tr  v-if="sale.sale.sale_discount > 0">
                                <td class="pb-2">
                                    <span v-if="sale.sale.product_discount > 0 ">Sale</span>
                                    <span>Discount</span>({{sale.sale.discount}}%)</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.sale.sale_discount"/></td>
                            </tr>
                            <tr  v-if="sale.sale.total_discount != sale.sale.product_discount && sale.sale.total_discount != sale.sale.sale_discount">
                                <td class="pb-2">Total Discount</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.sale.total_discount"/></td>
                            </tr>
                            <tr  v-if="sale.sale.tax_1_amount > 0">
                                <td class="pb-2">{{ setting.tax_1_name }}({{ sale.sale.tax_1_rate }}%)</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.sale.tax_1_amount"/></td>
                            </tr>
                            <tr  v-if="sale.sale.tax_2_amount > 0" >
                                <td class="pb-2">{{ setting.tax_2_name }}({{ sale.sale.tax_2_rate }}%)</td>
                                <td class="pb-2 px-2 px-2">:</td>   
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.sale.tax_2_amount"/></td>
                            </tr>
                            <tr  v-if="sale.sale.tax_3_amount > 0">
                                <td class="pb-2">{{ setting.tax_3_name }}({{ sale.sale.tax_3_rate }}%)</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.sale.tax_3_amount"/></td>
                            </tr>
                            <tr  v-if="sale.sale.total_tax !=0 && sale.sale.total_tax > 1">
                                <td class="pb-2">Total Tax</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2"><CurrencyFormat :value="sale.sale.total_tax"/></td>
                            </tr>
                            <tr  >
                                <td class="pb-2">Grand Total</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.sale.grand_total"/></td>
                            </tr>
                            <tr  v-for="d in sale.sale.payment" :key="d.name">
                                <td class="pb-2">Paid by {{ d.payment_type }}</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :currency="d.currency" :value="d.input_amount"/></td>
                            </tr>
                            <tr  v-if="sale.sale.payment.length > 1">
                                <td class="pb-2">Total Paid</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.sale.total_paid"/></td>
                            </tr>                   
                            <tr class="font-bold" v-if="sale.sale.balance !=0">
                                <td class="pb-2">Balance</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.sale.balance"/></td>
                            </tr>
                            <tr  v-if="sale.sale.changed_amount !=0">
                                <td class="pb-2">Changed Amount</td>
                                <td class="pb-2 px-2 px-2">:</td>
                                <td class="pb-2 text-right"><CurrencyFormat :value="sale.sale.changed_amount"/></td>
                            </tr> 
                        </table>
                    </div>
            </v-card-text>
            <div>
                <v-divider></v-divider>
                <div class="text-right p-2">
                    <v-btn color="error" @click="onClose" class="mr-2">Close</v-btn>
                    <v-btn color="primary" @click="onConfirm">OK</v-btn>
                </div>
            </div>
        </v-card>
    </v-dialog>
</template>
  
<script setup>
import Enumerable from 'linq'
import { ref, defineEmits, inject, computed } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue'; 
const props = defineProps({
    params: {
        type: Object,
        require: true
    }
})

const emit = defineEmits(["resolve", "reject"])
const setting = computed(()=>{
    return JSON.parse(localStorage.getItem('setting'))
})
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