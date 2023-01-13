<template>
    <PageLayout title="Open Shift" icon="mdi-clock">
        <!-- {{ currentDate }} -->
        <v-row>
            <v-col md="6">
                <v-text-field label="Working Date" v-model="current_date" variant="solo" readonly></v-text-field>
            </v-col>
            <v-col md="6">
                <v-text-field label="POS Profile" v-model="pos_profile" variant="solo" readonly></v-text-field>
            </v-col>
        </v-row>
        
        <v-textarea label="Note" variant="solo" v-model="note"></v-textarea>
        <!-- <v-btn @click="addNewToDo">Add New Todo</v-btn> -->
        <v-btn @click="onStartWorking" color="primary">Start Working Day</v-btn>

    </PageLayout>
</template>
 

<script setup>
import moment from '@/utils/moment.js'
import { ref, createResource } from '@/plugin'
import PageLayout from '../../components/layout/PageLayout.vue';

const pos_profile = localStorage.getItem("pos_profile");
// const currentDate = ref(new Date().toLocaleDateString())
const note = ref("")
const current_date = moment(new Date).format('DD-MM-YYYY');



// let toDoList = createResource({
//     url: "frappe.client.get_list",
//     params: {
//         doctype: "Customer",
//         fields: ["name", "customer_name_en"]
//     },
//     auto: true
// })

let workingDay = createResource({
    url: "frappe.client.get_list",
    params: {
        doctype: "Working Day",
        fields: ["posting_date", "pos_profile", "note"]
    },
    auto: true
})

// function addNewToDo() {
//     alert(note.value)
//         createResource({
//             url:"frappe.client.insert",
//             params:{
//                 doc:{
//                     doctype:"ToDo",
//                     pos_profile:pos_profile,

//                     date:'2023-05-01'
//                 }
//             },
//             onSuccess(data) {
//                 alert("add data todo done")
//             },

//             auto:true
//      })
// }

function onStartWorking() {
    alert(note.value)
        createResource({
            url:"frappe.client.insert",
            params:{
                doc:{
                    doctype:"Working Day",
                    pos_profile:pos_profile,
                    date:'2023-01-13',
                    note:"Hello"
                }
            },
            onSuccess(data) {
                alert("add data todo done")
            },

            auto:true
     })
}


</script>