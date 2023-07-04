<template>
    <ComModal :fullscreen="false" :persistent="true" @onClose="onClose" :titleOKButton="$t('Ok')" @onOk="onConfirm"
        :fill="true" contentClass="h-full">
        <template #title>
            {{ $t('Payment') }}
        </template>
        <template #content>
            <template v-if="params.data.use_room_offline" > 
                <div  v-for="(r, index) in rooms" :key="index" @click="(()=>onOfflineRoomPressed(r))">
                    {{ r.room_name }} {{ r.selected?'Selected':'' }}
                </div>
            </template>
            <template v-else>
                <v-select :label="$t('Room Type')" 
                item-title="type"
                :item-value="room_type"
                v-model="room_type" 
                return-object
                variant="solo"               
                density="compact"
                :items="room_types"></v-select> 
                <hr/>
       
                <div v-for="(r, index) in get_folio_data" :key="index">
                    <div>
                       <span>Folio: #{{ r.name }}</span>
                    </div>
                    <div>
                        <span>Room Type: #{{ r.room_type }}</span>
                     </div>
                     <div>
                        <span>Room: {{ r.rooms }}</span>
                     </div>
                     <hr/>                     
                </div>
               
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
const room_type = ref({ "name":"all","type":$t("All"), "sort_order":-9999})
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
            "sort_order":-9999
        })
        onGetReservationFolio();
    }
}) 

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
                    "sort_order":t.sort_order
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
    if(room_type.value.name=="all"){
        return folio_data.value;
    }
    const data = folio_data.value.filter((r)=>r.room_types.includes(room_type.value.type));
    return data
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

 
function onConfirm(){
 
    const room = rooms.value.filter((r)=>r.selected)
    if(room.length <=0){
        toaster.warning($t("msg.Please select a room to continue"))
        return
    } 
    const r = room[0];
    emit("resolve", {
        "room":r.room_name,
        "folio":null,
        "use_room_offline":true,
    });
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