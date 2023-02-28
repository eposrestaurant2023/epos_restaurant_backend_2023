<template>
     <ComModal :mobileFullscreen="true"  @onClose="onClose" :hideOkButton="true" :hideCloseButton="true">
        <template #title>
           Change Table - Select Order - {{ params?.data?.tbl_no }} - {{sale.sale.name }}
        </template>
        <template #content>
            <ComLoadingDialog v-if="isLoading" />
            <v-alert
            type="info"
            
            text="Click on existing order if you want to merge order or click on button Create New Bill to create order in the same table"
            variant="tonal"
        ></v-alert>
            <v-btn @click="onSaleOrderClick(s)" v-for="(s, index) in params.data.sales" :key="index" height="100" :color="s.sale_status_color" class="ma-2">
                
                    {{ s.name }} 
                
                <br/>

                <CurrencyFormat :value="s.grand_total" />
              
                
                
            </v-btn>
        </template>
        <template #action>
            <v-btn variant="flat"  color="primary" @click="onCreateNewBill()">Create New Bill</v-btn>
        </template>

     </ComModal>
</template>
  
<script setup>
import { ref, defineEmits, inject,useRouter,createDocumentResource  } from '@/plugin'
import { createToaster } from '@meforma/vue-toaster';
import ComLoadingDialog from '@/components/ComLoadingDialog.vue';
const sale = inject("$sale")
const router = useRouter();
const isLoading = ref(false)
 
const toaster = createToaster({ position: "top" })

const props = defineProps({
    params: {
        type: Object,
        require: true
    }
})

const emit = defineEmits(["resolve", "reject"])

function onCreateNewBill(){
    emit("resolve", {action:"create_new_bill"});
}

async function onSaleOrderClick(s){

    if(s.name == sale.sale.name){
        toaster.warning("You cannot merge order to the current order.");
        return;
    } 
    isLoading.value = true;
    const resource = createDocumentResource({
        doctype: "Sale",
        name: s.name,
    });
    await resource.get.fetch().then(async (v)=>{

        const sale_products = v.sale_products;
        sale_products.push(...sale.sale.sale_products);
        await resource.setValue.submit({sale_products:sale_products}).then((data)=>{
            console.log(data);
            router.push({
                    name: "AddSale", params: {
                        name:data.name
                    }
                });
            isLoading.value = false;
            emit("resolve", {action:"reload_sale",name:data.name})
        }); 
	})

     
}



function onClose() {
    emit("resolve", false)
}

 

 


</script>