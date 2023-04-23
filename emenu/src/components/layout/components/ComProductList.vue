<template>
    <v-card elevation="0" @click="onView()" rounded="0">
        <div class="flex p-2 border-b">
            <v-avatar 
            size="80px"
            rounded="lg">
                <div class="h-full w-full bg-cover bg-center" v-bind:style="{ 'background-image': 'url(' + (product.photo || 'https://th.bing.com/th/id/R.86d14ad8fcdcd57a60db73f08bf7decd?rik=%2bZC5AvgTB0HOpA&riu=http%3a%2f%2fwww.foodista.com%2fsites%2fdefault%2ffiles%2fdefault_images%2fplaceholder_rev.png&ehk=MaPzc0Tw0%2b0a9CX200TtE46nENEFJD7YY33iWfp0oV8%3d&risl=&pid=ImgRaw&r=0') + ')' }"></div>
            </v-avatar>
            <div class="grow pl-2 grid content-between">
                <div class="flex justify-between">
                    <h3 class="font-bold">{{ product.product_name_en }}</h3>
                    <div class="text-lg font-bold text-right"  :style="{color:gv.setting?.template_style?.title_color}" :class="gv.setting?.template_style?.title_class">12$</div>
                </div>
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
        </div> 
    </v-card> 
</template>
<script setup>
    import { inject,ref } from 'vue'
    const gv = inject('$gv')
    const props = defineProps({
        product: Object
    })
    const emit = defineEmits(['onView'])
    let qty = ref(0)
    function onView(){
        emit('onView', props.product)
    }
    function onAdd(){ 
        qty.value++
    }
    function onMinus(){
        qty.value--
    }
    function onDelete(){
        qty.value = 0
    }
</script>