<template>
    <v-dialog v-model="open" persistent :max-width="800">
        <v-card>
            <v-toolbar color="default" title="Change Price Rule">
                <v-toolbar-items>
                    <v-btn icon @click="onClose()">
                        <v-icon>mdi-close</v-icon>
                    </v-btn>
                </v-toolbar-items>
            </v-toolbar>
            <v-card-text class="p-0">
                <div>
                    <ComInput 
                    autofocus
                    ref="searchTextField"
                    keyboard
                    class="mb-4"
                    v-model="search"
                    placeholder="Search Price Rule"
                    v-debounce="onSearch"
                    @onInput="onSearch"/>
                    <ComPlaceholder :is-not-empty="dataResource.data && dataResource.data.length > 0">
                        <div>
                            <v-chip 
                                v-for="(item, index) in dataResource.data" 
                                :key="index"
                                class="m-1" 
                                @click="onSelect(item.name)"
                                :size="mobile ? 'small' : 'default'">
                                <v-icon start icon="mdi-checkbox-marked-circle-outline" v-if="selectedPriceRule == item.name" color="orange"></v-icon>
                                {{ item.rule_name }}
                            </v-chip>
                        </div>
                    </ComPlaceholder>
                </div>
            </v-card-text>
            <v-card-actions class="justify-end">
                <v-btn variant="flat" @click="onClose(false)" color="error">
                    Close
                </v-btn>
                <v-btn variant="flat" @click="onOK()" color="primary">
                    OK
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
<script setup>
import { ref, defineEmits, createToaster, createResource, inject } from '@/plugin'
import ComPlaceholder from '../../../components/layout/components/ComPlaceholder.vue';
import { useDisplay } from 'vuetify'
const {mobile} = useDisplay()
const emit = defineEmits(['resolve'])
const props = defineProps({
    params:Object
})
const toaster = createToaster({ position: "top" })

const sale  = inject('$sale')
let open = ref(true)
let selectedPriceRule = ref(sale.setting.price_rule)
let search = ref('')
const searchFields = ref(["name","rule_name","note"]);


  const dataResource = createResource({
    url: "frappe.client.get_list",
    params: getDataResourceParams(),
    auto: true
  });

 
  function getDataResourceParams (){
    return {  
        doctype: "Price Rule",
        fields: ["name", "rule_name", "note"],
        order_by: "modified desc",
        or_filters: getFilter(),
        limit_page_length: 20
    }
  }

  function getFilter(){
    let filters = {};
     searchFields.value.forEach((r)=>{
      filters[r] = ["like",'%'+ search.value + '%']
     })
    
     return filters;
  }


  function onSearch(keyword) {
    search.value = keyword;
    dataResource.params = getDataResourceParams()
    dataResource.fetch()
  }
  function onSelect(name){
    alert(name)
  }
function onClose() {
    emit('resolve',false)
}
function onOK(){
    sale.sale.price_rule = result;
    sale.setting.price_rule = result;

    emit('resolve', true)
}
 
</script>