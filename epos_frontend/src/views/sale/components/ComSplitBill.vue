<template>

  <ComModal :fullscreen="true" :persistent="true" @onClose="onClose" @onOk="onConfirm" :loading="resource.loading" :customActions="true">
    <template #title>
      <span>{{ props.params.title }}</span>
    </template>
    <template #content> 
      <ComLoadingDialog v-if="is_loading" />
          <ComSplitBillList :data="groupSales" /> 
    </template>
    <template #action>
      <v-btn variant="flat" color="error" @click="onClose()">
          {{ $t('Close') }}
      </v-btn>
      <v-btn variant="flat" color="accent" large  elevation="2" outlined  plain @click="onCreateNew()">        
          {{ $t('Create New Bill') }}        
      </v-btn>
      <v-btn variant="flat" color="success" @click="onSave()">
          {{ $t('Save') }}
      </v-btn>
 
    </template>
  </ComModal>
</template>

<script setup>  

import ComLoadingDialog from '@/components/ComLoadingDialog.vue';

import { ref,onMounted, defineEmits, createToaster, createResource, inject,i18n } from '@/plugin'
import ComSplitBillList from './split_bill/ComSplitBillList.vue';

const { t: $t } = i18n.global; 

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

let is_loading = ref(true)

const groupSales=ref([])

onMounted(()=>{ 

  //get current sale 
 const current_sale = JSON.parse(JSON.stringify(sale.sale))
 current_sale_id = current_sale.name;

  
  groupSales.value = [];
  groupSales.value.push({   
    deleted:false,
    visibled:true,
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

        values.sort(function(a,b){          
          return ('' + a.name).localeCompare(b.name);
        });

        values.forEach((v)=>{
          onGenerateTempField((v.sale_products||[]));
          groupSales.value.push({
            deleted:false,
            visibled:true,
            is_current:false,
            show_download:false,
            generate_id:uuidv4(),
            no: groupSales.value.length +1,
            sale:v
          });
        });  
        is_loading.value = false   ;   
      },
      onError(err){ 
        toaster.warning($t('msg.Sales data cannot load please reload to retry'));
        is_loading.value = false   ;   
      }      
  });  
  } else{
    is_loading.value = false   ;   
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
  const _deleted = groupSales.value.filter((r)=>r.deleted ==true && r.sale.name !="");
  let _sale = JSON.parse(JSON.stringify(sale.sale));
  _sale.name = "";
  if(_deleted.length >0){
    _sale = JSON.parse(JSON.stringify(_deleted[0].sale));
    _deleted[0].sale.name = "";
  }

  _sale.sale_products =[]; 
  _sale.commission_type= "Percent";
  _sale.commission= 0;
  _sale.commission_note= '';
  _sale.commission_amount= 0;

  const _newGroup = {
    deleted:false,
    is_current:false,
    visibled:true,
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
  //check if current sale empty data
  const _current_sale = groupSales.value.filter((r)=>r.deleted == false && r.is_current == true);
  
  if(_current_sale.length<=0){
    toaster.warning($t('msg.Bill not allow to save without items',[(_current_sale[0].no+'(#'+_current_sale[0].sale.name + ')')]));
    return;
  }else{
    if((_current_sale[0].sale.sale_products||[])<=0){
      toaster.warning($t('msg.Bill not allow to save without items',[(_current_sale[0].no+'(#'+_current_sale[0].sale.name + ')')]));
      return;
    }
  }

  is_loading.value = true;

  let _active_sales = groupSales.value.filter((r)=>r.deleted == false); 
  _active_sales.forEach((a)=>{

    //check if empty sale products in sale
    if((a.sale.sale_products||[]).length <= 0){
      a.deleted = true;
    }

    //update parent id of sale product 
    (a.sale.sale_products||[]).forEach((sp)=>{
        sp.deleted_quantity = 0;
        if(sp.parent != a.sale.name){
          sp.name = ""
        } 
    });
  });  

  const sales = [];
  groupSales.value.forEach((g)=>{
    if(g.deleted && g.sale.name == ""){
      //nothing do
    }else{
      g.sale.temp_deleted = g.deleted;
      sales.push(g.sale);
    }
  });  

  sales.sort(function(a,b){          
          return ('' + a.name).localeCompare(b.name);
  });
  
  createResource({
      url: "epos_restaurant_2023.api.split_bill.on_save",
      params:{
        data:sales,
        current_sale_id:current_sale_id
      },
      auto:true,
      onSuccess(doc){ 
        is_loading.value = false;
        sale.sale = doc ;
        toaster.success($t('msg.Split bill successfully'));
        emit("resolve", true);
      },
      onError(err){ 
        is_loading.value = false;
        toaster.warning($t('msg.there are some trouble during save so please reload data to retry'));
        // emit("resolve", false);
      }      
  });  
}

function onClose() {
  emit('resolve', false);
}

</script>