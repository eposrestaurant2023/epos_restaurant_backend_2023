<template>
    <div class="h-full">
        <div class="pb-1 px-1 font-bold">{{ $t('Filter') }}</div>
        <div class="overflow-y-auto p-1">
            <ComInput v-if="reportOption.show_keyword" :label="$t('Search Receipt Number')" density="default" class="mb-4" keyboard
                v-model="currentFilter.keyword" v-debounce="onSearchKeyword()" />
            <v-select v-if="reportOption.show_cashier_shift" :label="$t('Cashier Shift')" :items="filterResource.data?.cashier_shifts"
                variant="solo" v-model="filter.cashier_shift" item-title="title" item-value="name"></v-select>

            <v-select v-if="reportOption.show_outlet" :label="$t('Outlet')" :items="filterResource.data?.outlets" variant="solo"
                v-model="filter.outlet" item-title="title" item-value="name"></v-select>

            <v-select v-if="reportOption.show_table_group" :label="$t('Table Group')" :items="filterResource.data?.table_groups"
                variant="solo" v-model="filter.table_group" item-title="title" item-value="name"></v-select>

            <v-select v-if="reportOption.show_sale_type" :label="$t('Sale Type')" :items="filterResource.data?.sale_types" variant="solo"
                v-model="filter.sale_type" item-title="title" item-value="name"></v-select>

            <v-select v-if="reportOption.show_payment_type" :label="$t('Payment Type')" :items="paymentTypes" variant="solo"
                v-model="filter.payment_type" item-title="payment_method" item-value="payment_method"></v-select>
          
            <div class="flex justify-between -m-1">
                <div class="m-1 grow">
                    <v-select v-if="reportOption.show_order_by" :label="$t('Sort Order by')" :items="orderByOptions" variant="solo"
                    v-model="filter.order_by" item-title="title" item-value="name"></v-select>
                </div>
                <div class="m-1 flex-none">
                    <v-select v-if="reportOption.show_order_by" :label="$t('Sort Order Type')" :items="orderByType" variant="solo"
                        v-model="filter.order_by_type" item-title="title" item-value="name"></v-select>
                    </div>
                </div>
        </div>
        <div class="flex items-end pt-1">
            <v-btn prepend-icon="mdi-magnify" block size="large" @click="onSearch" color="success">
                {{$t('Search')}}
            </v-btn>
        </div>
    </div>
</template>
<script setup>
import { defineProps, defineEmits, ref, inject,computed, i18n} from '@/plugin';
import { useDisplay } from 'vuetify'
const { t: $t } = i18n.global;  

const {mobile} = useDisplay();
const props = defineProps({
    filterResource: {
        type: Object,
        require: true
    },
    reportOption: {
        type: Object,
        require: true
    },
    currentFilter: {
        type:Object
    }
})
const emit = defineEmits(["onSearch"])
const gv = inject("$gv")
const filter = computed({
    get(){
        return props.currentFilter
    },
    set(newValue){
        return newValue
    }
})

const groupByOptions = ref([
    { name: "Sale Type" },
    { name: "Payment Type" },
])
const orderByOptions = [
    { "name": "name", title: "Receipt Number" },
    { "name": "posting_date", title: "Date" },
    { "name": "modified", title: "Closed Date" },
    { "name": "total_quantity", title: "Quantity" },
    { "name": "grand_total", title: "Grant Total" },
]
const orderByType = [
    { "name": "asc", title: "Ascending" },
    { "name": "desc", title: "Descending" }
]

const paymentTypes = JSON.parse(JSON.stringify(gv.setting?.payment_types));
paymentTypes.unshift({ payment_method: "All Payment Type" });

function onSearchKeyword(){
    if(!mobile){
        onSearch();
    }
}

function onSearch() {

    emit("onSearch", filter.value)
}


</script>