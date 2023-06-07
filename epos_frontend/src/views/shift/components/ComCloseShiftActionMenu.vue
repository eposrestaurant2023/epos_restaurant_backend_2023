<template>
    <v-menu>
        <template v-slot:activator="{ props }">
            <v-btn color="primary" v-bind="props">
                {{ $t('View Shift Report') }}
            </v-btn>
        </template>

        <v-list v-if="reportList.data">
            <v-list-item v-for="(r, index) in reportList.data" :key="index" @click="onViewReport(r)">
                <v-list-item-title>{{ r.name }}</v-list-item-title>
            </v-list-item>

        </v-list>
    </v-menu>
    <ComPrintButton doctype="Cashier Shift" />
 
</template>
<script setup>
import { createResource,defineProps,i18n } from '@/plugin';
import { printPreviewDialog } from '@/utils/dialog';
import ComPrintButton from '@/components/ComPrintButton.vue';

const { t: $t } = i18n.global;
const props = defineProps({ name:String})


const reportList = createResource({
    url: "epos_restaurant_2023.api.api.get_pos_print_format",
    params: {
        doctype: "Cashier Shift"
    },
    auto: true,

})

function onViewReport(r){
    printPreviewDialog( { title:$t('Cashier Shift Report') + " #" + props.name, doctype: "Cashier Shift", name: props.name,"report": r});

}
function onPrintReport(r){
    
    window.chrome.webview.postMessage("ss")
}

</script>