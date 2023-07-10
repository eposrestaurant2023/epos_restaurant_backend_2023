<template>
  <ComModal :fullscreen="mobile" @onClose="onClose" :loading="loading" @onOk="onSeachCustomer()">
    <template #title>
      {{ $t('Scan Membership Card Number') }}
    </template>
    <template #content>
      <v-alert icon="mdi-cards-outline" prominent type="success" variant="outlined" class="mb-2">
        {{ $t('msg.Please enter or scan customer membership card number') }}.
      </v-alert>
      <ComInput v-model="customerCode" autofocus :placeholder="$t('Scan Membership Card Number')" variant="outlined"
        density="default" />
    </template>
  </ComModal>
</template>
<script setup>
import { ref, defineEmits, watch,inject,createToaster,i18n } from '@/plugin'
import ComInput from '../../../components/form/ComInput.vue';
import { useDisplay } from 'vuetify';

const { t: $t } = i18n.global;
const moment = inject('$moment')
const sale = inject('$sale');
const frappe = inject('$frappe');
const call = frappe.call()
const { mobile } = useDisplay()
let customerCode = ref("");
const toaster = createToaster({ position: "top" })

const loading = ref(false)

const emit = defineEmits(["resolve", "reject"])
function onClose() {
  emit("resolve", false);
}

async function onSeachCustomer() {
  if(customerCode.value!=""){
    loading.value = true;
    const data = await call.get('epos_restaurant_2023.api.api.get_customer_on_membership_scan', {"card":customerCode.value});
    if(data.message != "Invalid Card"){     
      const cards = data.message.card.filter((r)=>r.card_code==customerCode.value);
      if(cards.length > 0){
        const current_date = new Date();  
        const d = moment(current_date).format("YYYY-MM-DD");    
        if(getDate(cards[0].expiry) < getDate(d))  {
          toaster.warning($t("msg.Card Number was expired"))
        } else{
          data.message.card = cards[0]
          emit("resolve", data.message);
        }
      } 
    }
    loading.value = false;
  }

}


function getDate(value){
   return new Date(value)
}
 
 

</script>