<template>
    <div class="wrap"> 
        
        <v-row class="h-full !m-0">
            <v-col class="h-full !p-0" cols="hide" xs="12" sm="8" md="8" lg="8" xl="8">
                <v-carousel height="100vh" cycle :show-arrows="false" hide-delimiters :interval="6000" vertical-delimiters>
                    <v-carousel-item :src="item.photo" cover v-for="(item, index) in gv.setting.pos_setting.customer_display_slideshow" :key="index"></v-carousel-item> 
                </v-carousel>
            </v-col>
            <v-col class="h-100vh !p-0" cols="12" xs="12" sm="4" md="4" lg="4" xl="4">
                <div class="h-full flex-col flex px-1">
                    <div class="profile">
                        <div style=" border-bottom: 1px solid black; width: 100%; height: 60px; margin: 0; ">
                            <div style=" height: 50px;
                                width: 50px;
                                background: red;
                                margin-top: 30px;
                                border-radius: 25px;"></div>

                        </div>
         
                    </div>
                    <div class="product-list overflow-auto h-full">
                        <ul class="bg-violet-950" style=" padding: 12px;">
                            <li v-for="(p, index) in data.sale_products" :key="index" class="border-b">
                                <div style="display: flex;">
                                    <div style=" height: 50px; width: 50px; 
                                    background: green;
                                        margin: 10px 15px 0px -10px;
                                        border-radius: 25px"></div>
                                    <div>
                                        <div>{{ p.product_code }}</div>
                                        <div>{{ p.price }} x {{ p.quantity }}</div>
                                        {{ p.product_name }}
                                    </div>
                                    <div style="
                                            position: absolute;
                                            right: 3%;
                                    
                                            font-size: 20px;
                                        "><CurrencyFormat :value="p.amount"></CurrencyFormat></div>
                                </div>
                            </li>
                        </ul>
                    </div>
                    <div class="summary"> 
                        Total: <CurrencyFormat :value="data.grand_total" />
                    </div>
                </div>
            </v-col>
        </v-row>
    </div>
</template>
<script setup>
import { inject } from '@/plugin';
import { ref } from 'vue';
const data = ref({
    "doctype": "Sale",
    "sale_status": "New",
    "cashier_shift": "CS2023-0020",
    "working_day": "WD2023-0014",
    "exchange_rate": 1,
    "table_id": "69f7404a62",
    "tbl_number": "S010",
    "pos_profile": "Main POS Profile",
    "pos_station_name": "Admin",
    "customer": "000001",
    "customer_photo": "/files/girl477476.jpg",
    "customer_name": "General",
    "price_rule": "Normal Rate",
    "business_branch": "Main Branchs",
    "sale_products": [
        {
            "menu_product_name": "025d88c4ed",
            "product_code": "FR0001",
            "product_name": "Fried Rice",
            "product_name_kh": "បាយឆា",
            "unit": "Unit",
            "quantity": 1,
            "sub_total": 2,
            "total_discount": 0,
            "total_tax": 0,
            "discount_amount": 0,
            "sale_discount_amount": 0,
            "note": "",
            "price": 2,
            "modifiers_price": 0,
            "product_photo": null,
            "selected": false,
            "modified": "2023-04-07 15:25:15.704",
            "append_quantity": 1,
            "allow_discount": 1,
            "allow_free": 1,
            "allow_change_price": 1,
            "is_open_product": 0,
            "portion": "ចានតូច",
            "modifiers": " Pork",
            "modifiers_data": "[{\"name\":\"3d9d0cf744\",\"modifier\":\"Pork\",\"price\":0}]",
            "is_free": 0,
            "sale_product_status": "New",
            "discount_type": "Percent",
            "discount": 0,
            "order_by": "Administrator",
            "order_time": "2023-04-07 15:25:15.705",
            "printers": "[]",
            "amount": 2
        },
        {
            "menu_product_name": "025d88c4ed",
            "product_code": "FR0001",
            "product_name": "Fried Rice",
            "product_name_kh": "បាយឆា",
            "unit": "Unit",
            "quantity": 1,
            "sub_total": 2,
            "total_discount": 0,
            "total_tax": 0,
            "discount_amount": 0,
            "sale_discount_amount": 0,
            "note": "",
            "price": 2,
            "modifiers_price": 0,
            "product_photo": null,
            "selected": false,
            "modified": "2023-04-07 15:27:22.977",
            "append_quantity": 1,
            "allow_discount": 1,
            "allow_free": 1,
            "allow_change_price": 1,
            "is_open_product": 0,
            "portion": "ចានតូច",
            "modifiers": "",
            "modifiers_data": "[]",
            "is_free": 0,
            "sale_product_status": "New",
            "discount_type": "Percent",
            "discount": 0,
            "order_by": "Administrator",
            "order_time": "2023-04-07 15:25:15.705",
            "printers": "[]",
            "amount": 2
        },
        {
            "menu_product_name": "3b5c1a24f8",
            "product_code": "001",
            "product_name": "Big fish fries",
            "product_name_kh": "Open Food",
            "unit": "Unit",
            "quantity": 1,
            "sub_total": 0,
            "total_discount": 0,
            "total_tax": 0,
            "discount_amount": 0,
            "sale_discount_amount": 0,
            "note": "",
            "price": 0,
            "modifiers_price": 0,
            "product_photo": null,
            "selected": false,
            "modified": "2023-04-07 15:32:42.152",
            "append_quantity": 0,
            "allow_discount": 0,
            "allow_free": 1,
            "allow_change_price": 1,
            "is_open_product": 1,
            "portion": "",
            "modifiers": "",
            "is_free": 0,
            "sale_product_status": "New",
            "discount_type": "Percent",
            "discount": 0,
            "order_by": "Administrator",
            "order_time": "2023-04-07 15:25:15.705",
            "printers": "[]",
            "amount": 0
        },
        {
            "menu_product_name": "9e62c03831",
            "product_code": "PW001",
            "product_name": "www",
            "product_name_kh": "www",
            "unit": "Unit",
            "quantity": 1,
            "sub_total": 8.5,
            "total_discount": 0,
            "total_tax": 0,
            "discount_amount": 0,
            "sale_discount_amount": 0,
            "note": "",
            "price": 8,
            "modifiers_price": 0.5,
            "product_photo": null,
            "selected": false,
            "modified": "2023-04-07 15:39:15.640",
            "append_quantity": 1,
            "allow_discount": 1,
            "allow_free": 1,
            "allow_change_price": 1,
            "is_open_product": 0,
            "portion": "Small",
            "modifiers": "Extrax 2 Eggs",
            "modifiers_data": "[{\"name\":\"dc1ec7690b\",\"modifier\":\"2 Eggs\",\"price\":0.5}]",
            "is_free": 0,
            "sale_product_status": "New",
            "discount_type": "Percent",
            "discount": 0,
            "order_by": "Administrator",
            "order_time": "2023-04-07 15:25:15.705",
            "printers": "[]",
            "amount": 8.5
        },
        {
            "menu_product_name": "025d88c4ed",
            "product_code": "FR0001",
            "product_name": "Fried Rice",
            "product_name_kh": "បាយឆា",
            "unit": "Unit",
            "quantity": 1,
            "sub_total": 3,
            "total_discount": 0,
            "total_tax": 0,
            "discount_amount": 0,
            "sale_discount_amount": 0,
            "note": "",
            "price": 3,
            "modifiers_price": 0,
            "product_photo": null,
            "selected": true,
            "modified": "2023-04-07 15:39:20.568",
            "append_quantity": 1,
            "allow_discount": 1,
            "allow_free": 1,
            "allow_change_price": 1,
            "is_open_product": 0,
            "portion": "ចានធំ",
            "modifiers": " Pork",
            "modifiers_data": "[{\"name\":\"3d9d0cf744\",\"modifier\":\"Pork\",\"price\":0}]",
            "is_free": 0,
            "sale_product_status": "New",
            "discount_type": "Percent",
            "discount": 0,
            "order_by": "Administrator",
            "order_time": "2023-04-07 15:25:15.705",
            "printers": "[]",
            "amount": 3
        }
    ],
    "sale_type": "Dine In",
    "discount_type": "Percent",
    "grand_total": 15.5,
    "guest_cover": 3,
    "discount": 0,
    "sub_total": 15.5,
    "payment": [],
    "posting_date": "2023-04-07",
    "total_quantity": 5,
    "sale_discountable_amount": 15.5,
    "sale_discount": 0,
    "product_discount": 0,
    "total_discount": 0,
    "tax_1_amount": 0,
    "tax_2_amount": 0,
    "tax_3_amount": 0,
    "total_tax": 0,
    "balance": 15.5
})

const socket = inject("$socket")
const gv = inject("$gv")
const sale = ref({})
const open = ref(true)
 
socket.on("ShowOrderInCustomerDisplay", (arg) => {
    sale.value = arg;
})




</script>
<style scoped>

</style>