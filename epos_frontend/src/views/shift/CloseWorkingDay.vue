<template>
    <PageLayout :title="$t('Working Day')" icon="mdi-calendar-clock">
        <template #title>
            #{{ shiftInformation.data?.working_day?.name }}
        </template>
        <template #action>
            <v-btn prepend-icon="mdi-printer" @click="onOpenReport">{{ $t('Report') }}</v-btn>
        </template>
        <template #default>
            <div class="pa-4">
                <ComAlertPendingOrder v-if="shiftInformation.data?.working_day" type="warning" :working_day="shiftInformation.data?.working_day?.name" @getPendingOrder="onGetPendingOrder($event)"/>
              
                <v-row v-if="shiftInformation.data?.working_day">
                    <v-col cols="12" md="6">
                        <ComInput :title="$t('POS Profile')" :label="$t('POS Profile')" v-model="shiftInformation.data.working_day.pos_profile"
                            readonly></ComInput>

                    </v-col>
                    <v-col md="6" cols="12">
                        <ComInput :title="$t('Working Date')" :label="$t('Working Date')" v-model="working_date" readonly></ComInput>
                    </v-col>
                    <v-col md="12">
                        <ComInput class="mb-8" :title="$t('Enter Note')" keyboard :label="$t('Closed Note')" v-model="closed_note"
                            type="textarea"></ComInput>

                    </v-col>
                </v-row>

                <v-btn
                    :loading="(workingDayResourceResource.setValue && workingDayResourceResource.setValue.loading) ? workingDayResourceResource.setValue.loading : false"
                    @click="onCloseWorkingDay" color="primary">{{ $t('Close Working Day') }}</v-btn>

                <v-btn @click="router.push({ name: 'Home' })" color="error" class="ml-4">{{ $t('Cancel') }}</v-btn>
            </div>
        </template>
    </PageLayout>
</template>

<script setup>
import moment from '@/utils/moment.js';
import { inject, printPreviewDialog, confirm, onMounted, createDocumentResource, ref, createResource, useRouter, createToaster, computed,i18n } from "@/plugin";
import PageLayout from '../../components/layout/PageLayout.vue';
import ComAlertPendingOrder from '../../components/layout/components/ComAlertPendingOrder.vue';

const { t: $t } = i18n.global; 
const router = useRouter();
const toaster = createToaster({ position: 'top' });
const gv = inject('$gv')
const closed_note = ref("")
let pendingOrder = ref(0)
const working_date = computed(() => {
    return moment(shiftInformation.data?.working_day?.posting_date).format('DD-MM-YYYY');
})
const workingDayResourceResource = ref({});

const shiftInformation = createResource({
    url: "epos_restaurant_2023.api.api.get_current_shift_information",
    params: {
        business_branch: gv.setting?.business_branch,
        pos_profile: localStorage.getItem("pos_profile")
    },
    onSuccess(data) {

        if (data.cashier_shift != null) {
            toaster.warning($t("msg.Please close shift first"));
            router.push({ name: "Home" });
        } else if (data.working_day == null) {
            toaster.warning($t("msg.Please start working day first"));
            router.push({ name: "Home" });
        }
    }
})



onMounted(async () => {
    await shiftInformation.fetch();
    workingDayResourceResource.value = createDocumentResource({
        url: 'frappe.client.get',
        doctype: 'Working Day',
        name: shiftInformation.data.working_day.name,
        setValue: {
            async onSuccess(doc) {
                toaster.success($t("msg.Close Working Day successfully"), {
                    position: "top",
                });
                //check from setting if system will need to print after close shift
                if (localStorage.getItem("is_window") == 1) {
                    if (gv.setting.print_working_day_summary_after_close_working_day == 1) {
                        window.chrome.webview.postMessage("working_doc");
                    }
                    if (gv.setting.print_working_day_sale_product_summary_after_close_working_day == 1) {
                        window.chrome.webview.postMessage("working_doc");
                    }
                }
                await printPreviewDialog(
                    { title: $t('Working Day Report')+" #" + doc.name, doctype: "Working Day", name: doc.name }
                )

                router.push({ name: "Home" });
                gv.cashierShift = "";
                gv.workingDay = "";

            },

        },

    });
})

async function onCloseWorkingDay() { 
    if(pendingOrder.value == 0){
        if (await confirm({ title: $t("Close Working Day"), text: $t("msg.are you sure to close working day") })) {
            workingDayResourceResource.value.setValue.submit({
                is_closed: 1,
                closed_note: closed_note.value,
                closed_date: moment(new Date()).format('YYYY-MM-DD HH:mm:ss')
            })
        }
    }else{
        toaster.warning($t(`msg.There are pending orders`,[pendingOrder.value]))
    }
}
function onOpenReport() {
    printPreviewDialog({ title: $t('Working Day Report')+" #" + shiftInformation.data.working_day.name, doctype: "Working Day", name: shiftInformation.data.working_day.name });
}

function onGetPendingOrder(r){
    pendingOrder.value = r
}
</script>