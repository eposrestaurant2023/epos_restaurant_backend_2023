<template>
    <PageLayout title="Table Layout" full icon="mdi-cart">
        <template #centerCotent>
            <v-tabs align-tabs="center" v-if="table_groups && table_groups.length>1" v-model="tab"  >
                <v-tab v-for="g in table_groups" :key="g.key" :value="g.key" @click="onSaveSelectedTab(g.key)">
                    {{ g.table_group }}
                </v-tab>
            </v-tabs>
        </template>
        <template #action>
            <v-btn :loading="saleListResource.loading" icon color="info" @click="onRefreshSale">
                <v-icon>mdi-cached</v-icon>
            </v-btn>
            <v-btn @click="onChangeCurrentView" v-if="currentView=='table_group'">
                Pending order
            </v-btn>
            <v-btn @click="onChangeCurrentView" v-else >
                Table Layout
            </v-btn>
            <v-btn :loading="save_table_postion_resource.loading" v-if="canArrangeTable" @click="onSaveTablePosition">
                Save Table Position
            </v-btn>
            <v-menu>
                <template v-slot:activator="{ props }">
                    <v-btn v-bind="props">
                        <v-icon>mdi-dots-vertical</v-icon>
                    </v-btn>
                </template>
                <v-list>
                    <v-list-item @click="onEnableArrangeTable(true)">
                        <v-list-item-title>Arrange Table Layout</v-list-item-title>
                    </v-list-item>
                </v-list>
            </v-menu>
        </template>
        <template v-if="table_groups">
            <v-window v-model="tab" v-if="currentView=='table_group'">
                <template v-for="g in table_groups">
                    <v-window-item :value="g.key"
                        v-bind:style="{ 'background-image': 'url(' + g.background + ')', 'min-height': 'calc(100vh - 200px)' }"
                        class="bg-contain bg-center overflow-auto">
                        <div v-if="canArrangeTable">
                            <Vue3DraggableResizable v-for="(t, index) in g.tables" :key="index" class="table"
                                v-model:x="t.x" v-model:y="t.y" v-model:w="t.w" v-model:h="t.h" :draggable="true"
                                :resizable="true" @drag-end="onDragEnd(t)($event)">
                                <div class="h-full"
                                    v-bind:style="{ 'background-color': 'blue', 'color': '#fff', 'overflow': 'hidden' }">
                                    {{ t.x  }} x {{ t.y  }}
                                </div>
                            </Vue3DraggableResizable>
                        </div>
                        <div v-else>
                            <template v-for="(t, index) in g.tables" :key="index">
                                <div v-bind:style="{ 'height': t.h + 'px', 'width': t.w + 'px', 'left': t.x + 'px', 'top': t.y + 'px', 'background-color': t.background_color, 'position': 'absolute', 'box-sizing': 'border-box' }"
                                    class="text-center text-gray-100 cursor-pointer" @click="onTableClick(t)">
                                    <div class="flex items-center justify-center h-full">
                                        <div>
                                            <div><span class="font-bold">{{ t.tbl_no }}</span><span v-if="t.guest_cover">({{ t.guest_cover }})</span></div>
                                            <span v-if="isLoading"></span>
                                            <div v-if="t.grand_total"><CurrencyFormat :value="t.grand_total"></CurrencyFormat> </div>
                                            <div v-if="t.creation" class="text-xs">
                                                <v-icon icon="mdi-clock" size="x-small"></v-icon> <Timeago :long="long" :datetime="t.creation"  />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </template>
                        </div>

                    </v-window-item>
                    
                </template>
               
            </v-window>
            <v-window v-model="tab" v-else>
                <template v-for="g in table_groups">
                    <v-window-item :value="g.key"
                        v-bind:style="{ 'min-height': 'calc(100vh - 200px)' }"> 
                        <ComPendingSaleList :data="saleListResource.data.filter(r=>r.tbl_group==g.table_group)"/>
            </v-window-item>
            </template>
            </v-window>
        </template>
        <ComSaleStatusInformation/>
    </PageLayout>
</template>

<script setup>
import PageLayout from '../../components/layout/PageLayout.vue';
import { Timeago } from 'vue2-timeago'
import ComPendingSaleList from './ComPendingSaleList.vue';
import Vue3DraggableResizable from 'vue3-draggable-resizable'
import { inject, createResource, createToaster, useRouter, reactive, ref, selectSaleOrderDialog, keyboardDialog } from "@/plugin"

import ComSaleStatusInformation from './components/ComSaleStatusInformation.vue';
import Enumerable from 'linq'
const sale = inject("$sale");
const gv = inject("$gv");

const router = useRouter()

 

let tab = ref(null);
let isLoading = ref(false);
const currentView = ref("table_group")
const canArrangeTable = ref(false)
const toaster = createToaster({position:"top"});

//read value from local storage
const setting = JSON.parse(localStorage.getItem("setting"))
//resource section
const working_day = createResource({
    url: "epos_restaurant_2023.api.api.get_current_working_day",
    params: {
        pos_profile: localStorage.getItem("pos_profile")
    },
    auto: true,
    onSuccess(d) {
        if (d == undefined) {
            // toaster.error("Please start working day first", { position: "top" });
            // router.push({ name: "StartWorkingDay" })
        }
    }
})


let saleListResource = createResource({
    url: "frappe.client.get_list",
    params: {
        doctype: "Sale",
        fields: ["name","creation", "grand_total","total_quantity", "tbl_group", "tbl_number", "guest_cover", "grand_total", "sale_status", "sale_status_color", "sale_status_priority", "customer", "customer_name", "phone_number"],
        filters: {
            pos_profile: localStorage.getItem("pos_profile"),
            docstatus: 0
        },
    },
    onSuccess(data) {
        table_groups.forEach(function (g) {
            g.tables.forEach(function (t) {
                t.sales = data.filter(r => r.tbl_group == g.table_group && r.tbl_number == t.tbl_no)
                if (t.sales.length > 0) {
                    t.guest_cover = t.sales.reduce((n, r) => n + r.guest_cover, 0)
                    t.grand_total = t.sales.reduce((n, r) => n + r.grand_total, 0)
                    t.background_color = t.sales.sort((a, b) => a.sale_status_priority - b.sale_status_priority)[0].sale_status_color;
                    t.creation = Enumerable.from(t.sales).orderBy("$.creation").select("$.creation").toArray()[0]
                }

            })
        })
        loading()
    }
});

//table layout list
let table_groups = reactive(null)

table_groups = JSON.parse(localStorage.getItem("table_groups"));

showHiddentTable();

//render data from sale resource
saleListResource.fetch()

//get current selected tab from local storage
if (localStorage.getItem("selected_table_group")) {
    tab.value = localStorage.getItem("selected_table_group");
}

function onSaveSelectedTab(key) {
    localStorage.setItem("selected_table_group", key);
}

function onChangeCurrentView(){
    currentView.value = currentView.value=="table_group"?"pending_order":"table_group";
}

function loading() {
    isLoading.value = true;
    setTimeout(() => {
        isLoading.value = false;

    }, 5000)
}

function onTableClick(table) {
    gv.authorize("open_order_required_password", "make_order").then(async (v) => {
        if (v) {
            sale.orderBy = v.user;
            if (table.sales.length == 0) {
                let guest_cover = 0;
                if (setting.use_guest_cover == 1) {
                    const result = await keyboardDialog({ title: "Guest Cover", type: 'number', value: guest_cover });

                    if (result || String(result) == '' || result == 0) {

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
                        toaster.info("This table is discount " + table.default_discount + ' ' + setting.pos_setting.main_currency_name)
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

            } else if (table.sales.length == 1) {
                router.push({
                    name: "AddSale", params: {
                        name: table.sales[0].name
                    }
                });
            } else {
                sale.sale.table_id = table.id;
                sale.sale.tbl_number = table.tbl_no;
                await selectSaleOrderDialog({ data: table.sales, table: table });
            }
            return;






        }
    })


}


function onOpenSaleScreen(table, guest_cover) {
    router.push({ name: "AddSale", "table_number": table.tbl_number })
}


function onEnableArrangeTable(status) {
    if (status) {
        isLoading.value = true;
    }

    canArrangeTable.value = status;
    table_groups.forEach((d) => {
        d.tables.forEach((t) => {
            t.dragable = status;
            t.resizable = status;
        })
    });
    if (status) {
        setTimeout(() => {
            isLoading.value = false;

        }, 100);
    }

}

let save_table_postion_resource = createResource({
    url: "epos_restaurant_2023.api.api.save_table_position",
    onSuccess(d) {
        toaster.success("Save table position successfully", { position: "top" });
        onEnableArrangeTable(false);
        localStorage.setItem("table_groups", JSON.stringify(table_groups));
           
    }
})

function showHiddentTable() {
    
    const container = document.getElementsByClassName("v-window__container");
    const containerHeight = container[0]?.offsetHeight;
    const containerWidth = container[0]?.offsetWidth;

    table_groups.forEach(function (g) {
        g.tables.forEach(function (t) {
           
            if (t.x < 0) {
                t.x = 0;
            }

            if (t.y < 0) {
                t.y = 0
            }

        })
    })
}
function onSaveTablePosition() {
 

    save_table_postion_resource.params = {
        "device_name": localStorage.getItem("device_name"),
        "table_group": JSON.parse(JSON.stringify(table_groups))
    };
    save_table_postion_resource.submit();

}

function onRefreshSale() {


    saleListResource.fetch();
}

function onDragEnd(t) {
    const container = document.getElementsByClassName("v-window__container");
    const containerHeight = container[0]?.offsetHeight;
    const containerWidth = container[0]?.offsetWidth;
 
    return function(position){
        if (t.y+t.h>containerHeight){
            t.y = containerHeight - t.h;
        }
        if (t.x+t.w>containerWidth){
            t.x = containerWidth - t.w;
        }
        
    }
    
}



</script> 