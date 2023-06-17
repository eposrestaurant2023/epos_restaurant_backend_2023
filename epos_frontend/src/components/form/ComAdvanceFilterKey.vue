<template>
 
    <div class="flex flex-nowrap items-center -mx-1 my-1" v-if="item">
        <div class="w-[200px] mx-1">  
            <v-autocomplete
                v-model="item.fieldname"
                style="width: 100%;"
                hide-no-data
                hide-details
                density="compact"
                :items="fields.filter(r=>r.fieldname!='naming_series').filter((r)=> r.fieldtype=='Data' || r.fieldtype=='Date' || r.fieldtype=='Int' || r.fieldtype=='Float' || r.fieldtype=='Currency' || r.fieldtype=='Link' || r.fieldtype=='Select' || r.fieldtype=='Check')" 
                item-title="label"
                item-value="item"
                :custom-filter="OnFilter"
                :menu-props="{ maxHeight: 250 }"
                variant="solo"
                class="ma-0">
                <template v-slot:item="{ props, item }">
                    <v-list-item v-bind="props" :title="item.raw.label" @click="onSelected(item.raw)">
                    </v-list-item>
                </template>
            </v-autocomplete>
        </div>
        <div class="w-[150px] mx-1" v-if="item.fieldname">
            <div>
                <v-select
                    style="width: 100%;"
                    v-model="item.operator"
                    density="compact"
                    hide-no-data
                    hide-details
                    :items="getOperaters"
                    item-title="label"
                    item-value="operator"
                    placeholder="Fieldname"
                    variant="solo"></v-select>
            </div>
        </div>
        <div class="w-[200px] mx-1" v-if="item.fieldname">
             <div>
                <div v-if="selectedField?.fieldtype=='Link' && !loadingAutoComplete.value && (item.operator == '=' || item.operator == '!=')" >
                    <ComAutoComplete
                        :doctype="selectedField?.options"
                        v-model="item.value"/> 
                </div>
                <div v-else>
                    <ComInput 
                        keyboard
                        v-if="showInputText" 
                        v-model="item.value"/>
                    <ComInput 
                        keyboard 
                        type="number" 
                        v-else-if="showInputNumber"
                        v-model="item.value"/>
                    <ComInput v-else-if="showDatepick" type="date" v-model="item.value"/>
                    <v-select
                        density="compact"
                        hide-no-data
                        hide-details
                        variant="solo"
                        :multiple="multipleSelect"
                        v-else-if="showSelect"
                        v-model="item.value"
                        :items="getSelectOptions"
                        item-title="label"
                        item-value="value"
                        ></v-select> 
                </div>
             </div>
        </div>
        <div class="text-sm text-gray-400 w-[330px] mx-1" v-if="!item.fieldname">
            {{$t('Please select fieldname first')}}
        </div>
        <div class="mx-1">
            <v-icon size="large" @click="$emit('onRemove',item)" color="error">mdi-close</v-icon>
        </div>
    </div>
</template>
<script setup>
import { ref, defineProps, computed, reactive, watch } from '@/plugin'
import ComInput from './ComInput.vue';
import ComAutoComplete from "./ComAutoComplete.vue"
let props = defineProps({ 
    fields: Array,
    filter: Object
})
let item = ref(props.filter)
 
const operators = reactive([
    {label : 'Equals', operator:'=', link: true, check: true, date: true, text:true, select: true, number: true},
    {label : 'Not Equals', operator: '!=', link: true, date: true, text:true, select: true, number: true},
    {label : 'Like', operator: 'like', link: true, text:true},
    {label : 'Not Like', operator: 'not like', link: true, text:true},
    {label : 'In', operator: 'in', link: true, text:true, select: true},
    {label : 'Not In', operator: 'not in', link: true, text:true, select: true},
    // {label : 'Is', operator: 'is', link: true, text:true, select: true},
    {label : '>', operator:'>', date: true, text:true, select: true, number: true},
    {label : '>=', operator:'>=', date: true, text:true, select: true, number: true},
    {label : '<',operator:'<', date: true, text:true, select: true, number: true},
    {label : '<=', operator:'<=', date: true, text:true, select: true, number: true},
    // {label : 'Between', operator:'Between', date: true, text: false},
    {label : 'Timespan', operator:'Timespan', date: true, text: false}
])
const checkOptions = reactive([
    {label : 'Yes', value: 1},
    {label : 'No', value: 0}
])
const isOptions = reactive([
    {label : 'Set', value: 'set'},
    {label : 'Not Set', value: 'not set'}
])

const timespanOptions = reactive([
    {label : 'Last Week', value: 'last week'},
    {label : 'Last Month', value: 'last month'},
    {label : 'Last Quarter', value: 'last quarter'},
    {label : 'Last 6 months', value: 'last 6 months'},
    {label : 'Last Year', value: 'last year'},
    {label : 'Yesterday', value: 'yesterday'},
    {label : 'Today', value: 'today'},
    {label : 'Tomorrow', value: 'tomorrow'},
    {label : 'This Week', value: 'this week'},
    {label : 'This Month', value: 'this month'},
    {label : 'This year', value: 'this year'},
    {label : 'Next Year', value: 'next year'},
    {label : 'Next Month', value: 'next month'},
    {label : 'Next 6 months', value: 'next 6 months'},
    {label : 'Next Year', value: 'next year'},
])
let selectedField = ref(props.filter.field)
 
const loadingAutoComplete = ref(false)

const showAutocomplete = computed(()=>{
    return selectedField.value?.fieldtype=='Link' && !loadingAutoComplete.value && (item.operator == '=' || item.operator == '!=')
})

const showInputNumber = computed(()=>{
    return selectedField.value?.fieldtype=='Int' || selectedField.value?.fieldtype=='Float' || selectedField.value?.fieldtype=='Currency'
})
const showInputText = computed(() => {
    return ((selectedField.value?.fieldtype=='Link' 
    || selectedField.value?.fieldtype=='Data' 
    || selectedField.value?.fieldtype=='Text' 
    || selectedField.value?.fieldtype=='Small Text' 
    || selectedField.value?.fieldtype=='Long Text')
    && item.value.operator != 'is'
    )
})

const showDatepick = computed(()=>{
    return selectedField.value?.fieldtype=='Date' && item.value.operator != 'Timespan'
})
const showSelect = computed(()=>{
    return selectedField.value?.fieldtype=='Select' || selectedField.value?.fieldtype == 'Check' || item.value.operator == 'is' || item.value.operator == 'Timespan'
})
const getOperaters = computed(()=>{
    if(selectedField.value?.fieldtype=='Link'){
        return operators.filter(r=>r.link == true)
    }
    else if(selectedField.value?.fieldtype=='Data'){
        return operators.filter(r=>r.text == true)
    }
    else if(selectedField.value?.fieldtype=='Date'){
        return operators.filter(r=>r.date == true)
    }
    else if(selectedField.value?.fieldtype=='Select'){
        return operators.filter(r=>r.select == true)
    }
    else if(selectedField.value?.fieldtype=='Check'){
        return operators.filter(r=>r.check == true)
    }
    else if(selectedField.value?.fieldtype == 'Date'){
        return operators.filter(r=>r.date == true)
    }
    else{
        return operators.filter(r=>r.number == true)
    }
})

let getSelectOptions = ref()
watch(item.value,(value)=>{
    getSelectOptions.value = [];
    if(selectedField.value?.fieldtype == 'Check'){
        getSelectOptions.value = checkOptions
    }
    else if(selectedField.value?.fieldtype == 'Select'){
        getSelectOptions.value = selectedField.value?.options.split('\n')
    }
    else if(item.value.operator == 'is'){
        
        getSelectOptions.value = isOptions
    }
    else if(item.value.operator == 'Timespan'){
        getSelectOptions.value = timespanOptions
    }
    
})
const multipleSelect = computed(()=>{
    item.value.value = '';
    const isTure = selectedField.value?.fieldtype == 'Select' && (item.value.operator == 'in' || item.value.operator == 'not in')
    if(isTure){
        item.value.value = selectedField.value?.options.split('\n')[0]
    }
    return isTure;
})

function OnFilter(value, query, item) { 
    return String(item.title).toLocaleLowerCase().includes(query.toLocaleLowerCase())

}
function onSelected($event){
    if($event.fieldtype=="Link"){
        loadingAutoComplete.value = true;
        setTimeout(function(){
            loadingAutoComplete.value = false;
        },100)
    }
    
    item.value.field = $event

    editFilter($event)
}

function editFilter($event){
    item.value.operator = '=';
    if($event.fieldtype == "Date"){
        item.value.value = new Date().toISOString().split('T')[0]
    }else{
        item.value.value = '';
    }
    selectedField.value = $event
}

 
</script>