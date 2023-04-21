<template>
	<MainLayout>
		<router-view />
	</MainLayout>
	
</template>
<script setup>
	// import {inject} from 'vue'
	// import {useDisplay} from 'vuetify'
	import MainLayout from './components/layout/MainLayout.vue'
	// const {mobile} = useDisplay()
	// const frappe = inject('$frappe')
	// const gv = inject('$gv')
	// const call = frappe.call()
	// call.get('epos_restaurant_2023.api.api.get_emenu_settings')
	// .then((r)=> {
	// 	gv.setting = r.message
	// }).catch(er=> console.log(er))

</script>
