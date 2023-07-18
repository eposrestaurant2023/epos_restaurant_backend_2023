<template>
    <ComModal :fullscreen="false" :persistent="true" @onClose="onClose" :titleOKButton="$t('Ok')" @onOk="onConfirm"
        :fill="false" contentClass="h-full">
        <template #title>
            {{ $t('Payment') }} {{ get_room_selected==""?"":(": #" + get_room_selected) }}
        </template>
        <template #content>
            <template v-if="params.data.use_room_offline" >  
                <v-row no-gutters>
                    <v-col cols="12" class="pa-1" sm="3"  v-for="(r, index) in rooms" :key="index" @click="(()=>onOfflineRoomPressed(r))">
                        <div :class="r.selected ? 'bg-indigo-lighten-2' : ''" class="cursor-pointer border border-stone-500 pa-3 rounded-sm">
                            <div>
                                <span>{{$t("Room")}}: {{ r.room_name }}</span>
                            </div>
                        </div>                  
                    </v-col>
                </v-row>

            </template>
            <template v-else>
                <div class="grid mb-2 -m-1 border rounded-sm p-1 grid-cols-4">
                    <div  class="flex items-center justify-center cursor-pointer border border-stone-500 rounded-sm text-center hover:bg-slate-300 p-3"
                        :class="(t.selected?'bg-pink-lighten-4':'')"
                    
                        style="margin: 1px;"
                        :key="index"
                        v-for="(t, index) in room_types" @click="onRoomTypeSelected(t)">
                        {{ t.type }}
                    </div>
                </div> 

                <hr/>
                <template v-if="get_folio_data.length>0">
                    <v-row no-gutters>
                        <v-col cols="12" class="pa-1" sm="6" v-for="(r, index) in get_folio_data" :key="index" @click="(()=>onOnlineFolioPressed(r))">
                            <div :class="r.selected ? 'bg-indigo-lighten-2' : ''" class="btn-post-to-room cursor-pointer border border-stone-500 pa-1 rounded-sm">
                                <div>
                                    <span>{{ $t('Folio') }}: #{{ r.name }}</span>
                                </div> 
                                <div>
                                    <span>{{$t("Guest")}}: {{ r.guest_name }}</span>
                                </div>  
                                <div>
                                    <span>{{ $t("Room Type") }}: #{{ r.room_types }}</span>
                                </div>
                               
                                <div>
                                    <span>{{$t("Room")}}: {{ r.rooms }}</span>
                                </div>  
                            </div>                  
                        </v-col>
                    </v-row>
                </template>
                <template v-else>
                    <div class="flex items-center justify-center">{{$t("Empty Data")}}</div>
                </template>               
            </template>
        </template> 
    </ComModal>
</template>
<script setup>

import { ref,onMounted,i18n,inject,computed } from '@/plugin';
import { createToaster } from '@meforma/vue-toaster';

const gv = inject("$gv")
const frappe = inject("$frappe")
const { t: $t } = i18n.global;  

const emit = defineEmits(['resolve'])
const toaster = createToaster({ position: "top" })
const props = defineProps({
    params: Object
})

const call = frappe.call();

const rooms = ref([]);
const folio_data = ref([]);
const reservation_folio = ref({});
const room_types = ref([]);

onMounted(()=>{
    if(props.params.data.use_room_offline){
        const _room_numbers = props.params.data.rooms.split(',');
        _room_numbers.forEach(room => {
            rooms.value.push({
                "uid":uuidv4(),
                "room_name":room,
                "selected":false
            })            
        });
    } else{
        room_types.value.push({
            "name":"all",
            "type":$t("All"),
            "sort_order":-9999,
            "selected":true
        })
        onGetReservationFolio();
    }
}) 

function onRoomTypeSelected(type) {
    room_types.value.forEach((r)=>{    
        r.selected =false;   
        if(r.name==type.name){
            r.selected = true;
        }
    })
}

function onGetReservationFolio(){
    call.get('epos_restaurant_2023.api.api.get_reservation_folio', {
        "property":gv.setting?.business_branch
    }).then((result) => {
        reservation_folio.value = result.message;
        if(reservation_folio.value){
            reservation_folio.value.room_types.forEach((t)=>{
                room_types.value.push({
                    "name":t.name,
                    "type": t.room_type,
                    "sort_order":t.sort_order,
                    "selected":false
                })
            })

            //get rooms
            reservation_folio.value.folio_data.forEach((f)=>{
                folio_data.value.push(f)
            })

            room_types.value.sort((a, b) =>a.sort_order > b.sort_order?1:-1)
        }
    });
} 

const get_folio_data = computed(()=>{
    const check =  room_types.value.filter((r)=>r.selected);
    if(check.length >0){
        if(check[0].name=="all"){
            return folio_data.value;
        }else{
            const data = folio_data.value.filter((r)=>r.room_types.includes(check[0].type));
            return data;
        }
    }
    return folio_data.value; 
})

const get_room_selected = computed(()=>{
    let room = []
    if(props.params.data.use_room_offline){
        room = rooms.value.filter((r)=>r.selected)
        if(room.length>0){
            return room[0].room_name;
        }
    }else{
        room = folio_data.value.filter((r)=>r.selected)
        if(room.length>0){
            return room[0].rooms;
        }
    }  
    
    return "";
})





function onOfflineRoomPressed(room){   
    rooms.value.forEach((r)=>{       
        if(r.uid==room.uid){
            r.selected = !r.selected;
        }
        else{
            r.selected =false;
        }
    })
}

function onOnlineFolioPressed(folio){  
    
    folio_data.value.forEach((f)=>{
        if(f.name==folio.name){
            f.selected = !f.selected;
        }else{
            f.selected = false
        }
    })   
}

 
function onConfirm(){ 
    let room = [] 
    if(props.params.data.use_room_offline){
        room = rooms.value.filter((r)=>r.selected)
    }else{
        room =folio_data.value.filter((r)=>r.selected||false)
    }
  
    if(room.length <=0){
        toaster.warning($t("msg.Please select a room to continue"))
        return
    } 
    const r = room[0];
    if(props.params.data.use_room_offline){
            emit("resolve", {
            "room":r.room_name,
            "folio":null,
        });
    }
    else{
         emit("resolve", {
            "room":r.rooms,
            "folio":r.name
        });
    }
    
}

function onClose() {    
    emit("resolve", false);
}


function uuidv4() {
  return ([1e7]+-1e3+-4e3+-8e3+-1e11).replace(/[018]/g, c =>
    (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
  );
}


</script>
<style>
.btn-post-to-room{
    width: 100%;
    text-align: start; 
    display: block !important;
}
.btn-post-to-room .v-btn__content{
    white-space: normal !important;
    display: block !important;
    line-height: 1.5;
}
</style>