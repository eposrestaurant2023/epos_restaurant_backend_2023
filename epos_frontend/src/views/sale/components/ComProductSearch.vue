<template>
    <div class="px-6">
        <div class="search-box my-0 mx-auto max-w-[350px]">
           
            <ComInput
              
                keyboard
                variant="outlined"
                placeholder="Search..."
                prepend-inner-icon="mdi-magnify"
                v-debounce="onSearch"
                @onInput="onSearch"
                />
        </div>
    </div>
</template>
<script setup>
import { inject }   from '@/plugin';
import ComInput from '../../../components/form/ComInput.vue';
const product =inject("$product") 
function onSearch(key) {
    product.searchProductKeyweord = key;
}
</script>