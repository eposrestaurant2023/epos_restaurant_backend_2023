<template>
    <ComModal :fullscreen="true" :persistent="true" @onClose="onClose" :hide-close-button="true" :hide-ok-button="true"
        :fill="true" contentClass="h-full">
        <template #title>
            {{ $t('Employee') }}
        </template>
        <template #content>
            <div class="search-box my-0 mx-auto" :class="small ? 'w-full' : 'max-w-[350px]'">
            <ComInput
                autofocus
                keyboard
                variant="outlined"
                :placeholder="$t('Search...')"
                v-model="keyword"
                prepend-inner-icon="mdi-magnify"
                v-debounce="getEmployessList"
                :ref="control"
                />
            </div>

            <div class="grid gap-2" :class="mobile ? 'grid-cols-2' : 'sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 2xl:grid-cols-6'">          
                <div v-for="(e, index) in getEmployessList(keyword)" :key="index" class="h-10">
                    <div class="h-full rounded-lg shadow-lg cursor-pointer " :class="e.is_selected?'bg-red':'bg-gray-500'">
                        <div v-ripple class="relative p-2 w-full h-full flex justify-center items-center" @click="onEmployeeSelected(e)">
                            <div class="text-white">{{`${e.name} - ${e.employee_name}` }}</div>
                        </div>
                    </div>
                </div> 
            </div>

            <hr/>
            <div>Duration</div>
            <div class="grid gap-2" :class="mobile ? 'grid-cols-2' : 'sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 2xl:grid-cols-6'">          
                <div v-for="(e, index) in get_duration_commission_list" :key="index" class="h-10">
                    <div class="h-full rounded-lg shadow-lg cursor-pointer " :class="e.is_selected?'bg-red':'bg-gray-500'">
                        <div v-ripple class="relative p-2 w-full h-full flex justify-center items-center" @click="onDurationSelected(e)">
                            <div class="text-white">{{`${e.duration_title}` }}</div>
                        </div>
                    </div>
                </div> 
            </div>
            <hr/>
            <div>Commission</div>
            <div class="grid gap-2" :class="mobile ? 'grid-cols-2' : 'sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 2xl:grid-cols-6'">          
                <div v-for="(e, index) in get_commission_list" :key="index" class="h-10">
                    <div class="h-full rounded-lg shadow-lg cursor-pointer " :class="e.is_selected?'bg-red':'bg-gray-500'">
                        <div v-ripple class="relative p-2 w-full h-full flex justify-center items-center" @click="onCommissionSelected(e)">
                            <div class="text-white">{{`${e.commission_value}` }}</div>
                        </div>
                    </div>
                </div> 
            </div>


            <hr/>

           <div>Selected Employees</div>
            <ul  v-for="(e, index) in employees_selected" :key="index">
                <li>{{e.employee_name}}</li>
            </ul>


        </template>
        <template #action>
            
            <v-btn @click="onChooseEmployee">
                {{$t('Choose Employee')}}
              </v-btn>
              <v-btn >
                {{$t('Remove Employee')}}
              </v-btn>
              <v-btn @click="onConfirm">
                {{$t('Confirm')}}
              </v-btn>
        </template>
    </ComModal>
</template>
<script setup>

import { inject, ref,i18n,onMounted } from '@/plugin';
import { useDisplay } from 'vuetify'
import { createToaster } from '@meforma/vue-toaster';
import ComInput from '../../../components/form/ComInput.vue';
import { computed } from 'vue';

const frappe = inject("$frappe")
const db = frappe.db();

const { t: $t } = i18n.global;  
const { mobile } = useDisplay();
const sale = inject("$sale");
const gv = inject("$gv");
const emit = defineEmits(['resolve'])
const toaster = createToaster({ position: "top" })
const props = defineProps({
    params: Object
})


const keyword = ref("");
const is_load = ref(true);
const employee_list = ref([]);
const commission_list = ref([]);
const duration_commission_list = ref([]);
let control=ref(null);

const employees_selected = ref([])

onMounted(async ()=>{
    employee_list.value = await  db.getDocList('Employee',{
                        fields: ['name', 'employee_name'],
                        filters: [['disabled', '=', 0]],
                        limit: 250
                        }) 
    commission_list.value = await db.getDocList('Predefine SPA Commission Code',{ 
        fields: ['name', 'commission_value'],
        limit: 100
    })
    duration_commission_list.value = await db.getDocList('Predefine SPA Duration Code',{ 
        fields: ['name', 'duration_title','duration_value','commission_value'],
        limit: 100
    })   


    
    employees_selected.value = JSON.parse(props.params?.data.employees||"");
    is_load.value = false;

})

const get_duration_commission_list = computed(()=>{
   return duration_commission_list.value.sort((a, b) =>a.duration_value > b.duration_value?1:-1)
})

const get_commission_list = computed(()=>{
   return commission_list.value.sort((a, b) =>a.commission_value > b.commission_value?1:-1)
})


function onEmployeeSelected(e){
    employee_list.value.forEach((r)=>{
        r.is_selected = false;
    }) 
    e.is_selected = true;
}

function onDurationSelected(e){
    duration_commission_list.value.forEach((r)=>{
        r.is_selected = false;
    }) 
    e.is_selected = true;
}

function onCommissionSelected(e){
    commission_list.value.forEach((r)=>{
        r.is_selected = false;
    }) 
    e.is_selected = true;
}

function getEmployessList(text){
    return employee_list.value.filter((r)=>(r.name + ''+r.employee_name).toLowerCase().includes(text.toLowerCase()))
}


function onChooseEmployee(){
    const emp = employee_list.value.filter((r)=>r.is_selected||false);
    if(emp.length<=0){
        return;
    }
    const dur = duration_commission_list.value.filter((r)=>r.is_selected||false);
    if(dur.length<=0){
        return;
    }
    const com = commission_list.value.filter((r)=>r.is_selected||false);
    if(com.length<=0){
        return;
    }
 
    employees_selected.value.push({
        "employee_id":emp[0].name,
        "employee_name":emp[0].employee_name,
        "duration_title":dur[0].duration_title,
        "duration_value":dur[0].duration_value,
        "commission_value":com[0].commission_value
    });

    //clear selected value
    employee_list.value.forEach((r)=>{
        r.is_selected = false;
    }) 
    duration_commission_list.value.forEach((r)=>{
        r.is_selected = false;
    }) 
    commission_list.value.forEach((r)=>{
        r.is_selected = false;
    }) 

    //end clear selected value
}

function onConfirm(){
    if(employees_selected.length<=0){
        return;
    }
    emit("resolve", employees_selected.value);
}

function onClose(){
    emit("resolve", false);
}


 
</script>