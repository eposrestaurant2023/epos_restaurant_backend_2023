<template>
    <ComModal :mobileFullscreen="true" :width="params.data.category_note_name ? '1200px' : '400px'" @onClose="onClose()" @onOk="onOK()">
        <template #title>
            {{ params.data.title }}
        </template>
        <template #content>
            <div class="p-2">
            <v-row>
                <v-col cols="12" :md="params.data.category_note_name ? 5 : 12">
                    <div class="mb-2">
                        <div class="mb-2">
                            <ComInput v-model="price" type="number" keyboard class="mb-2" :label="params.data.label_input || 'Enter Number'"
                                :required-autofocus="true" />
                            <ComInlineInputNumber :hide-input="true" v-model="price" :disabled="isDeleteNote"
                                v-if="!mobile" />
                        </div>
                    </div>
                </v-col>
                <v-col cols="12" md="7" v-if="params.data.category_note_name">
                    <v-card title="Note">
                        <v-card-text>
                            <ComInput :autofocus="!mobile" keyboard v-model="search" v-debounce="onSearch"
                                label="Search or Add Note" />
                            <v-alert class="mt-4" v-if="getSelectedNote() != ''" :text="getSelectedNote()"></v-alert>
                            <div class="-mx-1">
                                <template v-for="(item, index) in getNote()" :key="index">
                                    <v-chip :closable="isDeleteNote" v-if="item.chip" @click:close="item.chip = false" class="m-1"
                                        @click="onSelected(item)">
                                        <v-icon start icon="mdi-checkbox-marked-circle-outline" v-if="item.selected"
                                            color="orange"></v-icon>
                                        <span>
                                            {{ item.note }}
                                        </span>
                                    </v-chip>
                                </template>
                            </div>
                        </v-card-text>
                        <v-card-actions class="justify-end">
                            <v-btn size="small" v-if="search && !isDeleteNote" variant="flat" @click="onSaveNote" color="success">
                                Save
                            </v-btn>
                            <template v-if="isDeleteNote">
                                <v-btn size="small" variant="flat" @click="onCancelDeleteNote" color="warning">
                                    Cancel
                                </v-btn>
                                <v-btn size="small" v-if="noteResource.doc.notes.filter(r => r.chip == false).length > 0" variant="flat"
                                    @click="onDeleteNote" color="primary">
                                    Confirm
                                </v-btn>
                            </template>
                            <v-btn size="small" v-else class="mr-2" variant="flat" @click="onEnableDeleteNote" color="error">
                                Delete
                            </v-btn>
                        </v-card-actions>
                    </v-card>
                </v-col>
            </v-row>
        </div>
        </template>
    </ComModal>
</template>
<script setup>
import { defineEmits, ref, createDocumentResource, confirmDialog } from '@/plugin'
import { createToaster } from '@meforma/vue-toaster';
import ComInlineInputNumber from '../ComInlineInputNumber.vue';
import Enumerable from 'linq'
import { useDisplay } from 'vuetify'
const { mobile } = useDisplay()
const emit = defineEmits(['resolve'])
const props = defineProps({
    params: Object
})
const toast = createToaster({ position: "top" })
let search = ref()
const isDeleteNote = ref(false);
let selectedNotes = ref([]);
const price = ref(props.params.data.number || 0);

let noteResource = ref(null)
if(props.params.data.category_note_name){
    noteResource = createDocumentResource({
    url: "frappe.client.get_list",
    doctype: "Category Note",
    name: props.params.data.note, //"Open Menu Note"
    auto: true,
    cache: ['category_note', props.params.data.note],//"Open Menu Note"
    setValue: {
        onSuccess() {
            toast.success("Success");

        },
    },
    transform(doc) {
        doc.notes.forEach(r => {
            r.selected = false,
                r.chip = true
        })
        return doc
    }
});
}



function onSearch(keyword) {
    search.value = keyword;
    if(search.value){
        Enumerable.from(noteResource.doc?.notes).where(`$.selected==true`).forEach("$.selected=false");
        getSelectedNote(true)
    }
}


function getNote() {
    if (search.value == undefined) search.value = "";
    if (noteResource.doc !== null) {
        let notes = noteResource.doc.notes;
        if (props.params.data.product_code) {
            notes =
                notes.filter((r) => {
                    return (r.product_code == props.params.data.product_code || r.product_code == '' || r.product_code == null)
                })
                ;
        } else {

            notes = noteResource.doc?.notes.filter((r) => {
                return (!r.product_code || props.params.data.product_code)
            });
        }
        return notes.filter((r) => {
            return String(r.note).toLocaleLowerCase().includes(search.value.toLocaleLowerCase());
        });
    }
}
function getSelectedNote(clear = false) {
    if (noteResource.doc == null) {
        return "";
    } else {
        return Enumerable.from(noteResource.doc.notes).where("$.selected==true").select("$.note").toJoinedString(", ")
    }
}

function onClose() {
    emit('resolve', false)
}
function onOK() {
    let selectedNote = getSelectedNote();

    if (search.value) {

        selectedNote = search.value;
    }
    if (props.params.data.category_note_name && (selectedNote == "" || selectedNote == undefined)) {
        toast.warning("Please select or enter note");
        return;
    }
    emit('resolve', { note: selectedNote, number: parseFloat(price.value) })

}

function onSelected(value) {
    if (isDeleteNote.value == false) {
        const selected = value.selected
        if (noteResource.doc.multiple_selected == 0) {
            Enumerable.from(noteResource.doc?.notes).where(`$.selected==true`).forEach("$.selected=false");
        }
        value.selected = !selected;
    }

}

function onEnableDeleteNote() {
    isDeleteNote.value = true;
    noteResource.doc.notes.forEach((r) => {
        r.selected = false;
    })
}
function onCancelDeleteNote() {
    isDeleteNote.value = false;
    noteResource.doc.notes.forEach((r) => {
        r.chip = true;
    })

}
async function onDeleteNote() {
    if (await confirmDialog({ title: "Delete Note", text: "Are you sure you want to delete note?" })) {
        const notes = noteResource.doc.notes.filter(r => r.chip == true);
        noteResource.doc.notes = notes
        noteResource.setValue.submit({ notes: notes });
        onCancelDeleteNote()
    }

}
function onSaveNote() {
    const notes = noteResource.doc.notes;
    notes.push({ note: search.value, product_code: props.params.data.product_code })
    noteResource.setValue.submit({ notes: notes });
    search.value = "";
}
</script>