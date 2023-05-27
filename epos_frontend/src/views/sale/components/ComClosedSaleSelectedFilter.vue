<template>

    <v-chip size="small" class="mr-2 mb-2" v-if="currentFilter.keyword && reportOption.show_keyword" closable @click:close="onRemove('keyword', '')">
        {{currentFilter.keyword}}
        <v-tooltip
        activator="parent"
        location="top"
      >{{ $t('Keyword') }}</v-tooltip>
    </v-chip>
    
    <v-chip size="small" class="mr-2 mb-2" v-if="currentFilter.working_day && !currentFilter.cashier_shift">
        {{currentFilter.working_day}}
        <v-tooltip
        activator="parent"
        location="top"
      >{{ $t('Working Day') }}</v-tooltip>
    </v-chip>
    
    <v-chip size="small" class="mr-2 mb-2" v-if="currentFilter.cashier_shift && reportOption.show_cashier_shift"  closable @click:close="onRemove('cashier_shift', '')">
        {{currentFilter.cashier_shift}}
        <v-tooltip
        activator="parent"
        location="top"
      >{{ $t('Cashier Shift') }}</v-tooltip>
    </v-chip>
    
    <v-chip size="small" class="mr-2 mb-2" v-if="currentFilter.outlet && reportOption.show_outlet" closable @click:close="onRemove('outlet', '')">
        {{currentFilter.outlet}}
        <v-tooltip
        activator="parent"
        location="top"
      >{{ $t('Outlet') }}</v-tooltip>
    </v-chip>
    
    <v-chip size="small" class="mr-2 mb-2" v-if="currentFilter.table_group && reportOption.show_table_group"  closable @click:close="onRemove('table_group', '')">
        {{currentFilter.table_group}}
        <v-tooltip
        activator="parent"
        location="top"
      >{{ $t('Table Group') }}</v-tooltip>
    </v-chip>
    
    <v-chip size="small" class="mr-2 mb-2" v-if="currentFilter.sale_type && reportOption.show_sale_type"  closable @click:close="onRemove('sale_type', '')">
        {{currentFilter.sale_type}}
        <v-tooltip
        activator="parent"
        location="top"
      >{{ $t('Sale Type') }}</v-tooltip>
    </v-chip>
    
    <v-chip size="small" class="mr-2 mb-2" v-if="currentFilter.payment_type!='All Payment Type' && reportOption.show_payment_type"  closable @click:close="onRemove('payment_type', 'All Payment Type')">
        {{currentFilter.payment_type}}
        <v-tooltip
        activator="parent"
        location="top"
      >{{ $t('Payment Type') }}</v-tooltip>
    </v-chip>


</template>
<script setup>
import { defineProps, defineEmits, ref, inject } from '@/plugin';
const props = defineProps({
    reportOption: {
        type: Object,
        require: true
    },
    currentFilter: {
        type:Object
    }
})
const emit = defineEmits(["onSearch"])

function onRemove(key, value){
  let result = JSON.parse(JSON.stringify(props.currentFilter))
  result[key] = value
  emit("onSearch",result)
}

</script>