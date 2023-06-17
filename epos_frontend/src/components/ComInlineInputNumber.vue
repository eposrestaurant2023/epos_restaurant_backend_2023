<template>
  <v-card class="mx-auto my-2 py-2" :elevation="hideInput ? '0' : '2'">
    <v-card-title v-if="title">{{ title }}</v-card-title>
    <v-card-text :class="hideInput ? '!p-1' : ''">
      <v-text-field v-if="!hideInput" label="Enter Price" variant="solo" v-model="number" clearable maxlength="10" :disabled="disabled"></v-text-field>
      <div>
        <div class="grid grid-cols-3 gap-3">
          <v-btn @click="numpad_click('1')" size="large" :disabled="disabled">
            1
          </v-btn>
          <v-btn @click="numpad_click('2')" size="large" :disabled="disabled">
            2
          </v-btn>
          <v-btn @click="numpad_click('3')" size="large" :disabled="disabled">
            3
          </v-btn>
          <v-btn @click="numpad_click('4')" size="large" :disabled="disabled">
            4
          </v-btn>
          <v-btn @click="numpad_click('5')" size="large" :disabled="disabled">
            5
          </v-btn>
          <v-btn @click="numpad_click('6')" size="large" :disabled="disabled">
            6
          </v-btn>
          <v-btn @click="numpad_click('7')" size="large" :disabled="disabled">
            7
          </v-btn>
          <v-btn @click="numpad_click('8')" size="large" :disabled="disabled">
            8
          </v-btn>
          <v-btn @click="numpad_click('9')" size="large" :disabled="disabled">
            9
          </v-btn>
          <v-btn @click="numpad_click('0')" size="large" :disabled="disabled">
            0
          </v-btn>
          <v-btn @click="numpad_click('.')" size="large" :disabled="disabled">
            .
          </v-btn>
          <v-btn color="error" @click="onClear()" size="large" :disabled="disabled">
            {{$t('Clear')}}
          </v-btn>

        </div>
      </div>
    </v-card-text>
  </v-card>
</template>
<script setup>
import { ref, defineProps, defineEmits } from "@/plugin"

const emit = defineEmits(['modelValue'])
const props = defineProps({
  disabled: Boolean,
  modelValue: Number,
  hideInput: {
    default: false,
    type: Boolean
  },
  title: {
    default: '',
    type: String
  }
})
const number = ref(0)
function numpad_click(n) {
  if (n == "." && number.value.includes(".")) {

    return;
  }
  if (!number.value) {
    number.value = "";
  }
  number.value = number.value + n;
  emit('update:modelValue', number.value)
}
function onClear(){
  number.value = '';
  emit('update:modelValue', 0)
}
</script>
