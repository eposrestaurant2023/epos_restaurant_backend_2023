<template>
    <template v-if="printFormatResource.loading">
      <v-btn icon v-if="mobile">
          <v-icon>mdi-printer</v-icon>
      </v-btn>
      <v-btn stacked color="primary" size="small" class="m-1 grow" prepend-icon="mdi-printer" v-else>
        Print Bill
      </v-btn>
    </template>
    <template v-else>
        <div class="cursor-pointer m-1 rounded-md p-2 grow bg-blue-600 text-white hover:bg-blue-700" v-if="printFormatResource.data.length==1" @click="onPrintReport(printFormatResource.data[0])" >
            <v-icon icon="mdi-printer"></v-icon>
        </div>
        <v-menu v-else>
        <template v-slot:activator="{ props }">
          <v-btn icon v-if="mobile" @click="$emit('onClose')" v-bind="props">
            <v-icon>mdi-printer</v-icon>
          </v-btn>
            <v-btn v-else stacked color="primary" size="small" class="m-1 grow" prepend-icon="mdi-printer"  @click="$emit('onClose')" :loading="printFormatResource.loading" v-bind="props">
              Print Bill
            </v-btn>
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
    import {defineProps,defineEmits, createResource, createToaster,inject,useRouter} from "@/plugin"

    const router = useRouter();
 
    const sale = inject('$sale')
    const props = defineProps({
        doctype:String,
        title:{
            type:String,
            default:""
        },
        mobile: {
          type: Boolean,
          default: false
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
  if (sale.sale.sale_products?.length == 0) {
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
        content:`Print request bill`
              
      });
    await sale.onSubmit().then(async (value) => {
      if (value) {
        
        router.push({ name: "TableLayout" });
      }
    });
  }
}
</script>