<template>
    <div class="p-1 border border-gray-600 rounded-md">
        <div class="flex">
            <div class="flex-auto cursor-pointer" @click="onSearchCustomer">
                <div class="flex items-center">
                    <v-avatar v-if="sale.sale.customer_photo">
                        <v-img :src="sale.sale.customer_photo"></v-img>
                    </v-avatar>
                    <template v-if="sale.sale.customer_name != undefined">
                        <avatar v-if="sale.sale.customer_photo == undefined" :name="sale.sale.customer_name"
                            class="mr-4" size="40"></avatar>
                    </template>
                    <div class="px-2">
                        <div class="font-bold">{{ sale.sale.customer_name }}</div>
                        <div class="text-gray-400 text-sm">{{ subTitle }}</div>
                    </div>
                    <div> 
                        <template v-if="customerPromotion?.length > 0">
                            <ComChip v-for="(item, index) in customerPromotion" :key="index" color="orange" tooltip="Happy Hour Promotion" prepend-icon="mdi-tag-multiple">{{ item.promotion_name }}</ComChip>
                        </template>
                        <v-chip v-else-if="sale.sale.customer_default_discount > 0" color="error">{{ sale.sale.customer_group }} % OFF</v-chip>
                    </div>
                </div>
            </div>
            <div class="flex-none" v-if="sale.sale.customer != setting.customer">
                <v-btn size="small" variant="text" color="primary" icon="mdi-account-plus"
                    @click="onAddCustomer()"></v-btn>
                <v-btn size="small" variant="text" color="primary" icon="mdi-account-edit"
                    @click="onViewCustomerDetail()"></v-btn>
                <v-btn size="small" variant="text" color="error" icon="mdi-delete" @click="onRemove()"></v-btn>

            </div>
            <div class="flex-none" v-else>
                    <v-btn size="small" variant="text" color="primary" icon="mdi-magnify"
                    @click="onSearchCustomer()"></v-btn>
                    <v-btn size="small" variant="text" color="primary" icon="mdi-account-plus"
                    @click="onAddCustomer()"></v-btn>
                <v-btn size="small" variant="text" color="primary" icon="mdi-qrcode-scan"
                    @click="onScanCustomerCode()"></v-btn>
            </div>

        </div>
    </div>
</template>
<script setup>
import { computed, inject,ref, searchCustomerDialog,createResource, customerDetailDialog, scanCustomerCodeDialog, confirmDialog, onMounted,createToaster,addCustomerDialog } from "@/plugin"
import ComCustomerPromotionChip from "./ComCustomerPromotionChip.vue";
const sale = inject("$sale")
const gv = inject("$gv")
const socket = inject("$socket")
const moment = inject("$moment")
const toaster = createToaster({ position: "top" });

let customerPromotion = computed({
    get(){
        return gv.getPromotionByCustomerGroup(sale.sale.customer_group)
    },
    set(newValue){
        return newValue
    }
})
async function onSearchCustomer() {
    if (!sale.isBillRequested()) {
        const result = await searchCustomerDialog({});
        if (result) {
            assignCustomerToOrder(result);
        }
    }
}

function assignCustomerToOrder(result) {
    sale.sale.customer = result.name;
    sale.sale.customer_name = result.customer_name_en;
    sale.sale.customer_photo = result.photo;
    sale.sale.phone_number = result.phone_number;
    sale.sale.customer_group = result.customer_group;
    sale.sale.customer_default_discount = result.default_discount

    if(sale.promotion){
        customerPromotion.value = gv.getPromotionByCustomerGroup(sale.sale.customer_group)
        //sale.promotion.customer_groups.filter(r=>r.customer_group_name_en == result.customer_group).length > 0
 
        if(customerPromotion.value && customerPromotion.value.length > 0){
            customerPromotion.value.forEach((r)=>{
                toaster.info(`This customer has happy hours promotion ${r.promotion_name} : ${(( r.percentage_discount || 0))}%`);
            }) 
            updateProductAfterSelectCustomer(customerPromotion.value)
        }
    } 
    if (parseFloat(result.default_discount) && !customerPromotion.value) {
 
        sale.sale.discount_type="Percent";
        sale.sale.discount = parseFloat(result.default_discount);
        sale.updateSaleSummary();
        toaster.info("This customer has default discount " + sale.sale.discount + '%');
    }

    socket.emit("ShowOrderInCustomerDisplay",sale.sale);
}

const setting = computed(() => {
    return JSON.parse(localStorage.getItem('setting'))
})
const subTitle = computed(() => {
    let title = sale.sale.customer;
    if (sale.sale.phone_number != "" && sale.sale.phone_number != undefined) {
        title = title + " / " + sale.sale.phone_number
    }

    return title;
})

function onViewCustomerDetail() {
    customerDetailDialog({
        name: sale.sale.customer
    });
}
function updateProductAfterSelectCustomer(pro){
    const promotions = JSON.parse(JSON.stringify(pro))
    if(sale.sale.sale_products.length > 0){
        let product_checks = []
        sale.sale.sale_products.forEach((r)=>{
            product_checks.push({
                product_code: r.product_code,
                order_time: r.order_time
            })
        })
        createResource({
            url: 'epos_restaurant_2023.api.promotion.get_promotion_products',
            auto: true,
            params: { 
                products: product_checks,
                promotions: promotions
            },
            onSuccess(doc) {
                if(doc){
                    console.log(doc)
                    /// update products promotion
                    // doc.forEach(r=>{
                    //     let sale_products = sale.sale.sale_products.filter(x=>x.product_code == r.product_code)

                    //     console.log(sale_products)
                    //     sale_products.forEach((s)=>{
                    //         s.discount_type = (sale.promotion.info.start_time > moment(s.order_time).format('HH:mm:ss') ? 'Amount' : 'Percent')
                    //         s.discount = (sale.promotion.info.start_time > moment(s.order_time).format('HH:mm:ss') ? 0 : (sale.promotion?.info?.percentage_discount || 0))
                    //         s.happy_hours_promotion_title = (sale.promotion.info.start_time > moment(s.order_time).format('HH:mm:ss') ? '' : (sale.promotion?.info?.happy_hours_promotion_title || ''))
                    //         s.happy_hour_promotion = (sale.promotion.info.start_time > moment(s.order_time).format('HH:mm:ss') ? '' : (sale.promotion?.info?.name || ''))

                    //         console.log(s.happy_hour_promotion)
                    //     })
                    // })
                }else{
                    gv.promotion = null
                    sale.promotion = null
                }
            }
        });
    }
    // if(sale.sale.sale_products.length > 0){
    //     if(is_promotion){
    //         let product_checks = []
    //         sale.sale.sale_products.forEach(r => {
    //             product_checks.push({product_code: r.product_code, order_time: r.order_time})
    //         });
    //         createResource({
    //             url: 'epos_restaurant_2023.api.promotion.get_promotion_products',
    //             auto: true,
    //             params: { 
    //                 products: product_checks,
    //                 promotion_name: sale.promotion.info.name || ''
    //             },
    //             onSuccess(doc) {
    //                 if(doc){
    //                     console.log(doc)
    //                     /// update products promotion
    //                     doc.forEach(r=>{
    //                         let sale_products = sale.sale.sale_products.filter(x=>x.product_code == r.product_code)

    //                         console.log(sale_products)
    //                         sale_products.forEach((s)=>{
    //                             s.discount_type = (sale.promotion.info.start_time > moment(s.order_time).format('HH:mm:ss') ? 'Amount' : 'Percent')
    //                             s.discount = (sale.promotion.info.start_time > moment(s.order_time).format('HH:mm:ss') ? 0 : (sale.promotion?.info?.percentage_discount || 0))
    //                             s.happy_hours_promotion_title = (sale.promotion.info.start_time > moment(s.order_time).format('HH:mm:ss') ? '' : (sale.promotion?.info?.happy_hours_promotion_title || ''))
    //                             s.happy_hour_promotion = (sale.promotion.info.start_time > moment(s.order_time).format('HH:mm:ss') ? '' : (sale.promotion?.info?.name || ''))

    //                             console.log(s.happy_hour_promotion)
    //                         })
    //                     })
    //                 }else{
    //                     gv.promotion = null
    //                     sale.promotion = null
    //                 }
    //             }
    //         });
    //     }else{
    //     sale.sale.sale_products = sale.sale.sale_products.map(r=>{
    //             return {
    //                 ...r,
    //                 discount: 0,
    //                 happy_hours_promotion_title: '',
    //                 happy_hour_promotion: ''
    //             }
    //         })
    //     }
        
    // }
}
async function onScanCustomerCode() {
    if (!sale.isBillRequested()) {
        const result = await scanCustomerCodeDialog({});
        if (result) {

            assignCustomerToOrder(result);
        }
    }
}

async function onRemove() {
    if (!sale.isBillRequested()) {
        if (sale.sale.discount > 0) {
            if (await confirmDialog({ title: "Remove Discount", text: "Remove discount from this sale order?" })) {
                
                sale.sale.discount = 0;
                sale.updateSaleSummary();
            }
        } 
        sale.sale.customer = setting.value.customer
        sale.sale.customer_name = setting.value.customer_name
        sale.sale.customer_photo = setting.value.customer_photo
    }
}
async function onAddCustomer() {
    if (!sale.isBillRequested()) {
        const result = await addCustomerDialog ({title: "New Customer", value:  ''});
        if(result != false){
            sale.sale.customer = result.name
            sale.sale.customer_name = result.customer_name_en
            sale.sale.customer_photo = result.photo
            sale.sale.phone_number = result.phone_number && result.phone_number_2 ? result.phone_number + ' / ' + result.phone_number_2 : (result.phone_number ? result.phone_number : result.phone_number_2)
            sale.sale.customer_group = result.customer_group
        }
    }
}

onMounted(() => {
    if(!sale.sale.customer){
        onRemove()
    }
})

</script>