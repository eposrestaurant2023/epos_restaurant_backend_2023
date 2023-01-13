<template>
    <PageLayout title="Table Layout" full icon="mdi-cart">
        <v-tabs v-model="tab" align-tabs="title">
            <v-tab v-for="item in items" :key="item" :value="item">
                {{ item }}
            </v-tab>
        </v-tabs>
        <v-window v-model="tab">
            <v-window-item v-for="item in items" :key="item" :value="item">
                <v-card flat>
                    <v-card-text v-text="text"></v-card-text>
                </v-card>
            </v-window-item>
        </v-window>
    </PageLayout>
</template>

<script setup>
import PageLayout from '../../components/layout/PageLayout.vue';
import { createResource, createToaster, useRouter, reactive, ref } from "@/plugin"
let tab = reactive(null);
const items = reactive(['web', 'shopping', 'videos', 'images', 'news']);
const text = ref('Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.');

const toaster = createToaster();
const router = useRouter()
const working_day = createResource({
    url: "epos_restaurant_2023.api.api.get_current_working_day",
    params: {
        pos_profile: localStorage.getItem("pos_profile")
    },
    auto: true,
    onSuccess(d) {


        if (d == undefined) {
            toaster.error("Please start working day first", { position: "top" });
            router.push({ name: "StartWorkingDay" })
        }

    }
})

</script> 