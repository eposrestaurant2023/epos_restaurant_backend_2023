<template>
<PageLayout title="Close Cashier Shift" icon="mdi-calendar-clock">
    
{{ cashierShiftResource.doc }}
<v-row v-if="!working_day.loading && working_day.data">
            <!-- <v-col md="6">
                <v-text-field label="Close Shift" v-model="working_day.doc" variant="solo"
                    readonly></v-text-field>
            </v-col> -->
            <!-- <v-col md="6">
                <v-text-field label="Close Date" v-model="current_date" variant="solo" readonly></v-text-field>
            </v-col> -->
            <v-col md="6">
                <v-text-field label="POS Profile" v-model="pos_profile" variant="solo" readonly></v-text-field>
            </v-col>
            <v-textarea label="Closed Note" variant="solo" v-model="closed_note"></v-textarea>
        </v-row>


<button @click="onCloseShift">Save </button>
</PageLayout>
</template>
<script setup>
import {createDocumentResource, ref} from "@/plugin"
import PageLayout from '../../components/layout/PageLayout.vue';
const current_date = moment(new Date).format('DD-MM-YYYY');
const closed_note = ref("")
const pos_profile = localStorage.getItem("pos_profile");
// let workingDayResource = createDocumentResource({
//     url: 'frappe.client.get',
//     doctype: 'Cashier Shift',
//     name: working_day,
//     auto: true
// })
let cashierShiftResource = createDocumentResource({
    url: 'frappe.client.get',
    doctype: 'Cashier Shift',
    name: "workingDayResource",
    auto: true
})


function onCloseShift(){

cashierShiftResource.doc.closed_note = closed_note;
// cashierShiftResource.doc.posting_date=current_date(moment(new Date).format('YYYY-MM-DD'));
cashierShiftResource.doc.is_closed= 0;
cashierShiftResource.setValue.submit(cashierShiftResource.doc);
}

</script>