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
                @keydown="onKeyDown"
                
                />
                
        </div>
    </div>
</template>
<script setup>
import { inject,ref, defineProps,createResource }   from '@/plugin';
import ComInput from '../../../components/form/ComInput.vue';
import { createToaster } from '@meforma/vue-toaster';
const product =inject("$product") 
const sale=inject("$sale") 

const toaster = createToaster({position:'bottom'});
const props = defineProps({
    small: {
        type: Boolean,
        default: false
    }
});



function onSearch(key) {
    product.searchProductKeyword = key;
}
function onKeyDown(event) {
      if(event.key =="Enter"){
        toaster.info(product.searchProductKeywordStore)
       
        const searchProductResource = createResource({
                url: "epos_restaurant_2023.api.product.get_product_by_barcode",
                    params: {
                        barcode:product.searchProductKeywordStore
                    }
            });

        searchProductResource.fetch().then((doc)=>{
            sale.addSaleProduct(doc);
            product.searchProductKeywordStore = "";
        });

        
      }
    }

</script>