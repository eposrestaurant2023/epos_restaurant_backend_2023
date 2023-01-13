<template lang="">
    <v-list>
        <v-list-item :prepend-avatar="currentUser.photo"
            :title="currentUser.full_name" :subtitle="currentUser.role">
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