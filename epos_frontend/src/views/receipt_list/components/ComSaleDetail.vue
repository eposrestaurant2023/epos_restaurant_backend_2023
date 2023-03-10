<template>
    <ComModal @onClose="onClick" :isMoreMenu="true" :isShowBarMoreButton="true" :isPrint="true" @onPrint="onPrint" :hideOkButton="true" width="1200px" fullscreen="true">
        <template #title>
            Sale # : {{ sale.name }}
        </template>
        <template #bar_more_button>
            <v-list density="compact">
                <v-list-item>
                    <template v-slot:prepend>
                        <v-icon color="error">mdi-delete</v-icon>
                    </template>
                    <v-list-item-title>Delete</v-list-item-title>
                </v-list-item>
            </v-list>
        </template>
        <template #content>
            <iframe style="height:calc(100vh - 130px)" width="100%"
                    :src="url"></iframe>
        </template>
 
   </ComModal>
</template >

<script setup>

import { createDocumentResource, ref, inject, computed } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import ComPrintButton from '@/components/ComPrintButton.vue';
import { printPreviewDialog } from '@/utils/dialog';
import Enumerable from 'linq';
import { useDisplay } from 'vuetify'
import ComModal from '../../../components/ComModal.vue';

const { mobile } = useDisplay()

const gv = inject("$gv")
const props = defineProps({
    params: {
        type: Object,
        required: true,
    },
})
const emit = defineEmits(["resolve", "reject"])
const setting = computed(() => {
    return JSON.parse(localStorage.getItem('setting'))
})


const url = "http://192.168.10.114:1216/printview?doctype=Sale&name=" + props.params.name + "&format=Sale%20Invoice%20A4&no_letterhead=0&letterhead=Defualt%20Letter%20Head&settings=%7B%7D&_lang=en&show_toolbar=0"


let sale = createDocumentResource({
    url: 'frappe.client.get',
    doctype: 'Sale',
    name: props.params.name,
    auto: true
})

function onClick() {
    emit('reject', false);
}

async function onPrint(r) {

    if (r.pos_receipt_file_name && localStorage.getItem("is_window")) {
        let data = {
            action: "print_receipt",
            print_setting: r,
            setting: gv.setting.pos_setting,
            sale: sale.doc
        }
        window.chrome.webview.postMessage(JSON.stringify(data));
    } else {

        await printPreviewDialog({
            title: "Sale #: " + sale.doc.name,
            doctype: "Sale",
            name: sale.doc.name,
            "report": r.name,
            print: true
        });

    }

}
const saleProducts = computed(() => {
    return Enumerable.from(sale.doc.sale_products).groupBy(
        "{product_code:$.product_code,portion:$.portion,modifiers:$.modifiers,product_name:$.product_name,product_name_kh:$.product_name_kh,unit:$.unit,discount:$.discount,discount_type:$.discount_type,product_photo:$.product_photo,price:$.price}",
        "{quantity:$.quantity, amount: $.amount}",
        "{product_code:$.product_code,product_name:$.product_name,product_name_kh:$.product_name_kh,modifiers_price:$.modifiers_price,portion:$.portion,modifiers:$.modifiers,unit:$.unit,discount:$.discount,discount_type:$.discount_type,product_photo:$.product_photo,price:$.price, quantity: $$.sum('$.quantity'), amount: $$.sum('$.amount')}",
        "$.product_code+','+$.unit+','+$.quantity+','+$.portion+','+$.modifiers+',' + $.discount + ',' + $.discount_type + ',' + $.price + ',' + $.modifiers_price"
    ).toArray()
})
</script>

