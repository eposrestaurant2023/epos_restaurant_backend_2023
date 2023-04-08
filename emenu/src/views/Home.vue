<template>
  <div>
    <ComSlideShow/>
  </div>
</template>

<script setup>
import { useRoute } from 'vue-router'
import ComSlideShow from '../components/ComSlideShow.vue';

const route = useRoute()
if(route.params.branch){
  alert(route.params.branch)
}

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
