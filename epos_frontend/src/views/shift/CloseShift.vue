<template>
    <PageLayout class="pb-4" title="Close Cashier Shift" icon="mdi-calendar-clock">

        <template #action>
            <v-btn prepend-icon="mdi-printer" @click="onOpenReport">Report</v-btn>
        </template>
        <template #default>
            <ComAlertPendingOrder v-if="cashierShiftResource.doc" type="info" :working_day="cashierShiftResource.doc.working_day" :cashier_shift="cashierShiftResource.doc.name"/>
            
            <v-row v-if="cashierShiftResource.doc" class="mt-2 mx-2">
                <v-col cols="12" md="6">
                    <ComInput label="Working Day" v-model="cashierShiftResource.doc.working_day" readonly></ComInput>
                </v-col>
                <v-col cols="12" md="6">
                    <ComInput label="Cashier Shift" v-model="cashierShiftResource.doc.name" readonly></ComInput>
                </v-col>
                <v-col  cols="12" md="6">
                    <ComInput label="Close Date" v-model="current_date" readonly></ComInput>
                </v-col>
                <v-col cols="12" md="6"> 
                    <ComInput label="POS Profile" v-model="cashierShiftResource.doc.pos_profile" readonly/>
                </v-col>
            </v-row>
            <v-row class="mx-4"> 
                <v-table v-if="cashierShiftSummary.data">
                    <thead>
                        <tr>
                            <th class="text-left">
                                Payment Type
                            </th>
                            <th class="text-center">
                                Opening Amount
                            </th>

                            <th class="text-center">
                                System Close Amount
                            </th>

                            <th class="text-center">
                                Close Amount
                            </th>

                            <th class="text-center">
                                Difference Amount
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(d, index) in cashierShiftSummary.data" :key="index">
                            <td>{{ d.payment_method }}</td>
                            <td>
                                <CurrencyFormat :value="d.input_amount" :currency="d.currency" />
                            </td>
                            <td>
                                <CurrencyFormat :value="d.input_system_close_amount" :currency="d.currency" />
                            </td>
                            <td>
                                <ComInput type="number" v-model="d.input_close_amount" keyboard> </ComInput>
                            </td>
                            <td>
                                <CurrencyFormat :value="d.input_different_amount" :currency="d.currency" />
                            </td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td>
                                Total
                            </td>
                            <td>
                                <CurrencyFormat :value="cashierShiftSummary.data.reduce((n, r) => n + r.opening_amount, 0)" />
                            </td>
                            <td>
                                <CurrencyFormat :value="cashierShiftSummary.data.reduce((n, r) => n + r.system_close_amount, 0)" />

                            </td>
                            <td>
                                <CurrencyFormat :value="totalCloseAmount" />
                            </td>
                            <td>
                                <CurrencyFormat :value="totalDifferentAmount" />

                            </td>
                        </tr>
                    </tfoot>
                </v-table>
            </v-row>

            <ComInput class="my-8 mx-4" title="Enter Note" label="Closed Note" v-model="doc.closed_note"
                type="textarea">
            </ComInput>
            <div class="flex justify-between items-center mx-4">
                <v-btn @click="onCloseShift" color="primary"
                    :loading="(cashierShiftResource.setValue && cashierShiftResource.setValue.loading) ? cashierShiftResource.setValue.loading : false">Close
                    Cashier Shift</v-btn>
                <v-btn @click="router.push({ name: 'Home' })" color="error" class="ml-4">Cancel</v-btn>
            </div>
        </template>
    </PageLayout>
</template>

<script setup>
import moment from '@/utils/moment.js';
import { watch, onMounted, createDocumentResource, ref, createResource, useRouter, createToaster, computed, inject,pendingSaleListDialog } from "@/plugin";
import PageLayout from '../../components/layout/PageLayout.vue';

import ComInput from '../../components/form/ComInput.vue';
import { printPreviewDialog, confirm } from '@/utils/dialog';
import ComAlertPendingOrder from '../../components/layout/components/ComAlertPendingOrder.vue';
import { useDisplay } from 'vuetify'
const { mobile } = useDisplay()

const router = useRouter();
const toaster = createToaster();
const gv = inject('$gv');
const sale = inject('$sale');
const setting = gv.setting

const current_date = moment(new Date).format('DD-MM-YYYY');
let doc = ref({
    closed_note: "",
    is_closed: 1,
    closed_date: moment(new Date()).format('YYYY-MM-DD HH:mm:ss'),
    cash_float: []
})

const cashierShiftResource = ref({});
let cashierShiftSummary = ref({});
let totalDifferentAmount = ref(0);
const totalCloseAmount = computed(() => {
    if (cashierShiftSummary.value.data) {

        return cashierShiftSummary.value.data.reduce((n, r) => n + (r.input_close_amount / r.exchange_rate), 0);
    }
    return 0;
})

let cashierShiftInfo = createResource({
    url: "epos_restaurant_2023.api.api.get_current_cashier_shift",
    params: {
        pos_profile: localStorage.getItem("pos_profile")
    }
});

onMounted(async () => {
    await cashierShiftInfo.fetch();
    if (!cashierShiftInfo.data) {
        toaster.warning("There's no cashier shift opened", {
            position: "top",
        });
        router.push({ name: "Home" });

    }
    cashierShiftResource.value = createDocumentResource({
        url: 'frappe.client.get',
        doctype: 'Cashier Shift',
        name: cashierShiftInfo.data.name,
        setValue: {
            async onSuccess(doc) {

                toaster.success("Close cashier shift successfully", {
                    position: "top",
                });
                //check from setting if system will need to print after close shift
                if (localStorage.getItem("is_window") == 1) {
                    if (setting.print_cashier_shift_summary_after_close_shift == 1) {
                        window.chrome.webview.postMessage("hello");
                    }

                    if (setting.print_cashier_shift_sale_product_summary_after_close_shift == 1) {
                        window.chrome.webview.postMessage("hello");
                    }
                }
                await printPreviewDialog(
                    { title: "Cashier Shift Report #" + doc.name, doctype: "Cashier Shift", name: doc.name }
                )

                router.push({ name: "Home" });
                //for disable close shift in drawer menu
                gv.cashierShift = '';




            },

        },

    });

    //   get cashier shift summary
    cashierShiftSummary.value = createResource({
        url: "epos_restaurant_2023.api.api.get_close_shift_summary",
        params: {
            cashier_shift: cashierShiftInfo.data.name,
        },
        auto: true
    });
    watch(cashierShiftSummary.value, (currentValue, oldValue) => {

        cashierShiftSummary.value.data.forEach(function (d) {
            d.input_different_amount = d.input_close_amount - d.input_system_close_amount;
            d.different_amount = (d.input_close_amount - d.input_system_close_amount) / d.exchange_rate;

        });
        totalDifferentAmount.value = cashierShiftSummary.value.data.reduce((n, r) => n + r.different_amount, 0);
    }); 
})

async function onCloseShift() {
    if (await confirm({ title: "Close Cashier Shift", text: "Are sure you want to close cashier shift?" })) {
        doc.value.cash_float = cashierShiftSummary.value.data;
        cashierShiftResource.value.setValue.submit(doc.value)
    }
}
function onOpenReport() {
    printPreviewDialog({ title: "Cashier Shift #" + cashierShiftResource.value.doc.name, doctype: "Cashier Shift", name: cashierShiftResource.value.doc.name });
}

</script>