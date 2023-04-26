<template>
  <ComModal :mobileFullscreen="true" @onClose="onClose()" width="1200px" :hideOkButton="true">
    <template #title>
      Pending Sale
    </template>
    <template #content>
      <ComPlaceholder :loading="saleResource.loading" :is-not-empty="saleResource?.data?.length > 0"
        text="No Pending Bill" icon="mdi-note-outline">
        <div>
          <div class="grid gap-2 sm:grid-cols-2 md:grid-cols-2 lg:grid-cols-2 xl:grid-cols-3">
            <v-card v-for="(s, index) in saleResource.data" :key="index">
              <v-card-title class="!p-0">
                <v-toolbar height="55">
                  <v-toolbar-title class="text">
                    <span>#: {{ s.name }}</span> -
                    <Timeago class="!text-sm" :long="long" :datetime="s.modified" />
                  </v-toolbar-title>
                  <template v-slot:append>
                    <v-chip class="ma-2" :color="s.sale_status_color" text-color="white">
                      {{ s.sale_status }}
                    </v-chip>
                  </template>
                </v-toolbar>
              </v-card-title>
              <v-card-text class="!pt-0 !pr-0 !pb-14 !pl-0">
                <v-list :lines="false" density="compact" class="pa-0">
                  <v-list-item title="Table #" v-if="s.tbl_number">
                    <template v-slot:append>
                      {{ s.tbl_number }}
                    </template>
                  </v-list-item>
                  <v-list-item title="Sale Type" v-else>
                    <template v-slot:append>
                      <v-chip size="x-small" :color="s.sale_type_color">{{ s.sale_type }}</v-chip>
                    </template>
                  </v-list-item>
                  <v-list-item title="Guest Cover" v-if="s.guest_cover">
                    <template v-slot:append>
                      {{ s.guest_cover }}
                    </template>
                  </v-list-item>
                  <v-list-item title="Customer Code">
                    <template v-slot:append>
                      {{ s.customer }}
                    </template>
                  </v-list-item>

                  <v-list-item title="Customer Name">
                    <template v-slot:append>
                      {{ s.customer_name }}
                    </template>
                  </v-list-item>
                  <v-list-item title="Total Qty">
                    <template v-slot:append>
                      {{ s.total_quantity }}
                    </template>
                  </v-list-item>
                  <v-list-item title="Grand Total">
                    <template v-slot:append>
                      <CurrencyFormat :value="s.grand_total" />
                    </template>
                  </v-list-item>
                </v-list>
              </v-card-text>
              <v-card-actions class="pt-0 flex items-center justify-between absolute bottom-0 w-full">
                <v-btn variant="tonal" color="primary" @click="onViewSaleOrder(s.name)">
                  Sale Detail
                </v-btn>
                <v-btn variant="tonal" color="success" @click="onOpenOrder(s.name)">
                  Open Order
                </v-btn>
              </v-card-actions>
            </v-card>
          </div>
        </div>
      </ComPlaceholder>
    </template>
  </ComModal>
</template>
<script setup>
import { useRouter, defineProps, createResource, defineEmits, ref, inject, createToaster } from "@/plugin"
import { Timeago } from 'vue2-timeago'
import { saleDetailDialog } from "@/utils/dialog";
import ComModal from "../../components/ComModal.vue";
import ComPlaceholder from "../../components/layout/components/ComPlaceholder.vue";
const router = useRouter();
const emit = defineEmits(["resolve"])
const gv = inject('$gv')
const toaster = createToaster({ position: "top" })
const props = defineProps({
  params: {
    type: Object,
    required: true,
  }
})
let filter = ref({
  working_day: null,
  cashier_shift: null
})
function onOpenOrder(sale_id) {
  createResource({
    url: "epos_restaurant_2023.api.api.get_current_shift_information",
    params: {
      business_branch: gv.setting?.business_branch,
      pos_profile: localStorage.getItem("pos_profile")
    },
    auto: true,
    onSuccess(data) {
      if (data.cashier_shift == null) {
        toaster.warning("Please start cashier shift first");
        router.push({ name: "OpenShift" }).then(()=>{
            onClose()
          })
      } else if (data.working_day == null) {
        toaster.warning("Please start working day first");
        router.push({ name: "StartWorkingDay" }).then(()=>{
            onClose()
          }) 
      } else {
        if(sale_id){
          router.push({ name: "AddSale", params: { name: sale_id } }).then(()=>{
            onClose()
          })
        } 
        else{
          toaster.error("Cannot get sale name")
        }
          
      }
    },
    onError(er){
      toaster.error(JSON.stringify(er))
    }
  })

  
}

async function onViewSaleOrder(sale_id) {
  const result = await saleDetailDialog({ name: sale_id });
  if (result) {
    if (result == "open_order") {
      onClose();
    }
    else if (result == "delete_order") {
      saleResource.fetch();
    }
  }
}
let params = ref({
  doctype: "Sale",
  fields: ["name", "modified", "sale_status", "sale_status_color","sale_type","sale_type_color", "tbl_number", "guest_cover", "customer", "customer_name", "total_quantity", "grand_total"],
  order_by: "modified desc",
  filters: {
    'docstatus': 0,
    'working_day': props.params.data.working_day,
    'cashier_shift': props.params.data.cashier_shift
  },
  limit_page_length: 200,
})

const saleResource = createResource({
  url: "frappe.client.get_list",
  params: params.value
});
if (!props.params.data.cashier_shift)
  delete params.value.filters.cashier_shift
if (!props.params.data.working_day)
  delete params.value.filters.working_day

saleResource.params = params.value
saleResource.fetch()

function onClose() {
  emit('resolve', false);
}
</script>