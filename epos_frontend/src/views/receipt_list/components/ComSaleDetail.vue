<template>
    <ComModal @onClose="onClose(false)" :fullscreen="true" :isPrint="true" @onPrint="onPrint()" :hide-ok-button="true"
        :hide-close-button="true" :isShowBarMoreButton="true">
        <template #title>
            Sale Detail: {{ params.name }}
        </template>
        <template #bar_more_button>
            <v-list density="compact">
                <v-list-item @click="onOpenOrder()" v-if="canOpenOrder">
                    <template v-slot:prepend>
                        <v-icon>mdi-note-outline</v-icon>
                    </template>
                    <v-list-item-title>Open Order</v-list-item-title>
                </v-list-item>
                <v-list-item @click="onEditOrder()" v-if="canEdit">
                    <template v-slot:prepend>
                        <v-icon>mdi-checkbox-marked-outline</v-icon>
                    </template>
                    <v-list-item-title>Edit Order</v-list-item-title>
                </v-list-item>
                <v-list-item v-if="canDelete" @click="OnDeleteOrder()">
                    <template v-slot:prepend>
                        <v-icon color="error">mdi-delete</v-icon>
                    </template>
                    <v-list-item-title>Delete</v-list-item-title>
                </v-list-item>
            </v-list>
        </template>

        <template #content>
            <ComLoadingDialog v-if="isLoading" />
            <v-card>


                <template #title>
                    <div class="px-1 py-2 -m-1 whitespace-normal">
                        <v-row>
                            <v-col cols="12" sm="7" md="7" lg="7" xl="8">
                                <v-tabs show-arrows>
                                    <v-tab
                                        v-for="(r, index) in gv.setting.reports.filter(r => r.doc_type == 'Sale' && r.show_in_pos == 1)"
                                        :key="index" @click="onViewReport(r)">
                                        {{ r.title }}
                                    </v-tab>
                                </v-tabs>
                            </v-col>
                            <v-col cols="12" sm="5" md="5" lg="5" xl="4">
                                <div class="flex items-center">
                                    <v-select prepend-inner-icon="mdi-content-paste" density="compact"
                                        v-model="selectedLetterhead" :items=gv.setting.letter_heads item-title="name"
                                        item-value="name" hide-no-data hide-details variant="solo" class="mx-1"></v-select>
                                    <v-select prepend-inner-icon="mdi-google-translate" density="compact"
                                        v-model="selectedLang" :items="gv.setting.lang" item-title="language_name"
                                        item-value="language_code" hide-no-data hide-details variant="solo"
                                        class="mx-1"></v-select>
                                    <v-icon class="mx-1" icon="mdi-refresh" size="small" @click="onRefresh()" />
                                </div>
                            </v-col>
                        </v-row>
                    </div>
                </template>
                <v-card-text style="height: calc(100vh - 200px);">
                    <iframe id="report-view" height="100%" width="100%" :src="printPreviewUrl"></iframe>
                </v-card-text>
            </v-card>
        </template>
    </ComModal>
</template>
  
<script setup>

import { inject, ref, computed, onUnmounted, createDocumentResource, useRouter, createResource, confirm } from '@/plugin'
import { createToaster } from '@meforma/vue-toaster';
import ComLoadingDialog from '@/components/ComLoadingDialog.vue';
import { webserver_port } from "../../../../../../../sites/common_site_config.json"
const gv = inject("$gv")



const serverUrl = window.location.protocol + "//" + window.location.hostname + ":" + webserver_port;

const toaster = createToaster({ position: "top" })
const router = useRouter();

const props = defineProps({
    params: {
        type: Object,
        require: true
    }
})
const selectedLetterhead = ref(getDefaultLetterHead());
const selectedLang = ref(gv.setting.lang[0].language_code);
const activeReport = ref(gv.setting.reports.filter(r => r.doc_type == "Sale" && r.show_in_pos == 1)[0]);
const isLoading = ref(false)

const sale = createDocumentResource({
    url: 'frappe.client.get',
    doctype: 'Sale',
    name: props.params.name,
    auto: true
})


const cashierShiftInfo = createResource({
    url: "epos_restaurant_2023.api.api.get_current_cashier_shift",
    params: {
        pos_profile: localStorage.getItem("pos_profile")
    },
    auto: true
});

const salePaymentResource = createResource({
    url: "frappe.client.get_list",
    params: {
        doctype: "Sale Payment",
        filters: {
            sale: props.params.name
        },
        fields: ["name"],
    },

    auto: true
});


const canEdit = computed(() => {
    return sale.doc?.docstatus == 1 && sale.doc?.cashier_shift == cashierShiftInfo?.data?.name;
})
const canOpenOrder = computed(() => {
    return sale.doc?.docstatus == 0 && sale.doc?.cashier_shift == cashierShiftInfo?.data?.name;
})
const canDelete = computed(() => {
    return (sale.doc?.docstatus == 1 || sale.doc?.docstatus == 0) && sale.doc?.cashier_shift == cashierShiftInfo?.data?.name;
})





const printPreviewUrl = computed(() => {
    let letterhead = "";
    if (selectedLetterhead.value == "") {
        letterhead = getDefaultLetterHead();
    } else {
        letterhead = selectedLetterhead.value;
    }
    const url = `${serverUrl}/printview?doctype=Sale&name=${props.params.name}&format=${activeReport.value.name}&no_letterhead=0&show_toolbar=0&letterhead=${letterhead}&settings=%7B%7D&_lang=${selectedLang.value}`;
    return url;
})


function getDefaultLetterHead() {
    let letterhead = "";

    letterhead = gv.setting.letter_heads.filter(r => r.is_default == 1)[0]?.name;
    if (!letterhead) {
        letterhead = "No Letterhead";
    }
    return letterhead;
}

const emit = defineEmits(["resolve"])

const triggerPrint = ref(0)


if (props.params.print) {
    triggerPrint.value = 1;
} else {
    triggerPrint.value = 0;
}


function onViewReport(r) {
    activeReport.value = r.title;
}

function onClose(isClose) {
    emit('resolve', isClose);
}

function onRefresh() {

    document.getElementById("report-view").contentWindow.location.replace(printPreviewUrl.value)

}

async function onPrint() {

    if (localStorage.getItem("is_window") == "1") {

        if (activeReport.value.pos_receipt_file_name != "" && activeReport.value.pos_receipt_file_name != null) {
            if (await confirm({ title: 'Print Receipt', text: 'Are you sure you want to price receipt?' })) {
                const data = {
                    action: "print_receipt",
                    print_setting: activeReport.value,
                    setting: gv.setting?.pos_setting,
                    sale: sale.doc
                }
                window.chrome.webview.postMessage(JSON.stringify(data));
            }

            return;
        }

    }

    window.open(printPreviewUrl.value + "&trigger_print=1").print();
    window.close();

}

function onOpenOrder(sale_id) {
    router.push({ name: "AddSale", params: { name: props.params.name } });
    emit('resolve', "open_order");
}

function onEditOrder() {
    //check authorize and     check reason

    gv.authorize("edit_closed_receipt_required_password", "edit_closed_receipt", "edit_closed_receipt_required_note", "Edit Closed Receipt").then(async (v) => {
        if (v) {
            //cancel payment first
            isLoading.value = true;
            const cancelSaleResource = createResource({
                url: "epos_restaurant_2023.api.api.edit_sale_order",
                params: {
                    name: props.params.name,
                    auth: { full_name: v.user, username: v.username, note: v.note }
                },
                onError(err) {
                    isLoading.value = false;
                }
            });

            await cancelSaleResource.fetch().then((v) => {
                router.push({ name: "AddSale", params: { name: props.params.name } });

                isLoading.value = false;
                emit('resolve', "open_order");

            })



        }
    })

}

function OnDeleteOrder() {
    //check authorize and     check reason

    gv.authorize("delete_bill_required_password", "delete_bill", "delete_bill_required_note", "Delete Bill Note").then(async (v) => {
        if (v) {
            if (v.show_confirm == 1) {
                if (await confirm({ title: 'Delete Sale Order', text: 'Are you sure you want delete this sale order?' }) == false) {
                    return;
                }
            }

            //cancel payment first
            isLoading.value = true;
            const deleteSaleResource = createResource({
                url: "epos_restaurant_2023.api.api.delete_sale",
                params: {
                    name: props.params.name,
                    auth: { full_name: v.user, username: v.username, note: v.note }
                },
                onError(err) {
                    isLoading.value = false;
                }
            });

            await deleteSaleResource.fetch().then((v) => {
                isLoading.value = false;
                emit('resolve', "delete_order");

            })



        }
    })

}


const reportClickHandler = async function (e) {
    if (e.isTrusted && typeof (e.data) == 'string') {

        const data = e.data.split("|")

        if (data.length > 0) {


        }

    }
};

window.addEventListener('message', reportClickHandler, false);

onUnmounted(() => {
    window.removeEventListener('message', reportClickHandler, false);
})

</script>

