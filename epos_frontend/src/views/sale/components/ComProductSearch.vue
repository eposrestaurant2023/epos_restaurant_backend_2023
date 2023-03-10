<template>
    <div :class="small ? 'px-2' : 'px-6'">
        <div class="search-box my-0 mx-auto" :class="small ? 'w-full' : 'max-w-[350px]'">
            <ComInput
                autofocus
                keyboard
                variant="outlined"
                placeholder="Search..."
                prepend-inner-icon="mdi-magnify"
                v-model="product.searchProductKeywordStore"
                v-debounce="onSearch"
                @onInput="onSearch"
                />
        </div>
    </div>
</template>
<script setup>
import { inject,ref, defineProps }   from '@/plugin';
import ComInput from '../../../components/form/ComInput.vue';
const product =inject("$product") 
 
const props = defineProps({
    small: {
        type: Boolean,
        default: false
    }
});
function onSearch(key) {
    product.searchProductKeyword = key;
}
 

</script>