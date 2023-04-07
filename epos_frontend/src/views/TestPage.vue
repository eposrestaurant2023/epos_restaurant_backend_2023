<template>
  <ComLoadingDialog v-if="saleDocResource.loading"/>

  <div v-else>
    <v-btn @click="ReloadData()">Reload Data</v-btn>
    <v-btn @click="UpdateValue()">Update</v-btn>
    
    {{ saleDocResource.doc?.note }}
    <div class="wrap">
    <v-row class="h-full">
      <v-col class="A h-full" cols="hide" xs="12" sm="8" md="8" lg="8" xl="8">
        <v-carousel show-arrows="hover">
  <v-carousel-item
    src="https://cdn.vuetifyjs.com/images/cards/docks.jpg"
    cover
  ></v-carousel-item>

  <v-carousel-item
    src="https://cdn.vuetifyjs.com/images/cards/hotel.jpg"
    cover
  ></v-carousel-item>

  <v-carousel-item
    src="https://cdn.vuetifyjs.com/images/cards/sunshine.jpg"
    cover
  ></v-carousel-item>
</v-carousel>
      </v-col>
      <v-col class="h-full" cols="12" xs="12" sm="4" md="4" lg="4" xl="4">
        <div class="wrap-list">
          <div class="profile">
            profile
          </div>
          <div class="product-list">
            
          </div>
          <div class="wrap-footer">
            <div>Total Amount</div>
            <div>Total</div>
          </div>
        </div>
      </v-col>
    </v-row>
  </div>

  </div>


</template>

<script setup>
import { createDocumentResource, inject } from "@/plugin"
import ComLoadingDialog from "../components/ComLoadingDialog.vue";
const gv = inject('$gv')
const auth = inject('$auth')

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


