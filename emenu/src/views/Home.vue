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
import {inject,ref} from 'vue'
import ComSlideShow from '../components/ComSlideShow.vue';
import ComWelcome from '../components/layout/components/ComWelcome.vue';
import ComShortcutMenu from '../components/layout/components/ComShortcutMenu.vue';
import ComCategoryCard from '../components/layout/components/ComCategoryCard.vue';
const frappe = inject('$frappe')
const call = frappe.call()
let categories = ref([])

async function onSelected(shortcut){
  await call.get('epos_restaurant_2023.api.api.get_emenu_category',{
    shortcut: shortcut.name,
    is_main_emenu: shortcut.is_main_emenu
  })
  .then((r)=> {
    categories.value = r.message 
  }).catch(er=> console.log(er))
}
 
</script>
