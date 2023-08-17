<template>
	<template v-if="!not_found">
	<MainLayout>
		<router-view />
	</MainLayout>
</template>
<template v-else>
	<NoFoundLayout>
	 
	</NoFoundLayout>
</template>
	
</template>
<script setup>
	import {inject,ref, onMounted } from 'vue';
	import MainLayout from './layout/MainLayout.vue'
	import { useRoute,useRouter } from 'vue-router'; 
	import NoFoundLayout from './layout/NoFoundLayout.vue';
	const gv = inject('$gv');
	const frappe = inject('$frappe');
	const call = frappe.call();
	const route = useRoute();
	const not_found = ref(false)
	//
	onMounted(async ()=>{   
		await call.get('epos_restaurant_2023.api.emenu.get_pos_profile',{name:(route.params?.pos_profile||"")})
		.then((val)=>{
			not_found.value = false; 
			gv.pos_profile = val.message;
			gv.table_name = route.params.table_name
		})
		.catch((er)=>{
			not_found.value = true; 
		});
	});

</script>
