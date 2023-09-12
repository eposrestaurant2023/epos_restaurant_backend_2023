<template>
    <ComModal @onClose="onClose(false)" :fullscreen="true" :isPrint="true" @onPrint="onPrint()" :hide-ok-button="true"
        :hide-close-button="true" :isShowBarMoreButton="canOpenOrder || canEdit || canDelete">
        <template #title>
            {{ $t('Sale Detail') }} #{{ params.name }}
        </template>
        <template #bar_more_button>
            <v-list density="compact">
                <v-list-item @click="onOpenOrder()" v-if="canOpenOrder">
                    <template v-slot:prepend>
                        <v-icon>mdi-note-outline</v-icon>
                    </template>
                    <v-list-item-title>{{ $t('Open Order') }}</v-list-item-title>
                </v-list-item>
                <v-list-item @click="onEditOrder()" v-if="canEdit">
                    <template v-slot:prepend>
                        <v-icon>mdi-checkbox-marked-outline</v-icon>
                    </template>
                    <v-list-item-title>{{ $t('Edit Order') }}</v-list-item-title>
                </v-list-item>
                <v-list-item @click="OnDeleteOrder()" v-if="canDelete">
                    <template v-slot:prepend>
                        <v-icon color="error">mdi-delete</v-icon>
                    </template>
                    <v-list-item-title>{{ $t('Delete') }}</v-list-item-title>
                </v-list-item>
            </v-list>
        </template>

        <template #content>
            <ComLoadingDialog v-if="isLoading" />
            <v-card>
                <template #title>
                    <div class="px-1 py-2 -m-1 whitespace-normal">
                        <v-row>
                            <v-col cols="12" sm="12" md="7" lg="7" xl="8">
                                <v-tabs show-arrows>
                                    <v-tab
                                        v-for="(r, index) in gv.setting.reports.filter(r => r.doc_type == 'Sale' && r.show_in_pos == 1)"
                                        :key="index" @click="onPrintFormat(r)">
                                        {{ $t(r.title) }}
                                    </v-tab>
                                </v-tabs>
                            </v-col>
                            <v-col cols="12" sm="12" md="5" lg="5" xl="4">
                                <div>
                                    <v-row>
                                        <v-col cols="12" sm="7">
                                            <v-select prepend-inner-icon="mdi-content-paste" density="compact"
                                                v-model="selectedLetterhead" :items=gv.setting.letter_heads
                                                item-title="name" item-value="name" hide-no-data hide-details variant="solo"
                                                class="mx-1"></v-select>
                                        </v-col>
                                        <v-col cols="12" sm="5">
                                            <div class="flex items-center">
                                                <v-select prepend-inner-icon="mdi-google-translate" density="compact"
                                                    v-model="selectedLang" :items="gv.setting.lang"
                                                    item-title="language_name" item-value="language_code" hide-no-data
                                                    hide-details variant="solo" class="mx-1"></v-select>
                                                <v-icon class="mx-1" icon="mdi-refresh" size="small" @click="onRefresh()" />
                                            </div>
                                        </v-col>
                                    </v-row>
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

import { inject, ref, computed, onUnmounted, createDocumentResource, useRouter, createResource, confirm,smallViewSaleProductListModal,i18n } from '@/plugin';
import ComLoadingDialog from '@/components/ComLoadingDialog.vue';
import { useDisplay } from 'vuetify';
const { mobile } = useDisplay();
const router = useRouter();
const gv = inject("$gv")
const inject_sale = inject("$sale");
const tableLayout = inject("$tableLayout");
const socket = inject("$socket");
const emit = defineEmits(["resolve"])
const triggerPrint = ref(0);
const { t: $t } = i18n.global;  
const serverUrl = window.location.protocol + "//" + window.location.hostname + ":" + gv.setting.pos_setting.backend_port;


const props = defineProps({
    params: {
        type: Object,
        require: true
    }
})
const selectedLetterhead = ref(getDefaultLetterHead());
const selectedLang = ref(gv.setting.lang[0].language_code);
const activeReport = ref(JSON.parse(JSON.stringify(gv.setting.reports.filter(r => r.doc_type == "Sale" && r.show_in_pos == 1)[0])));
const isLoading = ref(false);

let deletedSaleProducts =[];
let productPrinters = [];

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




if (props.params.print) {
    triggerPrint.value = 1;
} else {
    triggerPrint.value = 0;
}

function onClose(isClose) {
    emit('resolve', isClose);
}

function onRefresh() {
    document.getElementById("report-view").contentWindow.location.replace(printPreviewUrl.value);
}

async function onPrint() {
    const data = {
        action: "print_receipt",
        print_setting: activeReport.value,
        setting: gv.setting?.pos_setting,
        sale: sale.doc
    }
    if (localStorage.getItem("is_window") == "1") {

        if (activeReport.value.pos_receipt_file_name != "" && activeReport.value.pos_receipt_file_name != null) {
            if (await confirm({ title: 'Print Receipt', text: 'Are you sure you want to price receipt?' })) {
                window.chrome.webview.postMessage(JSON.stringify(data));
            }
            return;
        }
    } else {
        if (activeReport.value.pos_receipt_file_name != "" && activeReport.value.pos_receipt_file_name != null) {
            socket.emit('PrintReceipt', JSON.stringify(data));
            return;
        }
    }
    window.open(printPreviewUrl.value + "&trigger_print=1").print();
    window.close();
}

function onOpenOrder() {
    gv.authorize("open_order_required_password", "make_order").then(async (v) => {
        if(v){
            const make_order_auth = {"username":v.username,"name":v.user,discount_codes:v.discount_codes }; 
            if(mobile.value){
                await inject_sale.LoadSaleData(props.params.name).then(async (_sale)=>{
                    localStorage.setItem('make_order_auth',JSON.stringify(make_order_auth));
                    emit('resolve', "open_order");
                    const result =  await smallViewSaleProductListModal ({title: props.params.name ? props.params.name : $t('New Sale'), data: {from_table: true}});                      
                    if(result){   
                        tableLayout.saleListResource.fetch();
                    }else{
                        localStorage.removeItem('make_order_auth'); 
                    }                                          
                });
            } 
            else{
                localStorage.setItem('make_order_auth',JSON.stringify(make_order_auth));               
                router.push({ name: "AddSale", params: { name: props.params.name } });
                emit('resolve', "open_order");
            }
        }
    });
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
    });

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
            const _sale = JSON.parse(JSON.stringify(sale.sale));
            generateSaleProductPrintToKitchen(_sale,v.note);
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
                onProcessPrintToKitchen(_sale);

                emit('resolve', "delete_order");
            })
        }
    })

}

function generateSaleProductPrintToKitchen(doc,note){
    this.deletedSaleProducts = [];
    (doc.sale_products||[]).forEach((sp)=>{
        if(sp.sale_product_status=="Submitted"){
            sp.note = note;
            sp.deleted_item_note = "Bill Deleted";
            this.deletedSaleProducts.push(sp);
        }
    });

    //generate deleted product to product printer list
    this.deletedSaleProducts.filter(r => JSON.parse(r.printers).length > 0).forEach((r) => {
            const pritners = JSON.parse(r.printers);
            pritners.forEach((p) => {
                this.productPrinters.push({
                    printer: p.printer,
                    group_item_type: p.group_item_type,
                    product_code: r.product_code,
                    product_name_en: r.product_name,
                    product_name_kh: r.product_name_kh,
                    portion: r.portion,
                    unit: r.unit,
                    modifiers: r.modifiers,
                    note: r.note,
                    quantity: r.quantity,
                    is_deleted: true,
                    is_free: r.is_free == 1,
                    deleted_note: r.deleted_item_note,
                    order_by: r.order_by,
                    creation: r.creation,
                    modified: r.modified
                })
            });
        });
}


function onProcessPrintToKitchen(doc){
    const data = {
            action: "print_to_kitchen",
            setting: this.setting?.pos_setting,
            sale: doc,
            product_printers: this.productPrinters
        }

        if (localStorage.getItem("is_window") == 1) {
            window.chrome.webview.postMessage(JSON.stringify(data));
        } else {
            socket.emit("PrintReceipt", JSON.stringify(data))
        }
        this.deletedSaleProducts = [];
        this.productPrinters = [];
}


const reportClickHandler = async function (e) {
    if (e.isTrusted && typeof (e.data) == 'string') {

        const data = e.data.split("|")

        if (data.length > 0) {


        }

    }
};

function onPrintFormat(report) {
    activeReport.value.name = report.name;
    activeReport.value.print_report_name = report.print_report_name || report.name
    onRefresh()

}

window.addEventListener('message', reportClickHandler, false);

onUnmounted(() => {
    window.removeEventListener('message', reportClickHandler, false);
})

</script>

