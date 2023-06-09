<template>
    <ComModal @onClose="onClose" width="350px" :hide-ok-button="true">
      <template #title>
       
      </template>
      <template #content>
        <p class="text-gray-400 text-sm px-2">{{ $t('msg.You have some items that unsubmited order') }}</p>
        <div class="-mx-2">
            <v-list>
              <v-list-item>
                <v-btn class="w-full" variant="flat" @click="onOk('hold')" color="primary">
                   {{ $t('Hold Order') }}
                </v-btn>
              </v-list-item>
              <v-list-item>
                <v-btn class="w-full" variant="flat" @click="onOk('submit')" color="success">
                 {{ $t('Submit Order') }}
                </v-btn>
              </v-list-item>
              <v-list-item>
                <v-btn class="w-full" variant="flat" @click="onOk('continue')" color="warning">
                  {{ $t('Continue') }}
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
  