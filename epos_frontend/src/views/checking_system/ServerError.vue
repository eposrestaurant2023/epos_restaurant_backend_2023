<template lang="">
    <div>
        <div class="bg-gray-100 font-sans leading-normal tracking-normal" v-if="!isLoading">
            <div class="h-screen flex flex-col items-center justify-center p-3 text-center">
                <h1 class="text-5xl font-bold text-gray-900">500</h1>
                <h2 class="text-xl font-semibold text-gray-600">Internal Server Error</h2>
                <p class="text-gray-500 py-3">Oops! Something went wrong. Please contact to our system administartor.</p>
                <button class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-full" @click="onRefresh()">{{$t('Try again')}}</button>
            </div>
        </div>        
    </div>
</template>
<script setup>
    import {onMounted, useRouter, createToaster, createResource, inject,computed ,i18n} from '@/plugin'
    import { useStore } from 'vuex';
    const { t: $t } = i18n.global; 

    const store = useStore()
    const auth = inject('$auth')
    const router = useRouter()
    const toaster = createToaster({position: 'top'})
    const isLoading = computed(() => {
        return store.state.isLoading
    })
    function onRefresh(){
        window.location.reload()
    }
    onMounted(() => {
        store.state.isLoading = true
        createResource({
            url: 'epos_restaurant_2023.api.api.get_user_information',
            auto: true,
            async onSuccess(doc) {
                if(doc){
                    const user = JSON.parse(localStorage.getItem('current_user'))
                    if(user.name == doc.name){
                        router.push({ name: "Home" });
                    }else{
                        auth.logout().then(() => {
                            onRefresh()
                        })
                    }
                }
                
                store.state.isLoading  = false
            },
            onError(x) {
                toaster.error($t('msg.Please contact to our system administartor'))
                store.state.isLoading  = false
            }
        }) 
    })
</script>
<style lang="">
    
</style>