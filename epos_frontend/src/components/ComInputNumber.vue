<template>
  <v-dialog v-model="open" >
    <v-card width="400" class="mx-auto my-2 py-2 ">
      <v-card-item>
        <v-card-title>{{ params.title }}</v-card-title>
    </v-card-item>
      <v-card-text>
        <v-text-field  :label="$t('Enter Number')" variant="solo" v-model="number" clearable maxlength="10"></v-text-field>
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
            <v-btn color="error" @click="number = ''" size="x-large">
              {{$t('Clear')}}
            </v-btn>
            <v-btn color="primary" @click="onCancel" size="x-large">{{$t('Cancel')}}</v-btn>
            <v-btn color="success" @click="onOk" size="x-large">{{$t('Ok')}}</v-btn>
          </div>
        </div>
      </v-card-text>

    </v-card>
  </v-dialog>
</template>
<script setup>
import { ref } from "@/plugin"


const props = defineProps({
  params: {
    type: Object,
    require:true
  }
})
const emit = defineEmits(["resolve"])

const open = ref(true);

const number = ref("");

  

function numpad_click(n) {
  if (n == "." && number.value.includes(".")) {
    return;
  }
  if (!number.value) {
    number.value = "";
  }
  number.value = number.value + n;
}

function onOk(){
  emit('resolve', number.value)
}
function onCancel(){
  emit('resolve', false);
}

</script>
