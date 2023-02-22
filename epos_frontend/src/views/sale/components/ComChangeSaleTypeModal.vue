<template>
  <v-dialog v-model="open" @update:modelValue="onClose">
    <v-card class="mx-auto my-2 py-2 w-80 max-w-sm" title="Change Sale Type">
      <v-card-text> 
        <v-list>
          <v-list-item v-for="(item, index) in saleTypeResource.data" :key="index" class="!p-0">
            <v-btn class="w-full text-white" variant="flat"  @click="onChangeSaleType(item)" :color="item.color">
              {{ item.name }}
            </v-btn>
          </v-list-item>
        </v-list>
      </v-card-text>
      <v-card-actions class="text-right">
        <v-spacer></v-spacer>
        <v-btn variant="flat" @click="onClose" color="error">
          Cancel
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
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
let open = ref(true)

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
  