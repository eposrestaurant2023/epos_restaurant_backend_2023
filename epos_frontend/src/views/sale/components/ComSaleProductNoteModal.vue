<template>
    <ComModal :persistent="true" @onClose="onClose()" @onOk="onOK" title-ok-button="OK">
        <template #title>
            Notice
        </template>
        <template #content>
            <div>
                <div class="mb-2">
                    <ComInput keyboard v-model="search" v-debounce="onSearch" />
                </div>
                <div class="-m-1">
                    <template
                        v-for="(item, index) in noteResource.doc?.notes.filter(r => r.product_code == params.data.product_code || r.product_code == null)"
                        :key="index">
                        <v-chip class="m-1" @click="onSelected(item)">
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
    </ComModal>
</template>
<script setup>
import { defineEmits, ref, createDocumentResource } from '@/plugin'
import Enumerable from 'linq'
const emit = defineEmits(['resolve'])
const props = defineProps({
    params: Object
})
let open = true;
let search = ref()

const noteResource = createDocumentResource({
    url: "frappe.client.get_list",
    doctype: "Category Note",
    name: props.params.name,
    auto: true,
    cache: ['category_note', props.params.name],
    transform(doc) {
        doc.notes.forEach(r => {
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
    selected.forEach(r => {
        result = result + (r.note + " | ")
    })

    if (result)
        emit('resolve', result.slice(0, -3))
    else
        onClose()
}
function onSelected(value) {
    const selected = value.selected
    if (noteResource.doc.multiple_selected == 0) {
        Enumerable.from(noteResource.doc?.notes).where(`$.selected==true`).forEach("$.selected=false");
    }
    value.selected = !selected;
}
</script>