<template>

    <v-btn @click="onOpen">click me</v-btn>
</template>
<script setup>
import { comPopupDialog, printPreviewDialog } from '../utils/dialog.ts';

async function onOpen(){
    const result = await comPopupDialog({
        text: "Do you like vue-promise-dialogs?"
    });
   console.log(result)
   alert(result)
}

</script>