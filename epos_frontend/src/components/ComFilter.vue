<template>
    <div class="pb-3">
        <div class="flex justify-between items-end" v-if="resource.data">
            <div>
                <div class="flex flex-wrap items-end">
                    <div style="min-width: 140px">
                        <!-- <comInput keyboard label="ID" v-model="name" variant="underlined"/> -->
                    </div>
                    <template v-for="(f, index) in resource.data.fields.filter(r => r.in_standard_filter == 1)" :key="index">
                        <div style="min-width: 160px">
                            <comInput keyboard
                                v-if="f.fieldtype == 'Data' || f.fieldtype == 'Text' || f.fieldtype == 'Small Text' || f.fieldtype == 'Long Text'"
                                v-model="f.value" :label="f.label"   variant="underlined"/>
                            <comInput keyboard type="number"
                                v-if="f.fieldtype == 'Int' || f.fieldtype == 'Float' || f.fieldtype == 'Currency'" v-model="f.value"
                                :label="f.label" variant="underlined"/>
                            <Datepicker v-if="f.fieldtype == 'Date'" v-model="f.value" format="yyyy-MM-dd"></Datepicker>
                            <ComAutoComplete v-model="f.value" v-if="f.fieldtype == 'Link'" :doctype="f.options"   variant="underlined"/>
                            <v-select 
                                v-if="f.fieldtype == 'Select'" 
                                v-model="f.value" 
                                :label="f.label"
                                :items="f.options.split('\n')"
                                @click:clear="f.value=''"
                                hide-no-data
                                hide-details 
                                clearable
                                variant="underlined"
                                ></v-select>
                        </div>
                    </template>
                </div>
            </div>
            <div>
                <div class="flex flex-wrap justify-end">
                <!-- Advance Filter -->
                    <v-btn @click="onDefualtFilter" prepend-icon="mdi-magnify" size="small" class="mr-4 mt-1 mb-1">Search</v-btn>
                    <ComAdvanceFilter :resource="resource" @onApplyFilter="onApplyFilter"/>
                    <ComOrderBy :fields="resource.data.fields" @onOrderby="onOrderby" :default-orderby="order_by"/>
                    <v-btn class="ml-4 mt-1 mb-1" size="small" @click="$emit('onRefresh')" variant="tonal">
                        <v-icon>mdi-refresh</v-icon>
                    </v-btn>
                </div>
            </div>
        </div>
    </div>
</template>
<script setup>

import { defineProps, createResource, ref, reactive, defineEmits } from '@/plugin'
 
import ComAutoComplete from "./form/ComAutoComplete.vue"
import comInput from "./form/ComInput.vue"
import moment from '@/utils/moment.js'
import ComOrderBy from './table/ComOrderBy.vue';
import ComAdvanceFilter from './table/ComAdvanceFilter.vue';

const props = defineProps({ doctype: String })
const emit = defineEmits(['onFilter'])
const name = ref("")

let filter = reactive({})
let order_by = ref('modified desc')
const advancefilters = ref([])

const resource = createResource({
    url: "epos_restaurant_2023.api.api.get_meta",
    params:{
        doctype: props.doctype,
    },
    
    auto: true,
    onSuccess(data) {
        if(data.sort_field && data.sort_order){
            order_by.value = data.sort_field +  ' ' + data.sort_order
        }
        data.fields.forEach(function (r) {
            r.value = ""
        })
    }
})

function generateFilter() {
    
    //check name filter
    // filter = {}
    if (name.value) {
        filter["name"] = ["like", '%' + name.value + '%']
    }

    resource.data.fields.filter(r => r.in_standard_filter == 1).forEach((r) => {
        if (r.value) {
            filter[r.fieldname] = ["=", getFiltervalue(r,"=")]
        }
    });
    
    advancefilters.value.filter(r => r.value!="" ).forEach((r) => {
        if (r.value) {
            filter[r.field.fieldname] = [r.operator, getFiltervalue(r,r.operator)]
        }
    });

    emit('onFilter', filter, order_by.value)
}
function getFiltervalue(d, operator="=") {
    let value ="";
    if (d.fieldtype == "Date") {
        value =  moment(d.value).format('YYYY-MM-DD');
    } else {
        
        value =  d.value;
    }
    if (operator.toLocaleLowerCase()=='like'){
        value = "%" + value + "%";
    }
    return value;
}
function onOrderby($event) {
    order_by.value = $event;
    emit('onFilter', filter, $event)
}
function onSearch(){
    generateFilter();
    menu.value = false;
}
function onApplyFilter($event){
    filter = {}
    advancefilters.value = $event
    generateFilter()
}
function onDefualtFilter(){
    filter = {} 
    generateFilter()
}
</script>