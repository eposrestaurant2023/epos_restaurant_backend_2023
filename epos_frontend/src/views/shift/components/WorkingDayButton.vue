<template>
    
    <div v-if="!current_working_day.loading">
     <ComButton icon-class="text-white" class="bg-red-600 text-gray-100" v-if="current_working_day.data" title="Close Working Day" icon="mdi-calendar-clock" @click="onCloseWorkingDay" />
     <ComButton   v-else title="Start Working Day"  icon="mdi-calendar-clock" @click="onStartWorkingDay" />

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