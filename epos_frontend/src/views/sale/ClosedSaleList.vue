<template>
    <PageLayout :title="$t('Closed Receipt')" icon="mdi-file-document" full>
        <template #action>
            <v-btn v-if="mobile" icon="mdi-filter-outline" @click="onOpenDrawer"></v-btn>
        </template>
        <template #default>          
            <ComLoadingDialog v-if="filterResource.loading && filterResource.data" />
            <template v-else>
                <div v-if="filterResource.data?.cashier_shifts?.length == 0">{{ $t('There is no cashier shift opened') }}</div>
                <div v-else>
                    <v-row>
                        <v-col cols="12" sm="5" md="3" lg="3">
                            <div  style="height: calc(100vh - 157px);" v-if="!mobile">
                                <ComClosedSaleFilter :currentFilter="filter" :filterResource="filterResource" :reportOption="reportOption"
                                    @onSearch="onSearch" />
                            </div>
                        </v-col>
                        <v-col cols="12" sm="7" md="9" lg="9">
                            <div class="flex justify-between">
                                <div class="pb-2 -mr-2">
                                    <v-tabs show-arrows>
                                        <v-tab
                                        v-for="(r, index) in gv.setting.reports.filter(r => r.show_in_pos_closed_sale == 1 && r.doc_type == 'POS Profile')"
                                        :key="index" @click="onReportClick(r)">
                                        {{ $t(r.title) }}</v-tab>
                                    </v-tabs>
                                </div>
                                <div>
                                    <v-icon icon="mdi-reload" size="small" @click="onRefresh"></v-icon>
                                </div>
                            </div>
                            <ComClosedSaleSelectedFilter  :currentFilter="resultFilter"  :reportOption="reportOption"  @onSearch="onSearch"/>
                            <div style="height: calc(100vh - 202px);">
                                <iframe @load="onIframeLoaded()" id="report-view" height="100%" width="100%" :src="reportUrl"></iframe>
                            </div>
                        </v-col>
                    </v-row>
                </div>
               
              
            </template>
            <Sheet v-if="mobile" v-model:visible="drawer" onlyHeaderSwipe>
                <div class="pa-4">
                    <ComClosedSaleFilter :currentFilter="filter" :filterResource="filterResource" :reportOption="reportOption"
                        @onSearch="onSearch" />
                </div>
            </Sheet>
        </template>
    </PageLayout>
</template>
  
<script setup>
import PageLayout from '@/components/layout/PageLayout.vue';
import ComLoadingDialog from '@/components/ComLoadingDialog.vue';
import { useDisplay } from 'vuetify'


import { inject, ref, createResource, saleDetailDialog, onUnmounted, onMounted, computed,printPreviewDialog,customerDetailDialog,i18n } from '@/plugin'
import { createToaster } from '@meforma/vue-toaster';
import ComClosedSaleFilter from './components/ComClosedSaleFilter.vue';
import ComClosedSaleSelectedFilter from './components/ComClosedSaleSelectedFilter.vue';
import { Sheet } from 'bottom-sheet-vue3';

const { t: $t } = i18n.global;  

const { mobile } = useDisplay()
const gv = inject("$gv")
const keyword = ref("")
const drawer = ref(false)
const serverUrl = window.location.protocol + "//" + window.location.hostname + ":" + gv.setting.pos_setting.backend_port;

 
const filter = ref({
    cashier_shift: "",
    payment_type: "",
    outlet: "",
    table_group: "",
    group_by: "Sale Type",
    order_by: "name",
    sale_type: "",
    payment_type: "All Payment Type",
    keyword: "",
    order_by_type:"asc"
})

let resultFilter = ref({})

const toaster = createToaster({ position: "top" })
const reportUrl = ref("");

const activeReport = ref(gv.setting.reports.filter(r => r.show_in_pos_closed_sale == 1 && r.doc_type == 'POS Profile')[0]);

const filterResource = createResource({
    url: "epos_restaurant_2023.api.api.get_filter_for_close_sale_list",
    params: {
        business_branch: gv.setting?.business_branch,
        pos_profile: localStorage.getItem("pos_profile")
    },
    cache: "close_sale_list_filters"

});

const reportOption = computed(() => {
    let option = {};
    if (activeReport.value?.report_options) {
        option = (JSON.parse(activeReport.value?.report_options));
    }
    return option;
}) 

function getReportUrl() {

    let url = `${serverUrl}/printview?doctype=${activeReport.value.doc_type}&name=${localStorage.getItem("pos_profile")}&format=${activeReport.value.name}&no_letterhead=1&show_toolbar=0&view=ui`;

    if (filter.value.keyword && reportOption.value.show_keyword) {
        url += "&keyword=" + filter.value.keyword;
    }

    if (filter.value.cashier_shift && reportOption.value.show_cashier_shift) {
    
        filter.value.working_day = "";
        url += "&cashier_shift=" + filter.value.cashier_shift;
    } else {
        filter.value.working_day =filterResource.data?.working_day.name; 
        url += "&working_day=" + filterResource.data?.working_day.name;
    }

    if (filter.value.outlet && reportOption.value.show_outlet) {
        url += "&outlet=" + filter.value.outlet;
    }

    if (filter.value.table_group && reportOption.value.show_table_group) {
        url += "&table_group=" + filter.value.table_group;
    }
    if (filter.value.sale_type && reportOption.value.show_sale_type) {
        url += "&sale_type=" + filter.value.sale_type;
    }

    if (filter.value.payment_type != 'All Payment Type' && reportOption.value.show_payment_type) {
        url += "&payment_type=" + filter.value.payment_type;
    }


    if (filter.value.group_by && reportOption.value.show_group_by) {
        url += "&group_by=" + filter.value.group_by;
    }

    if (filter.value.order_by && reportOption.value.show_order_by) {
        url += "&order_by=" + filter.value.order_by;
    }

    if (filter.value.order_by_type && reportOption.value.show_order_by) {
        url += "&order_by_type=" + filter.value.order_by_type;
    }


    return url;
}

function onReportClick(r) {
    activeReport.value = r;
    reportUrl.value = getReportUrl();
  
}
function onIframeLoaded(){
    const iframe = document.getElementById("report-view");

   iframe.height = iframe.contentWindow.document.body.scrollHeight;
}
function onSearch(f) {
    filter.value = f;
    reportUrl.value = getReportUrl();
    drawer.value = false;
    resultFilter.value =  JSON.parse(JSON.stringify(filter.value));
}

function onRefresh() {
    document.getElementById("report-view").contentWindow.location.replace(reportUrl.value)

}

function onOpenDrawer() {
    drawer.value = true
}
 




const reportClickHandler = async function (e) {
    if (e.isTrusted && typeof (e.data) == 'string') {

        const data = e.data.split("|")

        if (data.length > 0) {

            if (data[0] == "view_sale_detail") {
                saleDetailDialog({
                    name: data[1]
                });

            }else if(data[0] == "view_cashier_shift"){
                printPreviewDialog({ title: $t("Cashier Shift")+" #" + data[1], doctype: "Cashier Shift", name: data[1] });
            }
            else if(data[0] == "view_customer"){
            
                const result = await customerDetailDialog({
                    name: data[1]
                });
            }
        }

    }
};

window.addEventListener('message', reportClickHandler, false);

onMounted(async () => {

    await filterResource.fetch().then((doc) => {
       
        filter.value.cashier_shift = doc.cashier_shift.name;
        filter.value.working_day = doc.working_day.name;
        reportUrl.value = getReportUrl();
        resultFilter.value = JSON.parse(JSON.stringify(filter.value));

    })
})

onUnmounted(() => {
    window.removeEventListener('message', reportClickHandler, false);
})

</script>
<style>
.bottom-sheet {
    max-height: 90% !important;
}
</style>

