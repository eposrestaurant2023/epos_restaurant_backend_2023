<template>
    <template v-if="printFormatResource.loading">
      <v-btn icon v-if="mobile">
          <v-icon>mdi-printer</v-icon>
      </v-btn> 
      <div class="cursor-pointer p-2 grow bg-blue-600 text-white  hover:bg-blue-700" v-else >Print Bill</div>
    </template>
    <template v-else>
        <div class="cursor-pointer p-2 grow bg-blue-600 text-white hover:bg-blue-700" v-if="printFormatResource.data.length==1" @click="onPrintReport(printFormatResource.data[0])" >
            <v-icon icon="mdi-printer"></v-icon>
        </div>
        <v-menu v-else>
        <template v-slot:activator="{ props }">
          <v-btn icon v-if="mobile" @click="$emit('onClose')" v-bind="props">
            <v-icon>mdi-printer</v-icon>
        </v-btn>
            <div v-else class="cursor-pointer p-2 grow bg-blue-600 text-white hover:bg-blue-700" @click="$emit('onClose')" :loading="printFormatResource.loading" v-bind="props">
                Print Bill
            </div>
        </template>
        <v-list v-if="printFormatResource.data">
            <v-list-item v-for="(r, index) in printFormatResource.data" :key="index"  @click="onPrintReport(r)">
                <v-list-item-title>{{ r.name }}</v-list-item-title>
            </v-list-item>

        </v-list>
    </v-menu>
    </template>
       
</template>
<script setup>
    import {defineProps,defineEmits, createResource, createToaster,inject} from "@/plugin"
    import { useDisplay } from 'vuetify'
    const { mobile } = useDisplay()  
    const sale = inject('$sale')
    const props = defineProps({
        doctype:String,
        title:{
            type:String,
            default:""
        }
    });
    const emit = defineEmits(["onPrint"])
    const toaster = createToaster({poisition: 'top'})

    const printFormatResource = createResource({
        url: "epos_restaurant_2023.api.api.get_pos_print_format",
        params: {
            doctype:props.doctype
        },
        cache:["print_format",props.doctype],
        auto: true,
    })


    async function onPrintReport(r) {
  if (sale.sale.sale_products.length == 0) {
    toaster.warning("Please select a menu item to submit order");
  } else {
    sale.sale.sale_status = "Bill Requested";
    sale.action = "print_bill";
    sale.pos_receipt = r;

    //add to auddit trail
    sale.auditTrailLogs.push({
        doctype:"Comment",
        subject:"Print Bill",
        comment_type:"Comment",
        reference_doctype:"Sale",
        reference_name:"New",
        comment_by:"cashier@mail.com",
        content:`User sengho print bill. Amount:100$, Total Qty:5`
              
      });

    await sale.onSubmit().then(async (value) => {
      if (value) {
        router.push({ name: "TableLayout" });
      }
    });
  }
}
</script>