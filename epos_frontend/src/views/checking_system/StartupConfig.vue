<template lang="">
    <ComToolbar :isClose="false">
        <template #title>
            POS Configuration
        </template>
    </ComToolbar>
    
    <v-container>
        <v-card
            class="mx-auto mt-12"
            color="grey-lighten-3"
            max-width="400"
        >
            <v-card-title>
                <div class="text-center p-4">
                    ePOS System
                </div>
            </v-card-title>
            <v-card-text>

                <form @submit.prevent="onSave()">
                    <v-text-field
                        class="mb-2"
                        density="compact"
                        variant="solo"
                        label="Device Name"
                        append-inner-icon="mdi-cellphone-link"
                        single-line
                        hide-details
                        v-model="state.device_name"
                    ></v-text-field>
                    <v-text-field
                        class="mb-6"
                        density="compact"
                        variant="solo"
                        label="POS Profile"
                        append-inner-icon="mdi-account"
                        single-line
                        hide-details
                        v-model="state.pos_profile"
                    ></v-text-field>
                    <div class="text-right">
                        <v-btn type="sumbit" class="w-full" color="primary" :loading="store.state.isLoading">Save</v-btn>
                    </div>
                </form>
            </v-card-text>
        </v-card>
    </v-container>
</template>
<script setup>
    import {reactive, createResource, createToaster, useStore, inject} from '@/plugin'

    import ComToolbar from '@/components/ComToolbar.vue';
    const auth = inject('$auth')
    const toast = createToaster()
    const store = useStore()
    const state = reactive({
        valid: true,
        device_name: '',
        pos_profile: '',
        loading: true
    })
    if(auth.isLoggedIn){
        alert('logout')
        auth.logout()
    }
    function onSave() {
        if(!state.device_name || !state.pos_profile)
        {
            toast.warning('Invalid field',{ position: 'top'});
            return
        }
        store.dispatch('startLoading')
        createResource({
            url: 'epos_restaurant_2023.api.api.check_pos_profile',
            params:{
                pos_profile_name:state.pos_profile,
            },
            auto:true,
            onSuccess(){
                localStorage.setItem('device_name',state.device_name)
                localStorage.setItem('pos_profile',state.pos_profile)
                location.reload();
            },
            onError(){
                store.dispatch('endLoading')
            }
        })
    }

    </script>