<template>
  <ComModal :mobileFullscreen="true" @onClose="onClose()" width="1200px" :hideOkButton="true">
    <template #title>
      {{ $t('Select Sale') }}
    </template>
    <template #content>
      <div>
        <div>
          <ComInput autofocus ref="searchTextField" keyboard class="m-2" v-model="search" :placeholder="$t('Search Sale')" />
        </div>
        <div class="overflow-auto px-2 pb-2">
          <ComPlaceholder :is-not-empty="getSaleList().length > 0">
            <v-row class="!-m-1">
              <v-col class="!p-0" cols="12" md="6" v-for="(s, index) in getSaleList()" :key="index">
                <ComSaleListItem :sale="s" @click="onSelect(s)" />
              </v-col>
            </v-row>
          </ComPlaceholder>
        </div>
      </div>
    </template>
  </ComModal>
</template>
<script setup>

import { ref, defineProps, defineEmits, inject } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import ComInput from '@/components/form/ComInput.vue';
import { useDisplay } from 'vuetify'
import ComSaleListItem from './ComSaleListItem.vue';
import ComPlaceholder from '../../../components/layout/components/ComPlaceholder.vue';

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
      return (String(r.name) + ' ' + String(r.customer_name) + String(r.phone_number)).toLocaleLowerCase().includes(search.value.toLocaleLowerCase());
    });
  } else {
    return sale.tableSaleListResource?.data || [];
  }
}


function onClose() {
  emit("resolve", false);
}

function onSelect(c) {
  emit("resolve", c);
}

</script>