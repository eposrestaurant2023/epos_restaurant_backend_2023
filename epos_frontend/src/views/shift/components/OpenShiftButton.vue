<template>
    <div v-if="!current_open_shift.loading">
     <ComButton icon-class="text-white" class="bg-red-600 text-gray-100" v-if="current_open_shift.data" title="Close Shift" icon="mdi-calendar-clock" @click="onCloseShift" />
     <ComButton   v-else title="Open Shift"  icon="mdi-calendar-clock" @click="onOpenShift" />
    </div>
    <div v-else class="shadow-md text-center p-4 rounded-md cursor-pointer flex items-center justify-center h-full bg-white">
        <div>
            <v-icon class="m-2" icon="mdi-spin mdi-loading" size="x-large"></v-icon>
            <div class="text-gray-400">loading</div>
        </div>
    </div>
</template>
<script setup>

    import ComButton from '../../../components/ComButton.vue';
    import {createResource,useRouter } from "@/plugin"
    
    const router = useRouter();
    
    const current_open_shift = createResource({
        url: "epos_restaurant_2023.api.api.get_current_cashier_shift",
        params: {
            pos_profile: localStorage.getItem("pos_profile")
        },
        auto: true,
        
    })

    function onOpenShift(){
        router.push({name:"OpenShift"});
    }
    function onCloseShift(){
        router.push({name:"CloseShift"});
    }

</script>