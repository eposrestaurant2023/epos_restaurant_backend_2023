<template>
    <template v-for="g in tableLayout.table_groups">
        <v-window-item :value="g.key"
            v-bind:style="{ 'background-image': 'url(' + g.background + ')', 'min-height': 'calc(100vh - 200px)' }"
            class="bg-contain bg-center overflow-auto" v-if="!mobile">
            <template v-for="(t, index) in g.tables" :key="index">

                <div v-bind:style="{ 'height': t.h + 'px', 'width': t.w + 'px', 'left': t.x + 'px', 'top': t.y + 'px', 'background-color': t.background_color, 'position': 'absolute', 'box-sizing': 'border-box' }"
                    class="text-center text-gray-100 cursor-pointer" @click="onTableClick(t)">
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
                                <Timeago :long="long" :datetime="t.creation" />
                            </div>
                        </div>
                    </div>

                </div>

            </template>
        </v-window-item>

        <v-window-item v-else :value="g.key" v-bind:style="{ 'min-height': 'calc(100vh - 200px)' }" class="mt-2">
            <v-row>
                <v-col cols="6" v-for="(t, index) in g.tables" :key="index">
                    <div v-bind:style="{ 'height': '75px', 'background-color': t.background_color }"
                        class="text-center text-gray-100 cursor-pointer  rounded-lg" @click="onTableClick(t)">
                        <v-badge :content="t.sales.length" color="error" style="float:right;" class="mr-2"
                            v-if="t.sales.length > 1"></v-badge>
                        <div class="flex items-center justify-center h-full">
                            <div>

                                <div><span class="font-bold">{{ t.tbl_no }}</span><span v-if="t.guest_cover">({{
                                    t.guest_cover }})</span></div>

                                <div v-if="t.grand_total">
                                    <CurrencyFormat :value="t.grand_total"></CurrencyFormat>
                                </div>
                                <div v-if="t.creation" class="text-xs">
                                    <v-icon icon="mdi-clock" size="x-small"></v-icon>
                                    <Timeago :long="long" :datetime="t.creation" />
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

import { inject, useRouter, createToaster, defineProps, selectSaleOrderDialog, keyboardDialog } from '@/plugin';
import { Timeago } from 'vue2-timeago'
import { useDisplay } from 'vuetify'

const { mobile } = useDisplay()
const toaster = createToaster({ position: "top" });
const tableLayout = inject("$tableLayout");
const gv = inject("$gv");
const sale = inject("$sale");

const router = useRouter()


function onTableClick(table, guest_cover) {
    gv.authorize("open_order_required_password", "make_order").then(async (v) => {
        if (v) {
            sale.orderBy = v.user;
            if (table.sales.length == 0) {
                newSale(table);

            } else if (table.sales.length == 1) {
                router.push({
                    name: "AddSale", params: {
                        name: table.sales[0].name
                    }
                });
            } else {
                sale.sale.table_id = table.id;
                sale.sale.tbl_number = table.tbl_no;
                const result = await selectSaleOrderDialog({ data: table.sales, table: table });
                if (result) {
                    if (result.action == "new_sale") {
                        newSale(table);
                    }
                }
            }
            return;
        }
    })


}

async function newSale(table) {
    let guest_cover = 0;
    if (gv.setting.use_guest_cover == 1) {
        const result = await keyboardDialog({ title: "Guest Cover", type: 'number', value: guest_cover });

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
    sale.sale.table_id = table.id;
    sale.sale.tbl_number = table.tbl_no;
    sale.sale.guest_cover = guest_cover;
    if (parseFloat(table.default_discount) > 0) {
        sale.sale.discount_type = table.discount_type;
        sale.sale.discount = parseFloat(table.default_discount);
        if (table.discount_type == "Percent") {
            toaster.info("This table is discount " + table.default_discount + '%')
        } else {
            toaster.info("This table is discount " + table.default_discount + ' ' + gv.setting.pos_setting.main_currency_name)
        }
    }


    if (table.sale_type) {
        sale.sale.sale_type = table.sale_type
    }
    if (table.price_rule) {
        sale.sale.price_rule = table.price_rule;
    }
    if (gv.setting.price_rule != sale.sale.price_rule) {
        toaster.info("Your current price rule is " + sale.sale.price_rule);
    }
    router.push({ name: "AddSale" });
}


</script>