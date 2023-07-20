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
import { inject , payToRoomDialog,createToaster,i18n } from '@/plugin';
import { useDisplay } from 'vuetify'
const {mobile} = useDisplay()
const gv = inject("$gv")
const sale = inject("$sale")
const { t: $t } = i18n.global;  
const toaster = createToaster({ position: "top" });

async function onPaymentTypeClick(pt)  { 
    let room = null;
    let folio = null;
    if(mobile.value){
        sale.is_payment_first_load = false;
    }


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
    
    if(sale.is_payment_first_load){
        sale.paymentInputNumber = sale.paymentInputNumber * pt.exchange_rate;
    }
    sale.onAddPayment(pt, sale.paymentInputNumber,room,folio);
   
}
</script>