<template>
    <PageLayout :title="`${$t('Sale Order')}: ${route.params.sale_type}`" icon="mdi-cart" full>
        <template #centerCotent>
            <v-tabs align-tabs="center"  v-model="selected" v-if="gv.setting?.pos_setting?.sale_types && gv.setting?.pos_setting?.sale_types.filter(r=>r.is_order_use_table == false).length > 0 && !mobile" @update:modelValue="onSelected">
                <v-tab v-for="st in gv.setting?.pos_setting?.sale_types.filter(r=>r.is_order_use_table == false)" :key="st.name" :value="st.name">
                    {{ st.name }}
                </v-tab> 
            </v-tabs>
            <v-bottom-navigation align-tabs="center"  v-if="gv.setting?.pos_setting?.sale_types && gv.setting?.pos_setting?.sale_types.filter(r=>r.is_order_use_table == false).length > 0 && mobile">
                <v-tabs height="100%"  center-active  v-model="selected" @update:modelValue="onSelected">
                    <v-tab v-for="st in gv.setting?.pos_setting?.sale_types.filter(r=>r.is_order_use_table == false)" :key="st.name" :value="st.name" :disabled="selected == st.name">
                        {{ st.name }}
                    </v-tab>
                </v-tabs>
            </v-bottom-navigation> 
        </template>
        <template #action>
            <v-btn color="info" variant="tonal" icon="mdi-view-dashboard" type="button" @click="onTableLayout"></v-btn>
            <v-btn color="error" variant="tonal" prepend-icon="mdi-cart" type="button" @click="newSale">
                <span>{{!mobile? $t('New Order') : $t('New') }}</span>
            </v-btn>
        </template>
        <template #default> 
            <ComPlaceholder :is-not-empty="saleResource.data?.length > 0" :loading="saleResource.loading">
                <div class="pb-2">
                    <div class="grid gap-2 sm:grid-cols-2 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
                        <ComSaleCardItem :data="saleResource.data" @onOpenOrder="onOpenOrder" @onViewSaleOrder="onViewSaleOrder"/> 
                    </div>
                </div>
            </ComPlaceholder>
        </template>
    </PageLayout>
</template>
<script setup>
import PageLayout from '../../components/layout/PageLayout.vue';
import { useRouter, useRoute, createResource, ref, inject, createToaster,onMounted,smallViewSaleProductListModal,i18n } from "@/plugin"
import { saleDetailDialog } from "@/utils/dialog";
import ComPlaceholder from "@/components/layout/components/ComPlaceholder.vue";
import ComSaleCardItem from './components/ComSaleCardItem.vue';
import { useDisplay } from 'vuetify';
const { t: $t } = i18n.global; 

const {mobile} = useDisplay()
const router = useRouter();
const route = useRoute();
const emit = defineEmits(["resolve"])
const gv = inject('$gv')
const sale = inject('$sale')
const toaster = createToaster({ position: "top" })
const posProfile = localStorage.getItem('pos_profile')
const props = defineProps({
    params: {
        type: Object,
        required: true,
    }
})
let selected = ref(route.params.sale_type)
let filter = ref({
    working_day: null,
    cashier_shift: null
})
const workingDayResource = createResource({
    url: "epos_restaurant_2023.api.api.get_current_working_day",
    params: {
        business_branch: gv.setting?.business_branch
    },
    auto: true
});
function onSelected(selected){ 
    router.push({name: 'AddSaleNoTable', params: {sale_type: selected}}).then(()=>{
        onLoad()
    })
}
function onTableLayout(){
    router.push({name: 'TableLayout'})
}
const cashierShiftResource = createResource({
    url: "epos_restaurant_2023.api.api.get_current_cashier_shift",
    params: {
        pos_profile: posProfile
    },
    auto: true
});

function onOpenOrder(sale_id) {
    gv.authorize("open_order_required_password", "make_order").then(async (v) => {
        if (v) {
            const make_order_auth = {"username":v.username,"name":v.user,discount_codes:v.discount_codes }; 
            if(mobile.value){
                    await sale.LoadSaleData(sale_id).then(async (v)=>{
                        localStorage.setItem('redirect_sale_type', selected.value);
                        localStorage.setItem('make_order_auth',JSON.stringify(make_order_auth));
                        const result =  await smallViewSaleProductListModal ({title: sale.sale.name ? sale.sale.name : 'New Sale', data: {from_table: true}});
                        if(result){
                            //
                        }else{
                            localStorage.removeItem('make_order_auth'); 
                        }
                      
                    })
            }else{
                if (sale_id) {
                    localStorage.setItem('make_order_auth',JSON.stringify(make_order_auth));
                    router.push({ name: "AddSale", params: { name: sale_id } }).then(()=>{
                        localStorage.setItem('redirect_sale_type', selected.value)
                    })
                }
                else {
                    toaster.error($t("msg.System can not get sale name"));
                }        
            } 
            return;
        }
    })

}

async function onViewSaleOrder(sale_id) {
    const result = await saleDetailDialog({ name: sale_id });
    if (result) {
        if (result == "open_order") {
            onClose();
        }
        else if (result == "delete_order") {
            saleResource.fetch();
        }
    }
}

let params = ref({
    doctype: "Sale",
    fields: ["name", "modified", "sale_status", "sale_status_color", "sale_type","sale_type_color", "tbl_number", "guest_cover", "customer", "customer_name", "total_quantity", "grand_total"],
    order_by: "modified desc",
    filters: {
        'docstatus': 0,
        'working_day': workingDayResource.data?.name,
        'cashier_shift': cashierShiftResource.data?.name,
        'sale_type': route.params.sale_type
    },
    limit_page_length: 200,
})

const saleResource = createResource({
    url: "frappe.client.get_list",
    params: params.value
});
function onLoad(){
    params.value.filters.sale_type = route.params.sale_type
    saleResource.params = params.value
    saleResource.fetch()
}

onMounted(() => {
    onLoad()
})

 
 
async function newSale() { 
    gv.authorize("open_order_required_password", "make_order").then(async (v) => {
            if (v) {
                const make_order_auth = {"username":v.username,"name":v.user,discount_codes:v.discount_codes };                 
                   
                localStorage.setItem('make_order_auth',JSON.stringify(make_order_auth));
                if(!_onNewSale()){
                    return;
                }

                router.push({ name: "AddSale" }).then(()=>{
                    localStorage.setItem('redirect_sale_type', selected.value)
                });           
               
                return;
            }
        })

}


function _onNewSale(){
    let guest_cover = 0;
    sale.newSale();
    sale.sale.guest_cover = guest_cover;
    sale.sale.table_id = '';
    sale.sale.tbl_number = '';
    sale.sale.sale_type = route.params.sale_type
    if (gv.setting.price_rule != sale.sale.price_rule) {
        toaster.info($t('msg.Your current price rule is',[sale.sale.price_rule]));
        return false;
    }
    return true;
}
</script> 