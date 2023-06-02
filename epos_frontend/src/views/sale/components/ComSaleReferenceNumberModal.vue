<template>
    <ComModal
        width="500px"
        @onClose="onClose"
        @onOk="onConfirm"
        titleOKButton="OK"
        >
        <template #title>
            <span>{{ $t('Reference') }}#</span>
        </template>
        <template #content>
            <div class="py-2">
                <ComInput v-model="data.reference_number" :required="true" keyboard :label="($t('Reference')+'#')"/>
            </div>
        </template>
    </ComModal>
</template>
  
<script setup>
import { ref,defineEmits,createToaster } from '@/plugin'
import ComInput from '@/components/form/ComInput.vue';
const toaster = createToaster({ position: 'top' })
const props = defineProps({
    params: {
        type: Object,
        require: true
    }
})
let data = ref(JSON.parse(JSON.stringify(props.params.data)))
const emit = defineEmits(["resolve","reject"])

function onConfirm(){ 
    emit("resolve",data.value.reference_number)
}

function onClose() {
    emit('resolve',false);
}

</script>