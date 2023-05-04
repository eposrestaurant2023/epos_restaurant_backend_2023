<template> 
    <div 
        v-ripple
        class="relative overflow-hidden h-full bg-cover bg-no-repeat rounded-lg shadow-md cursor-pointer bg-gray-300 "
        :class="{'border-red-700 border-2':product.selected}"
        v-bind:style="{ 'background-image': 'url(' + (product.photo || '') + ')' }" @click="onClickProduct()">
        <div class="absolute top left bg-red-700 rounded-br-sm z-20">
            <v-icon icon="mdi-checkbox-marked-circle-outline" color="white" v-if="product.selected"></v-icon>
        </div>
        <div class="absolute top-1 right-1 bg-gray-100 rounded-md z-20 px-1" :class="{'!bg-red-700 text-white':product.selected}">
            <span class="text-xs">x</span><b class="text-sm">{{ product.quantity }}</b>
        </div>
        <div class="absolute top-0 bottom-0 right-0 left-0 z-10" v-if="!product.photo && product.product_name">
            <avatar class="!h-full !w-full" :name="product.product_name" :rounded="false" background="#f1f1f1"></avatar>
        </div>
        <div class="block relative p-2 w-full h-full">
            <div class="p-1 rounded-md absolute bottom-1 right-1 left-1 bg-gray-50 bg-opacity-90 text-sm text-center z-20">
                {{ product.product_code }} - {{ product.product_name }}
            </div>
        </div>
    </div>
</template>
<script setup>
    import {inject} from 'vue'
    const pro = inject('$product')
    const props = defineProps({ product: Object, group: Object })
    function onClickProduct(){
        pro.onSelectComboMenu(props.product, props.group)
    }
</script>