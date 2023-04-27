<template>
    <div class="p-1 border border-gray-600 rounded-md">
        <div class="flex">
            <div class="flex-auto">
                <div class="flex">
                    <template v-if="data.customer_name === undefined">
                        <v-avatar v-if="gv.setting.customer_photo">
                            <v-img :src="gv.setting.customer_photo"></v-img>
                        </v-avatar>
                        <template v-else>
                            <avatar v-if="gv.setting.customer_name" :name="gv.setting.customer_name" class="mr-4" size="40">
                            </avatar>
                        </template>
                        <div class="px-2">
                            <div class="font-bold">{{ gv.setting.customer_name }}</div>
                            <div class="text-gray-400 text-sm">{{ gv.setting.customer }}</div>
                        </div>
                    </template>
                    <template v-else>
                        <v-avatar v-if="data.customer_photo">
                            <v-img :src="data.customer_photo"></v-img>
                        </v-avatar>
                        <template v-else>
                            <avatar v-if="data.customer_name" :name="data.customer_name" class="mr-4" size="40">
                            </avatar>
                        </template>
                        <div class="px-2">
                            <div class="font-bold">{{ data.customer_name }}</div>
                            <div class="text-gray-400 text-sm">{{ subTitle }}</div>
                        </div>
                    </template>
                </div>
            </div>
        </div>
    </div>
</template>
<script setup>
import { computed, inject } from "@/plugin"
const gv = inject('$gv')
const props = defineProps({
    data: Object,
})
const subTitle = computed(() => {
    let title = props.data.customer;
    if (props.data.phone_number != "" && props.data.phone_number) {
        title = title + " / " + props.data.phone_number
    }

    return title;
})



</script>