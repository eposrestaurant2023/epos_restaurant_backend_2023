<template>
    <v-dialog v-model="open" persistent style="max-width: 800px;">
        <v-card>
            <v-toolbar color="default" title="Notice">
                <v-toolbar-items>
                    <v-btn icon @click="onClose()">
                        <v-icon>mdi-close</v-icon>
                    </v-btn>
                </v-toolbar-items>
            </v-toolbar>
            <v-card-text>
                <div class="mb-2">
                    <div class="mb-2">
                        <ComInput keyboard v-model="search" v-debounce="onSearch"/>
                    </div>
                    <div class="-m-1">
                        <template v-for="(item, index) in noteResource.doc?.notes.filter(r=>r.product_code == params.data.product_code || r.product_code == null)" :key="index">
                            <v-chip 
                                class="m-1"
                                @click="onSelected(item)" >
                                <v-icon start icon="mdi-checkbox-marked-circle-outline" v-if="item.selected" color="orange"></v-icon>
                                <span>
                                    {{ item.note }}
                                </span>
                            </v-chip> 
                        </template>
                    </div>
                </div>
                <div>
                    <div class="text-right pt-4">
                        <v-btn class="mr-2" variant="flat" @click="onClose(false)" color="error">
                            Close
                        </v-btn>
                        <v-btn variant="flat" @click="onOK()" color="primary">
                            OK
                        </v-btn>
                    </div>
                </div>
            </v-card-text>
        </v-card>
    </v-dialog>
</template>
<script setup>
import { defineEmits, ref,createDocumentResource } from '@/plugin'
import Enumerable from 'linq'
const emit = defineEmits(['resolve'])
const props = defineProps({
    params: Object
})
const open = true;
let search = ref()

const noteResource = createDocumentResource({
    url: "frappe.client.get_list",
    doctype: "Category Note",
    name: props.params.name,
    auto: true,
    cache: ['category_note', props.params.name],
    transform(doc) {
        doc.notes.forEach(r=>{
            r.selected = false
        })
        return doc
    }
});


function onSearch(keyword) {
    search.value = keyword;
}
 
function onClose() {
    emit('resolve', false)
}
function onOK() {
    const selected = Enumerable.from(noteResource.doc?.notes).where(`$.selected==true`).toArray()
    let result = '';
    selected.forEach(r=>{
        result = result + (r.note + " | ")
    })
    
    if(result) 
        emit('resolve', result.slice(0,-3))
    else
        onClose()
}
function onSelected(value){
    const selected = value.selected
    if(noteResource.doc.multiple_selected == 0){
        Enumerable.from(noteResource.doc?.notes).where(`$.selected==true`).forEach("$.selected=false");
    }
    value.selected = !selected;
}
</script>