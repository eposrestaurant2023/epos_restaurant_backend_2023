<template>
    <PageLayout title="Table Layout" full icon="mdi-cart">
        <template #centerCotent>
            <ComTableGroupTabHeader />
        </template>
        <template #action>
            <ComTableLayoutActionButton/>
        </template>
       
        <template v-if="tableLayout.table_groups">
    
            <v-window v-model="tableLayout.tab" v-if="tableLayout.currentView == 'table_group'">
                <ComArrangeTable  v-if="tableLayout.canArrangeTable"/>
                <ComRenderTableNumber v-else/>

            </v-window>
            <v-window v-model="tableLayout.tab" v-else>
               
                <template v-for="g in tableLayout.table_groups">
                    
                    <v-window-item :value="g.key" v-bind:style="{ 'min-height': 'calc(100vh - 200px)' }">
                    
                        <ComPendingSaleList :data="tableLayout.saleListResource?.data?.filter(r => r.tbl_group == g.table_group)" />
                    </v-window-item>
                </template>
            </v-window>
        </template>
        <ComSaleStatusInformation />
    </PageLayout>
</template>
<script setup>
import PageLayout from '../../components/layout/PageLayout.vue';

import ComPendingSaleList from './ComPendingSaleList.vue';

import { inject, createToaster, useRouter } from "@/plugin"
import ComTableGroupTabHeader from './components/table_layouts/ComTableGroupTabHeader.vue';
import ComSaleStatusInformation from './components/ComSaleStatusInformation.vue';
 
import ComTableLayoutActionButton from './components/table_layouts/ComTableLayoutActionButton.vue';
import ComArrangeTable from './components/table_layouts/ComArrangeTable.vue';
import ComRenderTableNumber from './components/table_layouts/ComRenderTableNumber.vue';

const tableLayout = inject("$tableLayout");

const router = useRouter()



tableLayout.getSaleList()
 

showHiddentTable();
 

function onOpenSaleScreen(table, guest_cover) {
    router.push({ name: "AddSale", "table_number": table.tbl_number })
}




function showHiddentTable() {

    const container = document.getElementsByClassName("v-window__container");

    tableLayout.table_groups.forEach(function (g) {
        g.tables.forEach(function (t) {

            if (t.x < 0) {
                t.x = 0;
            }

            if (t.y < 0) {
                t.y = 0
            }

        })
    })
}





</script> 