<template>
  <v-dialog v-model="open" persistent max-width="400">
    <v-card>
      <v-card-item>
        <v-card-title>Enter Your Pin Code</v-card-title>
      </v-card-item>
      <v-card-text>
        <v-text-field type="password" label="Enter pin code" variant="solo" v-model="number" clearable
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
            <v-btn @click="numpad_click('.')" size="large">
              .
            </v-btn>
            <v-btn color="error" @click="number = ''" size="large">
              Clear
            </v-btn>
          </div>
          <div>
              <div class="text-right pt-4">
                  <v-btn class="mr-2" variant="flat" @click="onCancel()" color="error">
                      Close
                  </v-btn>
                  <v-btn variant="flat" @click="onOk()" color="primary">
                      OK
                  </v-btn>
              </div>
          </div>
        </div>
      </v-card-text>

    </v-card>
  </v-dialog>
</template>
<script setup>
import { ref, createResource, inject,  } from "@/plugin"
import { createToaster } from "@meforma/vue-toaster";
const gv = inject("$gv")
const toaster = createToaster({ position: "top" })

const props = defineProps({
  params: {
    type: Object,
  }
})
const emit = defineEmits(["resolve"])

const open = ref(true);

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
 
  createResource({
    url: 'epos_restaurant_2023.api.api.check_username',
    auto: true,
    params: {
      "pin_code": number.value
    },
    async onSuccess(doc) {
      
      if (doc.permission[props.params.permissionCode] == 1) {
        
        emit('resolve', {name:doc.full_name, discount_codes:doc.permission.discount_codes})
      } else {
        toaster.warning("You don't have permission to perform this action");
      }


    }
  })

}
function onCancel() {
  emit('resolve', false);
}

</script>
