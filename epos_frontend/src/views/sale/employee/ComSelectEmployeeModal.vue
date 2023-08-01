<template>
    <ComModal :fullscreen="true" :persistent="true" @onClose="onClose" :hide-close-button="true" :hide-ok-button="true"
        :fill="true" contentClass="h-full">
        <template #title>
            {{ $t('Employee') }}
        </template>
        <template #content>
            <div class="container mx-auto grid grid-cols-3 gap-4 mt-4" :class="mobile ? 'grid-cols-1' : 'sm:grid-cols-1 md:grid-cols-1 lg:grid-cols-2 xl:grid-cols-3 2xl:grid-cols-3'">
                <div class="box-panel-custom p-4">
                    <label>Employee</label>
                    <!-- <div class="search-box my-0 mx-auto" :class="small ? 'w-full' : 'max-w-[350px]'"> -->
                    <div class="search-box my-0 mx-auto w-full py-2">
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

                    <div class="grid grid-cols-2 gap-2" :class="mobile ? 'grid-cols-2' : 'sm:grid-cols-1 md:grid-cols-2 lg:grid-cols-2 xl:grid-cols-2 2xl:grid-cols-2'">          
                        <div v-for="(e, index) in getEmployessList(keyword)" :key="index">
                            <div class="h-full rounded-lg shadow-lg cursor-pointer border" :class="e.is_selected?'bg-red':'bg-white'">
                                <div v-ripple class="relative p-2 w-full h-full inline-flex" @click="onEmployeeSelected(e)">
                                    <span class="pr-3">
                                        <img class="rounded-full border" :src="avatarProfile"/>
                                    </span>
                                    <span class="flex items-center">
                                        <div>
                                            {{`${e.name} - ${e.employee_name}` }}<br/>
                                            <small>Therapies</small>
                                        </div>
                                        
                                    </span>
                                </div>
                            </div>
                        </div> 
                    </div>
                </div>

                <div class="box-panel-custom p-4">
                    <div class="box-panel-custom p-4 rounded">
                        <label>Duration</label>
                        <div class="w-full py-2">
                            <v-btn @click="onDurationTypePressed()" class="rounded-t-lg" :class="!is_overtime?'bg-red':'bg-gray-500'">
                                {{$t('General')}}
                            </v-btn>
                            <v-btn @click="onDurationTypePressed(true)" class="rounded-t-lg" :class="is_overtime?'bg-red':'bg-gray-500'">
                                {{$t('Overtime')}}
                            </v-btn>
                            <hr/>
                        </div>
                    
                        <div class="grid gap-2 mt-4" :class="mobile ? 'grid-cols-2' : 'sm:grid-cols-3 md:grid-cols-3 lg:grid-cols-3 xl:grid-cols-6 2xl:grid-cols-3'">          
                            <div v-for="(e, index) in get_duration_commission_list" :key="index" class="h-10">
                                <div class="h-full rounded-lg shadow-lg cursor-pointer " :class="e.is_selected?'bg-red':'bg-gray-500'">
                                    <div v-ripple class="relative p-2 w-full h-full flex justify-center items-center" @click="onDurationSelected(e)">
                                        <div class="text-white">{{`${e.duration_title}${e.is_overtime?'-OT':''}` }}</div>
                                    </div>
                                </div>
                            </div> 
                        </div>
                    </div>

                    <div class="box-panel-custom p-4 mt-4 rounded">
                        <div>Commission</div>
                        <div class="grid gap-2 py-2" :class="mobile ? 'grid-cols-2' : 'sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 2xl:grid-cols-6'">          
                            <div v-for="(e, index) in get_commission_list" :key="index" class="h-10">
                                <div class="h-full rounded-lg shadow-lg cursor-pointer " :class="e.is_selected?'bg-red':'bg-gray-500'">
                                    <div v-ripple class="relative p-2 w-full h-full flex justify-center items-center" @click="onCommissionSelected(e)">
                                        <div class="text-white">{{`${e.commission_value}` }}</div>
                                    </div>
                                </div>
                            </div> 
                        </div>
                    </div>
                </div>


                <div class="box-panel-custom p-4">
                    <div>Selected Employees</div>
                    <ul v-for="(e, index) in employees_selected" :key="index">
                        <li class="w-full py-2">
                            <div class="inline-flex box-panel-custom w-full p-4">
                                <div class="pr-3">
                                    <img class="rounded-full border" :src="avatarProfile" width="100"/>
                                </div>
                                <div class="w-full">
                                    <div class="flex justify-between">
                                        <div>
                                            <span>{{e.employee_name }}</span><br/>
                                            <small>Therapy</small><br/>
                                            <small><v-icon icon="md:home"></v-icon>{{e.duration_title}}</small>
                                        </div>
                                        <div>
                                            <strong>${{e.commission_amount}}</strong>
                                        </div>
                                    </div>
                                    <v-btn @click="onRemoveSelectedEmployee(e)" class="w-full bg-red dl">
                                        {{$t('Delete')}}
                                    </v-btn>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>


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

import avatarProfile from '@/assets/svg/user-icon-profile.svg';

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

<style>
.box-panel-custom{
    box-shadow: rgba(0, 0, 0, 0.02) 0px 1px 3px 0px, rgba(27, 31, 35, 0.15) 0px 0px 0px 1px;
}
.box-panel-custom .v-btn:not(.dl){
    border-bottom-left-radius: 0 !important;
    border-bottom-right-radius: 0 !important;
}
.box-panel-custom .v-btn--variant-elevated{
    box-shadow: unset !important;
    background-color: rgb(228 228 231);
}
</style>