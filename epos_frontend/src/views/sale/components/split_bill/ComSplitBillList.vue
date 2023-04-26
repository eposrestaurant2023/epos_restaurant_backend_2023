<template lang=""> 
     <div class="grid gap-2 sm:grid-cols-2 md:grid-cols-2 lg:grid-cols-2 xl:grid-cols-4">
        <v-card v-for="(g, index) in data" :key="index">
            <v-card-title class="!p-0">
                <v-toolbar height="55">
                    <v-toolbar-title class="text">
                        <div class="text-lg">No: {{ g.no }}</div>
                        <div class="text-sm">#{{g.sale.name || "New"}}</div>
                       
                    </v-toolbar-title>
                    <template v-slot:append>  
                        <v-btn v-if="g.show_download" @click="onDownloadPressed(g)" style="height:35px; width:35px; margin-right:5px" variant="outlined" color="primary" icon="mdi-download-circle-outline">
             
                        </v-btn>
                        <v-btn v-if="(!g.is_current)" style="height:35px; width:35px; margin-left:5px" variant="outlined" color="error" icon="mdi-delete-outline">
                            
                        </v-btn> 
                    </template>
                </v-toolbar>
            </v-card-title>
            <v-card-text class="!pt-0 !pr-0 !pb-14 !pl-0">       
                <v-list>
                    <v-list-item v-for="(sp, _index) in g.sale.sale_products" :key="_index">                       
                            <div class="text-sm" style="margin-bottom:10px;">
                                <div class="flex">
                                    <div class="grow">
                                        <div> {{ sp.product_name }}<v-chip class="ml-1" size="x-small"
                                                color="error" variant="outlined" v-if="sp.portion">{{ sp.portion }}</v-chip> <v-chip
                                                v-if="sp.is_free" size="x-small" color="success" variant="outlined">Free</v-chip>
                                        </div> 
                                        <div class="text-xs pt-1"> 
                                            <div v-if="sp.modifiers">
                                                <span>{{ sp.modifiers }} (<CurrencyFormat :value="sp.modifiers_price * sp.quantity" />)
                                                </span>
                                            </div>
                                            <div class="text-red-500" v-if="sp.discount > 0">
                                                Discount :
                                                <span v-if="sp.discount_type == 'Percent'">{{
                                                    sp.discount
                                                }}%</span>
                                                <CurrencyFormat v-else :value="parseFloat(sp.discount)" />
                                            </div>
                                            <v-chip color="blue" size="x-small" v-if="sp.seat_number">Seat# {{
                                                sp.seat_number
                                            }}</v-chip>
                                            <div class="text-gray-500" v-if="sp.note">
                                                Note: <span>{{ sp.note }}</span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="flex-none text-right w-36">
                                        <div class="text-sm"> 
                                            {{ sp.quantity }} x <CurrencyFormat :value="sp.price" /> 
                                        </div> 
                                        <v-btn color="success"  style="height:35px; margin-right:5px" variant="outlined" @click="onSelected(sp)">
                                            <span>Selected
                                                <template v-if="((sp.total_selected || 0)>0)">
                                                    ({{sp.total_selected}})
                                                </template>
                                            </span>
                                        </v-btn>
                                    </div>
                                </div>
                            </div>           
                            <hr/>          
                    </v-list-item> 
                </v-list>
            </v-card-text>
            <v-card-actions class="pt-0 flex items-center justify-between absolute bottom-0 w-full bg-gray-400">
                <!-- <ComSplitBillSaleSummary :sale="g.sale"/> -->
                {{g.generate_id}}
            </v-card-actions>
        </v-card>
        {{ showDownload()}}
    </div>
</template>
<script setup> 
import ComSplitBillSaleSummary from './ComSplitBillSaleSummary.vue';
const props = defineProps({
    data: Object
}) 

function onSelected(sp){
    if((sp.total_selected || 0) >= sp.quantity){
        sp.total_selected = 0;
    }else{
        sp.total_selected = (sp.total_selected || 0) + 1;
    }

    showDownload();
}


function  showDownload(){
    const _sale_products = props.data.flatMap(a => (a.sale.sale_products||[]).filter((r)=>(r.total_selected||0) >0) ); 
    if(_sale_products.length > 0){ 
        props.data.forEach((g)=>{
            g.show_download = true;
            const chk = (g.sale.sale_products||[]).filter((r)=>(r.total_selected||0) > 0);
            if(chk.length > 0){
                g.show_download = false;
            }
        });
    }
    else{
        props.data.forEach((g)=>{
            g.show_download = false;
        });
    } 
}

function onDownloadPressed(group){
 const result = props.data.flatMap(a => (a.sale.sale_products||[]).filter((r)=>(r.total_selected||0) >0) )
    result.forEach((sp)=>{
        sp.quantity -=  sp.total_selected;        
        const _sp = JSON.parse(JSON.stringify(sp));
        _sp.quantity =  sp.total_selected ;
        _sp.total_selected  =sp.total_selected = 0;
        group.sale.sale_products.push(_sp)
    }) 
}


</script>
<style lang="">
    
</style>