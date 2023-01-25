<template>
    
    <div v-if="dataResource.data">
        <div class="bg-gray-50 p-2 elevation-1">
            <ComFilter doctype="Sale" @onFilter="onFilter" @onRefresh="onRefresh"/>
        </div>
        <div>
            <v-progress-linear v-if="dataResource.loading"  style="position: absolute; z-index: 9999999999;" indeterminate
			color="teal"></v-progress-linear>
            <v-data-table 
                :headers="getHeaders()" 
                :items="dataResource.data"
                :items-per-page="pagerOption.itemPerPage"
                item-value="name"
                class="elevation-1">
                <template v-for="h in headers" v-slot:[`item.${h.key}`]="{ item }">
                    <template v-if="h.fieldtype == 'Currency'">
                        <div @click="callback(h, item.raw)">
                        
                            <CurrencyFormat :value="item.raw[h.key]"/>
                        </div>
                    </template>
                    <template v-else-if="h.fieldtype=='Date'">
                        <span @click="callback(h, item.raw)" >
                            {{  moment(item.raw[h.key]).format('DD-MM-YYYY') }}
                        </span>
                    </template>
                    <template v-else-if="!h.fieldtype">
                        <template v-if="h.template">
                            <div @click="callback(h, item.raw)" v-html="getFieldValue(h, item.raw)"></div>
                        </template>
                        <span @click="callback(h, item.raw)" v-else>
                            {{ item.raw[h.key] }}
                        </span>
                    </template>
                </template>
            </v-data-table>
            <div class="p-6 text-center elevation-1 text-gray-400" v-if="dataResource.data.length == 0">
                <div><v-icon icon="mdi-package-variant" style="font-size:60px"></v-icon></div>
                <div class="text-sm italic">There are no data.</div>
            </div>
        </div>
        <div>
            <div class="text-center items-center pt-2" v-if="countResource.data"> 
                <v-row justify="center" align="center" class="m-0">
                    <v-col cols="2">
                        <div class="w-24">
                            <v-select
                            label="Select"
                            :items="[5,10,20,30,40,50,100]"
                            v-model="pagerOption.itemPerPage"
                            hide-no-data
                            hide-details
                            variant="underlined"
                            ></v-select>
                        </div>
                    </v-col>
                    <v-col cols="8"> 
                        <v-pagination
                        v-model="pagerOption.currentPage"
                        class="my-4"
                        :length="pagerOption.totalPage"
                        total-visible="8"
                        ></v-pagination> 
                    </v-col>
                    <v-col cols="2"></v-col>
                </v-row>
            </div>
        </div>

    </div>


</template>
  
<script setup>
import { VDataTable } from 'vuetify/labs/VDataTable'
import { createResource, reactive, defineEmits, watch,inject } from '@/plugin'
import ComFilter from '../ComFilter.vue';

let filter = reactive({});
const emit = defineEmits(['callback'])
const moment = inject('$moment')
const props = defineProps({
    headers: {
        type: Array,
        default: []
    },
    doctype: {
        type: String,
        default: ''
    },
    extraFields: String,
  
})
 
let pagerOption = reactive({
    totalRecord: 0,
    itemPerPage: 20,
    totalPage: 0,
    currentPage: 1,
    filters:{}
})

function getHeaders(){
    let h = props.headers;
    h.forEach((r)=>{
        r.sortable=false;
    });
    return h;
}

let countResource = createResource({
    url: 'frappe.client.get_count',
    params:getCountResourceParams(),
    auto: true,
    onSuccess(r) {
        pagerOption.totalRecord = r
        pagerOption.totalPage = Math.ceil(r/pagerOption.itemPerPage)
    }
})

let dataResource = createResource({
    url: 'frappe.client.get_list',
    params: getDataResourceParams(),
    
})

function getDataResourceParams (){
    return {   doctype: props.doctype,
            fields: getFieldName(),
            filters: pagerOption.filters,
            order_by: pagerOption.orderBy,
            limit_page_length: pagerOption.itemPerPage,
            limit_start: ( (pagerOption.currentPage -1) * pagerOption.itemPerPage )+ 1
        }
}
function getCountResourceParams (){
    return {   
            doctype: props.doctype, 
            filters: pagerOption.filters
        }
}
 
watch(pagerOption , (currentValue) => {
    setTimeout(function(){
        dataResource.params =   getDataResourceParams()
        dataResource.fetch()

        countResource.params = getCountResourceParams()
        countResource.fetch()
    },500);

   
})
function getFieldName() {
    let fieldnames = []
    props.headers.forEach((r) => {
        fieldnames.push(r.key)
    })
    if (props.extraFields) {
        props.extraFields.split(',').forEach((r) => {
            fieldnames.push(r)
        })
    }
    return fieldnames;

}
dataResource.fetch();


function getFieldFromTemplate(str) {
    var results = [], re = /{([^}]+)}/g, text;

    while (text = re.exec(str)) {
        results.push(text[1]);
    }
    return results;
}

function getFieldValue(header, data) {
    const fields = getFieldFromTemplate(header.template);
    let value = header.template;
    fields.forEach((r) => {
        value = value.replace(`{${r}}`, data[r])
    });
    return value;

}

function callback(header, data) {
    if (header.callback) {
        var obj = {
            fieldname: header.key,
            data: data
        }
        emit('callback', obj)
    }

}

function onFilter($event, orderBy){ 
    pagerOption.currentPage = 1;
    pagerOption.filters = $event;
    pagerOption.orderBy = orderBy;
}

function onSearch(key, operator) {
    return function (value) {
        if (value) {
            if (operator == 'like') {
                value = `%${value}%`
            }
            filter[key] = [operator, value]
        } else {
            delete filter[key]
        }
        dataResource.fetch();
    }
}

function onRefresh() {
    dataResource.fetch()
}


</script>
<style>
.v-data-table-footer {
        display: none !important;
    }
</style>