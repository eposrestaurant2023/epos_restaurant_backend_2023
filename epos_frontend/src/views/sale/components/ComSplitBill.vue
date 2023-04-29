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

import { ref,onMounted, defineEmits, createToaster, createResource,createDocumentResource, inject } from '@/plugin'
import ComSplitBillList from './split_bill/ComSplitBillList.vue';
import { onUnmounted } from 'vue';
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

let current_sale_id ="";

const groupSales=ref([])

onMounted(()=>{ 
  //get current sale 
 const current_sale = JSON.parse(JSON.stringify(sale.sale))
 current_sale_id = current_sale.name;

  
  groupSales.value = [];
  groupSales.value.push({   
    deleted:false,
    is_current:true,
    show_download:false,
    generate_id:uuidv4(),
    no:1,
    sale: current_sale
  }); 
  
  
  //generate temp field 
  onGenerateTempField((current_sale.sale_products||[]));

  //get other sale in same table
  if (current_sale.table_id) {
    createResource({
      url: "epos_restaurant_2023.api.split_bill.get_sales",
      params:{
        sale_id:current_sale_id
      },
      auto:true,
      onSuccess(values){ 
        console.log(values)
        values.forEach((v)=>{
          onGenerateTempField((v.sale_products||[]));
          groupSales.value.push({
            deleted:false,
            is_current:false,
            show_download:false,
            generate_id:uuidv4(),
            no: groupSales.value.length +1,
            sale:v
          });
        });        
      },
      onError(err){ 
        toaster.warning("Sales cannot load, please reload for try again.");
      }      
  });  
  }  
});
 

function onGenerateTempField(sale_products){
  sale_products.forEach((sp)=>{
    sp.original_quantity = sp.quantity;
    sp.original_id = sp.generate_id = uuidv4();
    sp.original_parent = sp.parent;
    sp.original_name = sp.name;
  });
}

function onCreateNew(){ 
  const deleted = groupSales.value.filter((r)=>r.deleted ==true && r.sale.name !="");
  const _sale = JSON.parse(JSON.stringify(sale.sale));
  _sale.name = "";
  if(deleted.length >0){
    _sale.name = deleted[0].sale.name;
    deleted[0].sale.name = "";
  }
 
  _sale.sale_products =[]; 
  const _newGroup = {
    deleted:false,
    is_current:false,
    show_download:false,
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
  //
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
    //set sale product parent id
    _sp.parent = "";
      if(_sp.original_parent == group.sale.name){
          _sp.parent = _sp.original_parent;
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
  let _active_sales = groupSales.value.filter((r)=>r.deleted == false); 
  _active_sales.forEach((a)=>{

    //check if empty sale products in sale
    if((a.sale.sale_products||[]).length <= 0){
      a.deleted = true;
    }

    //update parent id of sale product 
    (a.sale.sale_products||[]).forEach((sp)=>{
        sp.parent = a.name;
    });
  });  

  const sales = [];
  _active_sales.forEach((g)=>{
    g.sale.temp_deleted = g.deleted;
    sales.push(g.sale);
  });   

  createResource({
      url: "epos_restaurant_2023.api.split_bill.on_save",
      params:{
        data:sales,
        current_sale_id:current_sale_id
      },
      auto:true,
      onSuccess(doc){ 
        sale.sale = doc ; 
        toaster.success("Split was successed.");
      },
      onError(err){ 
        toaster.warning("There're some trouble during save, please reload data and try again.");
      }      
  });  
  emit("resolve", false);
}

function onClose() {
  emit('resolve', false);
}

</script>