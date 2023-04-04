<template>
    <PageLayout class="pb-4" :title="`${activeReport.doc_type}: ${activeReport.report_id}`" icon="mdi-chart-bar" full>
        <template #action>
            <v-btn icon="mdi-printer" @click="onPrint()"></v-btn>
            <v-btn v-if="mobile" icon="mdi-filter-outline" @click="onOpenDrawer"></v-btn>
        </template>
        <v-row>
            <v-col sm="4" md="4" lg="3" v-if="!mobile">
                <v-card>
                    <v-card-item>
                        <v-card-subtitle>
                            <div class="flex justify-between">
                                <div>Working Day & Cashier Shift Report</div>
                                <div>
                                    <v-icon icon="mdi-reload" size="small" @click="onRefreshReport"></v-icon>
                                </div>
                            </div>
                        </v-card-subtitle>
                        <v-card-text style="height: calc(100vh - 214px); overflow-y: auto;">
                            <ComPlaceholder :loading="workingDayReportsTmp.loading === true || workingDayReports === null"
                                :is-not-empty="workingDayReports?.length > 0">
                                <template v-for="(c, index) in workingDayReports" :key="index">
                                    <v-card :color="activeReport.report_id == c.name ? 'info' : 'default'"
                                        :variant="activeReport.report_id == c.name || c.cashier_shifts.find(r => r.name == activeReport.report_id) ? 'tonal' : 'text'"
                                        class="bg-gray-200 my-2 subtitle-opacity-1" @click="onWorkingDay(c)">
                                        <template v-slot:title>
                                            <div class="flex justify-between">
                                                <div>{{ c.name }}</div>
                                                <div>
                                                    <v-chip v-if="c.is_closed" color="error" size="small"
                                                        variant="elevated">Closed</v-chip>
                                                    <v-chip v-else color="success" size="small"
                                                        variant="elevated">Opening</v-chip>
                                                </div>
                                            </div>
                                        </template>
                                        <template v-slot:subtitle>
                                            <div class="whitespace-normal">
                                                <div><v-icon icon="mdi-calendar" size="x-small" /> <span class="font-bold">{{
                                                    c.posting_date
                                                }}</span> opened by <span class="font-bold">{{ c.owner }}</span>
                                                </div>
                                                <div v-if="c.is_closed">
                                                    <v-icon icon="mdi-calendar-multiple" size="x-small" /> <span
                                                        class="font-bold">{{ c.closed_date }}</span> closed by <span
                                                        class="font-bold">{{ c.modified_by }}</span>
                                                </div>
                                                <div><v-icon icon="mdi-note-text" size="x-small"></v-icon> Total Shift: <span
                                                        class="font-bold">{{ c.cashier_shifts.length }}</span></div>
                                            </div>
                                        </template>
                                    </v-card>
                                    <div class="overflow-y-auto overflow-x-hidden max-h-44"
                                        v-if="activeReport.report_id == c.name || c.cashier_shifts.find(r => r.name == activeReport.report_id)">
                                        <div class="flex flex-wrap">
                                            <v-sheet 
                                                elevation="1" 
                                                :color="item.name == activeReport.report_id ? 'info' : 'default'" 
                                                class="m-1 p-2 text-center cursor-pointer"
                                                width="118px"
                                                rounded="sm"
                                                v-for="(item, index) in c.cashier_shifts"
                                                :key="index" @click="onCashierShift(item)"> 
                                                
                                                <div>{{ moment(item.creation).format('h:mm:ss A') }}</div>
                                                <div class="text-xs">
                                                    <span>#{{ item.name }}</span>
                                                </div>
                                                <div>
                                                    <v-chip v-if="item.is_closed" size="x-small" color="error">Closed</v-chip>
                                                    <v-chip v-else size="x-small" color="success">Opening</v-chip>
                                                </div>
                                            </v-sheet>
                                        </div>
                                    </div>
                                    <div class="pt-2">
                                        <hr />
                                    </div>
                                </template>
                            </ComPlaceholder>
                        </v-card-text>
                    </v-card-item>
                </v-card>
            </v-col>
            <v-col sm="8" md="8" lg="9">
                <v-card>
                    <template #title>
                        <div class="px-1 py-2 -m-1 whitespace-normal">
                            <v-row>
                                <v-col cols="12" lg="7" xl="8">
                                    <v-tabs
                                    show-arrows
                                    >
                                    <template v-if="activeReport.name == 'Cashier Shift'">
                                        <v-tab
                                            v-for="(r, index) in gv.setting.reports.filter(r => r.doc_type == 'Cashier Shift' && r.show_in_pos == 1)"
                                            :key="index"
                                            @click="onPrintFormat(r)"
                                            >
                                            {{ r.title }}
                                        </v-tab>
                                    </template>
                                    <template v-else-if="activeReport.name == 'Working Day'">
                                        <v-tab 
                                        v-for="(r, index) in gv.setting.reports.filter(r => r.doc_type == 'Working Day' && r.show_in_pos == 1)"
                                        :key="index"
                                        @click="onPrintFormat(r)"
                                        >
                                        {{ r.title }}
                                        </v-tab>
                                    </template>
                                    </v-tabs>
                                </v-col>
                                <v-col cols="12" lg="5" xl="4">
                                    <div class="flex items-center justify-end">
                                        <v-select prepend-inner-icon="mdi-content-paste" density="compact"
                                            v-model="selectedLetterhead" :items=gv.setting.letter_heads item-title="name"
                                            item-value="name" hide-no-data hide-details variant="solo" class="mx-1"
                                            @update:modelValue="onRefresh"></v-select>
                                        <v-select prepend-inner-icon="mdi-google-translate" density="compact"
                                            v-model="activeReport.lang" :items="lang" item-title="language_name"
                                            item-value="language_code" hide-no-data hide-details variant="solo" class="mx-1"
                                            @update:modelValue="onRefresh"></v-select>
                                        <v-icon class="mx-1" icon="mdi-refresh" size="small" @click="onRefresh" />
                                    </div>
                                </v-col>
                            </v-row>
                        </div>
                    </template>
                    <v-card-text style="height: calc(100vh - 235px);"> 
                        <ComPlaceholder :is-not-empty="activeReport.report_id != ''" :loading="workingDayReportsTmp.loading">
                            <template #default>
                                <iframe id="report-view" height="100%" width="100%" :src="printPreviewUrl"></iframe>
                            </template>
                        </ComPlaceholder>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
        <Sheet v-if="mobile" v-model:visible="drawer" onlyHeaderSwipe>
            <div class="pa-4">
                <ComPlaceholder :loading="workingDayReportsTmp.loading === true || workingDayReports === null"
                                :is-not-empty="workingDayReports?.length > 0">
                                <template v-for="(c, index) in workingDayReports" :key="index">
                                    <v-card :color="activeReport.report_id == c.name ? 'info' : 'default'"
                                        :variant="activeReport.report_id == c.name || c.cashier_shifts.find(r => r.name == activeReport.report_id) ? 'tonal' : 'text'"
                                        class="bg-gray-200 my-2 subtitle-opacity-1" @click="onWorkingDay(c)">
                                        <template v-slot:title>
                                            <div class="flex justify-between">
                                                <div>{{ c.name }}</div>
                                                <div>
                                                    <v-chip v-if="c.is_closed" color="error" size="small"
                                                        variant="elevated">Closed</v-chip>
                                                    <v-chip v-else color="success" size="small"
                                                        variant="elevated">Opening</v-chip>
                                                </div>
                                            </div>
                                        </template>
                                        <template v-slot:subtitle>
                                            <div class="whitespace-normal">
                                                <div><v-icon icon="mdi-calendar" size="x-small" /> <span class="font-bold">{{
                                                    c.posting_date
                                                }}</span> opened by <span class="font-bold">{{ c.owner }}</span>
                                                </div>
                                                <div v-if="c.is_closed">
                                                    <v-icon icon="mdi-calendar-multiple" size="x-small" /> <span
                                                        class="font-bold">{{ c.closed_date }}</span> closed by <span
                                                        class="font-bold">{{ c.modified_by }}</span>
                                                </div>
                                                <div><v-icon icon="mdi-note-text" size="x-small"></v-icon> Total Shift: <span
                                                        class="font-bold">{{ c.cashier_shifts.length }}</span></div>
                                            </div>
                                        </template>
                                    </v-card>
                                    <div class="overflow-y-auto overflow-x-hidden max-h-44"
                                        v-if="activeReport.report_id == c.name || c.cashier_shifts.find(r => r.name == activeReport.report_id)">
                                        <div class="flex flex-wrap">
                                            <v-sheet 
                                                elevation="1" 
                                                :color="item.name == activeReport.report_id ? 'info' : 'default'" 
                                                class="m-1 p-2 text-center cursor-pointer"
                                                width="118px"
                                                rounded="sm"
                                                v-for="(item, index) in c.cashier_shifts"
                                                :key="index" @click="onCashierShift(item)"> 
                                                
                                                <div>{{ moment(item.creation).format('h:mm:ss A') }}</div>
                                                <div class="text-xs">
                                                    <span>#{{ item.name }}</span>
                                                </div>
                                                <div>
                                                    <v-chip v-if="item.is_closed" size="x-small" color="error">Closed</v-chip>
                                                    <v-chip v-else size="x-small" color="success">Opening</v-chip>
                                                </div>
                                            </v-sheet>
                                        </div>
                                    </div>
                                    <div class="pt-2">
                                        <hr />
                                    </div>
                                </template>
                            </ComPlaceholder>
            </div>
        </Sheet>
    </PageLayout>
</template>
<script setup>
import { createResource, inject, computed, ref, saleDetailDialog, onUnmounted } from '@/plugin'
import Enumerable from 'linq'
import { webserver_port } from "../../../../../../sites/common_site_config.json"
import PageLayout from '@/components/layout/PageLayout.vue';
import { Sheet } from 'bottom-sheet-vue3'
import ComPlaceholder from '../../components/layout/components/ComPlaceholder.vue';
import {useDisplay} from 'vuetify'
const {mobile} = useDisplay()
const gv = inject('$gv')
const moment = inject('$moment')
const selectedLetterhead = ref(getDefaultLetterHead());
const printPreviewUrl = ref("");
const drawer = ref(false)
const serverUrl = window.location.protocol + "//" + window.location.hostname + ":" + webserver_port;

const activeReport = ref({
    name: 'Working Day',
    preview_report: '',
    print_report_name: '',
    report_id: '',
    doc_type: 'Working Day',
    lang: 'en',
    letterhead: gv.setting.letter_heads.find(r => r.is_default)?.name
}) 
function getReportUrl() {
    let letterhead = "";
    if (selectedLetterhead.value == "") {
        letterhead = getDefaultLetterHead();
    } else {
        letterhead = selectedLetterhead.value;
    }
    const url = `${serverUrl}/printview?doctype=${activeReport.value.doc_type}&name=${activeReport.value.report_id}&format=${activeReport.value.preview_report}&no_letterhead=0&show_toolbar=0&letterhead=${letterhead}&settings=%7B%7D&_lang=${activeReport.value.lang}`;
    return url;
}

const printUrl = computed(() => {
    let letterhead = "";
    if (selectedLetterhead.value == "") {
        letterhead = getDefaultLetterHead();
    } else {
        letterhead = selectedLetterhead.value;
    }
    const url = `${serverUrl}/printview?doctype=${activeReport.value.doc_type}&name=${activeReport.value.report_id}&format=${activeReport.value.print_report_name}&no_letterhead=0&show_toolbar=0&letterhead=${letterhead}&settings=%7B%7D&_lang=${activeReport.value.lang}`;
    return url;
})

const lang = gv.setting.lang

let workingDayReports = ref({})
const workingDayReportsTmp = createResource({
    url: "epos_restaurant_2023.api.api.get_working_day_list_report",
    auto: true,
    onSuccess(doc) {
        if (doc.length > 0) {
            workingDayReports.value = Enumerable.from(doc).orderByDescending("$.creation").toArray()
            const reports = gv.setting.reports.filter(r => r.doc_type == 'Working Day' && r.show_in_pos == 1);
            activeReport.value.report_id = workingDayReports.value[0].name
            activeReport.value.preview_report = reports[0].name
            activeReport.value.doc_type = "Working Day"
            activeReport.value.print_report_name = reports[0].print_report_name || reports[0].name

            printPreviewUrl.value = getReportUrl();

        }
    }
})

function onOpenDrawer() {
    drawer.value = true
}


function onCashierShift(data) {

    const reports = gv.setting.reports.filter(r => r.doc_type == 'Cashier Shift' && r.show_in_pos == 1);

    activeReport.value.name = 'Cashier Shift'
    activeReport.value.report_id = data.name
    if (activeReport.value.doc_type != "Cashier Shift") {
        activeReport.value.preview_report = reports[0].name
        activeReport.value.print_report_name = reports[0].print_report_name || reports[0].name
    }
    activeReport.value.doc_type = "Cashier Shift";
    printPreviewUrl.value = getReportUrl();

}
function onPrintFormat(report) {
    activeReport.value.preview_report = report.name;
    activeReport.value.print_report_name = report.print_report_name || report.name
    printPreviewUrl.value = getReportUrl();

    onRefresh()

}

function onWorkingDay(working_day) {
    const reports = gv.setting.reports.filter(r => r.doc_type == 'Working Day' && r.show_in_pos == 1);

    activeReport.value.name = 'Working Day'
    activeReport.value.report_id = working_day.name


    if (activeReport.value.doc_type != "Working Day") {
        activeReport.value.preview_report = reports[0].name
        activeReport.value.print_report_name = reports[0].print_report_name || reports[0].name
    }
    activeReport.value.doc_type = "Working Day";

    printPreviewUrl.value = getReportUrl();

}


function getDefaultLetterHead() {
    let letterhead = "";

    letterhead = gv.setting.letter_heads.filter(r => r.is_default == 1)[0]?.name;
    if (!letterhead) {
        letterhead = "No Letterhead";
    }
    return letterhead;
}

function onRefresh() {
    printPreviewUrl.value = getReportUrl();
    document.getElementById("report-view").contentWindow.location.replace(printPreviewUrl.value)
}

function onPrint() {
    window.open(printUrl.value + "&trigger_print=1").print();
    window.close();
}
const reportClickHandler = async function (e) {
    if (e.isTrusted && typeof (e.data) == 'string') {

        const data = e.data.split("|")

        if (data.length > 0) {

            if (data[0] == "view_sale_detail") {
                saleDetailDialog({
                    name: data[1]
                });

            }

        }

    }
};

function onRefreshReport(){
    getDefaultLetterHead()
    activeReport.value.name = 'Working Day'
    workingDayReportsTmp.fetch()
}

window.addEventListener('message', reportClickHandler, false);

onUnmounted(() => {
    window.removeEventListener('message', reportClickHandler, false);
}) 
</script>
<style>
.subtitle-opacity-1 .v-card-subtitle {
    opacity: 1 !important;
}
</style>