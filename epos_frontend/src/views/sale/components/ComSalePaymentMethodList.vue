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
import { inject , payToRoomDialog,createToaster,i18n ,computed,keyboardDialog} from '@/plugin';
import { useDisplay } from 'vuetify'
const {mobile} = useDisplay()
const gv = inject("$gv")
const sale = inject("$sale")
const { t: $t } = i18n.global;  
const toaster = createToaster({ position: "top" });

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


    //check if payment exist manual fee

    let fee_amount = 0;
    if(pt.is_manual_fee==1){
        const fee = await keyboardDialog({ title:$t("Enter Fee Amount"), type: 'number', value: 0 });
        if(!fee){
            return;
        }      
        fee_amount = fee;
    }

    if(mobile.value){
        sale.is_payment_first_load = false;
    } 



    if(pt.allow_change==0 &&  parseFloat(sale.paymentInputNumber) > (balance.value * pt.exchange_rate)){
        sale.paymentInputNumber = balance.value *  pt.exchange_rate;
    }
    
    if(sale.is_payment_first_load){
        sale.paymentInputNumber = sale.paymentInputNumber * pt.exchange_rate;
    }   
 

    sale.onAddPayment(pt, sale.paymentInputNumber,fee_amount,room,folio);
   
}

const balance = computed(()=>{
    if(sale.sale?.balance>0){ 
        return Number(sale.sale.balance.toFixed(gv.setting.pos_setting.main_currency_precision));
    }else {
        return 0;
    }
})

 


</script>