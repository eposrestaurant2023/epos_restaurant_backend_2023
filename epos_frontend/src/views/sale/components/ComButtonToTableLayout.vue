<template>
  <template v-if="gv.setting.table_groups && gv.setting.table_groups.length > 0">
    <v-list-item @click="onToTableLayout()" v-if="isMobile">
      <template v-slot:prepend class="w-12">
        <v-icon icon="mdi-keyboard-return"></v-icon>
      </template>
      <v-list-item-title>{{ $t('Back') }}</v-list-item-title>
    </v-list-item>
    <template v-else>
      <v-btn v-if="!mobile" stacked variant="elevated" size="small" class="m-0-1 grow"
        :prepend-icon="'mdi-keyboard-return'" @click="onToTableLayout()">
        {{ $t('Back') }}
      </v-btn>
      <v-btn v-else variant="tonal" size="small" :height="mobile ? '35px' : undefined" class="m-0-1 grow"
        @click="onToTableLayout()">
        <v-icon icon="mdi-keyboard-return"></v-icon>
      </v-btn>
    </template>
  </template>

  
  <template v-else>
    <v-list-item @click="onToHomePage()" v-if="isMobile">
      <template v-slot:prepend class="w-12">
        <v-icon icon="mdi-keyboard-return"></v-icon>
      </template>
      <v-list-item-title>{{ $t('Back') }}</v-list-item-title>
    </v-list-item>
    <template v-else>
      <v-btn v-if="!mobile" stacked variant="elevated" size="small" class="m-0-1 grow"
        :prepend-icon="'mdi-keyboard-return'" @click="onToHomePage()">
        {{ $t('Back') }}
      </v-btn>
      <v-btn v-else variant="tonal" size="small" :height="mobile ? '35px' : undefined" class="m-0-1 grow"
        @click="onToHomePage()">
        <v-icon icon="mdi-keyboard-return"></v-icon>
      </v-btn>
    </template>
  </template>
</template>
<script setup>
import { inject, defineProps, confirmBackToTableLayout, useRouter, defineEmits } from '@/plugin'
import { createToaster } from "@meforma/vue-toaster";
import { useDisplay } from 'vuetify'
import Enumerable from 'linq';
const sale = inject('$sale')
const gv = inject('$gv')
const { mobile } = useDisplay()
const router = useRouter()
const emit = defineEmits('closeModel')
const props = defineProps({
  isMobile: Boolean
})
const toaster = createToaster({ position: "top" })

async function onToHomePage(){
  const sp = Enumerable.from(sale.sale.sale_products);

  if (sp.where("$.name==undefined").toArray().length > 0) {
    let result = await confirmBackToTableLayout({});
    if (result) {
      if (result == "hold" || result == "submit") {
        if (result == "hold") {
          sale.sale.sale_status = "Hold Order";
          sale.action = "hold_order";
        } else {
          sale.sale.sale_status = "Submitted";
          sale.action = "submit_order";
        }
        await sale.onSubmit().then(async (value) => {
          if (value) {
            if (mobile.value) {
              emit('closeModel')
            } else {
              router.push({ name: "Home" }).then(() => {
                emit('closeModel')
              })
            }
          }
        });
      } else {
        //continue
        sale.sale = {};
        if (mobile.value) {
          emit('closeModel')
        } else {
          router.push({ name: "Home" }).then(() => {
            emit('closeModel')
          })
        }
      }
    }
  } else {
    sale.sale = {};
    if (mobile.value) {
      emit('closeModel')
    } else {
      router.push({ name: "Home" }).then(() => {
        emit('closeModel')
      })
    }
  }
}

async function onToTableLayout() {
  const sp = Enumerable.from(sale.sale.sale_products);

  if (sp.where("$.name==undefined").toArray().length > 0) {
    let result = await confirmBackToTableLayout({});
    if (result) {
      if (result == "hold" || result == "submit") {
        if (result == "hold") {
          sale.sale.sale_status = "Hold Order";
          sale.action = "hold_order";
        } else {
          sale.sale.sale_status = "Submitted";
          sale.action = "submit_order";
        }
        await sale.onSubmit().then(async (value) => {
          if (value) {
            if (mobile.value) {
              emit('closeModel')
            } else {
              router.push({ name: "TableLayout" }).then(() => {
                emit('closeModel')
              })
            }
          }
        });
      } else {
        //continue
        sale.sale = {};
        if (mobile.value) {
          emit('closeModel')
        } else {
          router.push({ name: "TableLayout" }).then(() => {
            emit('closeModel')
          })
        }
      }
    }
  } else {
    sale.sale = {};
    if (mobile.value) {
      emit('closeModel')
    } else {
      router.push({ name: "TableLayout" }).then(() => {
        emit('closeModel')
      })
    }
  }
}
</script> 