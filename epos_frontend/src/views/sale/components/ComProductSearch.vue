<template>
    <div class="px-6">
        <div class="search-box my-0 mx-auto max-w-[350px]">
            <ComInput
                type="text"
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
import {useStore} from '@/plugin';
import ComInput from '../../../components/form/ComInput.vue';
const store = useStore() 
function onSearch(key) {
    store.state.sale.keyword = key;
}
</script>