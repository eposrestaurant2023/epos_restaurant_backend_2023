<template>
  <div>
    <ComSlideShow/>
    <div class="p-4">
      <ComWelcome/>
      <div class="pb-2">
        <ComShortcutMenu @on-selected="onSelected($event)"/>
      </div>
      <div> 
          <ComCategoryCard :categories="categories"/> 
      </div>
    </div>
  </div>
</template>

<script setup>
import {inject,ref,onMounted} from 'vue';
import { useRoute,useRouter } from 'vue-router'; 
import ComSlideShow from '../components/ComSlideShow.vue';
import ComWelcome from './components/ComWelcome.vue';
import ComShortcutMenu from './components/ComShortcutMenu.vue';
import ComCategoryCard from './category/components/ComCategoryCard.vue';

const route = useRoute();
const router = useRouter();
const gv = inject('$gv');
const frappe = inject('$frappe');

const call = frappe.call();
let categories = ref([]);
 

//
onMounted(async ()=>{   
  //  console.log(gv)
});


//
async function onSelected(shortcut){
  localStorage.setItem("_d",JSON.stringify({"name":shortcut.name,"is_main_emenu":shortcut.is_main_emenu}))
  await call.get('epos_restaurant_2023.api.emenu.get_emenu_menu',{
    shortcut: shortcut.name,
    is_main_emenu: shortcut.is_main_emenu
  })
  .then((r)=> { 
    categories.value = r.message 
  }).catch(er=> console.log(er))
}
 
</script>
