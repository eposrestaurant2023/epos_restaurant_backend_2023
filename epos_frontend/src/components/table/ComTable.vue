<template>
    <div>
        <v-progress-linear v-if="dataResource.loading" style="position: absolute; z-index: 9999999999;" indeterminate
                color="blue-lighten-3"></v-progress-linear>
        <div>
            <div class="bg-gray-50 p-2 elevation-1">
                <ComFilter v-if="!meta.loading" :meta="meta" @onFilter="onFilter" @onRefresh="onRefresh"/>
                <div v-else class="h-12"></div>
            </div>
            <div>
                <div class="relative">
                    <div v-if="!dataResource.data || dataResource.loading" class="absolute left-0 right-0 top-0 bottom-0 z-10" style="background-color: #26262661;">
                        <div class="h-full w-full flex justify-center items-center">
                            <div class="text-center">
                                <v-progress-circular indeterminate></v-progress-circular>
                                <div class="mt-1">{{ $t('Loading') }}...</div>
                            </div>
                        </div>
                    </div>
                    <div v-else>
                        <v-data-table
                            v-if="dataResource?.data?.length > 0 && !dataResource.loading"
                            :headers="getHeaders()" 
                            :items="dataResource.data"
                            :items-per-page="pagerOption.itemPerPage"
                            item-value="name"
                            class="elevation-1">
                            <template v-for="h in headers" v-slot:[`item.${h.key}`]="{ item }">
                                <template v-if="h.fieldtype == 'Image'">
                                    <div class="text-center">
                                        <v-avatar v-if="item.raw[h.key]">
                                            <v-img :src="item.raw[h.key]"></v-img>
                                        </v-avatar>
                                        <avatar v-else :name="item.raw[h.placeholder]" class="my-0 mx-auto" size="40"></avatar>
                                    </div>
                                </template>
                                <template v-if="h.fieldtype == 'Currency'">
                                    <div @click="callback(h, item.raw)" :class="{'text-blue-600 cursor-pointer':h.callback}">
                                        <CurrencyFormat :value="item.raw[h.key]"/>
                                    </div>
                                </template>
                                <template v-else-if="h.fieldtype=='Date'">

                                    <span @click="callback(h, item.raw)" :class="{'text-blue-600 cursor-pointer':h.callback}">
                                        {{  moment(item.raw[h.key]).format('DD-MM-YYYY') }}
                                    </span>
                                </template>
                                <template v-else-if="h.fieldtype=='Status'"> 
                                    <v-chip v-if="doctype=='Sale'" :color="item.raw[h.color_field]" size="small"> {{ item.raw[h.key] }}</v-chip>
                                    <template v-else>
                                        <v-chip v-if="item.raw[h.key]" color="success" size="small">{{ $t('Enabled') }}</v-chip>
                                        <v-chip v-else color="error" size="small">{{ $t('Disabled') }}</v-chip>
                                    </template>
                                </template>
                                <template v-else-if="!h.fieldtype">
                                    <template v-if="h.template">
                                        <div @click="callback(h, item.raw)" v-html="getFieldValue(h, item.raw)"  :class="{'text-blue-600 cursor-pointer':h.callback}"></div>
                                    </template>
                                    <span @click="callback(h, item.raw)" :class="{'text-blue-600 cursor-pointer':h.callback}" v-else>
                                        {{ item.raw[h.key] }}
                                    </span>
                                </template>
                                
                            </template>
                        </v-data-table>
                        <div class="p-6 text-center elevation-1 text-gray-400" v-else>
                            <div><v-icon icon="mdi-package-variant" style="font-size:60px"></v-icon></div>
                            <div class="text-sm italic">{{ $t('Empty Data') }}</div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <div class="text-center items-center pt-2" v-if="countResource.data"> 
                    <v-row justify="center" align="center" class="m-0">
                        <v-col cols="2">
                            <div class="w-24">
                                <v-select
                                size="small"
                                label=""
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
                            size="small"
                            v-model="pagerOption.currentPage"
                            class="my-4"
                            :length="totalPage"
                            total-visible="8"
                            ></v-pagination> 
                        </v-col>
                        <v-col cols="2"></v-col>
                    </v-row>
                </div>
            </div>
        </div>
    </div>
</template>
  
<script setup>
import { VDataTable } from 'vuetify/labs/VDataTable'
import { createResource, reactive, defineEmits, watch,inject,ref, onUnmounted } from '@/plugin'
import ComFilter from '../ComFilter.vue';

let filter = reactive({});
const emit = defineEmits(['callback'])
const moment = inject('$moment')
const gv = inject('$gv')
const order = ref({})
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
    businessBranchField: String,
    posProfileField: String
})
 
let pagerOption = reactive({
    itemPerPage: 20,
    currentPage: 1,
    orderBy: '',
    filters: {}
})

let totalRecord = ref(0)
let totalPage = ref(0)

function getHeaders(){
    let h = props.headers;
    h.forEach((r)=>{
        r.sortable=false;
    });
    return h;
}
const meta = createResource({
    url: "epos_restaurant_2023.api.api.get_meta",
    params:{
        doctype: props.doctype,
    },
    
    auto: true,
    async onSuccess(data) {
        order.value = data;
        data.fields.forEach(function (r) {
            r.value = null
        })
        await countResource.fetch()
    }
})

let countResource = createResource({
    url: 'frappe.client.get_count',
    params:getCountResourceParams(),
    auto: true,
    onSuccess(r) {
        totalRecord.value = r
        totalPage.value = Math.ceil(r/pagerOption.itemPerPage)
        dataResource.params =   getDataResourceParams()
        dataResource.fetch()
    }
})

let dataResource = createResource({
    url: 'frappe.client.get_list',
    params: getDataResourceParams(),      
})

function getDataResourceParams (){
 
    return {  
         doctype: props.doctype,
            fields: getFieldName(),
            filters: getFilter(),
            order_by: pagerOption.orderBy ? pagerOption.orderBy : order.value.sort_field && order.value.sort_order ? order.value?.sort_field + ' ' + order.value?.sort_order : '',
            limit_page_length: pagerOption.itemPerPage,
            limit_start: ( (pagerOption.currentPage -1) * pagerOption.itemPerPage )
        }
}
function getCountResourceParams (){
    return {   
            doctype: props.doctype, 
            filters: getFilter()
        }
}

function getFilter(){
    let filters = JSON.parse(JSON.stringify(pagerOption.filters))
    if(gv.setting.specific_pos_profile && props.posProfileField){
        filters[props.posProfileField] = ["=", localStorage.getItem('pos_profile')]
    }
    else if(gv.setting.specific_business_branch && props.businessBranchField){
        filters[props.businessBranchField] = ["=", gv.setting.business_branch]
    }
    filters["docstatus"] = ["in",[1,0]]
    return filters
}
 
watch(pagerOption , (currentValue) => {
    setTimeout(function(){
        dataResource.params = getDataResourceParams()
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


const pageListener = async function (e) {
    if (e.isTrusted && typeof (e.data) == 'string') {
        if(e.data=="refresh"){
            dataResource.fetch()
        }

    }
};



window.addEventListener('message', pageListener, false);

onUnmounted(() => {
    window.removeEventListener('message', pageListener, false);
}) 


</script>
<style>
.v-data-table-footer {
    display: none !important;
}
.v-table__wrapper table {
    white-space: nowrap;
}
.v-data-table-header__content {
    display: inline-block !important;
}
.v-data-table .v-table__wrapper > table > thead > tr > td.v-data-table-column--align-center,
.v-data-table .v-table__wrapper > table > thead > tr th.v-data-table-column--align-center,
.v-data-table .v-table__wrapper > table tbody > tr > td.v-data-table-column--align-center,
.v-data-table .v-table__wrapper > table tbody > tr th.v-data-table-column--align-center {
  text-align: center;
}
</style>