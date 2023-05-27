<template>
    <div class="mr-2 mt-1 mb-1">
        <v-menu v-model="menu" :close-on-content-click="false" location="end">
            <template v-slot:activator="{ props }">
                <v-btn color="primary" v-bind="props" :size="mdAndDown ? 'small' : 'default'">
                    <v-icon icon="mdi-filter-outline" :color="advancefilters.length > 0 ? 'black' : ''"></v-icon>
                </v-btn> 
            </template>
            <v-card style="width: 100%; max-width:600px; min-width:300px">
                <v-card-text>
                    <ComPlaceholder :text="$t('msg.Please add filter button to set filter')" :is-not-empty="advancefilters.length > 0" icon-size="40px" class-color="text-gray-300" icon="mdi-filter-outline">
                        <div v-if="!loadingFilter">
                            <div v-for="(f, index) in advancefilters" :key="index"> 
                                <ComAdvanceFilterKey
                                    :filter="f"
                                    :fields="resource.data.fields" 
                                    @onRemove="removeFilter" />
                            </div>
                        </div>
                        <div class="p-2 text-sm text-gray-400" v-if="advancefilters.filter(r=>r.operator=='in' || r.operator=='not in').length > 0">if use operator <b>In</b> or <b>Not In</b> this values must separated by commas <b>;</b></div>
                    </ComPlaceholder>
                </v-card-text>
                <v-card-actions> 
                    <v-btn @click="addFilter" color="primary" variant="text">{{$t('Add Filter')  }}</v-btn> 
                    <v-spacer></v-spacer>
                    <v-btn @click="clearFilter" color="error">{{ $t('Clear All') }}</v-btn>
                    <v-btn color="primary" variant="tonal" @click="onFilter(true)" prepend-icon="mdi-filter-outline">
                        {{ $t('Apply Filters') }}
                    </v-btn>
                </v-card-actions>
            </v-card>
        </v-menu>
    </div>
</template>
<script setup>
import ComAdvanceFilterKey from '../form/ComAdvanceFilterKey.vue';
import {  ref,defineProps, defineEmits, inject  } from '@/plugin'
import {useDisplay} from 'vuetify';
const {mdAndDown} = useDisplay()
const props = defineProps({
    resource: {
        type: Object
    }
})
const gv = inject('$gv')
const emit = defineEmits(['onApplyFilter'])
let loadingFilter = ref(false)
const menu = ref(false) 
const advancefilters = ref([])

 

function addFilter() {
    advancefilters.value.push({ fieldname: null, operator: "", value: null,field:{} })
}

function removeFilter(item) {
    loadingFilter.value = true
    advancefilters.value.splice(advancefilters.value.indexOf(item), 1);
    setTimeout(function(){
        onFilter(false)
        loadingFilter.value = false
    },100)
    
}
function clearFilter() {
    advancefilters.value= []
    onFilter()
}
function onFilter(isFilter){ 
    emit('onApplyFilter',advancefilters.value)
    if(isFilter)
        menu.value = false
}

</script>