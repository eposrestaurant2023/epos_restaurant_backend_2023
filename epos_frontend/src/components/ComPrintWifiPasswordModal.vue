<template>
    <ComModal :mobileFullscreen="true" @onClose="onClose()" width="500px" :hideOkButton="false" titleOKButton="Print" @onOk="onPrint()">
      <template #title>
        Print Wifi Password
      </template>
      <template #content>
        <ComInput placeholder="Enter wifi password" v-model="wifi_password" keyboard autofocus/>
      </template>
    </ComModal>
  </template>
  <script setup>
  import { defineEmits, ref} from "@/plugin"


 import ComModal from "./ComModal.vue";
import ComInput from "./form/ComInput.vue";
import { createToaster } from "@meforma/vue-toaster";
const toast = createToaster({position:"top"});

const wifi_password = ref("")
  const emit = defineEmits(["resolve"])
 
  function onClose() {
    emit('resolve', false);
  }
  function onPrint(){
    if(!wifi_password.value){
      toast.warning("Please enter wifi password")
    }else {  
      window.chrome.webview.postMessage(JSON.stringify({action:"print_wifi_password",setting:{wifi_password:wifi_password.value}}));
    emit('resolve', false);
    }
  }
  </script>