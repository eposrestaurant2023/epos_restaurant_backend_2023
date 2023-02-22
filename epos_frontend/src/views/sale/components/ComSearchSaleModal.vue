<template>
  <v-dialog :fullscreen="mobile" v-model="open" @update:modelValue="onClose" :style="mobile ? '' : 'width: 100%;max-width:800px'">
    <v-card class="mx-auto my-0 w-full">
      <ComToolbar @onClose="onClose">
        <template #title>
          Select Sale
        </template>
      </ComToolbar>
      <div>
        <ComInput 
          autofocus
          ref="searchTextField"
          keyboard
          class="m-4"
          v-model="search"
          placeholder="Search Sale"
          v-debounce="onSearch"
          @onInput="onSearch"/>
      </div>
      <div class="overflow-auto px-4 pb-4">
        <ComPlaceholder :is-not-empty="dataResource.data?.length>0" :loading="dataResource.loading"
          text="There is not sale" icon="mdi-account-outline">
          <v-card v-for="(s, index) in dataResource.data" :key="index"
            @click="onSelect(s)" class="mb-4">
            <template v-slot:title>
              <div>
                <span class="text-sm">{{ s.name }} - {{ s.customer_name }}</span> <v-chip :color="s.sale_status_color" :size="mobile ? 'x-small' : 'small'">{{ s.sale_status }}</v-chip>
              </div>
            </template>
            <template v-slot:subtitle>
              <div class="-m-1">
                <v-chip size="x-small" class="m-1" prepend-icon="mdi-chair-school" color="blue" v-if="s.tbl_number">{{ s.tbl_number }}</v-chip>
                <v-chip size="x-small" class="m-1" prepend-icon="mdi-account-multiple-outline" v-if="s.guest_cover > 0">{{ s.guest_cover }}</v-chip>
                <span class="m-1" v-if="s.phone_number">{{s.phone_number}}</span>
              </div>
            </template>
            <template v-slot:append>
              <CurrencyFormat :value="s.grand_total"/>
            </template>
            <v-card-text>
              <v-row class="!m-0">
                <v-col cols="12" sm="6" class="!p-0">
                  <table class="text-sm text-gray-500"> 
                  <tr>
                      <td>Date</td>
                      <td class=" px-2">:</td>
                      <td>{{ s.posting_date }}</td>
                  </tr>
                  <tr>
                      <td>Qty</td>
                      <td class=" px-2">:</td>
                      <td>{{ s.total_quantity }}</td>
                  </tr>
                  <tr v-if="(s.total_discount + s.total_tax) > 0">
                      <td>Sub Total</td>
                      <td class="px-2">:</td>
                      <td><CurrencyFormat :value="s.sub_total"></CurrencyFormat></td>
                  </tr>
              </table>
                </v-col>
                <v-col  cols="12" sm="6" class="!p-0">
                  <table class="text-sm text-gray-500">
                    <tr v-if="s.product_discount > 0">
                      <td>Product Discount</td>
                      <td class="px-2">:</td>
                      <td><CurrencyFormat :value="s.product_discount"/></td>
                    </tr>
                    <tr v-if="s.sale_discount > 0">
                      <td>Sale Discount</td>
                      <td class="px-2">:</td>
                      <td><CurrencyFormat :value="s.sale_discount"/></td>
                    </tr>
                    <tr v-if="s.product_discount > 0 && s.sale_discount > 0">
                      <td>Total Discount</td>
                      <td class=" px-2">:</td>
                      <td><CurrencyFormat :value="s.total_discount"></CurrencyFormat></td>
                  </tr>
                  <tr class="text-green-600" v-if="s.total_paid > 0">
                      <td>Total Paid</td>
                      <td class=" px-2">:</td>
                      <td><CurrencyFormat :value="s.total_paid"></CurrencyFormat></td>
                  </tr>
                  <tr class="text-red-500" v-if="s.balance > 0">
                      <td>Balance</td>
                      <td class=" px-2">:</td>
                      <td><CurrencyFormat :value="s.balance"></CurrencyFormat></td>
                  </tr>
                  </table>
                </v-col>
              </v-row>
              
            </v-card-text>
            <v-card-actions class="justify-between text-sm !py-0">
                <div><v-icon icon="mdi-clock" size="x-small"/> {{ moment(s.creation).format('yyyy-MM-DD h:mm:ss a') }}</div>
                <div><v-icon icon="mdi-account-outline" size="x-small"/> {{ s.created_by }}</div>
            </v-card-actions>
          </v-card>
        </ComPlaceholder>
      </div>
    </v-card>
  </v-dialog>


</template>
<script setup>
  import moment from '@/utils/moment.js';
  import { addCustomerDialog, ref, defineProps, defineEmits, createResource,inject } from '@/plugin'
  import ComToolbar from '@/components/ComToolbar.vue';
  import ComInput from '@/components/form/ComInput.vue';
  import { useDisplay } from 'vuetify'
 
 const { mobile } = useDisplay()

  const gv = inject("$gv");
  const sale = inject("$sale");
  const searchTextField = ref(null)
  const props = defineProps({
    params: {
      type: Object,
      required: true,
    }
  })
  
  const emit = defineEmits(["resolve", "reject"])

  let open = ref(true);
  let search = ref('')
  const searchFields = ref(["name"]);


  if(gv.saleMeta==null){
    createResource({
    url: "epos_restaurant_2023.api.api.get_meta",
    params: {
      doctype: "Sale"
    },
    auto:true,
    cache:["sale_meta_data"],
    onSuccess(doc){
      gv.saleMeta = doc;
      if(dos.search_fields){ 
        dos.search_fields.split(",").forEach(function(d){
          searchFields.value.push(d.trim())
        });
      }
    }

  })
  }else {
    if(gv.saleMeta.search_fields){
      gv.saleMeta.search_fields.split(",").forEach(function(d){
          searchFields.value.push(d.trim())
        });
      } 
  }


  const dataResource = createResource({
    url: "frappe.client.get_list",
    params: getDataResourceParams(),
    auto: true
  });

  
 
  function getDataResourceParams (){ 
    return {  
        doctype: "Sale",
        fields: ["name","creation","total_tax", "posting_date", "customer", "customer_name", "phone_number", "customer_group", "sale_status", "tbl_number","created_by","total_quantity","discount_type","discount","sub_total","product_discount","sale_discount","total_discount","grand_total","total_paid","balance","guest_cover","sale_status_color"],
        order_by: "modified desc",
        filters: {
          sale_status: ['=','Submitted'],
          working_day: ['=', sale.sale.working_day],
          cashier_shift: ['=', sale.sale.cashier_shift],
          balance: ['>',0]
        },
        or_filters: getFilter(),
        limit_page_length: 20
    }
  }

  function getFilter(){
    let filters = {};
     searchFields.value.forEach((r)=>{
      filters[r] = ["like",'%'+ search.value + '%']
     })
    
     return filters;
  }


  function onSearch(keyword) {
    search.value = keyword;
    dataResource.params = getDataResourceParams()
    dataResource.fetch()
  }
  function onClose() {
    emit("resolve", false);
  }

  function onSelect(c) {
    emit("resolve", c);
  }
 
</script>