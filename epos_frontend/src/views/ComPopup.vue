<template>
    <v-dialog fullscreen v-model="open">

        <v-card>
            <v-card-text>

                <h1>{{ params.text }}</h1>

                <div class="grid">
                    <v-btn @click="onOpen">Open Dialog box 2</v-btn>
                </div>


                <button @click="confirm">{{ params.confirmButtonText || 'Yes' }}</button>
                <v-btn color="error" @click="decline">{{ params.declineButtonText || 'Close Me' }}</v-btn>
            </v-card-text>
            <v-card-actions>

            </v-card-actions>
        </v-card>

    </v-dialog>
</template>

<script setup>
import { defineProps, defineEmits, ref } from '@/plugin'
import { comPopup2Dialog } from '../utils/dialog.ts';
const open = ref(true)


const props = defineProps({
    params: {
        type: Object,
        required: true,
    },
})

const emit = defineEmits(["resolve"])

function confirm() {
   emit('resolve', true);
}
function decline() {
    emit('resolve',false);
}
async function onOpen() {
    const result = await comPopup2Dialog({
        text: "Do you like my name is pheakdey?"
    });
    if (result) {
        alert(JSON.stringify(result))
        // const data =JSON.parse(result.toString()) 
        // alert(data.name)
    }
}
</script>