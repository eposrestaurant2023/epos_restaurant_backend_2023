 <template>
    <PageLayout class="pb-4" :title="`${$t(activeReport.doc_type)} #${activeReport.report_id}`" icon="mdi-chart-bar" full>
        <template #action>
            <v-btn icon="mdi-printer" @click="onPrint()"></v-btn>
        </template>
    <v-row>
        <v-col md="3">
            <v-card :subtitle="$t('Working Day and Cashier Shift Report')">
                <v-card-text>
                    <ComPlaceholder :loading="workingDayReportsTmp.loading === true || workingDayReports === null " :is-not-empty="workingDayReports?.length > 0">
                        <template v-for="(c, index) in workingDayReports" :key="index">
                            <v-card :color="activeReport.report_id == c.name ? 'info' : 'default'" :variant="activeReport.report_id == c.name || c.cashier_shifts.find(r=>r.name == activeReport.report_id) ? 'tonal' : 'text'" class="bg-gray-200 my-2 subtitle-opacity-1" @click="onWorkingDay(c)">
                                <template v-slot:title>
                                    <div class="flex justify-between">
                                        <div>{{ c.name }}</div>
                                        <div>
                                            <v-chip v-if="c.is_closed" color="error" size="small"
                                                variant="elevated">{{ $t('Closed') }}</v-chip>
                                            <v-chip v-else color="success" size="small" variant="elevated">{{ $t('Opening') }}</v-chip>
                                        </div>
                                    </div>
                                </template>
                                <template v-slot:subtitle>
                                    <div>
                                        <div><v-icon icon="mdi-calendar" size="x-small" /> <span class="font-bold">{{
                                            c.posting_date
                                        }}</span> {{ $t('was opening by') }} <span class="font-bold">{{ c.owner }}</span></div>
                                        <div v-if="c.is_closed">
                                            <v-icon icon="mdi-calendar-multiple" size="x-small" /> <span
                                                class="font-bold">{{ c.closed_date }}</span> {{ $t('was closed by') }} <span
                                                class="font-bold">{{ c.modified_by }}</span>
                                        </div>
                                        <div><v-icon icon="mdi-note-text" size="x-small"></v-icon> {{ $t('Total Shift') }}: <span
                                                class="font-bold">{{ c.cashier_shifts.length }}</span></div>
                                    </div>
                                </template>
                            </v-card>
                            <div v-if="activeReport.report_id == c.name || c.cashier_shifts.find(r=>r.name == activeReport.report_id)">
                                <div class="-m-1">
                                    <v-btn :color="item.name == activeReport.report_id ? 'info' : 'default'" variant="tonal" stacked class="m-1" v-for="(item, index) in c.cashier_shifts" :key="index" @click="onCashierShift(item)">
                                        <div>{{ moment(item.creation).format('h:mm:ss A') }}</div>
                                        <div class="text-xs">#{{ item.name }}</div>
                                    </v-btn>
                                </div>
                            </div>
                            <div class="pt-2">
                                <hr/>
                            </div>
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
                            <div> 
                                <div v-if="!cashierShiftReports.loading && cashierShiftReports.data?.length > 0 && activeReport.name == 'Cashier Shift'"> 
                                    <v-btn v-for="(r, index) in cashierShiftReports.data" :key="index" :color="activeReport.preview_report == r.name ? 'info' : 'default'" class="m-1" @click="onPrintFormat(r)">{{$t(r.title)  }}</v-btn>
                                </div>
                                <div v-else-if="workingDay?.length > 0 && activeReport.name == 'Working Day'">                                    
                                    <v-btn v-for="(r, index) in workingDay" :key="index" class="m-1" :color="activeReport.preview_report == r.name ? 'info' : 'default'" @click="onPrintFormat(r)">{{ $t(r.title)  }}</v-btn>
                                </div>
                            </div>
                            <div class="flex items-center"> 
                                <v-select 
                                prepend-inner-icon="mdi-content-paste"
                                density="compact"
                                v-model="activeReport.letterhead"
                                :items= gv.setting.letter_heads
                                item-title="name"
                                item-value="name"
                                hide-no-data
                                hide-details
                                variant="solo"
                                class="mx-1"
                                @update:modelValue="onRefresh"
                                ></v-select>
                                <v-select 
                                prepend-inner-icon="mdi-google-translate"
                                density="compact"
                                v-model="activeReport.lang"
                                :items="lang" 
                                item-title="language_name"
                                item-value="language_code"
                                hide-no-data
                                hide-details
                                variant="solo"
                                class="mx-1"
                                @update:modelValue="onRefresh"
                                ></v-select>
                                <v-icon class="mx-1" icon="mdi-refresh" size="small" @click="onRefresh"/>
                            </div>
                        </div>
                        <div class="flex pt-2 px-2 items-center border-t border-gray-300 mt-2" v-if="activeReport.preview_report == 'Working Day Inventory Transaction' || activeReport.preview_report == 'Cashier Inventory Transaction'">
                            <div class="flex-grow">
                                <div style="max-width: 250px;">
                                    <ComAutoComplete v-model="filter.product_category" doctype="Product Category" variant="solo" @onSelected="onFilter"/> 
                                </div>
                            </div>
                            <div class="flex-none">
                                <v-btn prepend-icon="mdi-filter" color="primary" @click="onFilter">{{ $t('Filter') }}</v-btn>
                            </div>
                        </div>
                    </div>
                </template>
                <v-card-text style="height: calc(100vh - 230px);">
                    <iframe v-if="(activeReport.doc_type !='')" id="report-view" height="100%" width="100%" :src="printPreviewUrl"></iframe>
                </v-card-text>
            </v-card>
        </v-col>
    </v-row>
</PageLayout>
</template>
<script setup>
import { createResource, inject, computed,ref,saleDetailDialog,onUnmounted, reactive} from '@/plugin'
import Enumerable from 'linq'

import PageLayout from '@/components/layout/PageLayout.vue';
import { createToaster } from '@meforma/vue-toaster';
import { onMounted } from 'vue';
const gv = inject('$gv')
const moment = inject('$moment')

const serverUrl = window.location.protocol + "//" + window.location.hostname + ":" + gv.setting.pos_setting.backend_port;
const toaster = createToaster({position:"top"})
 
let filter = reactive({
    product_category: 'All Product Categories',
    product_category_filter: ''
})
const activeReport = ref({
    name: 'Working Day',
    preview_report: '',
    print_report_name: '',
    report_id: '',
    doc_type: '',
    lang: 'en',
    letterhead: 'Defualt Letter Head',
    filter: {
        product_category : ''
    }
})
const workingDay = ref(null)


const printPreviewUrl = computed(()=>{
    return `${serverUrl}/printview?doctype=${activeReport.value.doc_type}&name=${activeReport.value.report_id}&product_category=${activeReport.value.filter.product_category}&format=${activeReport.value.preview_report}&no_letterhead=0&show_toolbar=0&letterhead=${activeReport.value.letterhead}&settings=%7B%7D&_lang=${activeReport.value.lang}`
})


const printUrl = computed(()=>{
    return `${serverUrl}/printview?doctype=${activeReport.value.doc_type}&name=${activeReport.value.report_id}&product_category=${activeReport.value.filter.product_category}&format=${activeReport.value.print_report_name}&no_letterhead=0&show_toolbar=0&letterhead=${activeReport.value.letterhead}&settings=%7B%7D&_lang=${activeReport.value.lang}`
})

const lang = gv.setting.lang
let workingDayReports = ref({})
const workingDayReportsTmp = createResource({
    url: "epos_restaurant_2023.api.api.get_working_day_list_report",
    auto: true,
    onSuccess(doc){

        if(doc.length > 0){
            workingDayReports.value = Enumerable.from(doc).orderByDescending("$.creation").toArray()
            activeReport.value.report_id = workingDayReports.value[0].name
            const workingDayTemp = createResource({
                url: 'frappe.client.get_list',
                params: {
                    doctype: "Print Format",
                    fields: ['*'],
                    filters: {
                        show_in_pos_report: 1,
                        doc_type: 'Working Day'
                    },
                    order_by: 'sort_order asc'
                },
                auto: true,
                onSuccess(doc){
                    activeReport.value.preview_report = doc[0].name
                    activeReport.value.name = "Working Day"
                    activeReport.value.doc_type = doc[0].doc_type
                    activeReport.value.print_report_name = doc[0].print_report_name || doc[0].name
                    workingDay.value = doc
                    
                }
            })
        }
    }
})
const cashierShiftReports = createResource({
    url: 'frappe.client.get_list',
    params: {
        doctype: "Print Format",
        fields: ['name', 'doc_type', 'print_report_name', 'title'],
        filters: {
            show_in_pos_report: 1,
            doc_type: 'Cashier Shift'
        },
        order_by: 'sort_order asc'
    },
    auto: true,
    
})
 
 
 

function onCashierShift(data){
    activeReport.value.name = 'Cashier Shift'
    activeReport.value.report_id = data.name
    activeReport.value.preview_report = cashierShiftReports.data[0].name
    activeReport.value.doc_type = cashierShiftReports.data[0].doc_type 
    activeReport.value.print_report_name = cashierShiftReports.data[0].print_report_name || cashierShiftReports.data[0].name
}
function onPrintFormat(value){
    activeReport.value.preview_report = value.name;
    activeReport.value.print_report_name = value.print_report_name || value.name
    onRefresh()
  
}

function onWorkingDay(working_day){
    activeReport.value.name = 'Working Day'
    activeReport.value.report_id = working_day.name
    activeReport.value.preview_report = workingDay.value[0]?.name
    activeReport.value.doc_type = workingDay.value[0]?.doc_type 
    const print_report_name = workingDay.value.filter(r=>r.name == working_day.name)
    
    activeReport.value.print_report_name = workingDay[0]?.print_report_name || workingDay[0]?.name
}

function onRefresh(){
    document.getElementById("report-view").contentWindow.location.replace(printPreviewUrl.value)
}

function onPrint(){
    window.open(printUrl.value + "&trigger_print=1").print();
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

function onFilter(){ 
    if(filter.product_category && filter.product_category != 'All Product Categories'){
        activeReport.value.filter.product_category = filter.product_category
        onRefresh()
    }else{
        activeReport.value.filter.product_category = ''
        onRefresh()
    }
}

window.addEventListener('message', reportClickHandler, false);

onUnmounted(() => {
    window.removeEventListener('message', reportClickHandler, false);
}) 
</script>
<style>
.subtitle-opacity-1 .v-card-subtitle {
    opacity: 1 !important;
}</style>