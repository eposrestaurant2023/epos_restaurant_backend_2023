<template>
	<!-- <button v-if="$auth.isLoggedIn" @click="$auth.logout()">Logout</button> -->
	<MainLayout>
		<router-view />
	</MainLayout>
</template>


<script>
import MainLayout from './components/layout/MainLayout.vue';

export default {
    inject: ["$auth"],
    components: { MainLayout }
};
</script>
