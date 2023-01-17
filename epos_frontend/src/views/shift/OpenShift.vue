<template>
    <PageLayout title="Open Shift" icon="mdi-clock">
        <v-row v-if="!working_day.loading && working_day.data">
            <v-col md="6">
                <v-text-field label="Open Shift Date" v-model="working_day.data.name" variant="solo"
                    readonly></v-text-field>
            </v-col>
            <v-col md="6">
                <v-text-field label="Working Day" v-model="working_date" variant="solo" readonly></v-text-field>
            </v-col>
            <v-col md="6">
                <v-text-field label="POS Profile" v-model="pos_profile" variant="solo" readonly></v-text-field>
            </v-col>
            <v-textarea label="Opened Note" variant="solo" v-model="opened_note"></v-textarea>
        </v-row>
        <v-divider />
        <h1>Cash Float</h1>
        <div v-if="payment_types">
            <!-- <v-row>
                <v-col md="6"> -->
                    <v-text-field v-for="p in payment_types.filter(p => p.allow_cash_float == 1)" :label="p.payment_method"
                        v-model="p.input_amount" variant="solo" append-inner-icon="mdi-keyboard"
                            @click:append-inner="OpenKeyboard(p)"></v-text-field>
                <!-- </v-col>
            </v-row> -->
        </div>
        {{ totalCashFloat }}
        <v-btn @click="onOpenShift" :loading="addCashierShiftResource.loading" color="primary">Open Shift</v-btn>

    </PageLayout>
</template>


<script setup>

import PageLayout from '../../components/layout/PageLayout.vue';
import { createResource, ref, createToaster, useRouter, reactive, computed } from '@/plugin'
import moment from '@/utils/moment.js'
import ComInputNumber from '../../components/ComInputNumber.vue';
import { openDialog } from 'vue3-promise-dialog';
const opened_note = ref("")
const working_date = ref("")
const pos_profile = localStorage.getItem("pos_profile");
const setting = JSON.parse(localStorage.getItem("setting"));
const payment_types = reactive(setting.payment_types)
const totalCashFloat = computed(() => {
    return payment_types.reduce((n, r) => n + parseFloat(r.input_amount) / parseFloat(r.exchange_rate), 0)
})

const business_branch = "SR Branch"
const router = useRouter();
const toaster = createToaster();

const working_day = createResource({
    url: "epos_restaurant_2023.api.api.get_current_working_day",
    params: {
        pos_profile: localStorage.getItem("pos_profile")
    },
    auto: true,
    onSuccess(data) {
        if (data == undefined) {
            toaster.warning("Please start working day first", { position: "top" });
            router.push({ name: "Home" });
        } else {
            working_date.value = moment(data.posting_date).format('DD-MM-YYYY');

        }

    }
})

createResource({
    url: "epos_restaurant_2023.api.api.get_current_cashier_shift",
    params: {
        pos_profile: localStorage.getItem("pos_profile")
    },
    auto: true,
    onSuccess(data) {
        if (data) {
            toaster.warning("Shift is already opened", { position: "top" });
            //router.push({ name: "Home" });
        }
    }

})


const addCashierShiftResource = createResource({
    url: "frappe.client.insert",
    params: {

    },
    onSuccess(data) {
        toaster.success("Open Shift Successfully", { position: "top" });
        router.push({ name: "Home" });
    }
})

function onOpenShift() {
    if (confirm("Are you sure you want to open cashier shift?")) {
        addCashierShiftResource.params = {
            doc: {
                doctype: "Cashier Shift",
                working_day: working_day.data.name,
                opened_note: opened_note.value,
                cash_float: payment_types.filter(r => parseFloat(r.input_amount) > 0)
            }
        };
        addCashierShiftResource.submit();
    }

}

async function OpenKeyboard(data){
const result =await openDialog(ComInputNumber,{"title":"Cash Float for " + data.payment_method});
data.input_amount = result;
}

</script>