<template lang="">
    <PageLayout class="pb-4" :title="active" icon="mdi-chart-bar" full>
        <template #action>
            <v-btn icon="mdi-printer"></v-btn>
        </template>
        <ComWorkingDayReport/>
    </PageLayout>
</template>
<script setup>
import {ref} from 'vue'
import PageLayout from '@/components/layout/PageLayout.vue';
import ComWorkingDayReport from './components/ComWorkingDayReport.vue';
 
</script>
 