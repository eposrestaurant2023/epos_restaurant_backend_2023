<template>  
    <template v-for="g in tableLayout.table_groups">
        <v-window-item :value="g.key" v-bind:style="{ 'background-image': 'url(' + g.background + ')', 'min-height': 'calc(100vh - 200px)','background-size':'100% 100%' }"
            class="bg-center overflow-auto" v-if="!mobile">
            <template v-for="(t, index) in g.tables" :key="index">
                <div v-bind:style="{ 'height': t.h + 'px', 'width': t.w + 'px', 'left': t.x + 'px', 'top': t.y + 'px', 'background-color': t.background_color, 'position': 'absolute', 'box-sizing': 'border-box' }"
                    class="text-center text-gray-100 cursor-pointer" :class="t.shape == 'Circle' ? 'shape-circle' : ''" @click="onTableClick(t)">
                    <v-badge :content="t.sales?.length" color="error" style="float: right;"
                        v-if="t.sales?.length > 1"></v-badge>
                    <div class="flex items-center justify-center h-full">
                        <div>
                            <div><span class="font-bold">{{ t.tbl_no }}</span><span v-if="t.guest_cover">({{ t.guest_cover }})</span></div>
                            <div v-if="t.grand_total">
                                <CurrencyFormat :value="t.grand_total"></CurrencyFormat>
                            </div>
                            <div v-if="t.creation" class="text-xs">
                                <v-icon icon="mdi-clock" size="x-small"></v-icon>
                                <Timeago :long="false" :datetime="t.creation" />
                            </div>
                        </div>
                    </div>
                </div>
            </template>
        </v-window-item>
        
        <v-window-item v-else :value="g.key" v-bind:style="{ 'min-height': 'auto' }" class="mt-2 mb-4">
            <v-row>
                <v-col cols="6" v-for="(t, index) in g.tables" :key="index">
                    <div v-bind:style="{ 'height': '75px', 'background-color': t.background_color }"
                        class="text-center text-gray-100 cursor-pointer  rounded-lg" @click="onTableClick(t)">
                        <v-badge :content="t.sales?.length" color="error" style="float:right;" class="mr-2"
                            v-if="t.sales?.length > 1"></v-badge>
                        <div class="flex items-center justify-center h-full">
                            <div>
                                <div><span class="font-bold">{{ t.tbl_no }}</span><span v-if="t.guest_cover">({{ t.guest_cover }})</span></div>
                                <div v-if="t.grand_total">
                                    <CurrencyFormat :value="t.grand_total"></CurrencyFormat>
                                </div>
                                <div v-if="t.creation" class="text-xs">
                                    <v-icon icon="mdi-clock" size="x-small"></v-icon>
                                    <Timeago :long="false" :datetime="t.creation" />
                                </div>
                            </div>
                        </div>
                    </div>
                </v-col>
            </v-row>
        </v-window-item>
    </template>
</template>
<script setup>
import { inject, useRouter, createToaster, selectSaleOrderDialog, keyboardDialog,smallViewSaleProductListModal,i18n } from '@/plugin';
import { Timeago } from 'vue2-timeago'
import { useDisplay } from 'vuetify';

const { t: $t } = i18n.global; 

const { mobile,platform } = useDisplay()
const toaster = createToaster({ position: "top" });
const tableLayout = inject("$tableLayout");
const gv = inject("$gv");
const sale = inject("$sale");
const router = useRouter()
function onTableClick(table, guest_cover) {    
    gv.authorize("open_order_required_password", "make_order").then(async (v) => {
        if (v) {             
            const make_order_auth = {"username":v.username,"name":v.user,discount_codes:v.discount_codes }; 
            if (table.sales.length == 0) {
                localStorage.setItem('make_order_auth',JSON.stringify(make_order_auth));
                newSale(table);
            } 
            else if (table.sales.length == 1) {             
                if(mobile.value){
                    await sale.LoadSaleData( table.sales[0].name).then(async (_sale)=>{
                        localStorage.setItem('make_order_auth',JSON.stringify(make_order_auth));
                        const result =  await smallViewSaleProductListModal ({title: sale.sale.name ? sale.sale.name : $t('New Sale'), data: {from_table: true}});                      
                        if(result){   
                            tableLayout.saleListResource.fetch();
                        }else{
                            localStorage.removeItem('make_order_auth'); 
                        }                                          
                    });
                } 
                else{
                        localStorage.setItem('make_order_auth',JSON.stringify(make_order_auth));
                        router.push({ 
                            name: "AddSale", 
                            params: {
                                name: table.sales[0].name
                            }
                        });
                }
            } 
            else {
                sale.sale.table_id = table.id;
                sale.sale.tbl_number = table.tbl_no;
                const result = await selectSaleOrderDialog({ data: table.sales, table: table,make_order_auth:make_order_auth });
                if (result) { 
                    localStorage.setItem('make_order_auth',JSON.stringify(make_order_auth));
                    if (result.action == "new_sale") {
                        newSale(table);
                    }
                } 
            }
            return;
        }
    });
}

async function newSale(table) {
    let guest_cover = 0;
    if (gv.setting.use_guest_cover == 1) {
        const result = await keyboardDialog({ title: $t('Guest Cover'), type: 'number', value: guest_cover });

        if (typeof result == 'number') {

            guest_cover = parseInt(result);
            if (guest_cover == undefined || isNaN(guest_cover)) {
                guest_cover = 0;
            }

        } else {
            return;
        }
    }
    sale.newSale();
    sale.sale.guest_cover = guest_cover;
    sale.sale.table_id = table.id
    sale.sale.tbl_number = table.tbl_no;
    if (parseFloat(table.default_discount) > 0) {
        sale.sale.discount_type = table.discount_type;
        sale.sale.discount = parseFloat(table.default_discount);
        if (table.discount_type == "Percent") {
            toaster.info($t("msg.This table have discount",[table.default_discount + '%']))
        } else {
            toaster.info($t("msg.This table have discount",[(table.default_discount + ' ' + gv.setting.pos_setting.main_currency_name)]))
        }
    }


    if (table.sale_type) {
        sale.sale.sale_type = table.sale_type
    }
    if (table.price_rule) {
        sale.sale.price_rule = table.price_rule;
    }
    if (gv.setting.price_rule != sale.sale.price_rule) {
        toaster.info($t('msg.Your current price rule is',[sale.sale.price_rule]));
    }
    router.push({ name: "AddSale" });
}


</script>
<style scoped>
.shape-circle {
    border-radius: 100%;
}
</style>