<template>

    <PageLayout class="pb-4" title="Close Cashier Shift" icon="mdi-calendar-clock">
     
        <template #action>
            <ComCloseShiftActionMenu v-if="cashierShiftResource.doc" :name="cashierShiftResource.doc.name" />
        </template>
        <v-row v-if="cashierShiftResource.doc">
            <v-col md="6">
                <v-text-field label="Working Day" v-model="cashierShiftResource.doc.working_day" variant="solo"
                    readonly></v-text-field>
            </v-col>
            <v-col md="6">
                <v-text-field label="Cashier Shift" v-model="cashierShiftResource.doc.name" variant="solo"
                    readonly></v-text-field>
            </v-col>

            <v-col md="6">
                <v-text-field label="Close Date" v-model="current_date" variant="solo" readonly></v-text-field>
            </v-col>
            <v-col md="6">
                <v-text-field label="POS Profile" v-model="cashierShiftResource.doc.pos_profile" variant="solo"
                    readonly></v-text-field>
            </v-col>
        </v-row>
        <v-row>

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
                            Differce Amount
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(d, index) in cashierShiftSummary.data" :key="index">
                        <td>{{ d.payment_method }}</td>
                        <td><CurrencyFormat :value="d.input_amount" :currency="d.currency" /> </td>
                        <td><CurrencyFormat :value="d.input_system_close_amount" :currency="d.currency" />
                        </td>
                        <td>
                            <ComInput title="Close Amount" keyboard type="number"   
                                v-model="d.input_close_amount"></ComInput>
                        </td>
                        <td> <CurrencyFormat :value="d.different_amount" :currency="d.currency" /></td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <td>
                            Total
                        </td>
                        <td>
                            <CurrencyFormat :value="cashierShiftSummary.data.reduce((n, r) => n + r.opening_amount,0)"  />

                            
                        </td>
                        <td>
                            <CurrencyFormat :value="cashierShiftSummary.data.reduce((n, r) => n + r.system_close_amount, 0)"  />

                        </td>
                        <td>
                            <CurrencyFormat :value="totalCloseAmount"  />
                        </td>
                        <td> 
                            <CurrencyFormat :value="totalDifferentAmount"/>
                             
                        </td>
                    </tr>
                </tfoot>
            </v-table>
        </v-row>
        
        <ComInput class="my-8" title="Enter Note" keyboard label="Closed Note" v-model="doc.closed_note" type="textarea"></ComInput>
       
        <v-btn @click="onCloseShift" color="primary"
            :loading="(cashierShiftResource.setValue && cashierShiftResource.setValue.loading) ? cashierShiftResource.setValue.loading : false">Close
            Cashier Shift</v-btn>
        <v-btn @click="router.push({ name: 'Home' })" color="error" class="ml-4">Cancel</v-btn>

    </PageLayout>
</template>

<script setup>
import moment from '@/utils/moment.js';
import { watch, onMounted, createDocumentResource, ref, createResource, useRouter, createToaster, computed  } from "@/plugin";
import PageLayout from '../../components/layout/PageLayout.vue';
import ComCloseShiftActionMenu from '../shift/components/ComCloseShiftActionMenu.vue';
import ComInput from '../../components/form/ComInput.vue';
import { printPreviewDialog,confirm } from '@/utils/dialog';

const router = useRouter();
const toaster = createToaster();

const setting = JSON.parse(localStorage.getItem("setting"))

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

        return cashierShiftSummary.value.data.reduce((n, r) => n + r.input_close_amount / r.exchange_rate, 0);
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
                if (setting.print_cashier_shift_summary_after_close_shift == 1) {
                    window.chrome.webview.postMessage("hello");
                }

                if (setting.print_cashier_shift_sale_product_summary_after_close_shift == 1) {
                    window.chrome.webview.postMessage("hello");
                }
                await printPreviewDialog(
                    { title: "Cashier Shift Report #" + doc.name, doctype: "Cashier Shift", name: doc.name }
                )
                
                router.push({ name: "Home" });



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
    watch(cashierShiftSummary.value , (currentValue,oldValue) => {
         
        cashierShiftSummary.value.data.forEach(function(d){
            d.different_amount = d.input_close_amount - d.input_system_close_amount
        });
        totalDifferentAmount.value = cashierShiftSummary.value.data.reduce((n,r)=>n + r.different_amount/r.exchange_rate , 0);
    })
})

async function onCloseShift() {
    

    if(await confirm({title:"Close Cashier Shift", text:"Are sure you want to close cashier shift?"})){
        doc.value.cash_float = cashierShiftSummary.value.data;
        cashierShiftResource.value.setValue.submit(doc.value)
    } 
  
   
    
}

</script>