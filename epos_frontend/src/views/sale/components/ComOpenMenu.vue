<template>
    <v-dialog v-model="open" @update:modelValue="onClose">
        <v-card max-width="960" width="960" class="mx-auto my-0">
            <ComToolBar @onClose="onClose">
                <template #title>
                    {{ params.data.product_name }}
                </template>

            </ComToolbar>
            <v-card-text>
                <v-row>
                    <v-col md="7">
                        <div class="mb-2">
                            <div class="mb-2">
                                <ComInput autofocus placeholder="Search or Add Note" keyboard v-model="search"
                                    v-debounce="onSearch" />
                                <v-alert class="mt-4" v-if="getSelectedNote() != ''"
                                    :text="getSelectedNote()"></v-alert>
                            </div>
                            <div class="-m-1">
                                <template v-for="(item, index) in getNote()" :key="index">

                                    <v-chip :closable="isDeleteNote" v-if="item.chip" @click:close="item.chip = false"
                                        class="m-1" @click="onSelected(item)">
                                        <v-icon start icon="mdi-checkbox-marked-circle-outline" v-if="item.selected"
                                            color="orange"></v-icon>
                                        <span>
                                            {{ item.note }}
                                        </span>
                                    </v-chip>

                                </template>
                            </div>
                        </div>
                    </v-col>
                    <v-col md="5">
                        <ComInlineInputNumber :disabled="isDeleteNote"/>
                    </v-col>
                </v-row>

                <div>
                    <div class="text-right pt-4">
                        <v-btn class="mr-2" v-if="search" variant="flat" @click="onSaveNote" color="success">
                            Save Note
                        </v-btn>
                        <template v-if="isDeleteNote">
                            <v-btn class="mr-2" variant="flat" @click="onCancelDeleteNote" color="warning">
                                Cancel
                            </v-btn>
                            <v-btn class="mr-2" v-if="noteResource.doc.notes.filter(r => r.chip == false).length > 0"
                                variant="flat" @click="onDeleteNote" color="primary">
                                Confirm
                            </v-btn>

                        </template>
                        <template v-else>
                            <v-btn class="mr-2" variant="flat" @click="onEnableDeleteNote" color="error">
                                Delete Note
                            </v-btn>

                            <v-btn class="mr-2" variant="flat" @click="onClose(false)" color="error">
                                Close
                            </v-btn>

                            <v-btn variant="flat" @click="onOK()" color="primary">
                                OK
                            </v-btn>
                        </template>

                    </div>
                </div>
            </v-card-text>
        </v-card>
    </v-dialog>
</template>
<script setup>
import { defineEmits, ref, createDocumentResource, confirmDialog } from '@/plugin'
import { createToaster } from '@meforma/vue-toaster';
import ComInlineInputNumber from '../../../components/ComInlineInputNumber.vue';
import ComToolBar from "../../../components/ComToolbar.vue"
import Enumerable from 'linq'
const emit = defineEmits(['resolve'])
const props = defineProps({
    params: Object
})
const toast = createToaster({ position: "top" })
const open = true;
let search = ref()
const isDeleteNote = ref(false);
let selectedNotes = ref([]);
const price = ref(10);

const noteResource = createDocumentResource({
    url: "frappe.client.get_list",
    doctype: "Category Note",
    name: "Open Menu Note",
    auto: true,
    cache: ['category_note', "Open Menu Note"],
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


function onSearch(keyword) {
    search.value = keyword;
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

            notes =
                noteResource.doc?.notes.filter((r) => {
                    return (!r.product_code || props.params.data.product_code)
                })
                ;
        }
        return notes.filter((r) => {
            return String(r.note).toLocaleLowerCase().includes(search.value.toLocaleLowerCase());
        });
    }
}
function getSelectedNote() {
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

    if (!selectedNote) {

        selectedNote = search.value;
    }
    if (selectedNote == "" || selectedNote == undefined) {
        toast.warning("Please select or enter note");
        return;
    }
    emit('resolve', { product_name: selectedNote, price: price.value })

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
    // search.value = "";
}
</script>