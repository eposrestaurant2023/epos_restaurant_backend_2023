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
            <v-btn @click="onDurationTypePressed()" :class="!is_overtime?'bg-red':'bg-gray-500'">
                {{$t('General')}}
              </v-btn>
              <v-btn @click="onDurationTypePressed(true)" :class="is_overtime?'bg-red':'bg-gray-500'">
                {{$t('Overtime')}}
              </v-btn>

            <div class="grid gap-2" :class="mobile ? 'grid-cols-2' : 'sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 2xl:grid-cols-6'">          
                <div v-for="(e, index) in get_duration_commission_list" :key="index" class="h-10">
                    <div class="h-full rounded-lg shadow-lg cursor-pointer " :class="e.is_selected?'bg-red':'bg-gray-500'">
                        <div v-ripple class="relative p-2 w-full h-full flex justify-center items-center" @click="onDurationSelected(e)">
                            <div class="text-white">{{`${e.duration_title}${e.is_overtime?'-OT':''}` }}</div>
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
                <li>
                    {{e.employee_name }}({{e.duration_title}})
                    <v-btn @click="onRemoveSelectedEmployee(e)">
                        {{$t('Delete')}}
                      </v-btn>
                </li>
            </ul>


        </template>
        <template #action>
            
            <v-btn @click="onChooseEmployee">
                {{$t('Choose Employee')}}
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

const is_overtime = ref(false);

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
        fields: ['name', 'duration_title','duration_value','commission_value','is_overtime'],
        limit: 100
    })   

    duration_commission_list.value.forEach((d)=>{
        if((props.params?.data.portion==d.duration_title && !d.is_overtime)){
            d.is_selected = true;
            const com = commission_list.value.filter((r)=>r.commission_value==d.commission_value);
            if(com.length > 0){
                com[0].is_selected = true;
            }
        }
    })
    

    //
    
    employees_selected.value = JSON.parse(props.params?.data.employees||"[]");
    is_load.value = false;

})

const get_duration_commission_list = computed(()=>{
   return duration_commission_list.value.filter((x)=>x.is_overtime==is_overtime.value).sort((a, b) =>a.duration_value > b.duration_value?1:-1)
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
        return ;
    }
    const dur = duration_commission_list.value.filter((r)=>r.is_selected||false);
    if(dur.length<=0){
        return ;
    }
    const com = commission_list.value.filter((r)=>r.is_selected||false);
    if(com.length<=0){
        return ;
    }

    const check = employees_selected.value.filter((r)=>r.employee_id==emp[0].name);
    
    if(check.length <=0){ 
        employees_selected.value.push({
            "employee_id":emp[0].name,
            "employee_name":emp[0].employee_name,
            "duration_title":dur[0].duration_title,
            "duration":dur[0].duration_value,
            "commission_amount":com[0].commission_value, 
            "is_overtime":dur[0].is_overtime,      
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

    }
    else{
        //clear selected value
        employee_list.value.forEach((r)=>{
                r.is_selected = false;
        }) 
    }

    
    //end clear selected value
}


function onRemoveSelectedEmployee(emp){
    employees_selected.value.splice(emp,1);
}

function onDurationTypePressed(overtime=false){
    is_overtime.value = overtime;
}

function onConfirm(){
    onChooseEmployee()
    emit("resolve", employees_selected.value);
    
}

function onClose(){
    emit("resolve", false);
}


 
</script>