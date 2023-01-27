<template>
    <div>
          <div>
            <div class="grid grid-cols-3 gap-3">
              <v-btn @click="numpad_click('1')" size="x-large">
                1
              </v-btn>
              <v-btn @click="numpad_click('2')" size="x-large">
                2
              </v-btn>
              <v-btn @click="numpad_click('3')" size="x-large">
                3
              </v-btn>
              <v-btn @click="numpad_click('4')" size="x-large">
                4
              </v-btn>
              <v-btn @click="numpad_click('5')" size="x-large">
                5
              </v-btn>
              <v-btn @click="numpad_click('6')" size="x-large">
                6
              </v-btn>
              <v-btn @click="numpad_click('7')" size="x-large">
                7
              </v-btn>
              <v-btn @click="numpad_click('8')" size="x-large">
                8
              </v-btn>
              <v-btn @click="numpad_click('9')" size="x-large">
                9
              </v-btn>
              <v-btn @click="numpad_click('0')" size="x-large">
                0
              </v-btn>
              <v-btn @click="numpad_click('.')" size="x-large">
                .
              </v-btn>
              <v-btn color="error" @click="numpad_click(false)" size="x-large">
                Clear
              </v-btn>
            </div>
          </div>
    </div>
  </template>
  <script setup>
  import { ref, defineProps,defineEmits } from "@/plugin"
  
  const props = defineProps({
    input: {
      type: String,
      default: ''
    }
  })
  const emit = defineEmits(['onChange'])

  const number = ref("");
  function numpad_click(n) {
    if(n == false){
        number.value = '';
        emit('onChange', number.value)
    }else{
        if (n == "." && number.value.includes(".")) {
            return;
        }
        if (!number.value) {
            number.value = "";
        }
        number.value = number.value + n;
        emit('onChange', number.value)
    }
    
  }
  
  
  </script>
  