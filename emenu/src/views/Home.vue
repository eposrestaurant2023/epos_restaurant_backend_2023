<template>
  <div>
    <ComSlideShow/>
    <div class="p-4">
      <ComWelcome/>
      <div class="pb-2">
        <ComShortcutMenu/>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useRoute } from 'vue-router'
import ComSlideShow from '../components/ComSlideShow.vue';
import ComWelcome from '../components/layout/components/ComWelcome.vue';
import ComShortcutMenu from '../components/layout/components/ComShortcutMenu.vue';

const route = useRoute()
 

// export default {
  // resources: {
  //   ping() {
  //     return {
  //       method: "frappe.ping", // Method to call on backend
  //       onSuccess(d) {
  //         alert(d);
  //       },
  //     };
  //   },
  // },
// };
</script>
