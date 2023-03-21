<template>
  <template v-if="gv.setting.table_groups && gv.setting.table_groups.length>0">
    <v-list-item @click="onToTableLayout()" v-if="isMobile">
        <template v-slot:prepend class="w-12">
            <v-icon icon="mdi-view-dashboard"></v-icon>
        </template>
        <v-list-item-title>Table Layout</v-list-item-title>
    </v-list-item>
    <v-btn stacked  size="small" class="m-1 grow" prepend-icon="mdi-view-dashboard" @click="onToTableLayout()" v-else>
      Table Layout
    </v-btn>
  </template>
</template>
<script setup>
import { inject, defineProps,confirmBackToTableLayout, useRouter, defineEmits } from '@/plugin'
import Enumerable from 'linq';
const sale = inject('$sale')
const gv = inject('$gv')
const router = useRouter()
const emit = defineEmits('closeModel')
const props = defineProps({
    isMobile: Boolean
})
async function onToTableLayout() {
  const sp = Enumerable.from(sale.sale.sale_products);

  if (sp.where("$.name==undefined").toArray().length > 0) {
    let result = await confirmBackToTableLayout({});
    if (result) {
      if (result == "hold" || result == "submit") {
        if (result == "hold") {
          sale.sale.sale_status = "Hold Order";
          sale.action = "hold_order";
        } else {
          sale.sale.sale_status = "Submitted";
          sale.action = "submit_order";
        }
        await sale.onSubmit().then(async (value) => {
          if (value) {
            router.push({ name: "TableLayout" });
            emit('closeModel')
          }
        });
      } else {
        //continue
        sale.sale = {};
        router.push({ name: "TableLayout" })
        emit('closeModel')
      }
    }
  } else {
    sale.sale = {};
    router.push({ name: "TableLayout" })
    emit('closeModel')
  }
}
</script> 