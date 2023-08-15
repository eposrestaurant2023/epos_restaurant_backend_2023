<template>
    <div>
        <div class="wrap-btn-back">
            <div class="btn-back">
                <v-btn size="small" icon="mdi-arrow-left" @click="onBack()"></v-btn>
            </div>
        </div>
        <div
            class="flex justify-center items-center bg-cover p-2 relative"
            style="background-size: cover; background-repeat: no-repeat; background-position: center center;"
            v-bind:style="[`height: ${gv.setting?.template_style?.height_banner}`,`background-color:${gv.setting?.template_style?.background_color_banner || '#c5c5c5'}`, `background-image: url(${banner})`]">
            
            <h1 class="text-white font-bold text-center">
                {{ category.pos_menu_name_en }}
            </h1>
        </div>
        <div class="py-4 px-2">
            <v-row class="!m-0">    
                <v-col class="!p-0 border-b" v-for="(c, index) in sub_categories" :key="index" cols="12" md="6"> 
                    <v-card elevation="0" @click="onCategoryClick(c)"  rounded="0">
                        <v-img   class="align-end" gradient="to bottom, rgba(0,0,0,.1), rgba(0,0,0,.5)"  height="105px" cover>
                            <div class="flex p-2">
                                <v-avatar  size="90px" rounded="lg">
                                    <div class="h-full w-full bg-cover bg-center" v-bind:style="{ 'background-image': 'url(' + (c.background_image==''?'/assets/frappe/images/emenu_placeholder.jpg' : c.background_image) + ')' }"></div>
                                </v-avatar>
                                <div class="grow pl-2">
                                    <div class="flex justify-between">
                                        <h2 class="font-bold">{{ c.title_en }}</h2>                                         
                                    </div> 
                                    <small v-html="c.description" ></small> 
                                </div>
                            </div> 
                        </v-img>
                        
                    </v-card>
                </v-col>
           
              
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
import { useRoute,useRouter } from 'vue-router' 
import ComProductList from '../components/ComProductList.vue';
import ProductDetail from './ProductDetail.vue';

const gv = inject('$gv')
const frappe = inject('$frappe')
const db = frappe.db()
const call = frappe.call()

const route = useRoute();
const router = useRouter()
const category = ref({})
const all_sub_categories = ref([])
const sub_categories = ref([])
let banner = ref('')
const products = ref([])
const selected = ref({})
const open = ref(false) 

const menus = ref([]);




// init data
onMounted(async ()=>{ 
    const menu = JSON.parse(localStorage.getItem("_d"))
    //get categories
    onLoadSubCategories(menu.name, menu.is_main_emenu)

    //
    localStorage.removeItem("_m")
    menus.value =[];
})



async function onLoadSubCategories(menu,main_emenu) {
   const res =  await call.get('epos_restaurant_2023.api.emenu.get_emenu_category',{
        shortcut: menu,
        is_main_emenu: main_emenu
    })
       
    const _res = res.message.filter((r)=>r.name==route.params.category);
   
    if (_res[0].is_group ==1 ){
        all_sub_categories.value =  _res[0].child;
        sub_categories.value = _res[0].child;       
    }
    await onLoadProducts(_res[0].name);
}

async function onLoadProducts(cat){
    const doc = await  db.getDoc('POS Menu', cat )
    category.value = doc ;  
    banner.value = ((category.value.banner || category.value.background_image) || gv.setting?.template_style?.background_image_banner)
    const res = await call.get('epos_restaurant_2023.api.emenu.get_emenu_product',{
        menu: category.value.name
    })
    products.value = res.message ; 
     
}

async function onBack(){
    menus.value = JSON.parse(localStorage.getItem("_m")) 
    if((menus.value||[]).length>0 ){
        const _m =  menus.value.sort((a, b) => b.idx - a.idx);
        menus.value.splice(menus.value.indexOf(_m[0]),1); 


        const _m_current =menus.value.sort((a, b) => b.idx - a.idx) ;

        if(_m_current.length>0){
    
            const cat = all_sub_categories.value.filter((r)=>r.name==_m_current[0].name);  
            
            await onLoadProducts(cat[0].name)
            sub_categories.value = cat[0].child; 
        }else{
            const menu = JSON.parse(localStorage.getItem("_d"))
            //get categories
            onLoadSubCategories(menu.name, menu.is_main_emenu)
        }
        
        localStorage.setItem("_m",JSON.stringify(menus.value));  

    }else{
        localStorage.removeItem("_m")
        router.back()
    }
}


async function onCategoryClick(cat){   
    await onLoadProducts(cat.name)
    sub_categories.value = cat.child;    
    
    // 
    menus.value = JSON.parse(localStorage.getItem("_m"))
 
  
    if((menus.value??[]).length>0 ){
        menus.value.push({
            "name":cat.name,
            "idx":(menus.valu??[]).length + 1
        })       
    }else{
        menus.value = [];
        menus.value.push({
            "name":cat.name,
            "idx":0
        })
    }
 
    localStorage.setItem("_m",JSON.stringify(menus.value))
}

function onView($event){
    selected.value = $event
    open.value = true
}

function onCloseModal(){
    open.value = false
}


</script> 
<style scoped>
.wrap-btn-back {
    position: fixed;
    left: 0px;
    right: 0px;
    z-index: 99;
}
.btn-back {
    max-width: 750px;
    margin: 0 auto;
}
.btn-back button {
    margin: 4px;
}
</style>