<template>
  <v-menu>
    <template v-slot:activator="{ props }">
      <v-chip v-bind="props" style="margin: 1px;" :size="screen.chipSize" rounded="pill" variant="tonal" append-icon="mdi-arrow-down-drop-circle-outline"
        :color="saleType?.color">
        {{ sale.sale.sale_type }}
      </v-chip>
    </template>
    <v-list>
      <v-list-item v-for="(item, index) in saleTypeResource.data" :key="index" @click="onChangeSaleType(item)">
        <v-list-item-title>{{ item.name }}</v-list-item-title>
      </v-list-item>
    </v-list>
  </v-menu>


</template>
<script setup>
import { createResource, inject, computed } from "@/plugin";
const sale = inject("$sale")
const screen = inject("$screen")
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
  }
}

</script>