<template>
  <v-dialog v-model="open">
      <v-card 
        class="mx-auto my-2 py-2"
        :title="params.title"
        :subtitle="params.text"
        >
        <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn variant="flat" @click="onClose" color="error">
              Cancel
            </v-btn>
            <v-btn variant="flat" @click="onOk" color="primary">
                OK
            </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
</template>

<script setup>
import {ref} from  "@/plugin"

const props = defineProps({
  params:{
    type:Object,
    require:true
  }
})
const emit = defineEmits(["resolve"])
const open = ref(true)

function onClose(){
  emit('resolve', false);
}
function onOk(){
  emit('resolve', true);
}



</script>
