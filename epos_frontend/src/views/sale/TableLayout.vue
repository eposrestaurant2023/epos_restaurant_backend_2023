<template>
    <PageLayout :title="$t('Table Layout')" full icon="mdi-cart">
        <template #centerCotent>
            <ComTableGroupTabHeader />
        </template>
        <template #action>
            <ComTableLayoutActionButton @onShowHide="onTableStatusHidden"/>
        {{ isShowTableStatus() }} 
        </template>
        <template v-if="tableLayout.table_groups">
            <v-window v-model="tableLayout.tab">
                <ComArrangeTable  v-if="tableLayout.canArrangeTable"/>
                <ComRenderTableNumber v-else/>
            </v-window>
        </template> 
        <ComSaleStatusInformation v-if="(table_status_color)"/>
    </PageLayout>
</template>
<script setup>
import PageLayout from '../../components/layout/PageLayout.vue';
import { inject, createResource, useRouter,createToaster,onMounted , onUnmounted,ref,i18n} from "@/plugin";
import ComTableGroupTabHeader from './components/table_layouts/ComTableGroupTabHeader.vue';
import ComSaleStatusInformation from './components/ComSaleStatusInformation.vue';
 
import ComTableLayoutActionButton from './components/table_layouts/ComTableLayoutActionButton.vue';
import ComArrangeTable from './components/table_layouts/ComArrangeTable.vue';
import ComRenderTableNumber from './components/table_layouts/ComRenderTableNumber.vue';

const { t: $t } = i18n.global; 

const toaster = createToaster({position:"top"});
const tableLayout = inject("$tableLayout");
const socket = inject("$socket");

const table_status_color = ref(false);
const router = useRouter();

socket.on("RefreshTable", () => {
  tableLayout.getSaleList(); 
})

//on init
onMounted(async ()=>{
    localStorage.removeItem('make_order_auth');
    const cashierShiftResource = createResource({
        url: "epos_restaurant_2023.api.api.get_current_cashier_shift",
        params: {
            pos_profile: localStorage.getItem("pos_profile")
        }
    });

    await cashierShiftResource.fetch().then(async (v) => {
        if (v==null) {
            toaster.warning($t('msg.Please start shift first'))
            router.push({ name: "Home" });
        }
    });
})

function onTableStatusHidden (value) { 
    localStorage.setItem('table_status_color', value) 
    table_status_color.value = value;
}

function isShowTableStatus(){
    try{
        const s = localStorage.getItem("table_status_color");
        if(s == null){
            table_status_color.value = false;
        }
        table_status_color.value = (s=="true"?true:false);
    }catch(e)
    {
        table_status_color.value = false;
    }
   
}


tableLayout.getSaleList()
 

showHiddentTable();
 

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



onUnmounted(()=>{
    socket.off('RefreshTable');      
});
 
if(localStorage.getItem('redirect_sale_type')){
    localStorage.removeItem('redirect_sale_type')
}

</script> 