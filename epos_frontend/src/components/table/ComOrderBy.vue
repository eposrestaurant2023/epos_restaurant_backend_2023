<template>
  <div>
    <v-menu>
      <template v-slot:activator="{ props }">
        <v-btn v-bind="props" variant="tonal" class="mr-1 mb-1 mt-1" :size="mdAndDown ? 'small' : 'default'">
          {{ $t(selected.label) }}
        </v-btn>
      </template>
      <v-list>
        <v-list-item v-for="(item, index) in defaultOption" :key="index" :value="index" @click="onClick(item)">
          <v-list-item-title>{{ $t(item.label) }}</v-list-item-title>
        </v-list-item>
        <v-list-item
          v-for="(item, index) in fields.filter(r => r.bold == 1 || r.in_list_view == 1 || r.in_standard_filter == 1)"
          :key="index" :value="index" @click="onClick(item)">
          <v-list-item-title>{{  $t(item.label) }}</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>
    <v-btn @click="onOrder()" variant="tonal" :size="mdAndDown ? 'small' : 'default'">
      <v-icon :icon="orderType == 'desc' ? 'mdi-sort-descending' : 'mdi-sort-ascending'"></v-icon>
    </v-btn>
  </div>
</template>
<script setup>
import { ref, defineProps, reactive, defineEmits, computed } from '@/plugin'
import {useDisplay} from 'vuetify'
const {mdAndDown} = useDisplay()
const props = defineProps({
  fields: Array,
  defaultOrderby: String
})


const emit = defineEmits(['onOrderby'])
const orderType = ref("desc")
let selected = reactive({
  fieldname: 'modified',
  label: 'Last Updated On',
})
const defaultOption = reactive([
  {
    fieldname: 'name',
    label: 'ID',

  },
  {
    fieldname: 'modified',
    label: 'Last Updated On',

  },
  {
    fieldname: 'creation',
    label: 'Created On',

  }

])



const arrOrderby = props.defaultOrderby.split(" ");
orderType.value = arrOrderby[1];
let defaultSortLabel = arrOrderby[0];
let objDefaultSortLabel = defaultOption.find(r => r.fieldname == arrOrderby[0])
if (objDefaultSortLabel) {
  defaultSortLabel = objDefaultSortLabel.label;
} else {
  objDefaultSortLabel = props.fields.find(r => r.fieldname == arrOrderby[0])
  if (objDefaultSortLabel) {
    defaultSortLabel = objDefaultSortLabel.label;
  }
}

selected = {
  fieldname: arrOrderby[0],
  label: defaultSortLabel
}



function onOrder() {
  orderType.value = orderType.value == 'desc' ? 'asc' : 'desc'
  onOrderby()
}

function onClick(item) {
  selected = item
  onOrderby()
}

function onOrderby() {
  emit('onOrderby', selected.fieldname + ' ' + orderType.value)
}
</script>
