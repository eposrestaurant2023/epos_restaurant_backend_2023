<template>
    <ComModal mobileFullscreen persistent @onClose="onClose()" hideOkButton hideCloseButton
        actionClass="flex justify-between">
        <template #content>
            <div class="-mt-2 -mx-2 h-48 bg-cover bg-no-repeat bg-center"
                v-bind:style="{ 'background-image': 'url(' + (product.photo || 'https://th.bing.com/th/id/R.86d14ad8fcdcd57a60db73f08bf7decd?rik=%2bZC5AvgTB0HOpA&riu=http%3a%2f%2fwww.foodista.com%2fsites%2fdefault%2ffiles%2fdefault_images%2fplaceholder_rev.png&ehk=MaPzc0Tw0%2b0a9CX200TtE46nENEFJD7YY33iWfp0oV8%3d&risl=&pid=ImgRaw&r=0') + ')' }">
            </div>
            <div class="flex justify-between items-center">
                <h2 class="pt-3 font-bold">{{ product.product_name_en }}</h2>
                <div class="pt-3">
                    <span class="pr-1 text-xs">From</span>
                <span class="text-lg font-bold text-right" :style="{ color: gv.setting?.template_style?.title_color }"
                    :class="gv.setting?.template_style?.title_class">12$</span>
                </div>
            </div>
            <div class="py-3">
                <hr />
            </div>
            <div v-html="product.description"></div>
            <ComCheckPortion v-if="product.product_price.length > 0" :portions="product.product_price"/>
            <ComCheckModifier v-if="product.product_modifiers.length > 0" :modifier="product.product_modifiers"/>
        </template>
        <template #action>
            <div class="flex justify-between grow border-t pt-2">
                <div class="grow flex items-center">
                    <button class="bg-black w-8 h-8 rounded-sm" type="button" @click.stop="onMinus()">
                        <v-icon>mdi-minus</v-icon>
                    </button>
                    <div class="h-8 px-2 flex items-center justify-center text-lg" style="min-width: 40px;">{{ qty }}</div>
                    <button type="button" @click.stop="onAdd()" class="bg-black w-8 h-8 rounded-sm">
                        <v-icon>mdi-plus</v-icon>
                    </button>
                </div>
                <button type="button" class="h-8 px-4 rounded-sm text-white"
                    :style="{ 'background-color': gv.setting?.template_style?.title_color }"><v-icon>mdi-cart-outline</v-icon>
                    Add to card</button>
            </div>
        </template>
    </ComModal>
</template>
<script setup>
import { inject, ref } from 'vue'
import ComModal from '../../components/ComModal.vue';
import ComCheckPortion from './components/ComCheckPortion.vue';
import ComCheckModifier from './components/ComCheckModifier.vue';
const props = defineProps({
    product: Object
})
let qty = ref(12330)
const gv = inject("$gv")
const emit = defineEmits(['onCloseModal'])
function onClose() {
    emit('onCloseModal')
}
</script> 
<style>
.v-list-item--variant-text .v-list-item__overlay {
    background: transparent !important;
}
</style>