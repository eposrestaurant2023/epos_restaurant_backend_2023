<template>
	<SplashScreen v-if="this.isLoading"/>
	<div v-else>
		<v-progress-linear class="progress_bar" v-if="this.$store.state.isLoading" indeterminate
			color="teal"></v-progress-linear>
		<MainLayout v-if="this.$route.meta.layout">
			<router-view />
		</MainLayout>
		<BlankLayout v-else>
			<router-view />
		</BlankLayout>
	</div>
</template>
<script>
import { useRoute } from 'vue-router'
import MainLayout from './components/layout/MainLayout.vue';
import BlankLayout from './components/layout/BlankLayout.vue';
import SplashScreen from './components/SplashScreen.vue';
export default {
	data() {
		return {
			isLoading: true,
			pos_profile : ""
		};
	},
	inject: ["$auth","$call"],
	components: { MainLayout, BlankLayout,SplashScreen },
	async created(){
		const route = useRoute()
		if(route.query.pos_profile!==undefined){
			this.pos_profile = route.query.pos_profile
		}
		try{ 
			let res = await this.$call("epos_restaurant_2023.api.api.get_system_settings")
		
			this.$store.commit("ePOSSettings", res);
		}catch(error){
			this.$toast.error(error);
		}
		this.isLoading = false;
	},
	mounted() {
		
	}
};
</script>
<style>
.progress_bar {
	position: absolute !important;
}
</style>