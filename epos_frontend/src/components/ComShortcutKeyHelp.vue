<template>
    <ComModal width="960px" :persistent="true" @onClose="onOK()" :hide-ok-button="true" :hide-close-button="true">
        <template #title>
            <div>{{ $t('Shortcut Key Help') }}</div>
        </template>
        <template #content>
            <div>
                <v-list lines="two" density="compact">
                    <v-list-item v-for="item in gv.setting.shortcut_key" :key="item.name" :title="(item.name + ` (`+ item.key +`)`)" :subtitle="item.description">
                    </v-list-item>
                    
                </v-list>
            </div>
        </template>
        <template #action>
            <template>
                <v-btn variant="flat" @click="onOK()" color="primary">
                    {{ $t('Ok') }}
                </v-btn>
            </template>
        </template>
    </ComModal>
</template>

<script setup>
import { inject } from 'vue'
const gv = inject("$gv");
const emit = defineEmits(['resolve'])
function onOK() {
    localStorage.removeItem('dialogstate')
    emit('resolve', false)
}

</script>