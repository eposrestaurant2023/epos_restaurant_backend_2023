<template>
    <v-btn :loading="tableLayout.saleListResource.loading" icon color="info" @click="onRefreshSale">
        <v-icon>mdi-cached</v-icon>
    </v-btn>
    <template v-if="!mobile">
        <v-btn @click="onViewPendingOrder">
            Pending order
        </v-btn>
         
    </template>
    <v-btn @click="onShowHideSaleStatus(true)">
            Hide Sale Status
    </v-btn>
    <v-btn @click="onShowHideSaleStatus(false)">
            Show Sale Status
    </v-btn>
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
        <v-card>
            <v-list v-if="gv.setting?.pos_setting?.sale_types && gv.setting?.pos_setting?.sale_types.filter(r=>r.is_order_use_table == false).length > 0">
                <v-list-subheader>Change Sale Type</v-list-subheader>
                <template  v-for="(st, index) in gv.setting?.pos_setting.sale_types.filter(r=>r.is_order_use_table == false)" :key="index">
                    <v-list-item @click="onSaleType(st.name)">
                        <v-list-item-title>{{ st.sale_type_name }}</v-list-item-title>
                    </v-list-item>
                </template>
            </v-list>
            <v-list>
                <template  v-if="!mobile"> 
                    <v-list-subheader>Table Position</v-list-subheader>
                    <v-list-item @click="onEnableArrageTable">
                        <v-list-item-title>Arrange Table Layout</v-list-item-title>
                    </v-list-item>
                </template>
                <template v-if="mobile">
                    <v-list-item @click="onViewPendingOrder">
                        <v-list-item-title>Pending Order</v-list-item-title>
                    </v-list-item>
                </template>

            </v-list>
        </v-card>
    </v-menu>
</template>
<script setup>
import { inject, pendingSaleListDialog,createResource,createToaster, useRouter } from '@/plugin';
import { useDisplay } from 'vuetify'
const gv = inject('$gv')
const tableLayout = inject("$tableLayout");
const emit = defineEmits(['onShowHide'])
const router = useRouter()
const { mobile } = useDisplay()
const toaster = createToaster({position: 'top'})
const posProfile = localStorage.getItem('pos_profile')
function onRefreshSale() {
    tableLayout.saleListResource.fetch();
}

function onEnableArrageTable(){
    tableLayout.canArrangeTable = true;
}

async function onViewPendingOrder() {
    if(workingDayResource.data.name && cashierShiftResource.data.name){
        const result = await pendingSaleListDialog({data:{working_day:workingDayResource.data.name, cashier_shift: cashierShiftResource.data.name}})
    }
    else{
        toaster.error("Cannot get current working day or cashier shift")
    }
}

async function onShowHideSaleStatus(show_hide) {
    emit('onShowHide',show_hide)
}


function onSaveTablePosition() {
    tableLayout.saveTablePositionResource.params = {
        "device_name": localStorage.getItem("device_name"),
        "table_group": JSON.parse(JSON.stringify(tableLayout.table_groups))
    };
    tableLayout.saveTablePositionResource.submit();
    tableLayout.canArrangeTable = false;

}
const workingDayResource = createResource({
    url: "epos_restaurant_2023.api.api.get_current_working_day",
    params: {
      business_branch: gv.setting?.business_branch
    },
    auto:true
});

const cashierShiftResource = createResource({
url: "epos_restaurant_2023.api.api.get_current_cashier_shift",
    params: {
        pos_profile: posProfile
    },
    auto:true
});

function onSaleType(name){
    router.push({name:'AddSaleNoTable',params:{sale_type: name}})
}
</script>