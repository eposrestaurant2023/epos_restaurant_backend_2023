<template>
      <PageLayout title="Close Receipt List" icon="mdi-note-outline" full>
    <v-card-text style="height: calc(100vh - 145px);">
                    <iframe id="report-view" height="100%" width="100%" src="http://192.168.10.114:1216/printview?doctype=Working%20Day&name=WD2023-0016&format=Working%20Day%20Summary%20Preview%20Report&no_letterhead=1&letterhead=Defualt%20Letter%20Head&settings=%7B%7D&_lang=en&show_toolbar=0&view=ui"></iframe>
                </v-card-text>
  </PageLayout>
</template>
  
<script setup>
import PageLayout from '@/components/layout/PageLayout.vue';
import { inject, ref,computed,saleDetailDialog, onUnmounted } from '@/plugin'
import { createToaster } from '@meforma/vue-toaster';
import { webserver_port } from "../../../../../../sites/common_site_config.json"
const gv = inject("$gv")

const serverUrl = window.location.protocol + "//" + window.location.hostname + ":" + webserver_port;

const toaster = createToaster({position:"top"})


const selectedLetterhead = ref(getDefaultLetterHead());
const selectedLang = ref(gv.setting.lang[0].language_code);

// const printPreviewUrl = computed(()=>{
//     let  letterhead = "";
//     if(selectedLetterhead.value==""){
//            letterhead = getDefaultLetterHead();
//     }else {
//         letterhead = selectedLetterhead.value;
//     }
//     const url =`${serverUrl}/printview?doctype=${activeReport.value.doc_type}&name=${props.params.name}&format=${activeReport.value.name}&no_letterhead=0&show_toolbar=0&letterhead=${letterhead}&settings=%7B%7D&_lang=${selectedLang.value}`; 
//     return url;
// })


function getDefaultLetterHead(){
    let  letterhead = "";
    
           letterhead = gv.setting.letter_heads.filter(r=>r.is_default==1)[0]?.name;
        if(!letterhead){
            letterhead = "No Letterhead";
        }
   return letterhead;
}

 
 
function onRefresh(){
  
    document.getElementById("report-view").contentWindow.location.replace(printPreviewUrl.value)
 
}

// function onPrint() {
//     if (localStorage.getItem("is_window")==1) {
//         if(props.params.doctype =="Sale" && activeReport.value.pos_receipt_file_name !="" && activeReport.value.pos_receipt_file_name !=null){
//             alert("Print bill silence")
//             window.chrome.webview.postMessage("doc");
//             return;
//         }
        
//     }
    
//     window.open(printPreviewUrl.value + "&trigger_print=1").print();
//     window.close();

// }


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

