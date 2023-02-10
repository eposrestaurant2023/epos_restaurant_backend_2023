<template>
  <v-table>
    <thead>
      <tr>
        <th class="text-left" style="width:250px">
          Name
        </th>
        <th class="text-left">
          Qty
        </th>
        
        <th class="text-left">
          Price
        </th>
        <th class="text-left">
          Discount
        </th>
        <th class="text-left">
          Amount
        </th>
<th></th>
      </tr>
    </thead>
    <tbody>
      <tr
      v-for="(item, index) in data" :key="index"
      >
        <td>
            <v-text-field v-model="item.name" variant="outlined"></v-text-field>
        </td>
        <td><v-text-field v-model="item.quantity" type="number" variant="outlined"></v-text-field></td>
        <td><v-text-field  v-model="item.price" type="number" variant="outlined"></v-text-field></td>
        <td><v-text-field  v-model="item.discount" type="number" variant="outlined"></v-text-field></td>
        <td>{{ (item.quantity * item.price) - item.discount }}</td>
        <td><v-btn @click="onDelete(index)" color="error">Delete</v-btn></td>
      </tr>
    </tbody>
    <tfoot>
        <tr>
            <td>Total</td>
            <td>{{ Enumerable.from(data).sum("$.quantity")  }}</td>
            <td></td>
            <td></td>
            <td>{{ Enumerable.from(data).sum("($.quantity*$.price) - $.discount")  }}</td>
        </tr>
    </tfoot>
  </v-table>

  <hr/>
  <v-btn @click="AddRow" class="mt-4">Add Row</v-btn>
</template>

<script setup>
import {ref} from "@/plugin"
import Enumerable from 'linq'
const data = ref(  [
          {
            name: 'xx',
            quantity: 1.0,
            price: 2,
            discount: 0,
            amount: 0,
          }
        ]);

function AddRow(){
    data.value.push({
        name:"Product Name",
        quantity:1,
        price:0,
        discount:0
    })
}

function onDelete(index){
    data.value.splice(index,1)

}
</script>

