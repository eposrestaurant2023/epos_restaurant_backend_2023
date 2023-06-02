<template>
    <ComModal
        :mobileFullscreen="true"
        @onClose="onClose"
        @onOk="onConfirm"
        titleOKButton="OK"
        :hideOkButton="isReadonly"
        >
        <template #title>
            <span v-if="isReadonly">{{ $t('View Commission') }}</span>
            <span v-else>{{ $t('Add or Edit Commission') }}</span>
        </template>
        <template #content>
            <v-row class="!m-0">
                <v-col cols="12" md="6">
                    <ComInput v-model="data.agent_name" :required="true" keyboard :label="$t('Agent Name')" :readonly="isReadonly"/>   
                </v-col>
                <v-col cols="12" md="6">
                    <ComInput v-model="data.agent_phone_number" keyboard :label="$t('Phone Number')" :readonly="isReadonly"/>   
                </v-col>
                <v-col cols="12" md="6">
                    <v-select
                        :label="$t('Commission Type')"
                        v-model="data.commission_type" 
                        :items="['Percent','Amount']"
                        density="compact"
                        variant="solo"
                        @update:modelValue="onUpdatedData" 
                        hide-details
                        type="number"
                        :readonly="isReadonly"
                    ></v-select>
                </v-col>
                <v-col cols="12" md="6">
                    <ComInput type="number" v-model="data.commission" v-debounce="onUpdatedData" keyboard :label="$t('Input Commission')" :readonly="isReadonly"/>   
                </v-col>
                <v-col cols="12">
                    <ComInput readonly type="number" v-model="data.commission_amount" :label="$t('Amount')"/>   
                </v-col>
                <v-col cols="12">
                    <ComInput v-model="data.commission_note" keyboard :label="$t('Note')" type="textarea" :readonly="isReadonly"/>
                </v-col>
            </v-row>
        </template>
        <template #action>
            <v-btn variant="flat" @click="onRemove()" color="error" prepend-icon="mdi-delete" v-if="!isReadonly">
                {{ $t('Remove') }}
            </v-btn>
        </template>
    </ComModal>
</template>
  
<script setup>
import { ref,defineEmits,createToaster,confirmDialog,onMounted, computed, inject,i18n } from '@/plugin'
import ComInput from '@/components/form/ComInput.vue';

const { t: $t } = i18n.global;  
const toaster = createToaster({ position: 'top' })
const sale = inject('$sale')
const props = defineProps({
    params: {
        type: Object,
        require: true
    }
})
let data = ref(JSON.parse(JSON.stringify(props.params.data)))
const emit = defineEmits(["resolve","reject"])
onMounted(() => {
    onUpdatedData()
})
const isReadonly = computed(()=>{
    return sale.isBillRequested()
})
function onUpdatedData(){
    if (data.value.commission_type=="Percent"){
        data.value.commission_amount = (data.value.grand_total * data.value.commission/100); 
    }else {
        data.value.commission_amount = data.value.commission;
    }
}
async function onRemove(){
    if(await confirmDialog({ title: $t('Delete Commission'), text:$t('msg.are you sure to delete this commission')})){
        data.value.agent_name = ""
        data.value.agent_phone_number = ""
        data.value.commission = 0
        data.value.commission_type = "Percent"
        data.value.commission_amount = 0
        data.value.commission_note = ""
        emit("resolve",{ data: data.value })
    }
    
}
function onConfirm(){
    emit("resolve",{ data: data.value })
}

function onClose() {
    emit('resolve',false);
}

</script>