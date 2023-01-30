<template>
  <div class="-mx-3 bg-blue-100 rounded-tl-md rounded-tr-md text-sm">
    <div class="px-3 py-2" v-if="(sale.sale.total_discount + sale.sale.total_tax) > 0">
      <div class="flex justify-between my-1">
        <div>
          Sub Total
        </div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.sub_total" />
        </div>
      </div>
      <div class="flex justify-between mb-1" v-if="sale.sale.product_discount > 0">
        <div>Product Discount</div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.product_discount" />
        </div>
      </div>
      <div class="flex justify-between mb-1" v-if="sale.sale.sale_discount > 0">
        <div>Sale Discount
          <span v-if="sale.sale.discount && sale.sale.discount_type=='Percent'"> - {{ sale.sale.discount }}%</span>
        </div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.sale_discount" />
        </div>
      </div>
      <div class="flex justify-between mb-1" v-if="sale.sale.sale_discount > 0 && sale.sale.product_discount > 0">
        <div>Total Discount</div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.total_discount" />
        </div>
      </div>
      <div class="flex justify-between mb-1" v-if="sale.sale.tax_1_amount > 0">
        <div>
          {{ setting.tax_1_name }}
        </div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.tax_1_amount" />
        </div>
      </div>
      <div class="flex justify-between mb-1" v-if="sale.sale.tax_2_amount > 0">
        <div>{{ setting.tax_2_name }}</div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.tax_2_amount" />
        </div>
      </div>
      <div class="flex justify-between mb-1" v-if="sale.sale.tax_3_amount > 0">
        <div>
          {{ setting.tax_3_name }}
        </div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.tax_3_amount" />
        </div>
      </div>
      <div class="flex justify-between" v-if="sale.sale.total_tax > 0">
        <div>Total Tax</div>
        <div class="font-bold">
          <CurrencyFormat :value="sale.sale.total_tax" />
        </div>
      </div>
    </div>
    <div class="overflow-hidden">
      <div class="button-group">
        <div class="d-flex text-center">
          <div class="cursor-pointer bg-red-800 p-2" @click="onToTableLayout">
            <v-icon icon="mdi-reply" color="white"/>
          </div>
          <!-- <v-btn color="error" round="0" icon="mdi-reply" class="btn-back-layout" @click="onToTableLayout"></v-btn> -->
          
          <ComPrintBillButton v-if="sale.sale.sale_status!='Bill Requested'" doctype="Sale" title="Print Bill" @onPrint="onPrintBill" />
          <div class="bg-red-600 text-white cursor-pointer grow p-2 hover:bg-red-700" v-else @click="onCancelPrintBill">Cancel Print Bill</div>
          
          <ComDiscountButton />
          <div class="cursor-pointer p-2 grow bg-orange-600 text-white hover:bg-orange-700" @click="onQuickPay">Quick Pay</div>
          <ComSaleButtonMore />
        </div>
      </div>
    </div>
    <div>
      <div class="flex">
        <div class="w-4/5 cursor-pointer bg-green-600 text-white p-3 hover:bg-green-700" @click="onPayment()">
          <div class="flex justify-between mb-2 text-xl">
            <div>Payment</div>
            <div>
              <CurrencyFormat :value="sale.sale.grand_total" />
            </div>
          </div>
          <div class="flex justify-between text-xl">
            <div>Total Quantity : <span>{{ sale.sale.total_quantity }}</span></div>
            <div>
              <ComExchangeRate />
              <CurrencyFormat :value="sale.sale.grand_total * sale.sale.exchange_rate"
                :currency="setting.pos_setting.second_currency_name" />
            </div>
          </div>
        </div>
        <div class="w-1/5">
          <div
            class="w-full h-full cursor-pointer flex justify-center items-center bg-blue-600 text-white p-3 hover:bg-blue-700 text-center"
            @click="onSubmit()">
            <div>
              <v-icon icon="mdi-arrow-right-thick"></v-icon>
              <div>Submit Order</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>

</template>
<script setup>
import { inject, useRouter, confirmBackToTableLayout } from '@/plugin';
import ComDiscountButton from './ComDiscountButton.vue';
import ComExchangeRate from './ComExchangeRate.vue';
import ComPrintBillButton from './ComPrintBillButton.vue';
import ComSaleButtonMore from './ComSaleButtonMore.vue';
import Enumerable from 'linq';
import {createToaster} from '@meforma/vue-toaster';
const router = useRouter()
const sale = inject("$sale")
const gv = inject("$gv")
const setting = gv.setting;
const toaster = createToaster({position:"top"})


async function onSubmit() {
  if (!sale.isBillRequested()) {
    sale.sale.sale_status = "Submitted";
    await sale.onSubmit().then((value) => {

      if (value) {
        router.push({ name: "TableLayout" });
      }
    });
  }
}

async function onPrintBill(r) {
  if (sale.sale.sale_products.length == 0) {
    toaster.warning("Please select a menu item to submit order");
  } else {
    sale.sale.sale_status = "Bill Requested";
    sale.action = "print_bill";
    sale.pos_receipt = r;

    await sale.onSubmit().then(async (value) => {
      if (value) {
        router.push({ name: "TableLayout" });
      }
    });
  }
}

async function onQuickPay() {

  await sale.onSubmitQuickPay().then((value) => {
    if (value) {
      router.push({ name: "TableLayout" });
    }
  });
}

function onCancelPrintBill(){
  alert("authorize")
  alert("note")
 
 
  sale.sale.sale_status = "Submitted";
  sale.sale.sale_status_color = setting.sale_status.find(r=>r.name=='Submitted').background_color;
  alert("writ to audit trail");

}

function onNote() {
  if (!sale.isBillRequested()) {
  alert('submit')
  }
}
function onSave() {
  alert('submit')
}
function onDiscount() {
  if (!sale.isBillRequested()) {
  alert('submit')
  }
}

async function onToTableLayout() {
  const sp = Enumerable.from(sale.sale.sale_products);

  if (sp.where("$.name==undefined").toArray().length > 0) {
    let result = await confirmBackToTableLayout({});
    if (result) {
      if (result == "hold" || result == "submit") {
        if (result == "hold") {
          sale.sale.sale_status = "Hold Order";
          sale.action = "hold_order";
        } else {
          sale.sale.sale_status = "Submitted";
          sale.action = "submit_order";
        }
        await sale.onSubmit().then(async (value) => {
          if (value) {
            router.push({ name: "TableLayout" });
          }
        });
      }else{
        //continue
        router.push({ name: "TableLayout" })
      }
    }
  }else{ 
    router.push({ name: "TableLayout" })
  }

}



</script>