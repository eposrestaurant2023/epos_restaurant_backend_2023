<template>
  <ComLoadingDialog v-if="saleDocResource.loading"/>

  <div v-else>
    <v-btn @click="ReloadData()">Reload Data</v-btn>
    <v-btn @click="UpdateValue()">Update</v-btn>
    
    {{ saleDocResource.doc?.note }}

  </div>


</template>

<script setup>
import { createDocumentResource, inject } from "@/plugin"
import ComLoadingDialog from "../components/ComLoadingDialog.vue";
const gv = inject('$gv')
const auth = inject('$auth')
console.log(gv.getCurrentUser())

if(!localStorage.getItem('current_user')){
   auth.logout().then((r)=>{
      router.push({name: 'Login'})
  })
   
}

const saleDocResource = createDocumentResource({
  url: "frappe.client.get",
  doctype: "Sale",
  name: "SO2023-0141",
  setValue: {
    onSuccess(d) {
      alert("updte success")

    },

  },
})

function UpdateValue(){
  saleDocResource.setValue.submit({
    note:"Hell Notex",
    docstatus:2
})

}

</script>


