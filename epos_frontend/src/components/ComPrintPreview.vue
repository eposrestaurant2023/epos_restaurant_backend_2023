<template>
    <ComModal @onClose="onClose(false)" :fullscreen="true" :isPrint="true" @onPrint="onPrint()"  :hide-ok-button="true" :hide-close-button="true">
        <template #title>
            {{ params.title }}
        </template>
        <template #bar_more_button>
          
        </template>
        <template #content>
            <v-card>

                <template #title>
                    <div class="px-1 py-2 -m-1"> 
                        <div class="flex justify-between">
                            <div> 
                                     <v-btn 
                                        v-for="(r, index) in gv.setting.reports.filter(r=>r.doc_type==params.doctype && r.show_in_pos == 1)" :key="index"  
                                        :color="activeReport.name == r.name ? 'info' : 'default'"
                                        class="m-1" @click="onViewReport(r)">{{ r.title }}</v-btn>
                                 
                            </div>
                            <div class="flex items-center"> 
                                <v-select 
                                prepend-inner-icon="mdi-content-paste"
                                density="compact"
                                v-model="selectedLetterhead"
                                :items=gv.setting.letter_heads
                                item-title="name"
                                item-value="name"
                                hide-no-data
                                hide-details
                                variant="solo"
                                class="mx-1"
                                ></v-select>
                                <v-select 
                                prepend-inner-icon="mdi-google-translate"
                                density="compact"
                                v-model="selectedLang"
                                :items="gv.setting.lang" 
                                item-title="language_name"
                                item-value="language_code"
                                hide-no-data
                                hide-details
                                variant="solo"
                                class="mx-1"
                               
                                ></v-select>
                                <v-icon class="mx-1" icon="mdi-refresh" size="small" @click="onRefresh()"/>
                            </div>
                        </div>
                    </div>
                </template>
                <v-card-text style="height: calc(100vh - 200px);">
                    
                    <iframe id="report-view" height="100%" width="100%" :src="printPreviewUrl"></iframe>
                </v-card-text>
            </v-card>
        </template>
    </ComModal>
</template>
  
<script setup>

import { inject, ref,computed,saleDetailDialog, onUnmounted } from '@/plugin'
import { createToaster } from '@meforma/vue-toaster';
import { webserver_port } from "../../../../../sites/common_site_config.json"
const gv = inject("$gv")

const serverUrl = window.location.protocol + "//" + window.location.hostname + ":" + webserver_port;

const toaster = createToaster({position:"top"})

const props = defineProps({
    params: {
        type: Object,
        require: true
    }
})
const selectedLetterhead = ref(getDefaultLetterHead());
const selectedLang = ref(gv.setting.lang[0].language_code);
const activeReport = ref(gv.setting.reports.filter(r=>r.doc_type==props.params.doctype)[0]) ;

const printPreviewUrl = computed(()=>{
    let  letterhead = "";
    if(selectedLetterhead.value==""){
           letterhead = getDefaultLetterHead();
    }else {
        letterhead = selectedLetterhead.value;
    }
    const url =`${serverUrl}/printview?doctype=${activeReport.value.doc_type}&name=${props.params.name}&format=${activeReport.value.name}&no_letterhead=0&show_toolbar=0&letterhead=${letterhead}&settings=%7B%7D&_lang=${selectedLang.value}`; 
    return url;
})


function getDefaultLetterHead(){
    let  letterhead = "";
    
           letterhead = gv.setting.letter_heads.filter(r=>r.is_default==1)[0]?.name;
        if(!letterhead){
            letterhead = "No Letterhead";
        }
   return letterhead;
}

const emit = defineEmits(["resolve"])



 
const triggerPrint = ref(0)


if (props.params.print) {
    triggerPrint.value = 1;
} else {
    triggerPrint.value = 0;
}
 
 
 
function onViewReport(r){
    activeReport.value = r;
}

function onClose(isClose) {
    emit('resolve', isClose);
}

function onRefresh(){
  
    document.getElementById("report-view").contentWindow.location.replace(printPreviewUrl.value)
 
}

function onPrint() {
    if (localStorage.getItem("is_window")==1) {
        if(props.params.doctype =="Sale" && activeReport.value.pos_receipt_file_name !="" && activeReport.value.pos_receipt_file_name !=null){
            alert("Print bill silence")
            window.chrome.webview.postMessage("doc");
            return;
        }
        
    }
    
    window.open(printPreviewUrl.value + "&trigger_print=1").print();
    window.close();

}


const reportClickHandler = async function (e) {
    if(e.isTrusted && typeof(e.data) == 'string'){
       
        const data = e.data.split("|")
        
        if(data.length>0){
      
            if(data[0]=="view_sale_detail"){
                saleDetailDialog({
            name:data[1]
            });
            
            }
        
        }
        
    }
};

window.addEventListener('message', reportClickHandler, false);

onUnmounted(() => {
    window.removeEventListener('message', reportClickHandler, false);
}) 

</script>

