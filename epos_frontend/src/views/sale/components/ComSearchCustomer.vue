<template>
  <ComModal :fullscreen="mobile" @onClose="onClose">
    <template #title>
          Select Customer
    </template>
    <template #content>
      <div>
        <div>
        <ComInput 
          autofocus
          ref="searchTextField"
          keyboard
          class="m-4"
          v-model="search"
          placeholder="Search Customer"
          v-debounce="onSearch"
          @onInput="onSearch"/>
      </div>
      <div class="px-4 pb-4">
        <ComPlaceholder v-if="customerResource.data?.length>0"  :is-not-empty="customerResource.data"
          text="There is not customer" icon="mdi-account-outline">
          <v-card v-for="(c, index) in customerResource.data.filter(r=>r.disabled == 0)" :key="index" :title="c.customer_name_en"
            @click="onSelectCustomer(c)" class="mb-4">
            <template v-slot:subtitle>
              {{ c.name }}
              <span v-if="c.phone_number != null && c.phone_number != '' && c.phone_number != undefined"> / {{
                c.phone_number
              }}</span>
            </template>
            <template v-slot:prepend>
              <v-avatar v-if="c.photo">
                <v-img :src="c.photo"></v-img>
              </v-avatar>
              <avatar v-else :name="c.customer_name_en" class="mr-4" size="40"></avatar>
            </template>
            <template v-slot:append>
              <v-chip v-if="c.default_discount > 0" color="error">{{ c.default_discount }} % OFF</v-chip>
            </template>
          </v-card>
        </ComPlaceholder>
        <div v-else>
          <v-alert v-if="!customerResource.loading"
            title="Customer not found"
            variant="tonal"
          >
            <div class="d-flex flex-row align-center justify-space-between">
              <div>
                There's no customer with keyword <strong>{{ search }}</strong>.
              </div>

              <v-btn
                color="info"
                variant="outlined"
                @click="addCustomer"
              >
                Add New Customer
              </v-btn>
            </div>
          </v-alert>
        </div>
      </div>
      </div>
    </template>
  </ComModal>
</template>
<script setup>
  import { addCustomerDialog, ref, defineProps, defineEmits, createResource,inject } from '@/plugin'
  import ComToolbar from '@/components/ComToolbar.vue';
  import ComInput from '@/components/form/ComInput.vue';
  import { useDisplay } from 'vuetify'
 
 const { mobile } = useDisplay()

  const gv = inject("$gv");
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


  if(gv.customerMeta==null){
    createResource({
    url: "epos_restaurant_2023.api.api.get_meta",
    params: {
      doctype: "Customer"
    },
    auto:true,
    cache:["customer_meta_data"],
    onSuccess(doc){
      gv.customerMeta = doc;
      if(doc.search_fields){
        doc.search_fields.split(",").forEach(function(d){
          searchFields.value.push(d.trim())
        });
      }
    }

  })
  }else {
    if(gv.customerMeta.search_fields){
      gv.customerMeta.search_fields.split(",").forEach(function(d){
          searchFields.value.push(d.trim())
        });
      } 
  }


  const customerResource = createResource({
    url: "frappe.client.get_list",
    params: getDataResourceParams(),
    auto: true
  });

  
 
  function getDataResourceParams (){
    return {  
        doctype: "Customer",
        fields: ["name", "customer_name_en", "customer_name_kh", "customer_group", "date_of_birth", "gender", "phone_number", "photo", "default_discount","disabled"],
        order_by: "modified desc",
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
    customerResource.params = getDataResourceParams()
    customerResource.fetch()
  }
  function onClose() {
    emit("resolve", false);
  }

  function onSelectCustomer(c) {
    emit("resolve", c);
  }

  async function addCustomer(){
    const result = await addCustomerDialog({})
    if(result){
      emit("resolve", result);
    }
  }

</script>