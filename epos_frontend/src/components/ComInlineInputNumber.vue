<template>
  <v-card class="mx-auto my-2 py-2 " title="Enter Price">
    <v-card-title>{{ title }}</v-card-title>
    <v-card-text>
      <v-text-field label="Enter Price" variant="solo" v-model="number" clearable maxlength="10" :disabled="disabled"></v-text-field>
      <div>
        <div class="grid grid-cols-3 gap-3">
          <v-btn @click="numpad_click('1')" size="x-large" :disabled="disabled">
            1
          </v-btn>
          <v-btn @click="numpad_click('2')" size="x-large" :disabled="disabled">
            2
          </v-btn>
          <v-btn @click="numpad_click('3')" size="x-large" :disabled="disabled">
            3
          </v-btn>
          <v-btn @click="numpad_click('4')" size="x-large" :disabled="disabled">
            4
          </v-btn>
          <v-btn @click="numpad_click('5')" size="x-large" :disabled="disabled">
            5
          </v-btn>
          <v-btn @click="numpad_click('6')" size="x-large" :disabled="disabled">
            6
          </v-btn>
          <v-btn @click="numpad_click('7')" size="x-large" :disabled="disabled">
            7
          </v-btn>
          <v-btn @click="numpad_click('8')" size="x-large" :disabled="disabled">
            8
          </v-btn>
          <v-btn @click="numpad_click('9')" size="x-large" :disabled="disabled">
            9
          </v-btn>
          <v-btn @click="numpad_click('0')" size="x-large" :disabled="disabled">
            0
          </v-btn>
          <v-btn @click="numpad_click('.')" size="x-large" :disabled="disabled">
            .
          </v-btn>
          <v-btn color="error" @click="number = ''" size="x-large" :disabled="disabled">
            Clear
          </v-btn>

        </div>
      </div>
    </v-card-text>
  </v-card>
</template>
<script setup>
import { ref, defineProps } from "@/plugin"
const open = ref(true);
const number = ref("");
const props = defineProps({
  disabled: Boolean
})
function numpad_click(n) {
  if (n == "." && number.value.includes(".")) {

    return;
  }
  if (!number.value) {
    number.value = "";
  }
  number.value = number.value + n;
}
</script>
