<template>
    <div class="flex items-center">
        <v-menu v-if="mobile">
            <template v-slot:activator="{props}">
                <v-chip color="warning" v-bind="props" prepend-icon="mdi-printer" variant="elevated">{{ selected }}</v-chip>
            </template>
            <v-list>
                <v-list-item @click="onSelectedReceipt(item)" v-for="(item, index) in printFormatResource.data" :key="index">
                    {{ item.name }}
                </v-list-item>
            </v-list>
        </v-menu>
        <div v-else class="mx-2" v-if="printFormatResource.data?.length > 1">
            <v-chip :color="item.name == selected?'warning':''" class="m-1" @click="onSelectedReceipt(item)"
                v-for="(item, index) in printFormatResource.data" :key="index">{{ item.name }}
            </v-chip>
        </div>
    </div>
</template>
<script setup>
import { defineEmits,createResource, defineProps } from '@/plugin'
import { useDisplay } from 'vuetify'
const {mobile} = useDisplay()
const emit = defineEmits('onClick')
const props = defineProps({
    selected: String
})
const printFormatResource = createResource({
    url: "epos_restaurant_2023.api.api.get_pos_print_format",
    params: {
        doctype: "Sale"
    },
    cache: ["print_format", "Sale"],
    auto: true,
})
function onSelectedReceipt(name){
    emit('onClick', name)
}
</script>
<style lang="">
    
</style>