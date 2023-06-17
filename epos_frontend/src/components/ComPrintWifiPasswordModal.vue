<template>
    <ComModal :mobileFullscreen="true" @onClose="onClose()" width="500px" :hideOkButton="false" titleOKButton="Print" @onOk="onPrint()">
      <template #title>
        {{$t('Print WiFi Password')}}
      </template>
      <template #content>
        <ComInput :placeholder="$t('Enter WiFi Password')" v-model="wifi_password" keyboard autofocus/>
      </template>
    </ComModal>
  </template>
<script setup>
import { defineEmits, i18n,ref} from "@/plugin";
import ComModal from "./ComModal.vue";
import ComInput from "./form/ComInput.vue";
import { createToaster } from "@meforma/vue-toaster";


const { t: $t } = i18n.global;

const toast = createToaster({position:"top"});

const wifi_password = ref("")
  const emit = defineEmits(["resolve"])
 
  function onClose() { 
    emit('resolve', false);
  }
  
  function onPrint(){
    if(!wifi_password.value){
      toast.warning($t('msg.Please enter WiFi password'))
    }else {  
      window.chrome.webview.postMessage(JSON.stringify({action:"print_wifi_password",setting:{wifi_password:wifi_password.value}}));
    emit('resolve', false);
    }
  }
  </script>