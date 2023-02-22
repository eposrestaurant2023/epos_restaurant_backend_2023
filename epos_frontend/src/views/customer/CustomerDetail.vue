<template>
<v-dialog v-model="open" style="max-width: 900px; width: 100%;" :fullscreen="mobile" scrollable>
  <v-card class="mx-auto my-0 w-full">
        <ComToolbar @onClose="onClose">
            <template #title>
                Customer Detail - {{params.name}}
            </template>
        </ComToolbar>
        <v-card-text class="!p-0">
        <v-img
        height="100"
        src="https://cdn.pixabay.com/photo/2020/07/12/07/47/bee-5396362_1280.jpg"
        cover
        class="m-auto"
      />
      <template v-if="customer.doc">
        <div class="text-center relative">
          <v-avatar size="100" class="-mt-14 mx-auto" v-if="customer.doc?.photo">
            <v-img :src="customer.doc?.photo"></v-img>
          </v-avatar> 
          <avatar v-else :name="customer.doc.customer_name_en" class="-mt-14 mx-auto" size="100"></avatar>
        </div>
      </template>
      <div class="text-h5 text-center mt-2">{{ customer.doc?.name }} - {{ customer.doc?.customer_name_en }} 
        <v-icon icon="mdi-account-edit" @click="onAddCustomer" size="small"></v-icon>
      </div>
      <v-row no-gutters>
        <v-col cols="6" sm="4">
          <v-card class="pa-2 ma-2" elevation="2" color="primary">
            <div class="text-h6 text-center" >{{ orderSummary.data?.total_visit }}</div>
            <div class="text-body-1 text-center mt-2  text-sm" >Total Visit</div>
          </v-card>
        </v-col>
        <v-col cols="6" sm="4">
          <v-card class="pa-2 ma-2" elevation="2" color="warning">
            <div class="text-h6 text-center" ><CurrencyFormat :value="orderSummary.data?.total_annual_order"/></div>
            <div class="text-body-1 text-center mt-2 text-sm" >Total Annual Order</div>
          </v-card>
        </v-col>
        <v-col cols="12" sm="4">
          <v-card class="pa-2 ma-2" elevation="2" color="success">
            <div class="text-h6 text-center" ><CurrencyFormat :value="orderSummary.data?.total_order"/></div>
            <div class="text-body-1 text-center mt-2 text-sm" >Total Order</div>
          </v-card>
        </v-col>
      </v-row>
      <v-tabs
        v-model="tab"
        color="deep-purple-accent-4"
        align-tabs="start"
        class="ma-2"
      >
        <v-tab value="about">About</v-tab>   
        <v-tab value="recentOrder">Recent Order</v-tab>
        
  </v-tabs>
  <v-window v-model="tab">
    <v-window-item value="about">
      <v-table fixed-header class="ma-2">
        <p class="font-weight-bold pb-1">
          CONTACT INFORMATION
        </p>
        <table class="ml-2">
          <tr v-if="customer.doc?.phone_number || customer.doc?.phone_number_2">
            <td class="pr-4">Phone Number</td>
            <td class="pr-4">:</td>
            <td>
              <span v-if="customer.doc?.phone_number">{{ customer.doc?.phone_number }}</span>
              <span v-if="customer.doc?.phone_number_2"> - {{ customer.doc?.phone_number_2 }}</span>
            </td>
          </tr>
          <tr v-if="customer.doc?.province || customer.doc?.country">
            <td>Address</td>
            <td>:</td>
            <td>{{ customer.doc?.province }} - {{ customer.doc?.country }}</td>
          </tr>
          <tr v-if="customer.doc?.email_address">
            <td>Email address</td>
            <td>:</td>
            <td>{{ customer.doc?.email_address }}</td>
          </tr>
        </table>
        <p class="font-weight-bold py-2 pt-4">
            BASIC INFORMATION
        </p>
        <table class="ml-2">
            <tr v-if="customer.doc?.date_of_birth">
                <td class="pr-8">Date of Birth</td>
                <td class="pr-4">:</td>
                <td>{{ customer.doc?.date_of_birth }}</td>
              </tr>
              <tr v-if="customer.doc?.gender">
                <td>Gender</td>
                <td>:</td>
                <td>{{ customer.doc?.gender }}</td>
              </tr>
              <tr v-if="customer.doc?.company_name">
                <td>Company Name</td>
                <td>:</td>
                <td>{{ customer.doc?.company_name }}</td>
              </tr>
              <tr v-if="customer.doc?.customer_group">
                <td>Customer Group</td>
                <td>:</td>
                <td>{{ customer.doc?.customer_group }}</td>
              </tr>
        </table>
      </v-table>
    </v-window-item>
    <v-window-item value="recentOrder">
        <v-table fixed-header class="ma-2">
          <thead>
            <tr>
              <th class="text-left">
                No
              </th>
              <th class="text-left">
                Qty
              </th>
              <th class="text-left">
                Grand Total
              </th>
              <th class="text-left">
                Date
              </th>
            </tr>
          </thead>
          <tbody v-for=" d in recentOrder.data" :key="name">
            <tr>
            <td @click="onSaleDetail(d.name)">{{ d.name }}</td>
            <td class="pl-4">{{ d.total_quantity }}</td>
            <td class="pl-4"><CurrencyFormat :value="d.grand_total"/></td>
            <td v-if="d.modified"><Timeago   :long="long" :datetime="d.modified"/></td>
          </tr>
          </tbody>
        </v-table>
    </v-window-item>    
  </v-window>
</v-card-text>
  </v-card>
  </v-dialog>


</template>
<script setup>
  import { ref, defineProps,defineEmits, createDocumentResource,createResource, addCustomerDialog, useRouter, saleDetailDialog} from '@/plugin'
  import ComToolbar from '@/components/ComToolbar.vue';
  import { Timeago } from 'vue2-timeago';
  import { useDisplay } from 'vuetify'

const { mobile} = useDisplay()
  const props = defineProps({
    params:{
      type:Object,
      required: true,
    }
  })

  const emit = defineEmits(["resolve","reject"]) 

  let open = ref(true);
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
  async function onAddCustomer() { 
    await addCustomerDialog ({title:  customer.doc?.name+ ' - ' +  customer.doc?.customer_name_en, name: customer.doc?.name});
}

const router = useRouter()
function onSaleDetail(data) {
    saleDetailDialog({
      name:data
    });
}

</script>