<template>
<PageLayout title="To Do App">
    <h1 class="bg-yellow-500 ma-4 rounded-lg text-center text-5xl">To Do List</h1>

    <div class="ma-4"> 
    <div class="mb-2">Add ToDo</div>
    <v-text-field v-model="toDo" placeholder="Add new todo"/>
    <v-btn @click="addToDo"   size="large" color="primary" class="mb-4">Save</v-btn>
  
 
    <v-card class="mb-4" v-for="(x, index) in todoList" :key="index">
        <v-card-item>
            <v-card-title>
                <div @click="onEdit(x)" v-if="x.is_edit==0" class="float-left" :class="{'line-through':x.status==1}">
                {{ index+1 }} -   {{ x.title }}
                </div>
                <v-text-field v-model="x.title" v-else append-inner-icon="mdi-content-save" @click:appendInner="onEditSave(x)"></v-text-field>

                <v-btn @click="onDelete(index)" class="float-right" icon="mdi-delete-circle" color="error"></v-btn>
                <v-btn @click="onDone(x)" class="float-right" icon="mdi-check" color="success"></v-btn>
            </v-card-title>
    
        </v-card-item>
    </v-card>
 
    {{ todoList }}


    </div>
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