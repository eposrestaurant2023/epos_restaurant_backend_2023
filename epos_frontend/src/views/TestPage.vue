<template>
{{ all_menus}}
</template>
<script setup>
  import { inject,ref } from '@/plugin';
  const product = inject("$product");
  alert(product.currentRootPOSMenu)
</script>