<template>
    <ComModal mobileFullscreen persistent @onClose="onClose()" hideOkButton hideCloseButton actionClass="flex justify-between">
        <template #content>
            <div class="-mt-2 -mx-2 h-48 bg-cover bg-no-repeat bg-center" v-bind:style="{'background-image': 'url(' + (product.photo|| 'https://th.bing.com/th/id/R.86d14ad8fcdcd57a60db73f08bf7decd?rik=%2bZC5AvgTB0HOpA&riu=http%3a%2f%2fwww.foodista.com%2fsites%2fdefault%2ffiles%2fdefault_images%2fplaceholder_rev.png&ehk=MaPzc0Tw0%2b0a9CX200TtE46nENEFJD7YY33iWfp0oV8%3d&risl=&pid=ImgRaw&r=0') + ')' }"></div>
            <h2 class="pt-3 font-bold">{{ product.product_name_en }}</h2>
            <div class="py-3">
                <hr/>
            </div>
            <div v-html="product.description"></div>
        </template>
        <template #action>
            <div class="text-lg font-bold text-right"  :style="{color:gv.setting?.template_style?.title_color}" :class="gv.setting?.template_style?.title_class">12$</div>
            <div>
                <div>
                    <div class="flex justify-end">
                        <template v-if="qty > 0">
                            <v-btn class="m-1" size="x-small" color="error" icon="mdi-delete" variant="tonal" @click.stop="onDelete()"></v-btn>
                            <v-btn class="m-1" size="x-small" color="pink" icon="mdi-minus" variant="tonal" @click.stop="onMinus()"></v-btn>
                            <div class="p-2">{{ qty }}</div>
                        </template>
                        <v-btn class="m-1" size="x-small" color="success" icon="mdi-plus" variant="tonal" @click.stop="onAdd()"></v-btn>
                    </div>
                </div>
            </div> 
        </template>
    </ComModal>
</template>
<script setup>
import {inject} from 'vue'
import ComModal from '../../components/ComModal.vue'; 
const props = defineProps({
    product: Object
})
const gv = inject("$gv")
const emit = defineEmits(['onCloseModal'])
function onClose(){
    emit('onCloseModal')
}
</script> 