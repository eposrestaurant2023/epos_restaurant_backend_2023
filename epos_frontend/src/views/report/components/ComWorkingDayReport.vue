<template>
    <v-row>
        <v-col md="3">
            <v-card subtitle="Working Day & Cashier Shift Report">
                <v-card-text>
                    <ComPlaceholder :loading="data.loading" :is-not-empty="data.data?.length > 0">
                        <template v-for="(c, index) in data.data" :key="index">
                            <v-card :variant="activeReport.working_day == c.name ? 'tonal' : 'text'" class="bg-gray-200 my-2 subtitle-opacity-1">
                                <template v-slot:title>
                                    <div class="flex justify-between">
                                        <div @click="onSelect(c)">{{ c.name }}</div>
                                        <div>
                                            <v-chip v-if="c.is_closed" color="error" size="small"
                                                variant="elevated">Closed</v-chip>
                                            <v-chip v-else color="success" size="small" variant="elevated">Opening</v-chip>
                                        </div>
                                    </div>
                                </template>
                                <template v-slot:subtitle>
                                    <div>
                                        <div><v-icon icon="mdi-calendar" size="x-small" /> <span class="font-bold">{{
                                            c.posting_date
                                        }}</span> was opening by <span class="font-bold">{{ c.owner }}</span></div>
                                        <div v-if="c.is_closed">
                                            <v-icon icon="mdi-calendar-multiple" size="x-small" /> <span
                                                class="font-bold">{{ c.closed_date }}</span> was closed by <span
                                                class="font-bold">{{ c.modified_by }}</span>
                                        </div>
                                        <div><v-icon icon="mdi-note-text" size="x-small"></v-icon> Total Shift: <span
                                                class="font-bold">{{ c.total_cashier_shift }}</span></div>
                                    </div>
                                </template>
                                <v-card-actions v-if="activeReport.working_day == c.name">
                                    <div class="-m-1">
                                        <v-btn color="primary" variant="tonal" stacked class="m-1" v-for="(item, index) in c.cashier_shifts" :key="index" @click="onCashierShift(item.name)">
                                            <div>{{ moment(item.creation).format('h:mm:ss A') }}</div>
                                            <div class="text-xs">#{{ item.name }}</div>
                                        </v-btn>
                                    </div>
                                </v-card-actions>
                            </v-card>
                        </template>
                    </ComPlaceholder>
                </v-card-text>
            </v-card>
        </v-col>
        <v-col md="9">
            <v-card>
                <template #title>
                    <div class="px-1 py-2 -m-1"> 
                        <div class="flex justify-between">
                            <div v-if="!reports.loading && reports.data?.length > 0"> 
                                <v-btn v-for="(r, index) in reports.data" :key="index" class="m-1" size="small" @click="onPrintFormat(r.name)">{{ r.title }}</v-btn>
                            </div>
                            <v-icon icon="mdi-refresh" size="small" @click="onRefresh"/>
                        </div>
                    </div>
                </template>
                <v-card-text style="height: calc(100vh - 230px);">
                    {{ printUrl }}
     
                        <iframe height="100%" width="100%"
                        :src="printUrl"></iframe>
      
                    <!-- <div v-else class="p-6 flex justify-center items-center">
 
                        <div>
                            <v-icon class="m-2" icon="mdi-spin mdi-loading" size="x-large"></v-icon>
                            <div class="text-gray-400">loading</div>
                        </div>
                    </div> -->
                </v-card-text>
            </v-card>
        </v-col>
    </v-row>
</template>
<script setup>
import { createResource, inject, computed,ref,nextTick } from '@/plugin'
import { webserver_port } from "../../../../../../../sites/common_site_config.json"
const gv = inject('$gv')
const moment = inject('$moment')
const serverUrl = window.location.protocol + "//" + window.location.hostname + ":" + webserver_port;
 
let activeReport = ref({
    working_day: '',
    cashier_shift: '',
    report_name: 'Close Shift Summary Report',
    loading: false
})
const printUrl = computed(()=>{
    return `${serverUrl}/printview?doctype=Cashier%20Shift&name=${activeReport.value.cashier_shift}&format=${activeReport.value.report_name}&no_letterhead=0&show_toolbar=0&letterhead=Defualt%20Letter%20Head&settings=%7B%7D&_lang=en`
})
const data = createResource({
    url: "epos_restaurant_2023.api.api.get_working_day_list_report",
    auto: true,
    onSuccess(doc){
        if(doc.length > 0){
            activeReport.value.working_day = doc[0].name
        }
    }
})
const reports = createResource({
    url: 'frappe.client.get_list',
    params: {
        doctype: "Print Format",
        fields: ['name','title'],
        filters: {
            show_in_pos_report: 1,
            doc_type: 'Cashier Shift'
        },
        order_by: 'sort_order asc'
    },
    auto: true,
    
})
 
function onCashierShift(name){
    activeReport.value.loading = true 
    activeReport.value.cashier_shift = name
    activeReport.value.loading = false
}
function onPrintFormat(name){
    activeReport.value.loading = true 
    activeReport.value.report_name = name
    activeReport.value.loading = false
}
function onRefresh(){
    alert('refresh')
}
</script>
<style>
.subtitle-opacity-1 .v-card-subtitle {
    opacity: 1 !important;
}</style>