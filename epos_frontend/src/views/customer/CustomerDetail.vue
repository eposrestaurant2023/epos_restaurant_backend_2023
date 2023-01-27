<template>
<v-dialog v-model="open">
  <v-card width="700" class="mx-auto my-0">
        <ComToolbar @onClose="onClose" >
            <template #title>
                Customer Detail- {{params.name}}
            </template>
        </ComToolbar>
        <v-img
        height="200"
        src="https://cdn.pixabay.com/photo/2020/07/12/07/47/bee-5396362_1280.jpg"
        cover
        class="text-white"
      />

      <v-avatar size="100" class="-mt-14 mx-auto">
        <v-img
          src="https://cdn.vuetifyjs.com/images/john.jpg"
          alt="John"
          
        ></v-img>
      </v-avatar>
      <div class="text-h5 mx-auto mt-2">{{ customer.doc?.name }} - {{ customer.doc?.customer_name_en }}</div>
      <v-row no-gutters>
        <v-col >
          <v-card class="pa-2 ma-6" elevation="2" color="primary">
            <div class="text-h6 text-center" >{{ orderSummary.data?.total_visit }}</div>
            <div class="text-body-1 text-center mt-2" >Total Visiit</div>
          </v-card>
        </v-col>
        <v-col>
          <v-card class="pa-2 ma-6" elevation="2" color="warning">
            <div class="text-h6 text-center" ><CurrencyFormat :value="orderSummary.data?.total_order"/></div>
            <div class="text-body-1 text-center mt-2" >Total Annual Order</div>
          </v-card>
        </v-col>
        <v-col>
          <v-card class="pa-2 ma-6" elevation="2" color="success">
            <div class="text-h6 text-center" ><CurrencyFormat :value="orderSummary.data?.total_annual_order"/></div>
            <div class="text-body-1 text-center mt-2" >Total Order</div>
          </v-card>
        </v-col>
      </v-row>
      <v-tabs
        v-model="tab"
        color="deep-purple-accent-4"
        align-tabs="start"
        class="ma-4"
      >
        <v-tab value="about">About</v-tab>
        <v-tab value="recentOrder">Recent Order</v-tab>
        
  </v-tabs>
  <v-window v-model="tab" >
    <v-window-item value="about">
      <table class="ml-10 mb-4">
        <p class="font-weight-black">
          CONTACT IMFORMATION
        </p>
        <table class="ml-5">
          <tr>
            <td>Phone</td>
            <td>:</td>
            <td>{{ customer.doc?.phone_number }}</td>
          </tr>
          <tr>
            <td>Address</td>
            <td>:</td>
            <td>{{ customer.doc?.province }} - {{ customer.doc?.country }}</td>
          </tr>
          <tr>
            <td>Email address</td>
            <td>:</td>
            <td>{{ customer.doc?.email_address }}</td>
          </tr>
        </table>
          <p class="font-weight-black">
            BASIC IMFORMATION
          </p>
          <table class="ml-5">
            <tr>
                <td>Birthday</td>
                <td>:</td>
                <td>{{ customer.doc?.date_of_birth }}</td>
              </tr>
              <tr>
                <td>Gender</td>
                <td>:</td>
                <td>{{ customer.doc?.gender }}</td>
              </tr>
          </table>
        </table>
      </v-window-item>
    <v-window-item value="recentOrder">
      <v-table class="ml-10 mb-4">
        <thead>
            <tr>
              <th>
                No
              </th>
              <th>
                Grand Total
              </th>
              <th>
                Total Quantity
              </th>
              <th class="text-left">
                Modified
              </th>
            </tr>
        </thead>
      </v-table>
    </v-window-item>
  </v-window>
  </v-card>
  </v-dialog>

</template>
<script setup>
  import { ref, defineProps,defineEmits, createDocumentResource,createResource } from '@/plugin'
  import ComToolbar from '@/components/ComToolbar.vue';


  const props = defineProps({
    params:{
      type:Object,
      required: true,
    }
  })

  const emit = defineEmits(["resolve","reject"]) 

  const open = ref(true);
  const tab = ref(null);

  function onClose(){
    emit("resolve",false);
  }
 
  let customer = createDocumentResource({
    url: 'frappe.client.get',
    doctype: 'Customer',
    name: props.params.name,
    auto:true
  })

  let recentOrder = createResource(
    {
      url:'frappe.client.get_list',
      params:{
        doctype:"Sale",
        fields:["name","grand_total","total_quantity","modified"],
        filters:{customer:props.params.name},
        order_by: "modified desc",
        limit_page_length: 5
      },
      auto:true
    }
  )
  let orderSummary = createResource(
    {
      url:'epos_restaurant_2023.selling.doctype.customer.customer.get_customer_order_summary',
      params:{
        customer: props.params.name,
      },
      auto:true
    }
  )
</script>