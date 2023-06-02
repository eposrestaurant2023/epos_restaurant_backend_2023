<template>
  <ComModal @onClose="onClose()" :hide-ok-button="true" width="400px">
    <template #title>
      <div>{{ $t('Change Sale Type') }}</div>
    </template>
    <template #content>
      <v-list>
          <v-list-item v-for="(item, index) in saleTypeResource.data" :key="index" class="!p-0">
            <v-btn class="w-full text-white mb-3" variant="flat" size="large" @click="onChangeSaleType(item)" :color="item.color">
              {{ item.name }}
            </v-btn>
          </v-list-item>
        </v-list>
    </template>
  </ComModal>
</template>
  
<script setup>
import { ref } from "@/plugin"
import { createResource, inject, computed } from "@/plugin";
import { useDisplay } from 'vuetify'
const { mobile } = useDisplay()
const props = defineProps({
  params: {
    type: Object,
    require: true
  }
})
const emit = defineEmits(["resolve"])

function onClose() {
  emit('resolve', false);
}

const sale = inject("$sale")
let saleTypeResource = createResource({
  url: "frappe.client.get_list",
  params: {
    doctype: "Sale Type",
    fields: ["name", "color", "is_order_use_table"],

  },
  cache: "sale_type",
  auto: true
})

const saleType = computed(() => {
  if (saleTypeResource.data) {
    return saleTypeResource.data.find(r => r.name == sale.sale.sale_type);
  }
  return { "name": sale.sale.sale_type }
})


function onChangeSaleType(s) {
  if (!sale.isBillRequested()) {
    sale.sale.sale_type = s.name;
    emit('resolve', true);
  }
}

</script>
  