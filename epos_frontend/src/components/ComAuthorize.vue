<template>
  <v-dialog v-model="open" persistent max-width="320">
    <v-card>
      <v-card-item>
        <v-card-title>{{ $t('Enter Your PIN Code') }}</v-card-title>
      </v-card-item>
      <v-card-text>
        <form v-on:submit.prevent="onOk">

        <v-text-field type="password" :label="$t('PIN Code')" variant="solo" v-model="number" clearable
          maxlength="10"></v-text-field>
        <div>
          <div class="grid grid-cols-3 gap-3">
            <v-btn @click="numpad_click('1')" size="large">
              1
            </v-btn>
            <v-btn @click="numpad_click('2')" size="large">
              2
            </v-btn>
            <v-btn @click="numpad_click('3')" size="large">
              3
            </v-btn>
            <v-btn @click="numpad_click('4')" size="large">
              4
            </v-btn>
            <v-btn @click="numpad_click('5')" size="large">
              5
            </v-btn>
            <v-btn @click="numpad_click('6')" size="large">
              6
            </v-btn>
            <v-btn @click="numpad_click('7')" size="large">
              7
            </v-btn>
            <v-btn @click="numpad_click('8')" size="large">
              8
            </v-btn>
            <v-btn @click="numpad_click('9')" size="large">
              9
            </v-btn>
            <v-btn @click="numpad_click('0')" size="large">
              0
            </v-btn>
            <v-btn class="col-span-2" color="error" @click="number = ''" size="large">
              {{ $t('Clear') }}
            </v-btn>
          </div>
          <div>
              <div class="text-right pt-4">
                  <v-btn class="mr-2" variant="flat" @click="onCancel()" color="error" size="large">
                      {{ $t('Close') }}
                  </v-btn>
                  <v-btn variant="flat" @click="onOk()" color="primary" size="large">
                      {{ $t('Ok') }}
                  </v-btn>
              </div>
          </div>
        </div>
      </form>
      </v-card-text>

    </v-card>
  </v-dialog>
</template>
<script setup>
import { ref, createResource, inject,i18n  } from "@/plugin"
import { createToaster } from "@meforma/vue-toaster";
const { t: $t } = i18n.global; 
 

const gv = inject("$gv")
const toaster = createToaster({ position: "top" })

const props = defineProps({
  params: {
    type: Object,
  }
})
const emit = defineEmits(["resolve"])

let open = ref(true);

const number = ref("");



function numpad_click(n) {
  if (n == "." && number.value.includes(".")) {

    return;
  }
  if (!number.value) {
    number.value = "";
  }
  number.value = number.value + n;
}

function onOk() {
  if((number.value||'').trim()==""){
    toaster.warning($t("msg.Please enter your pin code"));
    return;
  }
 
  createResource({
    url: 'epos_restaurant_2023.api.api.check_username',
    auto: true,
    params: {
      "pin_code": number.value
    },
    async onSuccess(doc) {
      
      if (doc.permission[props.params.permissionCode] == 1) {
       
        emit('resolve', {name:doc.full_name, discount_codes:doc.permission.discount_codes,username:doc.username})
      } else {
        toaster.warning($t("msg.You do not have permission to perform this action"));
      }
    },
   async onError(e){
      if(e.error_text !=undefined){
        toaster.warning($t(`msg.${$t(e.error_text[0])}`)); 
      } 
    }
  })

}
function onCancel() {
  emit('resolve', false);
}

</script>
