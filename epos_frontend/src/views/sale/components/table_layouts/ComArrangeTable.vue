<template>
    <template v-for="g in tableLayout.table_groups">
                    <v-window-item :value="g.key"
                        v-bind:style="{ 'background-image': 'url(' + g.background + ')', 'min-height': 'calc(100vh - 200px)','background-size':'100% 100%' }"
                        class="bg-center overflow-auto">
                        <Vue3DraggableResizable v-for="(t, index) in g.tables" :key="index" v-model:x="t.x" v-model:y="t.y" :w="t.w" :h="t.h"
        :draggable="true" :resizable="true" @resize-end="tableLayout.onResizeEnd(t)($event)">

        <div  class="flex items-center justify-center h-full" v-bind:style="{ 'background-color': '#002ac1', 'color': '#fff', 'overflow': 'hidden' }">
            {{ t.tbl_no }}
        </div>
    </Vue3DraggableResizable>
                    </v-window-item>

                </template>



</template>
<script setup>
    import Vue3DraggableResizable from 'vue3-draggable-resizable'
    import { inject } from '@/plugin';

    const tableLayout = inject("$tableLayout");
     
</script>