<template>
    <div class="text-center px-6">
        <v-text-field 
            v-model="keyword"
            variant="outlined"
            class="my-0 mx-auto max-w-md"
            single-line
            hide-details
            prepend-inner-icon="mdi-magnify"
            label="Search..."
            ></v-text-field>
    </div>
</template>
<script>
export default {
    name: "ComProductSearch",
    data() {
        return {
            keyword: ''
        }
    },
}
</script>
<style lang="">
    
</style>