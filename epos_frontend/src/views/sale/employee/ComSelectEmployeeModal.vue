<template>
    <ComModal :fullscreen="true" :persistent="true" @onClose="onClose" :hide-close-button="true" :hide-ok-button="true"
        :fill="true" contentClass="h-full">
        <template #title>
            {{ $t('Employee') }}
        </template>
        <template #content>
            <div>Comming Soon</div>
        </template>
        <template #action>
            
        </template>
    </ComModal>
</template>
<script setup>

import { inject, ref,i18n } from '@/plugin';
import { useDisplay } from 'vuetify'
import { createToaster } from '@meforma/vue-toaster';

const { t: $t } = i18n.global;  
const { mobile } = useDisplay();
const sale = inject("$sale");
const gv = inject("$gv");
const emit = defineEmits(['resolve'])
const toaster = createToaster({ position: "top" })
const props = defineProps({
    params: Object
})


function onClose(){
    emit("resolve", false);
}
 
</script>