<template>
    <div class="py-2">
        <div>
            <div class="px-2 pt-2 text-lg font-bold">Portion</div>
            <div class="text-xs mt-1 pl-2">Please select one.</div>
            <v-list lines="one"> 
                <v-list-item class="!pl-0" v-for="(p, index) in portions" :key="index" :value="p">
                    <template v-slot:prepend="{ isActive }">
                        <v-list-item-action start>
                            <v-radio color="primary" :model-value="isActive"></v-radio>
                        </v-list-item-action>
                    </template>
                    <v-list-item-title>
                        <div class="flex justify-between">
                            <div>{{ p.portion }}</div>
                            <div>{{ p.price }}</div>
                        </div>
                    </v-list-item-title>
                </v-list-item>
            </v-list>
        </div>
    </div>
</template>
<script setup>
const props = defineProps({
    portions: Array
})
</script>
<style lang="">
    
</style>