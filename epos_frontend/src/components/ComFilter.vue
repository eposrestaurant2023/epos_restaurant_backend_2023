<template>
    <div class="pb-3">
        <div class="flex items-end"  :class="mobile ? 'justify-end' : 'justify-between'" v-if="resource?.data">
            <div v-if="!mobile">
                <div class="flex flex-wrap items-end">
                    <div style="min-width: 140px"> 
                        <ComInput keyboard :label="$t('ID')" v-model="name" variant="solo" class="m-1" v-debounce="onSearch" @onInput="onSearch"/>
                    </div>
                    <template v-for="(f, index) in resource.data.fields.filter(r => r.in_standard_filter == 1)" :key="index">
                        <template v-if="!(f.options == 'POS Profile' && gv.setting.specific_pos_profile) && !(f.options == 'Business Branch' && gv.setting.specific_business_branch)">
                            <div style="min-width: 200px">
                                <ComInput keyboard
                                    v-if="f.fieldtype == 'Data' || f.fieldtype == 'Text' || f.fieldtype == 'Small Text' || f.fieldtype == 'Long Text'"
                                    v-model="f.value" :placeholder="$t(f.label)" variant="solo" class="m-1" />

                                <ComInput keyboard type="number"
                                    v-if="f.fieldtype == 'Int' || f.fieldtype == 'Float' || f.fieldtype == 'Currency'" v-model="f.value"
                                    :label="$t(f.label)" variant="solo" class="m-1"/>
                                <ComInput type="date" v-if="f.fieldtype == 'Date'" v-model="f.value" class="m-1" :label="$t(f.label)"></ComInput>
                                <ComAutoComplete v-model="f.value" v-if="f.fieldtype == 'Link'" :doctype="f.options"   variant="solo"  :label="$t(f.label)" :placeholder="$t(f.label)" class="m-1"/>
                                <v-select 
                                    density="compact"
                                    v-if="f.fieldtype == 'Select'" 
                                    v-model="f.value" 
                                    :label="$t(f.label)"
                                    :items="f.options.split('\n')"
                                    @click:clear="f.value=''"
                                    hide-no-data
                                    hide-details 
                                    clearable
                                    variant="solo" class="m-1"
                                    ></v-select>
                            </div>
                        </template>
                    </template>
                </div>
            </div>
            <div>
                <div class="flex flex-wrap justify-end">
                <!-- Advance Filter -->
                    
                    <ComAdvanceFilter :resource="resource" @onApplyFilter="onApplyFilter"/>
                    <v-btn v-if="mdAndDown && !mobile" class="ml-0 mt-1 mb-1 mr-1" @click="$emit('onRefresh')" variant="tonal" :size="mdAndDown ? 'small' : 'default'">
                        <v-icon>mdi-refresh</v-icon>
                    </v-btn>
                    <ComOrderBy :fields="resource.data.fields" @onOrderby="onOrderby" :default-orderby="order_by"/>
                    <v-btn v-if="!mdAndDown || mobile" class="ml-4 mt-1 mb-1" @click="$emit('onRefresh')" variant="tonal" :size="mdAndDown ? 'small' : 'default'">
                        <v-icon>mdi-refresh</v-icon>
                    </v-btn>
                </div>
            </div>
        </div>
    </div>
</template>
<script setup>
import { defineProps, createResource, ref, reactive, defineEmits, watch, inject } from '@/plugin'
import ComAutoComplete from "./form/ComAutoComplete.vue"
import ComInput from "./form/ComInput.vue"
import moment from '@/utils/moment.js'
import ComOrderBy from './table/ComOrderBy.vue';
import ComAdvanceFilter from './table/ComAdvanceFilter.vue';
import { useDisplay } from 'vuetify'
const {mobile, mdAndDown} = useDisplay()
const props = defineProps({ 
    // doctype: String 
    meta: Object
})
const emit = defineEmits(['onFilter'])
const name = ref("")
const gv = inject('$gv')
let filter = reactive({})
let order_by = ref('modified desc')
const advancefilters = ref([])

const resource = props.meta 

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
watch(resource, (newValue)=>{
    onSearch()
})
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
function onSearch(value){
    filter = {}
    generateFilter();
}
function onApplyFilter($event){
    filter = {}
    advancefilters.value = $event
    generateFilter()
}
 
</script>