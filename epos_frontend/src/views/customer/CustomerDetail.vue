<template >
  <ComModal isMoreMenu="true" @onPrint="onPrint" width="900px" @onClose="onClose"
    :hideOkButton="true" :fullscreen="mobile">
    <template #title>
      {{ $t('Customer Detail') }} - {{ params.name }}
    </template>
    <template #bar_custom>
      <v-btn append-icon="mdi-account-edit" @click="onAddCustomer()">{{ $t('Edit') }}</v-btn>
    </template>
    <template #content>
      <div class="-m-2">
        <v-img height="100" :src="background" cover class="m-auto" />
        <template v-if="customer.doc">
          <div class="text-center relative">
            <v-avatar size="100" class="-mt-14 mx-auto" v-if="customer.doc?.photo">
              <v-img :src="customer.doc?.photo"></v-img>
            </v-avatar>
            <avatar v-else :name="customer.doc.customer_name_en" class="-mt-14 mx-auto" size="100"></avatar>
          </div>
        </template>
        <div class="text-h5 text-center mt-2">{{ customer.doc?.name }} - {{ customer.doc?.customer_name_en }}</div>
        <v-row no-gutters>
          <v-col cols="6" sm="4">
            <v-card class="pa-2 ma-2" elevation="2" color="primary">
              <div class="text-h6 text-center">{{ orderSummary.data?.total_visit||0 }}</div>
              <div class="text-body-1 text-center mt-2  text-sm">{{ $t('Total Visit') }}</div>
            </v-card>
          </v-col>
          <v-col cols="6" sm="4">
            <v-card class="pa-2 ma-2" elevation="2" color="warning">
              <div class="text-h6 text-center">
                <CurrencyFormat :value="orderSummary.data?.total_annual_order" />
              </div>
              <div class="text-body-1 text-center mt-2 text-sm">{{ $t('Total Annual Order') }}</div>
            </v-card>
          </v-col>
          <v-col cols="12" sm="4">
            <v-card class="pa-2 ma-2" elevation="2" color="success">
              <div class="text-h6 text-center">
                <CurrencyFormat :value="orderSummary.data?.total_order" />
              </div>
              <div class="text-body-1 text-center mt-2 text-sm">{{ $t('Total Order') }}</div>
            </v-card>
          </v-col>
        </v-row>
        <v-tabs v-model="tab" color="deep-purple-accent-4" align-tabs="start" class="ma-2">
          <v-tab value="about">{{ $t('About') }}</v-tab>
          <v-tab value="recentOrder">{{ $t('Recent Order') }}</v-tab>

        </v-tabs>
        <v-window v-model="tab">
          <v-window-item value="about">
            <v-row>
              <v-col cols="6">
                <v-list>
                  <v-list-item
                    :subtitle="$t('Customer Group')"
                    :title="customer.doc?.customer_group"
                  ></v-list-item>
                  <v-list-item
                  v-if="customer.doc?.gender"
                    :subtitle="$t('Gender')"
                    :title="customer.doc?.gender"
                  ></v-list-item>
                  <v-list-item
                  v-if="customer.doc?.date_of_birth"
                    :subtitle="$t('Date Of Birth')"
                    :title="customer.doc?.date_of_birth"
                  ></v-list-item>
                  <v-list-item
                  v-if="customer.doc?.company_name"
                    :subtitle="$t('Company')"
                    :title="customer.doc?.company_name"
                  ></v-list-item>
                </v-list>
              </v-col>
              <v-col cols="6">
                <v-list>
                  <v-list-item
                    v-if="customerPhoneNumber"
                    :subtitle="$t('Phone Number')"
                    :title="customerPhoneNumber"
                  ></v-list-item>
                  <v-list-item
                  v-if="customer.doc?.email_address"
                    :subtitle="$t('Email Address')"
                    :title="customer.doc?.email_address"
                  ></v-list-item>
                  <v-list-item
                  v-if="customer.doc?.province"
                    :subtitle="$t('Province')"
                    :title="customer.doc?.province"
                  ></v-list-item>
                  <v-list-item
                  v-if="customer.doc?.country"
                    :subtitle="$t('Country')"
                    :title="customer.doc?.country"
                  ></v-list-item>
                  
              </v-list>
              </v-col>
            </v-row>
              
          </v-window-item>
          <v-window-item value="recentOrder">
            <v-table fixed-header class="ma-2">
              <thead>
                <tr>
                  <th class="text-left">
                    {{ $t('No') }}
                  </th>
                  <th class="text-left">
                    {{ $t('Qty') }}
                  </th>
                  <th class="text-left">
                    {{ $t('Grand Total') }}
                  </th>
                  <th class="text-left">
                    {{ $t('Date') }}
                  </th>
                </tr>
              </thead>
              <tbody v-for=" d in recentOrder.data" :key="name">
                <tr>
                  <td @click="onSaleDetail(d.name)">{{ d.name }}</td>
                  <td class="pl-4">{{ d.total_quantity }}</td>
                  <td class="pl-4">
                    <CurrencyFormat :value="d.grand_total" />
                  </td>
                  <td v-if="d.modified">
                    <Timeago :long="long" :datetime="d.modified" />
                  </td>
                </tr>
              </tbody>
            </v-table>
          </v-window-item>
        </v-window>
      </div>
    </template>
  </ComModal>
</template>
<script setup>
import { ref, defineProps, defineEmits, createDocumentResource, createResource, addCustomerDialog, useRouter, saleDetailDialog, computed,onMounted } from '@/plugin'
import { Timeago } from 'vue2-timeago';
import { useDisplay } from 'vuetify'
import ComModal from '../../components/ComModal.vue';

const background = JSON.parse(localStorage.getItem('setting')).login_background
const { mobile } = useDisplay()
const props = defineProps({
  params: {
    type: Object,
    required: true,
  }
})

const emit = defineEmits(["resolve", "reject"])

const customerPhoneNumber = computed(()=>{
  if(customer.doc?.phone_number_2 && customer.doc?.phone_number){
    return customer.doc?.phone_number + ' / ' +customer.doc?.phone_number_2
  }
  return customer.doc?.phone_number || '' + customer.doc?.phone_number_2 || ''
})
const tab = ref(null);

function onClose() {
  emit("resolve", false);
}

let customer = createDocumentResource({
  url: 'frappe.client.get',
  doctype: 'Customer',
  name: props.params.name,
  auto: true
})

let recentOrder = createResource(
  {
    url: 'frappe.client.get_list',
    params: {
      doctype: "Sale",
      fields: ["name", "grand_total", "total_quantity", "modified"],
      filters: { customer: props.params.name },
      order_by: "modified desc",
      limit_page_length: 5
    },
    auto: true
  }
)
let orderSummary =  {
  data:{

  }
};

onMounted(()=>{
      orderSummary = createResource( {
        url: 'epos_restaurant_2023.selling.doctype.customer.customer.get_customer_order_summary',
        params: {
          customer: props.params.name,
        },
        auto: true
      }
    )
})


async function onAddCustomer() {
  await addCustomerDialog({ title: customer.doc?.name + ' - ' + customer.doc?.customer_name_en, name: customer.doc?.name });
}

const router = useRouter()
async function onSaleDetail(data) {
  const result = await saleDetailDialog({
    name: data
  });
  
  if(result=="open_order" ){
    onClose(); 
  }
}

</script>