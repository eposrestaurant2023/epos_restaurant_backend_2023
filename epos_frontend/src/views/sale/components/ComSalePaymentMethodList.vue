<template>
    <div class="grid gap-1" :class="mobile ? 'grid-cols-3 text-sm' : 'grid-cols-2'">
        <div class="border rounded-sm px-2 py-4 text-center cursor-pointer bg-orange-100 hover:bg-orange-300 flex justify-center items-center"
            v-for="(pt, index) in gv.setting?.payment_types" :key="index"
            @click="onPaymentTypeClick(pt)">
            <div>
                {{ pt.payment_method }} 
            </div>
        </div>
    </div>
</template>
<script setup>
import { inject,onMounted , payToRoomDialog,createToaster,i18n } from '@/plugin';
import { useDisplay } from 'vuetify'
const {mobile} = useDisplay()
const gv = inject("$gv")
const sale = inject("$sale")
const { t: $t } = i18n.global;  
const toaster = createToaster({ position: "top" });

let is_first_input_amount = true;
let payment_input_number = 0;


onMounted(()=>{
    payment_input_number = sale.paymentInputNumber;
    is_first_input_amount = true;
});

async function onPaymentTypeClick(pt)  { 
    let room = null;
    let folio = null;

    if(pt.payment_type_group=="Pay to Room" ){ 

        if(sale.paymentInputNumber<=0){
            toaster.warning($t("msg.Please enter payment amount"));
            return
        }

        const result = await payToRoomDialog({
            data : pt
        });

        //
        if(result == false){
            return
        }
        room = result.room;
        folio = result.folio;      
    } 
   
    if( payment_input_number == sale.paymentInputNumber){
        is_first_input_amount = true;
    }
    
    if(is_first_input_amount){
        sale.paymentInputNumber = sale.paymentInputNumber * pt.exchange_rate;
    }
    sale.onAddPayment(pt, sale.paymentInputNumber,room,folio);
    is_first_input_amount = false;
}
</script>