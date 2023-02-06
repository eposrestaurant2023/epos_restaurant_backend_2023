<template>
    <v-dialog v-model="open" fullscreen scrollable>
        <v-card>
            <ComToolbar @onClose="onClose">
                <template #title>
                    Sale # : {{ sale.sale.name }}
                </template>
            </ComToolbar>
            <v-card-text>
                {{ sale.sale.sale_products }}
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
                            <td>{{ p.product_code }} - {{ p.product_name }}</td>
                            <td class="text-center">{{ p.unit }}</td>
                            <td class="text-center">{{ p.quantity }} || {{ p.is_free }}</td>
                            <td class="text-right">
                                <CurrencyFormat :value="p.price" />
                            </td>
                            <td class="text-right">
                                <CurrencyFormat :value="p.amount" />
                            </td>
                        </tr>
                    </template> 
                </ComTableView>
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
const sale = inject('$sale')
const open = ref(true);
const saleProducts = computed(() => {
    return Enumerable.from(sale.sale.sale_products).groupBy(
        "{product_code:$.product_code,portion:$.portion,modifiers:$.modifiers,product_name:$.product_name,product_name_kh:$.product_name_kh,unit:$.unit,discount:$.discount,discount_type:$.discount_type,product_photo:$.product_photo,is_free:$.is_free,price:$.price}",
        "{quantity:$.quantity, amount: $.amount}",
        "{product_code:$.product_code,product_name:$.product_name,product_name_kh:$.product_name_kh,portion:$.portion,modifiers:$.modifiers,unit:$.unit,discount:$.discount,discount_type:$.discount_type,product_photo:$.product_photo,price:$.price,is_free:$.is_free, quantity: $$.sum('$.quantity'), amount: $$.sum('$.amount')}",
        "$.product_code+','+$.unit+','+$.quantity+','+$.is_free+','+$.portion+','+$.modifiers+',' + $.discount + ',' + $.discount_type + ',' + $.price"
        ).toArray()
})
function onClose() {
    emit('reject', false);
}

</script>