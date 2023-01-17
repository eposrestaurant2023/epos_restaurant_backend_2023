<template>
  <v-dialog
      v-model="model"
    >
      <v-card width="400" class="mx-auto my-2 py-2 " >
        <v-card-item>
        <v-card-title>{{ title }}</v-card-title>

      </v-card-item>

        <v-card-text>
          <v-text-field label="Enter number" variant="solo" v-model="number"   clearable maxlength="10" ></v-text-field>
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
                  <v-btn  color="error" @click="number=''" size="x-large">
                    Clear
                  </v-btn>
                  <v-btn color="primary" @click="closeDialog(false)" size="x-large">Cancel</v-btn>
                  <v-btn color="success" @click="closeDialog(data)" size="x-large">OK</v-btn>
                </div>
              </div>
        </v-card-text>
        
      </v-card>
    </v-dialog>
  <!-- <div class="dialog">
    <div class="center">
      <p>{{model}}</p>
      <p>{{ data }}</p>
      <input type="text" v-model="data.name"/><br/>
      <input type="number" v-model="data.age"/><br/>
      <button @click="closeDialog(false)" class="btn">NO</button>
      <button @click="closeDialog(data)" class="btn">YES</button>
    </div>
  </div> -->
</template>

<script>
import { defineComponent, ref } from 'vue';
import {closeDialog} from 'vue3-promise-dialog'
export default defineComponent({
  props: {
    title: String,
    data: {}
  },
  setup() {
    let model = ref(true)
    function returnValue() {
      return true;
    }
    return {
      returnValue,
      closeDialog,
      model
    };
  },
});
</script>
