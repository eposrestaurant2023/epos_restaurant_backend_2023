<template>
  <div id="app">
    <div class="parent">
      <Vue3DraggableResizable
        v-for="(t, index) in data.tables" :key="index"
        class="table"
        :initW="110"
        :initH="120"
        v-model:x="t.x"
        v-model:y="t.y"
        v-model:w="t.w"
        v-model:h="t.h"
        v-model:active="active"
        :draggable="true"
        :resizable="true"
        @dragging="dragHandle()"
        
      >
      <div class="h-full" v-bind:style="{ 'background-color': t.bg_color}" style="color:red;">
        
      </div>
      </Vue3DraggableResizable>
    </div>
  </div>
</template>
<script setup>
import Vue3DraggableResizable from 'vue3-draggable-resizable'
import { reactive } from '@/plugin'
const data = reactive({
  tables: [
        {
          x: 100,
          y: 100,
          h: 100,
          w: 100,
          resizable: true,
          draggable: true,
          bg_color: '#000'
        },
        {
          x: 100,
          y: 100,
          h: 100,
          w: 100,
          bg_color: 'blue'
        }
      ]
})
function dragHandle(params) {

}

</script>
<style>
.parent {
  width: 200px;
  height: 200px;
  position: absolute;
  top: 100px;
  left: 100px;
  border: 1px solid #000;
  user-select: none;
}
</style>