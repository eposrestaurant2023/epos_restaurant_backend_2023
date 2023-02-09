<template>
    <v-dialog v-model="open" fullscreen @update:modelValue="onClose">
        <v-card v-if="reportList.data">
            <ComToolbar @onClose="onClose(false)" :isPrint="true" @onPrint="onPrint">
                <template #title>
                    {{ params.title }}
                </template>
                <template #more_menu>
                    <v-list density="compact">
                        <v-list-item>
                            <template v-slot:prepend>
                                <v-icon color="error">mdi-delete</v-icon>
                            </template>
                            <v-list-item-title>Delete</v-list-item-title>
                        </v-list-item>
                    </v-list>
                </template>
            </ComToolbar>
            <v-card-text>
                <v-card  class="mx-auto my-0 pa-4">
                
                    <v-select
                    label="Select a Report"
                    :items="reportList.data"
                    variant="solo"
                    v-model="selectedReport"
                    item-title="name"
                    item-value="name"
                  
                    ></v-select>
                    <v-select
                    v-if="letterHeadList.data"
                    label="Select a Letterhead"
                    :items="letterHeadList.data"
                    variant="solo"
                    v-model="selectedLetterhead"
                 
                    ></v-select>
                    <v-btn @click="onViewReport">View Report</v-btn>
                   
                     <iframe  style="height:calc(100vh - 130px)"   width="100%" :src="reportUrl + '&trigger_print=' + triggerPrint"></iframe>
                </v-card>
            </v-card-text>
        </v-card>
    </v-dialog>

</template>
  
<script setup>

import { createResource,ref} from '@/plugin'

import ComToolbar from './ComToolbar.vue';
import { webserver_port } from "../../../../../sites/common_site_config.json"
  
const serverUrl = window.location.protocol + "//" +  window.location.hostname + ":" +  webserver_port;
 
const props = defineProps({ 
    params:{
        type:Object,
        require:true
    }
})

 

const emit = defineEmits(["resolve"])


let open = ref(true);
const selectedReport = ref("");
const selectedLetterhead = ref("");
 
const reportUrl = ref("")
const triggerPrint = ref(0)


if(props.params.print){
    triggerPrint.value = 1;
}else {
    triggerPrint.value = 0;
}



const setting = JSON.parse(localStorage.getItem("setting"))
 
const reportList =  createResource({
    url: "epos_restaurant_2023.api.api.get_pos_print_format",
    params: {
        doctype: props.params.doctype
    },
    auto: true,
    onSuccess(data){
        if(props.params.report){
            selectedReport.value= props.params.report;
        }else{
            selectedReport.value= data[0];
        }
         
        if (props.params.print){
            onViewReport(1);
        }else {
            onViewReport();
        }
     
      

    }

})
const letterHeadList =  createResource({
    url: "epos_restaurant_2023.api.api.get_pos_letter_head",
    params: {
        doctype: props.params.doctype
    },
    auto: true,
 
})

function onViewReport(isPrint =0){
  
    reportUrl.value  = serverUrl + "/printview?doctype=" + props.params.doctype + "&name=" + props.params.name + "&format="+ selectedReport.value +"&no_letterhead=0&letterhead=Defualt%20Letter%20Head&settings=%7B%7D&_lang=en&d=" + new Date()
    
   triggerPrint.value=isPrint;

      
}
function onClose(isClose) {
    emit('resolve',isClose);
}

function onPrint() {
    if (localStorage.getItem("is_window")) {
       // window.chrome.webview.postMessage(JSON.stringify(sale));
    } else {
        onViewReport(1);
    }

}
</script>

