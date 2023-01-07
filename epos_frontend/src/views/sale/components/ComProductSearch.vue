<template>
    <div class="text-center">
        <input v-model="keyword" placeholder="Search Product"/>
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