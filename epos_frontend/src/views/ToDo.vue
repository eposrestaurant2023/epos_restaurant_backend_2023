<template>
<PageLayout icon="" title="">
     
</PageLayout>
</template>
<script setup>
import {ref,confirmDialog} from "@/plugin"
import PageLayout from '../components/layout/PageLayout.vue';
import { createToaster } from "@meforma/vue-toaster";
const toaster = createToaster({position:"top"});
const todoList = ref([]);
const toDo = ref("")

function addToDo(){
    todoList.value.push(
        {
            title:toDo.value,
            status:0,
            is_edit:0
        }
    )
}

function onDone(myTodo){
    myTodo.status = 1;
}
function onEdit(myTodo){
    myTodo.is_edit = 1;
}
function onEditSave(myTodo){
    myTodo.is_edit = 0;
}
async function onDelete(index){
    if(await confirmDialog({title:"Delete to do",text:"Are you sure?"})){ 
    todoList.value.splice(index,1);
    toaster.success("Delete to successfully")
    }
}
</script>