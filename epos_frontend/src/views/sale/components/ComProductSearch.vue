<template>
    <div class="px-6">
        <div class="search-box my-0 mx-auto max-w-[350px]">

            <ComInput
                autofocus
                keyboard
                variant="outlined"
                placeholder="Search..."
                prepend-inner-icon="mdi-magnify"
                v-model="search"
                v-debounce="onSearch"
                @onInput="onSearch"
                />
        </div>
    </div>
</template>
<script setup>
import { inject,ref }   from '@/plugin';
import ComInput from '../../../components/form/ComInput.vue';
const product =inject("$product") 
let search = ref('')
function onSearch(key) {
    product.searchProductKeyweord = key;
}
</script>