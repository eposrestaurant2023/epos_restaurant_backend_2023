<template>
    <div>
      
        <v-card
            :title="sale.sale.customer_name"
            :subtitle="subTitle"
            >
            <template v-slot:prepend>
                <v-avatar
                    color="grey-darken-3"
                    :image="sale.sale.customer_photo"
                ></v-avatar>
            </template>
            <template v-slot:append>
                <v-btn
                    size="small"
                    variant="text"
                    color="error"
                    icon="mdi-delete"
                    @click="onEdit()"
                ></v-btn>
                <v-btn
                    size="small"
                    variant="text"
                    color="error"
                    icon="mdi-delete"
                    @click="onRemove()"
                ></v-btn>
            </template>
        </v-card>        
    </div>
</template>
<script setup>
import {computed,inject} from "@/plugin"
 
const sale = inject("$sale")


function onEdit(){
   
     
}


const subTitle = computed(() => {
    let title = sale.sale.customer;
    if (sale.sale.phone_number !="" && sale.sale.phone_number !=undefined){
        title = title + " / " + sale.sale.phone_number
    }
  return title;
})



</script>