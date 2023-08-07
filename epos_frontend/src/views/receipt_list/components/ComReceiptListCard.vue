<template>
    <div v-if="dataResource.data">
        <div class="bg-gray-50 p-2 elevation-1 -mx-3">
            <ComFilter v-if="!meta.loading" :meta="meta" @onFilter="onFilter" @onRefresh="onRefresh"/>
        </div>
        <div>
            <v-progress-linear v-if="dataResource.loading" style="position: absolute; z-index: 9999999999;" indeterminate
			color="blue-lighten-3"></v-progress-linear>
            
            <div class="relative">
                <div v-if="dataResource.loading" class="absolute left-0 right-0 top-0 bottom-0 z-10" style="background-color: #26262661;">
                    <div class="h-full w-full flex justify-center items-center">
                        <div class="text-center">
                            <v-progress-circular indeterminate></v-progress-circular>
                            <div class="mt-1">{{ $t('Loading') }}...</div>
                        </div>
                    </div>
                </div>
                <div id="sale-card-list" class="-mx-3">
                    <v-list>
                        <v-list-item v-for="(s, index) in dataResource.data" :key="index" @click="callback(s)" class="border-b border-gray-400">
                          <template v-slot:title>
                            {{ s.name }} - {{  s.customer_name }}
                          </template>
                          <template v-slot:subtitle>
                            <div class="-ml-1">
                                <v-chip size="x-small" class="m-1" :color="s.sale_status_color" v-if="s.sale_status">{{ s.sale_status }}</v-chip>
                            </div>

                          </template>
                          <template v-slot:default> 
                                <table class="text-sm text-gray-500"> 
                                    <tr>
                                        <td>{{ $t('Table') }}</td>
                                        <td class=" px-2">:</td>
                                        <td>{{ s.tbl_number }}</td>
                                    </tr>
                                    <tr>
                                        <td>{{ $t('Date') }}</td>
                                        <td class=" px-2">:</td>
                                        <td>{{ s.posting_date }}</td>
                                    </tr>
                                    <tr>
                                        <td>{{ $t('Qty') }}</td>
                                        <td class=" px-2">:</td>
                                        <td>{{ s.total_quantity }}</td>
                                    </tr>
                                    <tr v-if="s.grand_total">
                                        <td>{{ $t('Grand Total') }}</td>
                                        <td class=" px-2">:</td>
                                        <td><CurrencyFormat :value="s.grand_total"></CurrencyFormat></td>
                                    </tr>
                                    <tr v-if="s.total_discount" class="text-blue">
                                        <td>{{ $t('Total Discount') }}</td>
                                        <td class=" px-2">:</td>
                                        <td><CurrencyFormat :value="s.total_discount"></CurrencyFormat></td>
                                    </tr>
                                    <tr class="text-green-600" v-if="s.total_paid_with_fee">
                                        <td>{{ $t('Total Paid') }}</td>
                                        <td class=" px-2">:</td>
                                        <td><CurrencyFormat :value="s.total_paid_with_fee"></CurrencyFormat></td>
                                    </tr>
                                    <tr class="text-red-500" v-if="s.balance">
                                        <td>{{ $t('Balance') }}</td>
                                        <td class=" px-2">:</td>
                                        <td><CurrencyFormat :value="s.balance"></CurrencyFormat></td>
                                    </tr>
                                </table>
                            </template>
                        </v-list-item>
                    </v-list>
                </div>
            </div>
            <div class="p-6 text-center elevation-1 text-gray-400" v-if="dataResource.data.length == 0">
                <div><v-icon icon="mdi-package-variant" style="font-size:60px"></v-icon></div>
                <div class="text-sm italic">{{ $t('Empty Data') }}</div>
            </div>
            
        </div>
        <div :class="mobile ? 'pb-7' :''">
            <div class="text-center items-center pt-2" v-if="countResource.data"> 
                <v-row justify="center" align="center" class="m-0">
                    <v-col cols="12">
                        <div class="w-24">
                            <v-select
                            size="x-small"
                            label="Select"
                            :items="[5,10,20,30,40,50,100]"
                            v-model="pagerOption.itemPerPage"
                            hide-no-data
                            hide-details
                            variant="underlined"
                            ></v-select>
                        </div>
                    </v-col>
                    <v-col cols="12" v-if="!mobile"> 
                        <v-pagination
                        size="x-small"
                        v-model="pagerOption.currentPage"
                        :length="totalPage"
                        total-visible="8"
                        ></v-pagination> 
                    </v-col>
                </v-row>
            </div>
        </div>
    </div>
</template>
  
<script setup>
import { createResource, reactive, defineEmits, watch,inject,ref } from '@/plugin'
import ComFilter from '../../../components/ComFilter.vue';
import {useDisplay} from 'vuetify'
let filter = reactive({});
const emit = defineEmits(['callback'])
const moment = inject('$moment')
const gv = inject('$gv')
const {mobile} = useDisplay()
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
    itemPerPage: 20,
    currentPage: 1,
    orderBy: '',
    filters:{}
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
        gv.customerMeta = data;
        if(data.sort_field && data.sort_order){
            getDataResourceParams(data.sort_field +  ' ' + data.sort_order)
        }
        
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
            filters: pagerOption.filters,
            order_by: pagerOption.orderBy ? pagerOption.orderBy : gv.customerMeta?.sort_field + ' ' + gv.customerMeta?.sort_order,
            limit_page_length: pagerOption.itemPerPage,
            limit_start: ( (pagerOption.currentPage -1) * pagerOption.itemPerPage )
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

function getFieldFromTemplate(str) {
    var results = [], re = /{([^}]+)}/g, text;

    while (text = re.exec(str)) {
        results.push(text[1]);
    }
    return results;
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

function callback(data) {
    const paramsCallBack = {fieldname: 'name', data: data}
    emit('callback', paramsCallBack)
}

</script>
 