<template>
    <ComModal width="800px" @onClose="onClose()" hideOkButton>
        <template #title>
            <div>Today Happy Hour Promotions</div>
        </template>
        <template #content>
            <div>
                <div class="grid gap-2" :class="mobile ? 'grid-cols-1' : 'sm:grid-cols-2 md:grid-cols-2 lg:grid-cols-2'">
                    <div 
                        v-for="(p, index) in gv.promotion" :key="index"
                        class="rounded-lg bg-gray-100 border-b border-gray-400"> 
                        <div class="block p-2 w-full h-full">
                            <div class="font-bold mb-2 pb-2 border-b">
                                <span class="pl-2">{{ p.promotion_name}}</span>
                            </div>
                            <div>
                                <div class="flex items-center mb-2">
                                    <v-icon size="x-small" icon="mdi-tag-multiple"></v-icon>
                                    <div class="text-sm ml-1">{{p.percentage_discount}}%</div>
                                </div>
                                <div class="flex items-center">
                                    <v-icon size="x-small" icon="mdi-clock"></v-icon>
                                    <div class="text-sm ml-1">{{p.start_time}} - {{p.end_time}}</div>
                                </div>
                                <div class="pt-1" v-if="p.customer_groups && p.customer_groups.length > 0">
                                    <div class="font-bold text-sm underline">On Customer:</div>
                                    <div class="-mx-1">
                                        <v-chip class="m-1" size="small" v-for="(c, index) in p.customer_groups" :key="index">{{c.customer_group_name_en}}</v-chip>
                                    </div>
                                </div>
                                <div>{{p.note}}</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </template>
    </ComModal>
</template>
<script setup>
import { defineEmits, inject } from '@/plugin' 
const emit = defineEmits(['resolve'])
const gv = inject('$gv')
const props = defineProps({
    params:Object
})
function onClose() {
    emit('resolve',false)
} 
</script>