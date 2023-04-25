<template>
    <v-card :class="{ 'selected': saleProduct.selected, 'submitted relative': saleProduct.sale_product_status == 'Submitted' }"> 
        <template v-slot:default>
            <div class="text-sm" style="padding: 2px;">
                <div class="flex">
                    <div class="grow">
                        <div> {{ saleProduct.product_name }}<v-chip class="ml-1" size="x-small"
                                color="error" variant="outlined" v-if="saleProduct.portion">{{ saleProduct.portion }}</v-chip> <v-chip
                                v-if="saleProduct.is_free" size="x-small" color="success" variant="outlined">Free</v-chip>
                        </div>
                        <div>
                            {{ saleProduct.quantity }} x <CurrencyFormat :value="saleProduct.price" />
                        </div>
                        <div class="text-xs pt-1"> 
                            <div v-if="saleProduct.modifiers">
                                <span>{{ saleProduct.modifiers }} (<CurrencyFormat :value="saleProduct.modifiers_price * saleProduct.quantity" />)
                                </span>
                            </div>
                            <div class="text-red-500" v-if="saleProduct.discount > 0">
                                Discount :
                                <span v-if="saleProduct.discount_type == 'Percent'">{{
                                    saleProduct.discount
                                }}%</span>
                                <CurrencyFormat v-else :value="parseFloat(saleProduct.discount)" />
                            </div>
                            <v-chip color="blue" size="x-small" v-if="saleProduct.seat_number">Seat# {{
                                saleProduct.seat_number
                            }}</v-chip>
                            <div class="text-gray-500" v-if="saleProduct.note">
                                Note: <span>{{ saleProduct.note }}</span>
                            </div>
                        </div>
                    </div>
                    <div class="flex-none text-right w-36">
                        <div class="text-lg"> 
                            <CurrencyFormat :value="saleProduct.amount" />
                        </div> 
                    </div>
                </div> 
            </div>
        </template>       
    </v-card> 
</template>
<script setup>
    import { inject, defineProps } from '@/plugin' 
    const sale = inject('$sale') 
    const props = defineProps({
        saleProduct: Object
    })
    
 
</script>
<style scoped>
.selected,
.item-list:hover {
    background-color: #ffebcc !important;
}
 
.item-list.submitted::before{
    content: '';
    position: absolute;
    top: 1px; 
    bottom: 1px;
    left: 0px;
    width: 2px;
    background: #75c34a;
    border-radius: 12px;
}
</style>
