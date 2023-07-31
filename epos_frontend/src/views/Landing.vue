<template>
    <PageLayout icon="" title="">
         <div>Landing</div>
    </PageLayout>
</template>
<script setup>
import {ref} from "@/plugin"
import PageLayout from '../components/layout/PageLayout.vue';
import { createToaster } from "@meforma/vue-toaster";
const toaster = createToaster({position:"top"}); 

</script>