<template>
	<SplashScreen v-if="state.isLoading" />
	<div v-else>
		<v-progress-linear class="progress_bar" v-if="isLoading" indeterminate color="teal"></v-progress-linear>
		<MainLayout v-if="isMainLayout" />
		<SaleLayout v-else-if="isSaleLayout" />
		<BlankLayout v-else />

		<DialogWrapper />

		<PromiseDialogsWrapper/>

	</div>
</template>
<script setup>
import { useRouter, useRoute } from 'vue-router'
import MainLayout from './components/layout/MainLayout.vue';
import BlankLayout from './components/layout/BlankLayout.vue';
import SplashScreen from './components/SplashScreen.vue';
import SaleLayout from './components/layout/SaleLayout.vue';
import { DialogWrapper } from 'vue3-promise-dialog';
 
import { PromiseDialogsWrapper } from 'vue-promise-dialogs';

import { createResource } from '@/resource.js'
import { reactive, computed, onMounted,inject } from 'vue'
import { useStore } from 'vuex'
 
const gv = inject("$gv");
const store = useStore()

let state = reactive({
	isLoading: false
})

const router = useRouter()
const route = useRoute()
const isMainLayout = computed(() => {
	return route.meta.layout == "main_layout"
})
const isSaleLayout = computed(() => {
	return route.meta.layout == "sale_layout"
})
const isLoading = computed(() => {
	return store.state.isLoading
})

if (!localStorage.getItem("pos_profile")) {
	state.isLoading = false
	router.push({ name: 'StartupConfig' })
} else {

	state.isLoading = true;
	createResource({
		url: 'epos_restaurant_2023.api.api.get_system_settings',
		params: {
			pos_profile: localStorage.getItem("pos_profile"),
			device_name: localStorage.getItem("device_name")
		},
		auto: true,
		onSuccess(doc) {

			state.isLoading = false;
			localStorage.setItem("setting", JSON.stringify(doc));
			gv.setting = doc;
			localStorage.setItem("table_groups", JSON.stringify(doc.table_groups))

		},
		onError(x) {

			if (x.error_text == undefined) {
				localStorage.removeItem("pos_profile")
				location.reload()
			} else {
				if (x.error_text[0] === 'Invalid POS Profile name') {
					localStorage.removeItem("pos_profile")
					location.reload()
				}
			}
			state.isLoading = false;

		}

	});

}



</script>
<style>
.progress_bar {
	position: absolute !important;
	z-index: 9999 !important;
}
</style>