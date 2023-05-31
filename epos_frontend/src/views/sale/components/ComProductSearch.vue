<template>
    <div :class="small ? 'px-2' : 'px-6'">
        <div class="search-box my-0 mx-auto" :class="small ? 'w-full' : 'max-w-[350px]'">
            <ComInput
                autofocus
                keyboard
                variant="outlined"
                :placeholder="$t('Search...')"
                prepend-inner-icon="mdi-magnify"
                v-model="product.searchProductKeywordStore"
                v-debounce="onSearch"
                @keydown="onKeyDown"
                :ref="control"
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
const frappe = inject("$frappe")
const db = frappe.db();
let control=ref(null)
const toaster = createToaster({position:'top-right',maxToasts:2});
const props = defineProps({
    small: {
        type: Boolean,
        default: false
    }
});

const doSearch = ref(true)

function onSearch(key) {
    if (key){
        doSearch.value = true
    }
    if(product.setting.pos_menus.length>0){
        product.searchProductKeyword = key;
        
    }else{
        //search product from db
 
        if(doSearch.value){ 
            product.getProductFromDbByKeyword(db,key)
        }
    }
    
}

function onKeyDown(event) {
      if(event.key =="Enter"){

        
       
        const searchProductResource = createResource({
                url: "epos_restaurant_2023.api.product.get_product_by_barcode",
                    params: {
                        barcode:product.searchProductKeywordStore
                    }
            });

        searchProductResource.fetch().then((doc)=>{

            sale.addSaleProduct(doc);
            toaster.success("Added product " + product.searchProductKeywordStore + " successfully")
            product.searchProductKeywordStore = "";
            doSearch.value = false

        });

        
      }
    }

</script>