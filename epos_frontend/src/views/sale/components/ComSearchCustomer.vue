<template>
  <v-dialog v-model="open" @update:modelValue="onClose">
    <v-card width="700" class="mx-auto my-0">
      <ComToolbar @onClose="onClose">
        <template #title>
          Select Customer
        </template>
      </ComToolbar>
      <div>
        <ComInput 
          keyboard
          class="m-4"
          placeholder="Search Customer"
          v-debounce="onSearch"
          @onInput="onSearch"/>
      </div>
      <div style="max-height: calc(100vh - 254px);" class="overflow-auto px-4 pb-4">
        <ComPlaceholder :loading="customerResource.loading" :is-not-empty="customerResource.data"
          text="There is not customer" icon="mdi-account-outline">
          <v-card v-for="(c, index) in customerResource.data" :key="index" :title="c.customer_name_en"
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
      </div>
    </v-card>
  </v-dialog>


</template>
<script setup>
  import { ref, defineProps, defineEmits, createDocumentResource, createResource, watch } from '@/plugin'
  import ComToolbar from '@/components/ComToolbar.vue';
  import ComInput from '@/components/form/ComInput.vue';

  const props = defineProps({
    params: {
      type: Object,
      required: true,
    }
  })

  const emit = defineEmits(["resolve", "reject"])

  const open = ref(true);
  let search = ref('')
  const customerResource = createResource({
    url: "frappe.client.get_list",
    params: getDataResourceParams(),
    auto: true
  });
 
  function getDataResourceParams (){
    return {  
        doctype: "Customer",
        fields: ["name", "customer_name_en", "customer_name_kh", "customer_group", "date_of_birth", "gender", "phone_number", "photo", "default_discount"],
        order_by: "modified desc",
        filters: { disabled: 0, customer_name_en:['like','%' + search.value +'%']},
        limit_page_length: 25
    }
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

</script>