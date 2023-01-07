<template>
  <div class="container">
    <v-sheet
      :elevation="2"
      class="mx-auto"
      height="100"
      width="100"
      @click="onPOS"
      >
      POS
    </v-sheet>
  </div>
</template>

<script>
export default {
  data() {
    return {
      i: ''
    }
  },
  methods: {
    onPOS(){
      this.$router.push({ path: "/add-sale" });
    }
  },
};
</script>
