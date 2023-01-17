<template >
    <div v-if="menus.data">
        {{ menus.data }}
    
    </div>

</template>
<script setup>
import {createResource,computed } from "@/plugin";

const menus = createResource({
url:"epos_restaurant_2023.api.product.get_product_by_menu",
params:{
    root_menu: "Main Menu"
},
auto:true
});

const shortcutMenus = computed(() => {
  menus.data.filter(r=>r.is_shortcut_menu ==1);

  return  menus.data.filter(r=>r.is_shortcut_menu ==1);
})
</script>