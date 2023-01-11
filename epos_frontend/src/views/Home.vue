<template>
  <div v-if="s.doc">
    <div>
      <h1>
        {{ s.name }}
        {{ s.doc.name }}
        {{ s.doc.message.customer }}
        <button @click="s.setValue.submit({ note: 'Hello note' })">
          Mark as Closed
        </button>
      </h1>
    </div>
  </div>
  <div class="flex flex-col items-center justify-center">
    <v-container class="mt-2">
      <div class="grid-container">
        <div>1</div>
      <div>2</div>
      </div>
    </v-container>
  </div>


</template>
<script setup>
import { createDocumentResource, } from 'frappe-ui'
let s = createDocumentResource({
  doctype: 'Sale',
  name: 'SO2023-0094',
  method: 'POST',
})
</script>


<style>
 .grid-container > div {
        background-color: #eee;
        text-align: center;
        padding: 30px 0;
        font-size: 30px;
      }
  .box {
        grid-row-start: auto;
      }
  .grid-container {
        display: grid;
        grid-template-columns: auto auto auto auto;
        grid-gap: 20px;
        background-color: #ccc;
        padding: 10px;
      }
</style>