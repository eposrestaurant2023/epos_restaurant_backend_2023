<template>
    
    <div v-if="!current_working_day.loading">
     <ComButton icon-class="text-white" class="bg-red-600 text-gray-100" v-if="current_working_day.data" title="Close Working Day" icon="mdi-calendar-clock" @click="onCloseWorkingDay" />
     <ComButton v-else title="Start Working Day"  icon="mdi-calendar-clock" @click="onStartWorkingDay" />
    </div>
    <div v-else class="shadow-md text-center bg-white p-4 rounded-md cursor-pointer flex items-center justify-center h-full">
        <div>
            <v-icon class="m-2" icon="mdi-spin mdi-loading" size="x-large"></v-icon>
            <div class="text-gray-400">loading</div>
        </div>
    </div>
</template>
<script setup>
    import ComButton from '../../../components/ComButton.vue';
    import {createResource,useRouter,inject } from "@/plugin"

    const gv = inject("$gv")

    
    const router = useRouter();
    
    const current_working_day = createResource({
        url: "epos_restaurant_2023.api.api.get_current_working_day",
        params: {
            business_branch: gv.setting.business_branch
        },
        auto: true,
        
    })

    function onStartWorkingDay(){
        router.push({name:"StartWorkingDay"});
    }
    function onCloseWorkingDay(){
        router.push({name:"CloseWorkingDay"});
    }

</script>