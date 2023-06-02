<template>
 
    <v-card>
        <v-card-title>{{ $t('Note') }}</v-card-title>
        <v-card-text class="!pb-0">
            <div>
                <div class="mb-2">
                    <ComInput autofocus :placeholder="$t('Search or Add Note')" keyboard v-model="search"
                        v-debounce="onSearch" />
                    <v-alert class="mt-4" v-if="getSelectedNote() != ''" :text="getSelectedNote()"></v-alert>
                </div>
                <div class="-m-1">
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
            </div>
        </v-card-text>
        <v-card-actions class="justify-end">
                <div>
                    <v-btn size="small" class="mr-2"  v-if="search" variant="flat" @click="onSaveNote" color="success">
                            {{ $t('Save Note') }}
                    </v-btn>
                    <template v-if="isDeleteNote">
                        <v-btn size="small" class="mr-2" variant="flat" @click="onCancelDeleteNote" color="warning">
                            {{ $t('Cancel') }}
                        </v-btn>
                        <v-btn size="small" class="mr-2" v-if="noteResource.doc.notes.filter(r => r.chip == false).length > 0"
                            variant="flat" @click="onDeleteNote" color="error">
                            {{ $t('Delete') }}
                        </v-btn>

                    </template>
                    <template v-else>
                        <v-btn size="small" class="mr-2" variant="flat" @click="onEnableDeleteNote" color="error">
                            {{ $t('Delete Note') }}
                        </v-btn>
                    </template>

                </div>
            </v-card-actions>
    </v-card>
 
</template>

<script setup>
import { defineEmits, ref, createDocumentResource, confirmDialog, i18n} from '@/plugin'
import Enumerable from 'linq';

const { t: $t } = i18n.global; 

const emit = defineEmits(['resolve','update:modelValue'])
const props = defineProps({
    category_note: String,
    product_code: String,
    modelValue: String
})

let search = ref()
const isDeleteNote = ref(false);

const noteResource = createDocumentResource({
    url: "frappe.client.get_list",
    doctype: "Category Note",
    name: props.category_note,
    auto: true,
    cache: ['category_note', props.category_note],
    setValue: {
    onSuccess() {
        //
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
    emit('update:modelValue', search.value)
}

function getNote() {
    if (search.value == undefined) search.value = "";
    if (noteResource.doc !== null) {
        let notes = noteResource.doc.notes;
        if (props.product_code) {
            notes =
                notes.filter((r) => {
                    return (r.product_code == props.product_code || r.product_code == '' || r.product_code == null)
                })
                ;
        } else {
 
            notes =
                noteResource.doc?.notes.filter((r) => {
                    return (!r.product_code || props.product_code)
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

function onOK() {
    let selectedNote = getSelectedNote();
    if(!selectedNote){
        selectedNote = search.value;
    }
    emit('update:modelValue', selectedNote)
}

function onSelected(value) {
    if (isDeleteNote.value == false) {
        const selected = value.selected
        if (noteResource.doc.multiple_selected == 0) {
            Enumerable.from(noteResource.doc?.notes).where(`$.selected==true`).forEach("$.selected=false");
        }
        value.selected = !selected;
        onOK()
    }

}

function onEnableDeleteNote() {
    isDeleteNote.value = true;
    noteResource.doc.notes.forEach((r) => {
        r.selected = false;
    })
    emit('update:modelValue', '')
}
function onCancelDeleteNote() {
    isDeleteNote.value = false;
    noteResource.doc.notes.forEach((r) => {
        r.chip = true;
    })
}
async function onDeleteNote() {
    if (await confirmDialog({ title: $t('Delete Note'), text: $t('msg.are you sure to delete note') })) {
        const notes = noteResource.doc.notes.filter(r => r.chip == true);
        noteResource.doc.notes = notes
        noteResource.setValue.submit({ notes: notes });
        onCancelDeleteNote()
    }

}
function onSaveNote(){

    const notes = noteResource.doc.notes;
    notes.push({note:search.value})
    noteResource.setValue.submit({ notes: notes });
   // search.value = "";
}
</script>