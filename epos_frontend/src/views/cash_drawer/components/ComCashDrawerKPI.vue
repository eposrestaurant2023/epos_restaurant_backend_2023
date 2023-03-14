<template>
    <div>
        <v-card variant="tonal" :color="backgroundColor" class="!h-full">
            <v-card-text>
                <div class="text-gray-600 mb-2">{{ title }}</div>
                <div class="text-lg">
                    <CurrencyFormat :value="value || 0"/>
                </div>
            </v-card-text>
        </v-card>
    </div>
</template>
<script setup>
    import {defineProps, computed} from 'vue'
    const props = defineProps({
        value: Number,
        title: String,
        backgroundColor: String
    })
</script>
<style lang="">
    
</style>