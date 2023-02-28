<template>
    <ComModal @onClose="onClose" width="350px" :hide-ok-button="true">
      <template #title>
        Back to table layout
      </template>
      <template #content>
        <p class="text-gray-400 text-sm px-2">Your have unsubmited sale order.</p>
        <div class="-mx-2">
            <v-list>
              <!-- <v-list-item prepend-icon="mdi-cash-100" title="Hold Order" @click="onOk('hold')"/>
              <v-list-item prepend-icon="mdi-cash-100" title="Submit Order" @click="onOk('submit')"/>
              <v-list-item prepend-icon="mdi-cash-100" title="Continue" @click="onOk('continue')"/> -->
              <v-list-item>
                <v-btn class="w-full" variant="flat" @click="onOk('hold')" color="primary">
                    Hold Order
                </v-btn>
              </v-list-item>
              <v-list-item>
                <v-btn class="w-full" variant="flat" @click="onOk('submit')" color="success">
                  Submit Order
                </v-btn>
              </v-list-item>
              <v-list-item>
                <v-btn class="w-full" variant="flat" @click="onOk('continue')" color="warning">
                  Continue
              </v-btn>
              </v-list-item>
            </v-list>
          </div>
      </template>
    </ComModal>
  </template>
  
  <script setup>
  import {ref} from  "@/plugin"
  import { useDisplay } from 'vuetify'
  const { mobile } = useDisplay()  
  const props = defineProps({
    params:{
      type:Object,
      require:true
    }
  })
  const emit = defineEmits(["resolve"])
  let open = ref(true)
  
  function onClose(){
    emit('resolve', false);
  }
  function onOk(action){
    emit('resolve', action);
  }

  
  
  
  </script>
  