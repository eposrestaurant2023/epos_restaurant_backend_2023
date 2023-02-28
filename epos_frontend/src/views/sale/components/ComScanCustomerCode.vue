<template>
  <ComModal :fullscreen="mobile" @onClose="onClose" :loading="resource?.loading" @onOk="onSeachCustomer()">
    <template #title>
      Scan Membership Card Number
    </template>
    <template #content>
      <v-alert icon="mdi-cards-outline" prominent type="success" variant="outlined" class="mb-2">
        Please enter or scan customer membership card number.
      </v-alert>
      <ComInput v-model="customerCode" autofocus placeholder="Scan membership Card" variant="outlined"
        density="default" />
    </template>
  </ComModal>
</template>
<script setup>
import { ref, defineProps, defineEmits, createDocumentResource, onUnmounted, watch } from '@/plugin'
import ComInput from '../../../components/form/ComInput.vue';
import { useDisplay } from 'vuetify'

const { mobile } = useDisplay()
let customerCode = ref("");
let resource = ref(createDocumentResource({
  url: 'frappe.client.get',
  doctype: 'Customer',
}));
const props = defineProps({
  params: {
    type: Object,
    required: true,
  }
})

watch(() => resource.value?.doc, (d) => {
  if (d != null) {
    emit("resolve", d);
  }
});

const emit = defineEmits(["resolve", "reject"])

let open = ref(true);


function onClose() {
  emit("resolve", false);
}

async function onSeachCustomer() {

  if (customerCode != "") {
    resource.value = createDocumentResource({
      url: 'frappe.client.get',
      doctype: 'Customer',
      name: customerCode.value,
      auto: true,

    })



  }

}

</script>