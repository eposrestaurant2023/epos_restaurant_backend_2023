<template>
    <div> 
        <div
            class="flex justify-center items-center bg-cover p-2"
            style="background-size: cover; background-repeat: no-repeat; background-position: center center;"
            v-bind:style="[`height: ${gv.setting?.template_style?.height_banner}`,`background-color:${gv.setting?.template_style?.background_color_banner || '#c5c5c5'}`, `background-image: url(${banner})`]">
            <h1 class="text-white font-bold text-center">
                {{ category.pos_menu_name_en }}
            </h1>
        </div>
        <div class="py-4 px-2">
            <v-row class="!m-0">
                <v-col class="!p-0" v-for="(p, index) in products" :key="index" cols="12" md="6">
                    <ComProductList :product="p" @on-view="onView($event)"/>
                </v-col>
            </v-row>
        </div>
        <ProductDetail :product="selected" v-if="open" @onCloseModal="onCloseModal"/>
    </div>
</template>
<script setup>
import { inject, ref,onMounted } from 'vue'
import { useRoute } from 'vue-router' 
import ComProductList from '../components/ComProductList.vue';
import ProductDetail from './ProductDetail.vue';

const gv = inject('$gv')
const frappe = inject('$frappe')
const db = frappe.db()
const call = frappe.call()
const route = useRoute()
const category = ref({})
let banner = ref('')
const products = ref([])
const selected = ref({})
const open = ref(false)

onMounted(()=>{
    // console.log(route.params)
})


db.getDoc('POS Menu', route.params.category)
  .then(async (doc) => {
    category.value = doc
    banner.value = (category.value.banner || gv.setting?.template_style?.background_image_banner)
    await call.get('epos_restaurant_2023.api.emenu.get_emenu_product',{
        menu: category.value.name
    })
    .then((r)=> {
        products.value = r.message 
    }).catch(er=> console.log(er))
  })
  .catch((error) => console.error(error));

  function onView($event){
    selected.value = $event
    open.value = true
  }
  function onCloseModal(){
    open.value = false
  }
</script> 