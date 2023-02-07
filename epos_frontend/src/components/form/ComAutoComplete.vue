<template> 
  <v-autocomplete 
      v-model="select"
      :clearable="clearable"
      v-model:search="search"
      :loading="doctypeResource.loading"
      :items="items"
      density="compact"
      hide-no-data
      hide-details
      :variant="variant"
      :placeholder="doctype"
      :custom-filter="OnFilter"
      :item-title="getTitleField()"
      item-value="name"
      :menu-props="{ maxHeight: 500 }"
      @click:clear="onClear"
  >

  <template v-slot:item="{ props, item }">
    <v-list-item class="py-2" v-if="meta.image_field" v-bind="props" 
      :prepend-avatar="item.raw[meta.image_field]?item.raw[meta.image_field]:'files/nophoto.png'"
      :title="item.raw[meta.title_field?meta.title_field:'name']"
      :subtitle="getSubTitle(item.raw)">
    </v-list-item> 
    <v-list-item class="py-2" v-else v-bind="props" 
      
      :title="item.raw[meta.title_field?meta.title_field:'name']"
      :subtitle="getSubTitle(item.raw)">
    </v-list-item> 
  </template>
  </v-autocomplete> 
</template>
<script setup>
import { watch,reactive, ref, defineProps, onMounted, createResource } from '@/plugin'
let props = defineProps({ 
  doctype: String,
  clearable: {
    type: Boolean,
    default: true
  },
  variant: {
    type: String,
    default: 'solo'
  }
})

const items = ref([])
const search = ref("")
const select = ref("")
const meta = ref({})
let filter = reactive({})
const fields = reactive([])
const doctypeResource = createResource({
  url: "frappe.client.get_list",
  params:doctypeParams(),
  onSuccess(data) {
    items.value = data;
    
  }
});

function  doctypeParams(){
  return {
    doctype: props.doctype,
    fields:fields,
    or_filters:filter
  }
}  

function onClear(){
  select.value ="";
}
onMounted(async () => {
  let metaResource = createResource({
    url: "epos_restaurant_2023.api.api.get_meta",
    params: {
      doctype: props.doctype
    },

  })
  meta.value = await metaResource.fetch();
  fields.push("name")
  if(meta.value.title_field){
    fields.push(meta.value.title_field)
  }
  
  if(meta.value.image_field){
    fields.push(meta.value.image_field)
  }
  
  if(meta.value.search_fields){
    meta.value.search_fields.split(",").forEach(function(d){
      fields.push(d.trim())
    });
  }

  doctypeResource.fetch();

  watch(search, (currentValue, oldValue) => {
    if(currentValue!=oldValue ){ 
     
      fields.forEach((r)=>{
        
        filter[r] = ["like",'%'+ currentValue + '%']
      })
      doctypeResource.params = doctypeParams();
      doctypeResource.fetch();
     
    }
  });
});

function getSubTitle(d){
  let titles = []
  if(meta.value.search_fields){
    meta.value.search_fields.split(",").forEach(function(r){
      if(d[r]){
        titles.push(d[r])
      }
    });
  }
  if(titles.length>0){
    return titles.join()
  }
  return "";
}
function OnFilter(value, query,item){
  let titles = []
    fields.forEach(function(r){
      if(item.raw[r]){
        titles.push(item.raw[r])
      }
    });
 
  return String(titles.join(" ")).toLocaleLowerCase().includes(search.value.toLocaleLowerCase())
  
}

function getSelectiontext(d){
  if (meta.value.title_field){
    if(d[meta.value.title_field]){
      return d[meta.value.title_field];
    }
  } 
    return d.name;
  
}
function getTitleField(){
  if (meta.value.title_field){
    return meta.value.title_field;
  }else {
    return "name";
  }
}

</script>