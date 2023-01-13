<template>
  <PageLayout title="Receipt List" icon="mdi-note-outline" full>
    <div>
        <div class="flex mb-4">
          <div class="w-44 ">
            <v-text-field
             density="compact"
            variant="solo"
            label="Sale Number"
            single-line
            hide-details
            v-debounce="onSearchSale('name','like',$event)"
          ></v-text-field>
          </div>
          <div class="w-44">
            <v-text-field label="Sale Number" v-model="current_date" variant="solo" single-line hide-details density="compact"
            v-debounce="onSearchSale('customer','like',$event)"
            ></v-text-field>
          </div>
          <div class="w-44">
            <v-text-field label="Sale Number" v-model="current_date" variant="solo" single-line hide-details density="compact"></v-text-field>
          </div>
          <div class="w-44">
            <v-text-field label="Sale Number" v-model="current_date" variant="solo" single-line hide-details density="compact"></v-text-field>
          </div>
          <div class="w-44">
            <v-text-field label="Sale Number" v-model="current_date" variant="solo" single-line hide-details density="compact"></v-text-field>
          </div>
        </div>
        <v-table fixed-header hover>
          <thead>
            <tr>
              <th>No</th>
              <th>Customer Name</th>
              <th class="text-center">Date</th>
              <th class="text-center">Qty</th>
              <th class="text-right">Sub Total</th>
              <th class="text-right">Total Discount</th>
              <th class="text-right">Grand Total</th>
              <th class="text-right">Total Paid</th>
              <th class="text-right">Balance</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="s in sales.data" :key="s.name">
              <td class="text-left"><v-btn variant="text" @click="onDialog(s.name)" class="pa-0">{{ s.name }}</v-btn></td>
              <td class="text-left"><v-btn variant="text" @click="onCustomer(s.customer)" class="pa-0">{{ s.customer }} - {{ s.customer_name }}</v-btn></td>
              <td class="text-center">{{ s.posting_date }}</td>
              <td class="text-center">{{ s.total_quantity }}</td>
              <td class="text-right">{{$filter.currency(s.sub_total) }}</td>
              <td class="text-right">{{$filter.currency(s.total_discount) }}</td>
              <td class="text-right">{{$filter.currency(s.grand_total) }}</td>
              <td class="text-right">{{$filter.currency(s.total_paid) }}</td>
              <td class="text-right">{{$filter.currency(s.balance)}}</td>
              <td class="text-right">
                <v-icon @click="onDialog(s.name)" color="info">mdi-eye</v-icon>
              </td>
            </tr>
          </tbody>
        </v-table>
      </div>
      <SaleDetail :selected="selected" :model-value="open" @update:model-value="open = $event" v-if="open"/>
    </PageLayout>
  </template>
  
  <script setup>
  import { createResource,reactive, ref, useRouter} from '@/plugin'
import PageLayout from '../../components/layout/PageLayout.vue';
  import SaleDetail from './SaleDetail.vue';
  const router = useRouter()
  let open = ref(false)
  let selected = ref('')
 
  let filter=reactive({});
  
  let sales = createResource({
      url: 'frappe.client.get_list',
      params:{
        doctype:"Sale",
        fields:["name","customer","customer_name","posting_date","total_quantity","grand_total","sub_total","total_discount","total_paid","balance"],
        filters:filter
      },
      
  })
  sales.fetch();
 

 

  

  function onDialog(name) {
    open.value = true
    selected.value = name;
   }
  function onCustomer(customer){
    router.push({ name: "CustomerDetail",params: { name: customer } });
  }
  function onSearchSale(key,operator){
    return function(value){
      
      if(value){
        if(operator == 'like'){
          value = `%${value}%`
        }
        filter[key] = [operator,value]
      }else{
        delete filter[key]
      }
      
    sales.fetch();
    }
    


  }

  </script>