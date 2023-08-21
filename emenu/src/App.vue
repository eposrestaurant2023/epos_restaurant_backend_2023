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

			if (gv.allow_make_order ==1){
				
				//check if shift and working day started
				await call.get('epos_restaurant_2023.api.emenu.get_current_shift_information',{
					business_branch: gv.pos_profile.business_branch,
					pos_profile: gv.pos_profile.name,
					customer:gv.pos_profile.default_customer,
				})
				.then(async (res)=>{  
					const _cus = res.message.default_customer; 
					if(res.message.working_day==null || res.message.cashier_shift==null){					 
						not_found.value = true;
					}  
					else{
						let table_groups = "";
						(gv.pos_profile.table_groups??[]).forEach((tg)=>{
							table_groups += `'${tg.table_group}',`
						})
						if(table_groups!=""){
							table_groups = table_groups.substring(0,table_groups.length -1) 
						}

				
						not_found.value = false;  
						await call.get('epos_restaurant_2023.api.emenu.get_pos_table',{name:gv.table_name,groups:table_groups})
						.then((t)=>{
							const table = t.message; 
							sale.sale_type = gv.pos_profile.default_sale_type;
							sale.table_id = table.name;
							sale.tbl_number = table.tbl_number;
						}).catch(er=>console.log(er))
						

						

						// default_customer = frappe.get_doc("Customer", profile.default_customer)
						sale.setting = gv;
						sale.cashier_shift = res.message.cashier_shift;
						sale.working_day = res.message.working_day;
						sale.default_customer = _cus


						//create new sale
						console.log(sale)
					} 

				})
				.catch((err)=>{
					console.log({"_working_day_or_shift ":err})
					not_found.value = true; 
				}) 
			}
			else{
				not_found.value = false; 
			}
		})
		.catch((er)=>{
			not_found.value = true; 
		});
	});

</script>
