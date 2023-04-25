<template>
  <ComModal :fullscreen="true" :persistent="true" @onClose="onClose" @onOk="onConfirm" :loading="resource.loading" :customActions="true">
    <template #title>
      <span>{{ props.params.title }}</span>
    </template>
    <template #content> 
          <ComSplitBillList :data="groupSales"/> 
    </template>
    <template #action>
      <v-btn variant="flat" color="error">
          Closeddd
      </v-btn>
    </template>
  </ComModal>
</template>

<script setup>
import { ref,onMounted, defineEmits, createToaster, createResource, inject } from '@/plugin'
import ComSplitBillList from './split_bill/ComSplitBillList.vue';
const emit = defineEmits(["resolve", "reject"])
const toaster = createToaster({ position: 'top' })
const sale = inject('$sale')
const resource = ref({})
const props = defineProps({
  params: {
    type: Object,
    require: true
  }
})

const groupSales=ref([])
onMounted(()=>{
  //get current sale
  groupSales.value = [];
  groupSales.value.push({
                  generate_id:sale.sale.name,
                  no:1,
                  sale: sale.sale
                });
                
              
                
  //get other sale in same table
  if (sale.sale.table_id) {
    resource.value = createResource({
      url: "frappe.client.get_list",
      params: {
        doctype: "Sale",
        fields: ["*"],
        filters: {
          name: ['!=',sale.sale.name],
          pos_profile: localStorage.getItem("pos_profile"),
          table_id: sale.sale.table_id,
          docstatus: 0
        },
        limit_page_length: 500,
      },
      auto: true,
      onSuccess(data) {  
        data.forEach(s => { 
          sale.LoadSaleData(s.name).then((v)=>{
            groupSales.value.push({
              generate_id:s.name,
                  no:1,
                  sale:v
                });
          });
        });
      }
    });
  }

});



function onConfirm() {
  emit("resolve", { data: data.value });
}

function onClose() {
  emit('resolve', false);
}

</script>