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
import { useRouter, useRoute, routeLocationKey } from 'vue-router'
import MainLayout from './components/layout/MainLayout.vue';
import BlankLayout from './components/layout/BlankLayout.vue';
import SplashScreen from './components/SplashScreen.vue';
import SaleLayout from './components/layout/SaleLayout.vue';
import { PromiseDialogsWrapper } from 'vue-promise-dialogs';
import { createResource } from '@/resource.js'
import { reactive, computed, onMounted, inject } from 'vue'
import { useStore } from 'vuex'
import { createToaster } from '@meforma/vue-toaster';
import { FrappeApp } from 'frappe-js-sdk';


const toaster = createToaster({position:'top'});
const gv = inject("$gv");
const sale = inject("$sale");
const product = inject("$product");
const tableLayout = inject("$tableLayout");
const socket = inject("$socket")
const auth = inject("$auth")
const store = useStore()
const screen = inject('$screen')
let state = reactive({
	isLoading: false
})



 
socket.on("PrintReceipt", (arg) => {	
	if(localStorage.getItem("is_window")=="1"){
		const device_setting = JSON.parse(localStorage.getItem("device_setting"));
		const station_device_printing = device_setting?.station_device_printing||"";
		const data = JSON.parse(arg) ;	
		if(data.sale.pos_profile == localStorage.getItem("pos_profile") && station_device_printing == data.station_device_printing){
			window.chrome.webview.postMessage(arg);
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
			localStorage.setItem("device_setting",JSON.stringify(  doc.device_setting))
			localStorage.setItem("table_groups", JSON.stringify(doc.table_groups || null))			
			checkPromotionDay(gv.setting.business_branch)
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
						gv.cashierShift = data.cashier_shift;
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
if(current_user!=null){
	const frappe = new FrappeApp();
	const auth = frappe.auth();
	auth.getLoggedInUser().then((user) => {
		current_user = current_user ? JSON.parse(current_user) : null
		if (!current_user) {	 
			createResource({
				url: 'epos_restaurant_2023.api.api.get_user_info',
				params: {
					name: current_user?.name
				},
				cache: "get_current_login_user",
				auto: true,
				onSuccess(doc) {  
					current_user = doc
					localStorage.setItem("current_user", JSON.stringify(current_user));
				}
			})
		}
	}).catch((error) => console.error(error));	
}

function checkPromotionDay(business_branch){
	if(auth.isLoggedIn){ 
	// check promotion
	createResource({
		url: 'epos_restaurant_2023.api.promotion.check_promotion',
		// cache: "check_promotion",
		auto: true,
		params: {
			business_branch: business_branch
		},
		onSuccess(doc) { 
			gv.promotion = doc;
			sale.promotion = doc;
		}
	});
}
}

function onResize() {
	screen.onResizeHandle()
}


function onLogout() {
    auth.logout().then((r) => {
        router.push({ name: 'Login' });
    })
}
 

onMounted(() => {
	//check if NN user 
	const current_user =  localStorage.getItem('current_user');
    if(current_user==null || current_user == undefined){
        onLogout();
    }else{
		const pos_user =JSON.parse(current_user)
		if(auth.cookie.user_id != pos_user.name){
			onLogout();
		}		 
	}
	gv.device_setting  = JSON.parse(localStorage.getItem("device_setting"));	
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