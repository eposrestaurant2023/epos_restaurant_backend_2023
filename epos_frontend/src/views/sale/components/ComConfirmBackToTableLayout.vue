<template>
    <v-dialog v-model="open" @update:modelValue="onClose" >
        <v-card 
          class="mx-auto my-2 py-2"
          title="Back to table layout"
          subtitle="Your have unsubmited sale order."
          >
          <v-card-text>
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
          </v-card-text>
          <v-card-actions class="text-right">
              <v-spacer></v-spacer>
              <v-btn variant="flat" @click="onClose" color="error">
                Cancel
              </v-btn>
              <!-- <v-btn variant="flat" @click="onOk('hold')" color="primary">
                  Hold Order
              </v-btn>
              <v-btn variant="flat" @click="onOk('submit')" color="success">
                  Submit Order
              </v-btn>
              <v-btn variant="flat" @click="onOk('continue')" color="warning">
                  Continue
              </v-btn> -->
          </v-card-actions>
        </v-card>
      </v-dialog>
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
  const open = ref(true)
  
  function onClose(){
    emit('resolve', false);
  }
  function onOk(action){
    emit('resolve', action);
  }

  
  
  
  </script>
  