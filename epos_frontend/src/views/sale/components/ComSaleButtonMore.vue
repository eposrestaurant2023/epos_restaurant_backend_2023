<template> 
    <v-menu>
        <template v-slot:activator="{ props }">
            <div class="cursor-pointer p-2 grow bg-gray-400 hover:bg-gray-500 text-white" v-bind="props">
                More
            </div>
        </template>
        <v-list>
            <ComSaleButtonMoreList/>
        </v-list>
    </v-menu>       
</template>
<script setup> 
    import ComSaleButtonMoreList from "./ComSaleButtonMoreList.vue";
</script>