<template>
    <v-btn :loading="tableLayout.saleListResource.loading" icon color="info" @click="onRefreshSale">
        <v-icon>mdi-cached</v-icon>
    </v-btn>
    <template v-if="!mobile">
        <v-btn @click="onViewPendingOrder">
            Pending order
        </v-btn>
         
    </template>
    <v-btn :loading="tableLayout.saveTablePositionResource.loading" v-if="tableLayout.canArrangeTable"
        @click="onSaveTablePosition">
        Save Table Position
    </v-btn>
    <v-menu>
        <template v-slot:activator="{ props }">
            <v-btn v-bind="props">
                <v-icon>mdi-dots-vertical</v-icon>
            </v-btn>
        </template>
        <v-list>
            <v-list-item v-if="!mobile" @click="onEnableArrageTable">
                <v-list-item-title>Arrange Table Layout</v-list-item-title>
            </v-list-item>
            <template v-if="mobile">
                <v-list-item @click="onViewPendingOrder">
                    <v-list-item-title>Pending Order</v-list-item-title>
                </v-list-item>
                

            </template>

        </v-list>
    </v-menu>
</template>
<script setup>
import { inject } from '@/plugin';
import { useDisplay } from 'vuetify'
const tableLayout = inject("$tableLayout");
const { mobile } = useDisplay()

function onRefreshSale() {


    tableLayout.saleListResource.fetch();
}

function onEnableArrageTable(){
    tableLayout.canArrangeTable = true;
}

function onViewPendingOrder() {
    alert("open pending order dialog box");
}


function onSaveTablePosition() {
    tableLayout.saveTablePositionResource.params = {
        "device_name": localStorage.getItem("device_name"),
        "table_group": JSON.parse(JSON.stringify(tableLayout.table_groups))
    };
    tableLayout.saveTablePositionResource.submit();
    tableLayout.canArrangeTable = false;

}


</script>