<template lang="">
    <PageLayout class="pb-4" :title="$t('Cash Drawer')" icon="mdi-currency-usd">
        <v-container class="!py-0">
        <div>
            <div class="mb-4 grid grid-cols-2 gap-2">
                <ComCashDrawerKPI backgroundColor="primary" :title="$t('Opening Amount')" :value="cashDrawerShiftBalance.total_opening_amount"/>
                <ComCashDrawerKPI backgroundColor="secondary" :title="$t('Cash Sale Amount')" :value="cashDrawerShiftBalance.total_amount_cash"/>
                <ComCashDrawerKPI backgroundColor="success" :title="$t('Cash In Amount')" :value="cashDrawerShiftBalance.total_amount_cash_in"/>
                <ComCashDrawerKPI backgroundColor="error" :title="$t('Cash Out Amount')" :value="cashDrawerShiftBalance.total_amount_cash_out"/>
                <ComCashDrawerKPI class="col-span-2" backgroundColor="info" :title="$t('Cash Drawer Balance')" :value="cashDrawerShiftBalance.total_balance"/>
            </div>
        </div>
        <div>
            <div class="font-bold py-2">
                <div :class="mobile ? '' : 'flex justify-between'">
                    <div>{{$t('Today Cash Transaction')}}</div>
                    <div class="text-right -m-1">
                        <v-btn class="m-1" color="success" @click="onCash('Cash In')" :size="mobile ? 'small' : 'default'">{{$t('Cash In')}}</v-btn>
                        <v-btn class="m-1" color="error" @click="onCash('Cash Out')" :size="mobile ? 'small' : 'default'">{{$t('Cash Out')}}</v-btn>
                        <v-btn class="m-1" color="primary" @click="onOpenCashDrawer" :size="mobile ? 'small' : 'default'" v-if="isWindow">{{$t('Open Cash Drawer')}}</v-btn>

                        <v-btn class="m-1" color="error" @click="onClose" :size="mobile ? 'small' : 'default'">{{$t('Close')}}</v-btn>
                    </div>
                </div>
            </div>
            <div class="mb-2">
                <v-divider></v-divider>
            </div>
            <ComPlaceholder :loading="cashierShiftInfo.loading || data.loading || dataResource.loading" :is-not-empty="transactions.length > 0">
                <v-timeline density="comfortable">
                    <v-timeline-item 
                        v-for="(t, index) in transactions" 
                        :key="index" 
                        :dot-color="t.transaction_status == 'Cash Out' ? '#b00020' : '#4caf50'" 
                        size="small" 
                        :hide-opposite="mobile"
                        width="100%"
                        > 
                    <v-card>
                        <v-card-text>
                            <div class="grid" :class="mobile ? '' : 'grid-cols-2'">
                                <div> 
                                    <div class="font-bold text-lg">
                                        <CurrencyFormat :value="t.input_amount" :currency="t.currency"/>
                                    </div>
                                    <div>
                                        <div class="text-sm text-gray-400">{{t.name}}</div>
                                        <v-icon icon="mdi-clock" size="x-small"/> {{moment(t.creation).format('HH:mm A')}}
                                    </div>
                                </div>
                                <div class="text-right text-sm">
                                    <div><v-icon icon="mdi-account" size="x-small"/> {{t.created_by}}</div>
                                    <div>
                                        <v-chip size="x-small" v-if="t.transaction_status == 'Cash Out'" color="error">{{$t('Cash Out')}}</v-chip>
                                        <v-chip size="x-small" v-else-if="t.transaction_status == 'Cash In'" color="success">{{$t('Cash In')}}</v-chip>
                                    </div>
                                </div>
                            </div>
                            <div class="text-gray-600 text-sm"> 
                                <div class="pt-1 whitespace-pre-wrap">
                                    {{t.note}}
                                </div>
                            </div> 
                        </v-card-text> 
                    </v-card>
                    </v-timeline-item>
                </v-timeline>
            </ComPlaceholder>
        </div>
        </v-container>
    </PageLayout>
    
</template>
<script setup> 
import moment from '@/utils/moment.js';
import PageLayout from '@/components/layout/PageLayout.vue';
import ComCashDrawerKPI from './components/ComCashDrawerKPI.vue';
import { addCashDrawerModalDialog, createResource, ref, onMounted, inject, createDocumentResource, createToaster, useRouter } from '@/plugin'
import { useDisplay } from 'vuetify';

const { mobile } = useDisplay()
const toaster = createToaster({ position: 'top' })
let transactions = ref({})
let dataResource = {};
let cashBalanceResource = ref({});
let cashDrawerShiftBalance = ref({})
const gv = inject('$gv')
const router = useRouter()
const isWindow = localStorage.getItem("is_window")
async function onCash(cash_type, name) {
    // current working day & shift
    const result = await addCashDrawerModalDialog({ name: name, data: { cash_type: cash_type, cashier_shift_info: cashierShiftInfo.data } })
    if (result == true) {
        cashierShiftInfo.fetch()
    }
}
let data = createResource({
        url: "epos_restaurant_2023.api.api.get_current_shift_information",
        params: {
            business_branch: gv.setting?.business_branch,
            pos_profile: localStorage.getItem("pos_profile")
        },
        onSuccess(data) { 
            if (data.cashier_shift == null) {
                toaster.warning($t('msg.Please start shift first'));
                router.push({name:"OpenShift"});
            } else if(data.working_day==null){
                toaster.warning($t('msg.Please start working day first'));
                router.push({name:"StartWorkingDay"});
            }
        }
    })

let cashierShiftInfo = createResource({
    url: "epos_restaurant_2023.api.api.get_current_cashier_shift",
    params: {
        pos_profile: localStorage.getItem("pos_profile")
    },
    async onSuccess(doc) {
        await onLoadCashDrawerShiftBalance(doc.name)
        await onLoadTrancation(doc.name)
    }
});

onMounted(() => {
    data.fetch().then(r=>{
        if(r.cashier_shift != null && r.working_day != null)
            cashierShiftInfo.fetch()
    })
})
function onLoadCashDrawerShiftBalance(cashier_shift) {
    cashBalanceResource.value = createResource({
        url: "epos_restaurant_2023.api.api.get_cash_drawer_balance",
        params: {
            cashier_shift: cashier_shift
        },
        auto: true,
        onSuccess(doc) {
            cashDrawerShiftBalance.value = doc
        }
    });
}
function onLoadTrancation(cashier_shift) {
    dataResource = createResource({
        url: 'frappe.client.get_list',
        params: {
            doctype: "Cash Transaction",
            fields: ['*'],
            filters: {
                cashier_shift: cashier_shift
            },
            order_by: 'creation desc'
        },
        auto: true,
        onSuccess(data) {
            transactions.value = data
        },

    })
}

function onClose(){
    router.push({name:"Home"});
}

//open cashdrawer
function onOpenCashDrawer(){
    window.chrome.webview.postMessage(JSON.stringify({action:"open_cashdrawer"}));
} 
</script> 