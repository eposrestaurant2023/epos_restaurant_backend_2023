<template>
    <ComModal :persistent="true" :fullscreen="mobile" @onClose="onClose()" :hide-ok-button="true" :hide-close-button="true">
        <template #title>
            <div>{{ $t('Note') }}</div>
        </template>
        <template #content>
            <div>
                <div class="mb-2">
                    <ComInput autofocus :placeholder="$t('Search or Add Note')" keyboard v-model="search"
                        v-debounce="onSearch" />
                    <v-alert class="mt-4 !p-2" v-if="getSelectedNote() != ''" :text="getSelectedNote()"></v-alert>
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
        </template>
        <template #action>

            <v-btn v-if="search && !isDeleteNote" variant="flat" @click="onSaveNote" color="success">
                {{ $t('Save') }}
            </v-btn>
 
            <template v-if="isDeleteNote">
                <v-btn variant="flat" @click="onCancelDeleteNote" color="warning">
                    {{ $t('Cancel') }}
                </v-btn>
                <v-btn v-if="noteResource.doc.notes.filter(r => r.chip == false).length > 0"
                    variant="flat" @click="onDeleteNote" color="primary">
                    {{ $t('Confirm') }}
                </v-btn>
            </template>
            <template v-else>
                <v-btn variant="flat" @click="onEnableDeleteNote" color="error">
                    {{ $t('Delete') }}
                </v-btn>
                <v-btn variant="flat" @click="onOK()" color="primary">
                    {{ $t('Ok') }}
                </v-btn>
            </template>
        </template>
    </ComModal>
</template>
<script setup>
import { defineEmits, ref, createDocumentResource, confirmDialog, i18n} from '@/plugin'
import { createToaster } from '@meforma/vue-toaster';
import Enumerable from 'linq'
import { useDisplay } from 'vuetify';
const { t: $t } = i18n.global; 
const emit = defineEmits(['resolve'])
const { mobile } = useDisplay()  
const props = defineProps({
    params: Object
})
const toast = createToaster({position:"top"})
let open = true;
let search = ref()
const isDeleteNote = ref(false);
let selectedNotes = ref([]);

const noteResource = createDocumentResource({
    url: "frappe.client.get_list",
    doctype: "Category Note",
    name: props.params.name,
    auto: true,
    cache: ['category_note', props.params.name],
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
    
    if(!selectedNote){
      
        selectedNote = search.value;
    }
    if (selectedNote=="" || selectedNote==undefined ){
        toast.warning($t('msg.Please select or enter note'));
        return;
    }

    emit('resolve', selectedNote)

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
    if (await confirmDialog({ title: $t('Delete Note'), text: $t("msg.are you sure to delete note") })) {
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
    search.value = "";
}
</script>