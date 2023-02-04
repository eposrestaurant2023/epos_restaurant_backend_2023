<template>
    <v-dialog v-model="open" persistent max-width="800">
        <v-card>
            <v-toolbar color="default" :title="params.title ? params.title : 'Product Discount'">
                <v-toolbar-items>
                    <v-btn icon @click="onClose()">
                        <v-icon>mdi-close</v-icon>
                    </v-btn>
                </v-toolbar-items>
            </v-toolbar>
            <v-card-text class="p-0">
                <div>
                    <div class="mb-2">
                        <div class="mb-2">
                            <v-alert variant="tonal" color="warning" class="!p-2">
                                Max discount <span class="mr-2">({{maxDiscountPercent * 100}}%)</span><span>{{discountAmount}}$</span>
                            </v-alert>
                        </div>
                        <ComInput 
                            keyboard
                            type="number"
                            v-model="discount"
                            :disabled="discount_type == 'Percent'"
                            />
                    </div>
                    <div class="flex -m-1">
                        <div
                            class="p-1"
                            v-for="(item, index) in discountCodes" 
                            :key="index">
                            <v-btn
                                size="large"
                                :color="item.discount_value == (discount_type == 'Percent' ? discount / 100 : discount) ? 'primary' : ''"
                                @click="onClick(item)">
                                {{ item.discount_code }}
                            </v-btn>
                        </div>
                    </div>
                </div>
                <div>
                    <div class="text-right pt-4">
                        <v-btn class="mr-2" variant="flat" @click="onClose(false)" color="error">
                            Close
                        </v-btn>
                        <v-btn variant="flat" @click="onOK()" color="primary">
                            OK
                        </v-btn>
                    </div>
                </div>
            </v-card-text>
        </v-card>
    </v-dialog>
</template>
<script setup>
import { ref, defineEmits, createToaster, computed } from '@/plugin'
import Enumerable from 'linq'
const emit = defineEmits(['resolve'])
const props = defineProps({
    params:Object
})
const toaster = createToaster({ position: "top" })
let open = ref(true)

let discount_type = ref(props.params.data.discount_type)
let discount = ref(props.params.data.discount_value)
let amount = ref(parseFloat(props.params.value))

const discountCodes = computed(()=>{
    return Enumerable.from(props.params.data?.discount_codes).where(`$.discount_type=='${discount_type.value}'`).toArray();
})
const maxDiscountPercent = computed(()=>{
    return Enumerable.from(props.params.data?.discount_codes).where(`$.discount_type=='Percent'`).max("$.discount_value").toFixed(4)
})
const discountAmount = computed(()=>{
    return (amount.value * maxDiscountPercent.value).toFixed(3)
})
function onClick(item){
    discount.value = (discount_type.value == 'Amount' ? 1 : 100) * item.discount_value;
}
function onClose() {
    emit('resolve',false)
}
function onOK(){
    if(discount.value <= 0){
        toaster.warning("Please select a discount");
    }
    else if(discount_type.value == 'Amount' && discountAmount.value < discount.value){
        toaster.warning(`This product can max discount ${maxDiscountPercent.value * 100}% : ${discountAmount.value}$ `);
    }
    else{
        const result = {
            discount: discount.value,
            discount_type: discount_type.value
        }
        emit('resolve', result)
    }
    
    
}
 
</script>