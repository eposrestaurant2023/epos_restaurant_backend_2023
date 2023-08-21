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
	const sale = inject('$sale');
	const frappe = inject('$frappe');
	const call = frappe.call();
	const route = useRoute();
	const not_found = ref(false)
	//
	onMounted(async ()=>{   
		await call.get('epos_restaurant_2023.api.emenu.get_pos_profile',{name:(route.params?.pos_profile||"")})
		.then(async (val)=>{			
			gv.pos_profile = val.message;
			gv.table_name = route.params.table_name;
			
			//check if shift and working day started
			await call.get('epos_restaurant_2023.api.emenu.get_current_shift_information',{
				business_branch: gv.pos_profile.business_branch,
       			pos_profile: gv.pos_profile.name
			})
			.then((res)=>{		
				if(res.message.working_day==null || res.message.cashier_shift==null){					 
					not_found.value = true;
				}  
				else{
					not_found.value = false; 
					gv.cashier_shift = res.message.cashier_shift;
					gv.working_day = res.message.working_day;


					//create new sale

					console.log(sale)
				} 

			})
			.catch((err)=>{
				console.log({"Working_Day_or_Shift ":err})
				not_found.value = true; 
			})

			
		})
		.catch((er)=>{
			not_found.value = true; 
		});
	});

</script>
