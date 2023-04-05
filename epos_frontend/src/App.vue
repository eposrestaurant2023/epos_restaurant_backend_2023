<template>
	<SplashScreen v-if="state.isLoading" />
	<v-sheet v-else id="app-container" v-resize="onResize">
		<v-progress-linear class="progress_bar" v-if="isLoading" indeterminate color="teal"></v-progress-linear>
		<MainLayout v-if="isMainLayout" />
		<SaleLayout v-else-if="isSaleLayout" />
		<BlankLayout v-else />

		<PromiseDialogsWrapper />

	</v-sheet>
</template>
<script setup>
import { useRouter, useRoute } from 'vue-router'
import MainLayout from './components/layout/MainLayout.vue';
import BlankLayout from './components/layout/BlankLayout.vue';
import SplashScreen from './components/SplashScreen.vue';
import SaleLayout from './components/layout/SaleLayout.vue';
import { PromiseDialogsWrapper } from 'vue-promise-dialogs';
import { createResource } from '@/resource.js'
import { reactive, computed, onMounted, inject } from 'vue'
import { useStore } from 'vuex'
import { createToaster } from '@meforma/vue-toaster';

const toaster = createToaster({position:'top'});



const gv = inject("$gv");
const sale = inject("$sale");
const product = inject("$product");
const tableLayout = inject("$tableLayout");
const socket = inject("$socket")

const store = useStore()
const screen = inject('$screen')
let state = reactive({
	isLoading: false
})

 
socket.on("PrintReceipt", (arg) => {
	if(localStorage.getItem("is_window")=="1"){
		const data = JSON.parse(arg) ;
		if(data.sale.pos_profile == localStorage.getItem("pos_profile")){
			window.chrome.webview.postMessage(arg);
			toaster.warning("Print Receipt in Progress")
		}
	}
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
		cache: "get_system_settings",
		auto: true,
		onSuccess(doc) {
			state.isLoading = false;
			localStorage.setItem("setting", JSON.stringify(doc));
			gv.setting = doc;
			sale.setting = doc;
			product.setting = doc;
			tableLayout.setting = doc;
			tableLayout.table_groups = doc.table_groups || '';
			localStorage.setItem("table_groups", JSON.stringify(doc.table_groups || null))
			let current_user = localStorage.getItem("current_user");
			if (current_user) {
				createResource({
					url: "epos_restaurant_2023.api.api.get_current_shift_information",
					params: {
						business_branch: gv.setting?.business_branch,
						pos_profile: localStorage.getItem("pos_profile")
					},
					onSuccess(data) {
						gv.workingDay = data.wroking_day;
						gv.cashier_shift = data.cashier_shift;
					},
					auto: true,
				})
			}  
		},
		onError(x) {
			if (x.error_text == undefined) {
				//localStorage.removeItem("pos_profile")
			} else {
				if (x.error_text[0] === 'Invalid POS Profile name') {
					localStorage.removeItem("pos_profile")
				}
				else if(x.error_text[0] === 'Internal Server Error'){	
					//router.push({ name: 'ServerError' })
					state.isLoading = false;
				}
				else{
					toaster.error(JSON.stringify(x))
				}
			}
			state.isLoading = false;

		}
	});

}

//get user info 
let current_user = localStorage.getItem('current_user')
if (current_user) {
	current_user = JSON.parse(current_user)
	createResource({
		url: 'epos_restaurant_2023.api.api.get_user_info',
		params: {
			name: current_user.name
		},
		cache: "get_current_login_user",
		auto: true,
		onSuccess(doc) { 
			current_user.permission = doc.permission;
			current_user.full_name = doc.full_name;
			localStorage.setItem("current_user", JSON.stringify(current_user));

		}
	})
}else{
	router.push({name:'Login'})
}


function onResize() {
	screen.onResizeHandle()
}
onMounted(() => {
	onResize()
})
</script>
<style>
.progress_bar {
	position: absolute !important;
	z-index: 9999 !important;
}

/* width */
::-webkit-scrollbar {
	width: 5px;
	height: 5px;
}

/* Track */
::-webkit-scrollbar-track {
	box-shadow: inset 0 0 5px rgb(206, 206, 206);
	border-radius: 10px;
}

/* Handle */
::-webkit-scrollbar-thumb {
	background: rgb(165, 165, 165);
	border-radius: 10px;
}
</style>