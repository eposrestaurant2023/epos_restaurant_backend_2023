<template lang="">
    <v-list>
        <v-list-item :title="currentUser?.full_name" :subtitle="currentUser?.role">
            <template v-slot:prepend>
                <v-avatar :image="currentUser?.photo"  v-bind="props" v-if="currentUser?.photo" class="cursor-pointer"></v-avatar>
                <avatar :name="currentUser?.full_name || 'No Name'" v-bind="props" v-else class="cursor-pointer mr-2" size="40"></avatar>
            </template>
        </v-list-item>
    </v-list>
</template>
<script>
export default {
    name: "ComCurrentUserAvatar",
    computed: {
        currentUser(){
            return JSON.parse(localStorage.getItem('current_user'))
        }
    }
}
</script>
<style lang="">
    
</style>