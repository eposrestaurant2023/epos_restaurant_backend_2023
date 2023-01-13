<template>
    <img :src="setting.home_background" />
    <img :src="setting.logo" />
    <h1>{{ setting.business_branch }}</h1>
    <h1>{{ setting.phone_number }}</h1>
    <h1>{{ setting.address }}</h1>
    <h1>{{ setting.outlet }}</h1>

    <v-btn v-if="current_working_day.data"  @click="onRoute('OpenShift')">
        Close Working Day
    </v-btn>
    <v-btn v-else  @click="onRoute('OpenShift')">
        Start Working Day
    </v-btn>


    <v-btn variant="tonal" @click="onRoute('ReceiptList')">
        Receipt List
    </v-btn>
    <v-btn @click="onRoute('Table')">
        POS
    </v-btn>
    

</template>
<script setup>
    import { useRouter,createResource, createDocumentResource } from '@/plugin'
    let setting = JSON.parse(localStorage.getItem("setting"))
    const current_working_day = createResource({
        url:"epos_restaurant_2023.api.api.get_current_working_day",
        params:{
            pos_profile: localStorage.getItem("pos_profile")
        },
        auto:true
    })
     

    const router = useRouter()
    function onRoute(page) {
        router.push({ name: page })
    }


</script>


