<template>
    <PageLayout :title="$t('Open Shift')" icon="mdi-clock" class="p-3">
        <v-row v-if="!working_day.loading && working_day.data">
            <v-col cols="12" md="6">
                <ComInput :label="$t('Working Day No')" type="text" v-model="working_day.data.name" readonly/>
            </v-col>
            <v-col cols="12" md="6">
                <ComInput :label="$t('Working Day')" v-model="working_date" variant="solo" readonly/>
            </v-col>
            <v-col cols="12" md="6">
                <ComInput :label="$t('POS Profile')" v-model="pos_profile" readonly/>
            </v-col>
            <v-col cols="12" md="6">
                <v-select
                :label="$t('Shift')"
                item-title="name"
                item-value="name"
                variant="solo"
                v-model="shift_type"
                density="compact"
                :items="gv.setting.shift_types"
                ></v-select>
            </v-col>
        </v-row>
        <h1 class="my-4">{{ $t('Cash Float') }}</h1>
        <v-row v-if="payment_types && payment_types.filter(p => p.allow_cash_float == 1).length > 0">
            <v-col cols="12" md="6" v-for="p in payment_types.filter(p => p.allow_cash_float == 1)">
                    <ComInput type="number" :label="p.payment_method" v-model="p.input_amount" keyboard/>
            </v-col>
        </v-row>
        <v-row v-if="payment_types && payment_types.filter(p => p.allow_cash_float == 1).length > 1">
            <v-col cols="12" md="6">
                <ComInput readonly :label="$t('Total Cash Float')" v-model="totalCashFloat" keyboard/>
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="12">
                <ComInput :title="$t('Enter Note')" :label="$t('Open Note')" v-model="opened_note" type="textarea" keyboard></ComInput>
            </v-col>
        </v-row>
        <div class="flex items-center justify-between mt-8 mb-3">
            <v-btn @click="onOpenShift" :loading="addCashierShiftResource.loading" color="primary">{{ $t('Open Shift') }}</v-btn>
            <v-btn @click="router.push({ name: 'Home' })" color="error" class="ml-4">{{ $t('Cancel') }}</v-btn>
        </div>
    </PageLayout>
</template>

<script setup>
import PageLayout from '../../components/layout/PageLayout.vue';
import { createResource, ref, createToaster, useRouter, reactive, computed, inject, confirm,i18n ,onMounted} from '@/plugin'
import moment from '@/utils/moment.js'
import ComInput from '../../components/form/ComInput.vue';

const { t: $t } = i18n.global; 
const gv = inject("$gv")
const opened_note = ref("")
const working_date = ref("")
const shift_type = ref("")
const pos_profile = localStorage.getItem("pos_profile");
const setting = JSON.parse(localStorage.getItem("setting"));
const payment_types = reactive(setting.payment_types)
const totalCashFloat = computed(() => {
    const total = payment_types.reduce((n, r) => n + parseFloat(r.input_amount || 0) / parseFloat(r.exchange_rate), 0)
    return Number(total.toFixed(gv.setting.pos_setting.main_currency_precision));
})



const router = useRouter();
const toaster = createToaster();


const working_day = createResource({
    url: "epos_restaurant_2023.api.api.get_current_working_day",
    params: {
        business_branch: gv.setting.business_branch
    },
    auto: true,
    onSuccess(data) {
        if (data == undefined) {
            toaster.warning($t("msg.Please start working day first"), { position: "top" });
            router.push({ name: "Home" });
        } 
        else {
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
            toaster.warning($t("msg.Shift is already opened"), { position: "top" });
            router.push({ name: "Home" });
        }
    }
})


const addCashierShiftResource = createResource({
    url: "frappe.client.insert",
    params: {

    },
    onSuccess(data) {
        toaster.success($t("msg.Open Shift successfully"), { position: "top" });
        router.push({ name: "Home" });
    }
})


onMounted(()=>{
    shift_type.value = gv.setting.shift_types.sort((a, b) => a.sort - b.sort )[0].name;
})

async function onOpenShift() {
    if (await confirm({ title: $t("Start Shift"), text: $t("msg.are you sure to start shift") })) {
        addCashierShiftResource.params = {
            doc: {
                doctype: "Cashier Shift",
                working_day: working_day.data.name,
                opened_note: opened_note.value,
                shift_name: shift_type.value,
                cash_float: payment_types.filter(r => parseFloat(r.input_amount) > 0)
            }
        };
        addCashierShiftResource.submit();
    }

}
</script>