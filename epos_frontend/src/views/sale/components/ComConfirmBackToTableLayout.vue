<template>
    <v-dialog v-model="open" @update:modelValue="onClose">
        <v-card 
          class="mx-auto my-2 py-2"
          title="Back to table layout"
          subtitle="Your have unsubmited sale order."
          >
          <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn variant="flat" @click="onClose" color="error">
                Cancel
              </v-btn>
              <v-btn variant="flat" @click="onOk('hold')" color="primary">
                  Hold Order
              </v-btn>
              <v-btn variant="flat" @click="onOk('submit')" color="success">
                  Submit Order
              </v-btn>
              <v-btn variant="flat" @click="onOk('continue')" color="warning">
                  Continue
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
  function onOk(action){
    emit('resolve', action);
  }

  
  
  
  </script>
  