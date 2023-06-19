<template>
    <ComModal :fullscreen="true" :hideCloseButton="true" :hideOkButton="true" :fill="true" :isShowBarMoreButton="false" @onClose="onClose()">
        <template #title>
            {{ $t('Bill') }}# {{ params.title }}
        </template>
        <template #bar_custom>
            <v-btn v-if="params.data?.from_table" icon @click="onAddNewOrder()" v-bind="props">
                <v-icon>mdi-plus</v-icon>
            </v-btn>
            <ComPrintBillButton doctype="Sale" :title="$t('Print Bill')" :isMobile="true" />
        </template>
        <template #content>
            <template v-if="!gv.device_setting.is_order_station"> 
                <div class="m-1">
                    <ComSelectCustomer/>
                </div>
            </template>
            <ComGroupSaleProductList/>
        </template>
        <template #action>
            <ComSmallSaleSummary @onClose="onGoHome()" @onSubmitAndNew="onSubmitAndNew()"/>
        </template>
    </ComModal>
</template>
<script setup>
import { defineProps, defineEmits, inject,useRouter,onUnmounted } from '@/plugin'
import ComGroupSaleProductList from '../ComGroupSaleProductList.vue';
import ComPrintBillButton from '../ComPrintBillButton.vue';
import ComSelectCustomer from '../ComSelectCustomer.vue';
import ComSmallSaleSummary from './ComSmallSaleSummary.vue';

const props = defineProps({
    params: Object
})
const sale = inject('$sale')
const gv = inject('$gv')
const emit = defineEmits(['resolve'])
const router = useRouter();

function onGoHome(){
    if(onRedirectSaleType()){
        if (gv.setting.table_groups.length > 0) {
        sale.sale = {};
        router.push({ name: 'TableLayout' }).then(()=>{
            emit('resolve', true)
        });
        }
        else {
            sale.newSale()
            router.push({ name: "AddSale" }).then(()=>{
                emit('resolve', true);
            });
        }
    }
}
function onRedirectSaleType(){
    const redirect_sale_type = localStorage.getItem("redirect_sale_type") || null
    if(redirect_sale_type){
        router.push({name: 'AddSaleNoTable', params: {sale_type: redirect_sale_type}}).then(()=>{
            emit('resolve', false)
        });
        return false;
    }
    return true;
}
function onSubmitAndNew(){
    onRedirectSaleType()
}

function onClose() {
    emit('resolve', false)
}

function onAddNewOrder(){
    if (!sale.isBillRequested()) {
        sale.no_loading = true        
        router.push({
            name: "AddSale", params: {
                name: sale.sale.name
            }
        }).then(()=>{
            emit('resolve', true)
        });
    }
}


const onEventListener = async function (e) {
    if (e.isTrusted && typeof (e.data) == 'string') {
        if(e.data == "close_modal"){
            emit('resolve', true);
        }
        
    }
};

 
window.addEventListener('message', onEventListener, false);

onUnmounted(() => {
    window.removeEventListener('message', onEventListener, false);
}) 
 

</script>