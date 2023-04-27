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

  const result =  groupSales.value.flatMap(a => (a.sale.sale_products||[]));
  result.forEach((sp)=>{
      sp.original_quantity = sp.quantity;
      sp.original_id = sp.generate_id = uuidv4();
      sp.original_parent = sp.parent;
      sp.original_name = sp.name;
  });

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

  onDownloadPressed(_newGroup);
}

function uuidv4() {
  return ([1e7]+-1e3+-4e3+-8e3+-1e11).replace(/[018]/g, c =>
    (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
  );
}

function onDownloadPressed(group){
  const result = groupSales.value.flatMap(a => (a.sale.sale_products||[]).filter((r)=>(r.total_selected||0) >0));
  const temp =[];
  result.forEach((sp)=>{    
    const _sp = JSON.parse(JSON.stringify(sp));
    sp.quantity -=  sp.total_selected; 
    if(sp.quantity <=0){      
      temp.push(sp);
    }
    else{ 
      _sp.name ="";
      _sp.quantity =  sp.total_selected ;     
    }
    _sp.total_selected  = sp.total_selected = 0;
    group.sale.sale_products.push(_sp);
  }); 
 
  //remove sale product when qty equal zero
  temp.forEach((t)=>{   
    groupSales.value.filter((x)=>x.no != group.no).forEach((z)=>{
      let sp = z.sale.sale_products;
      if(sp.filter((y)=>y.quantity==0).length > 0){
        sp.splice(sp.indexOf(t), 1);      
      }
    });    
  }); 

  //end remove sale product when qty equal zero
}


function onSave() {
  emit("resolve", { data: data.value });
}

function onClose() {
  emit('resolve', false);
}

</script>