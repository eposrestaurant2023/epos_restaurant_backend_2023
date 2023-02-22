<template>
  <v-dialog :fullscreen="mobile" v-model="open" @update:modelValue="onClose"
    :style="mobile ? '' : 'width: 100%;max-width:800px'">
    <v-card class="mx-auto my-0 w-full">
      <ComToolbar @onClose="onClose">
        <template #title>
          Select Sale
        </template>
      </ComToolbar>
      <div>
        <ComInput autofocus ref="searchTextField" keyboard class="m-4" v-model="search" placeholder="Search Sale"
         />
      </div>
      <div class="overflow-auto px-4 pb-4">
        <v-row>
          <v-col sm="12" md="6" v-for="(s, index) in getSaleList()" :key="index">
            <v-card class="mx-auto" prepend-icon="mdi-home" :color="s.sale_status_color" @click="onSelect(s)">
              <template v-slot:title>
                This is a title
              </template>

              <v-card-text>
                {{ s }}
              </v-card-text>
            </v-card>
          </v-col>
        </v-row>
      </div>
    </v-card>
  </v-dialog>
</template>
<script setup>
import moment from '@/utils/moment.js';
import { ref, defineProps, defineEmits, inject } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import ComInput from '@/components/form/ComInput.vue';
import { useDisplay } from 'vuetify'

const { mobile } = useDisplay()

const sale = inject("$sale");
const searchTextField = ref(null)
const props = defineProps({
  params: {
    type: Object,
    required: true,
  }
})

const emit = defineEmits(["resolve", "reject"])

const open = ref(true);
const search = ref('')

function getSaleList() {
  if (search.value) {
    return sale.tableSaleListResource?.data?.filter((r) => {
      return  (String(r.name) + ' ' + String(r.customer_name) + String(r.phone_number) ) .toLocaleLowerCase().includes(search.value.toLocaleLowerCase());
    });
  } else {
    return sale.tableSaleListResource?.data;
  }



}


function onClose() {
  emit("resolve", false);
}

function onSelect(c) {
  emit("resolve", c);
}

</script>