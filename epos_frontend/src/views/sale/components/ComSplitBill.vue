<template>
  <ComModal :fullscreen="true" :persistent="true" @onClose="onClose" @onOk="onConfirm" :loading="resource.loading" :customActions="true">
    <template #title>
      <span>{{ props.params.title }}</span>
    </template>
    <template #content> 
          <ComSplitBillList :data="groupSales" /> 
    </template>
    <template #action>
      <v-btn variant="flat" color="error" @click="onClose()">
          Close
      </v-btn>
      <v-btn variant="flat" color="primary" @click="onCreateNew()">
          New Bill
      </v-btn>
      <v-btn variant="flat" color="success" @click="onSave()">
          Save
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
    is_current:true,
    show_download:false,
    generate_id:uuidv4(),
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
              is_current:false,
              show_download:false,
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

function onCreateNew(){ 
  const _sale = JSON.parse(JSON.stringify(sale.sale));
  _sale.name = "";
  _sale.sale_products =[]; 
  const _newGroup = {
    generate_id:uuidv4(),
    no:groupSales.value.length + 1,
    sale: _sale
  }
  groupSales.value.push(_newGroup);
}

function uuidv4() {
  return ([1e7]+-1e3+-4e3+-8e3+-1e11).replace(/[018]/g, c =>
    (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
  );
}



function onSave() {
  emit("resolve", { data: data.value });
}

function onClose() {
  emit('resolve', false);
}

</script>