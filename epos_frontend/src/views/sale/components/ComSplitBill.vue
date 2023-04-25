<template>
  <ComModal :fullscreen="true" :persistent="true" @onClose="onClose" @onOk="onConfirm" :loading="resource.loading">
    <template #title>
      <span>{{ props.params.title }}</span>
    </template>
    <template #content>
        <div v-for="(item, index) in groupSales" :key="index">    
          <h1>{{ item.sale.name}}</h1>    
          <div v-for="(sp, _index) in item.sale.sale_products??[]" :key="_index">    
            <div style="margin:5px; padding-left: 10px;">
              <ComSplitBillSaleProductCard :sale-product="sp"/>
            </div>
          </div> 
          <ComSplitBillSaleSummary :sale="item.sale"/>
        </div>
    </template>
    <template #action>

    </template>
  </ComModal>
</template>

<script setup>
import { ref,onMounted, defineEmits, createToaster, createResource, inject } from '@/plugin'
import ComSplitBillSaleProductCard from './split_bill/ComSplitBillSaleProductCard.vue';
import ComSplitBillSaleSummary from "./split_bill/ComSplitBillSaleSummary.vue";
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


// let data = ref(JSON.parse(JSON.stringify(props.params.data)))
onMounted(()=>{
  if (sale.sale.table_id) {
    resource.value = createResource({
      url: "frappe.client.get_list",
      params: {
        doctype: "Sale",
        fields: ["*"],
        filters: {
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