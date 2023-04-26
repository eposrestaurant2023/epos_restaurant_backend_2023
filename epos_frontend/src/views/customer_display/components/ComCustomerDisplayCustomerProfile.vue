<template>
    <div class="p-1 border border-gray-600 rounded-md">
        <div class="flex">
            <div class="flex-auto">
                <div class="flex">
                    <v-avatar v-if="data.customer_photo">
                        <v-img :src="data.customer_photo"></v-img>
                    </v-avatar>
                    <template v-if="data.customer_name != undefined">
                        <avatar v-if="data.customer_photo == undefined" :name="data.customer_name" class="mr-4" size="40">
                        </avatar>
                    </template>
                    <div class="px-2">
                        <div class="font-bold">{{ data.customer_name }}</div>
                        <div class="text-gray-400 text-sm">{{ subTitle }}</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script setup>
import { computed } from "@/plugin"
const props = defineProps({
    data: Object,
})
const subTitle = computed(() => {
    let title = props.data.customer;
    if (props.data.phone_number != "" && props.data.phone_number != undefined) {
        title = title + " / " + props.data.phone_number
    }

    return title;
})


</script>