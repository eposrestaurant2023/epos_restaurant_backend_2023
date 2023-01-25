<template>
    <v-dialog
       v-model="open"
     >
     
     <v-card>
         <v-card-text>
             <article>
             <header> this is opoup 2 {{ params.text }}</header>
 
             <div class="grid">
                 
             </div>
 
              
         </article>
         <v-btn color="success" @click="confirm">{{ params.confirmButtonText || 'Yes' }}</v-btn>
                 <v-btn color="error" @click="decline">{{ params.declineButtonText || 'No' }}</v-btn>
         </v-card-text>
         <v-card-actions>
            
         </v-card-actions>
       </v-card>
         
     </v-dialog>
 </template>
 
 <script lang="ts">
     import { defineComponent, PropType } from 'vue';
 
     interface params {
         text: string;
         confirmButtonText?: string;
         declineButtonText?: string;
     }
 
     export default defineComponent({
         name: 'ComPopup',
         props: {
             open:{
                 type:Boolean,
                 default:true
             },
             params: {
                 type: Object as PropType<params>,
                 required: true,
             },
         },
         methods: {
             confirm() {
                 this.$emit('resolve', {id:1,name:"dara"});
             },
             decline() {
                 this.$emit('resolve', false);
             },
         },
     });
 </script>