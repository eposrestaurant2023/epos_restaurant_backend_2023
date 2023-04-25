<template>
    <PageLayout :title="`Sale Order: ${route.params.sale_type}`" icon="mdi-note-outline">
        <template #action>
            <v-btn color="error" variant="tonal" prepend-icon="mdi-cart" type="button" @click="onAddCustomer">
                New Order
            </v-btn>
        </template>
        <template #default>
            <ComPlaceholder :is-not-empty="saleResource.data?.length > 0" :loading="saleResource.loading">
                <div>
                    <div class="grid gap-2 sm:grid-cols-2 md:grid-cols-2 lg:grid-cols-2 xl:grid-cols-3">
                        <ComSaleCardItem :data="saleResource.data"/> 
                    </div>
                </div>
            </ComPlaceholder>
        </template>
    </PageLayout>
</template>
<script setup>
import PageLayout from '../../components/layout/PageLayout.vue';
import { useRouter, useRoute, createResource, ref, inject, createToaster } from "@/plugin"
import { Timeago } from 'vue2-timeago'
import { saleDetailDialog } from "@/utils/dialog";
import ComPlaceholder from "@/components/layout/components/ComPlaceholder.vue";
import ComSaleCardItem from './components/ComSaleCardItem.vue';
const router = useRouter();
const route = useRoute();
const emit = defineEmits(["resolve"])
const gv = inject('$gv')
const toaster = createToaster({ position: "top" })
const posProfile = localStorage.getItem('pos_profile')
const props = defineProps({
    params: {
        type: Object,
        required: true,
    }
})
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

const cashierShiftResource = createResource({
    url: "epos_restaurant_2023.api.api.get_current_cashier_shift",
    params: {
        pos_profile: posProfile
    },
    auto: true
});

function onOpenOrder(sale_id) {
    createResource({
        url: "epos_restaurant_2023.api.api.get_current_shift_information",
        params: {
            business_branch: gv.setting?.business_branch,
            pos_profile: localStorage.getItem("pos_profile")
        },
        auto: true,
        onSuccess(data) {
            if (data.cashier_shift == null) {
                toaster.warning("Please start cashier shift first");
                router.push({ name: "OpenShift" }).then(() => {
                    onClose()
                })
            } else if (data.working_day == null) {
                toaster.warning("Please start working day first");
                router.push({ name: "StartWorkingDay" }).then(() => {
                    onClose()
                })
            } else {
                if (sale_id) {
                    router.push({ name: "AddSale", params: { name: sale_id } }).then(() => {
                        onClose()
                    })
                }
                else {
                    toaster.error("Cannot get sale name")
                }

            }
        },
        onError(er) {
            toaster.error(JSON.stringify(er))
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
    fields: ["name", "modified", "sale_status", "sale_status_color", "sale_type", "tbl_number", "guest_cover", "customer", "customer_name", "total_quantity", "grand_total"],
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

saleResource.params = params.value
saleResource.fetch()


function onTableClick(table, guest_cover) {
    gv.authorize("open_order_required_password", "make_order").then(async (v) => {
        if (v) {
            sale.orderBy = v.user;
            if (table.sales.length == 0) {
                newSale(table);

            } else if (table.sales.length == 1) {

                if (mobile.value) {
                    await sale.LoadSaleData(table.sales[0].name).then(async (v) => {
                        const result = await smallViewSaleProductListModel({ title: sale.sale.name ? sale.sale.name : 'New Sale', data: { from_table: true } });
                        if (result) {
                            tableLayout.saleListResource.fetch();
                        }

                    })

                } else {

                    router.push({
                        name: "AddSale", params: {
                            name: table.sales[0].name
                        }
                    });

                }



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
    sale.sale.guest_cover = guest_cover;
    sale.sale.table_id = table.id
    sale.sale.tbl_number = table.tbl_no;
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