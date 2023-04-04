<template>
    <div class="pa-4">
        <ComPlaceholder :loading="loading === true || workingDayReports === null"
            :is-not-empty="workingDayReports?.length > 0">
            <template v-for="(c, index) in workingDayReports" :key="index">
                <v-card :color="activeReport.report_id == c.name ? 'info' : 'default'"
                    :variant="activeReport.report_id == c.name || c.cashier_shifts.find(r => r.name == activeReport.report_id) ? 'tonal' : 'text'"
                    class="bg-gray-200 my-2 subtitle-opacity-1" @click="onWorkingDay(c)">
                    <template v-slot:title>
                        <div class="flex justify-between">
                            <div>{{ c.name }}</div>
                            <div>
                                <v-chip v-if="c.is_closed" color="error" size="small" variant="elevated">Closed</v-chip>
                                <v-chip v-else color="success" size="small" variant="elevated">Opening</v-chip>
                            </div>
                        </div>
                    </template>
                    <template v-slot:subtitle>
                        <div class="whitespace-normal">
                            <div><v-icon icon="mdi-calendar" size="x-small" /> <span class="font-bold">{{
                                c.posting_date
                            }}</span> opened by <span class="font-bold">{{ c.owner }}</span>
                            </div>
                            <div v-if="c.is_closed">
                                <v-icon icon="mdi-calendar-multiple" size="x-small" /> <span class="font-bold">{{
                                    c.closed_date }}</span> closed by <span class="font-bold">{{ c.modified_by }}</span>
                            </div>
                            <div><v-icon icon="mdi-note-text" size="x-small"></v-icon> Total Shift: <span
                                    class="font-bold">{{ c.cashier_shifts.length }}</span></div>
                        </div>
                    </template>
                </v-card>
                <div class="overflow-y-auto overflow-x-hidden max-h-44"
                    v-if="activeReport.report_id == c.name || c.cashier_shifts.find(r => r.name == activeReport.report_id)">
                    <div class="flex flex-wrap">
                        <v-sheet elevation="1" :color="item.name == activeReport.report_id ? 'info' : 'default'"
                            class="m-1 p-2 text-center cursor-pointer" width="118px" rounded="sm"
                            v-for="(item, index) in c.cashier_shifts" :key="index" @click="onCashierShift(item)">

                            <div>{{ moment(item.creation).format('h:mm:ss A') }}</div>
                            <div class="text-xs">
                                <span>#{{ item.name }}</span>
                            </div>
                            <div>
                                <v-chip v-if="item.is_closed" size="x-small" color="error">Closed</v-chip>
                                <v-chip v-else size="x-small" color="success">Opening</v-chip>
                            </div>
                        </v-sheet>
                    </div>
                </div>
                <div class="pt-2">
                    <hr />
                </div>
            </template>
        </ComPlaceholder>
    </div>
</template>
<script setup>
import {defineProps, defineEmits} from 'vue'
import ComPlaceholder from '../../../components/layout/components/ComPlaceholder.vue';
const emit = defineEmits([''])
const props = defineProps({
    loading: false,
    activeReport: Object,
    workingDayReports: []
})

function onWorkingDay(p){
    emit('onWorkingDay', p)
}
</script>