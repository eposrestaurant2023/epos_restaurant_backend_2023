<template>
    <v-dialog v-model="open" fullscreen scrollable>
        <v-card>
            <ComToolbar @onClose="onClose">
                <template #title>
                    Sale # : {{ sale.sale.name }}
                </template>
            </ComToolbar>
            <v-card-text>
                <ComTableView>
                    <template #header>
                        <th>No</th>
                        <th>Image</th>
                        <th style="width: unset;" class="text-left">Description</th>
                        <th class="text-center">Unit</th>
                        <th class="text-center">QTY</th>
                        <th class="text-right">Price</th>
                        <th class="text-right">Amount</th>
                    </template>
                    <template #body>
                        <tr v-for="p in saleProducts">
                            <td>{{ p.idx }}</td>
                            <ComTdImage :photo="p.product_photo" :title="p.product_name"></ComTdImage>
                            <td>{{ p.product_code }} - {{ p.product_name }}</td>
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
    return sale.sale.sale_products;
})
function onClose() {
    emit('reject', false);
}

</script>