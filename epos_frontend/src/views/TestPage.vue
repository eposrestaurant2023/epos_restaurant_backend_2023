<!-- <template>
  <div class="p-4 bg-yellow-200">
    <v-btn @click="AddToDo">Add Todo</v-btn>
    <v-btn @click="APICall">Add Todo</v-btn>
{{ docResource }}
sdfsd
   
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { createResource,createDocumentResource } from '@/plugin';


const docResource = createDocumentResource({
  doctype:"Sale",
  name:"PK2023-000608"
})

let todoResource = createResource({
            url: "frappe.client.insert"
        }
        );
  const apiCallResource = createResource({
      url: "epos_restaurant_2023.api.api.sumbit_split_bill",
      params:{
        data:"hello world"
      }
      
  }
  );
  
function AddToDo(){
 
  todoResource.submit({doc:{
    doctype:"ToDo",
    description:"Test Insert to with outdom"
  }}).then((v)=>{
    console.log(v)
  });
}

function APICall(){
  createDocumentResource({
  doctype:"Sale",
  name:"PK2023-000607",
  onSuccess(doc){
    alert("done")
    console.log(doc);
  }
})


}


</script> -->


<template>
  <div class="v-container">
    Test Slot
    <childA> 
        <template #defualt>
          <h1>Defualt</h1>
        </template>
        <template #footer>
          this is contact list:
        </template>
        <template #header>
          Logo
        </template>
    </childA>
  </div>
</template>
<script setup>
 import childA from './deleteme/childA.vue'
</script>
<style lang="">
  
</style>