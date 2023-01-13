<template>
    <v-dialog v-model="open" fullscreen>
        <v-card>
            <ComToolbar @onClose="onClick" :isPrint="true" :isMoreMenu="true" @onPrint="onPrint">
                <template #title>
                    Sale # : {{ sale.name }}
                </template>
                <template #more_menu>
                    <v-list density="compact">
                        <v-list-item>
                            <template v-slot:prepend>
                                <v-icon color="error">mdi-delete</v-icon>
                            </template>
                            <v-list-item-title>Delete</v-list-item-title>
                        </v-list-item>
                    </v-list>
                </template>
            </ComToolbar>
            <v-card-text v-if="sale.doc" >
                <v-card max-width="960" class="mx-auto my-0 pa-4">
                    <div class="float-sm-left">  
                        <table class="tbl-list">
                            <tr>
                                <td>Customer Code</td>
                                <td>:</td>
                                <td>{{ sale.doc.customer }}</td>
                            </tr>
                            <tr>
                                <td>Customer Name</td>
                                <td>:</td>
                                <td>{{ sale.doc.customer_name }}</td>
                            </tr>
                            <tr>
                                <td>Phone Number</td>
                                <td>:</td>
                                <td>{{ sale.doc.phone_number }}</td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table class="tbl-list ml-auto tbl-list-right">
                            <tr>
                                <td>Sale #</td>
                                <td>:</td>
                                <td>{{ sale.name }}</td>
                            </tr>
                            <tr>
                                <td>Date</td>
                                <td>:</td>
                                <td>{{ sale.doc.posting_date }}</td>
                            </tr>
                            <tr>
                                <td>Branch</td>
                                <td>:</td>
                                <td>{{ sale.doc.business_branch }}</td>
                            </tr>
                            <tr>
                                <td>Stock Location</td>
                                <td>:</td>
                                <td>{{ sale.doc.stock_location }}</td>
                            </tr>
                        </table>
                    </div>
                    <v-table fixed-header hover>
                        <thead>
                            <tr>
                                <th>No</th>
                                <th>Description</th>
                                <th>QTY</th>
                                <th>Unit</th>
                                <th class="text-right">Price</th>
                                <th class="text-right">Amount</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="p in sale.doc.sale_products">
                                <th>{{ p.idx }}</th>
                                <td>{{ p.product_code }} - {{ p.product_name }}</td>
                                <th>{{ p.quantity }}</th>
                                <th>{{ p.unit }}</th>
                                <th class="text-right">{{ $filter.currency(p.price) }}</th>
                                <th class="text-right">{{ $filter.currency(p.amount) }}</th>
                            </tr>
                        </tbody>
                    </v-table>
                    <div class="float-sm-left">
                        <table class="ml-auto">
                            <tr>
                                <td>Customer</td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table class="ml-auto">
                            <tr>
                                <td>Total Quantity</td>
                                <td>:</td>
                                <td>{{ sale.doc.total_quantity }}</td>
                            </tr>
                            <tr>
                                <td>Sub Total</td>
                                <td>:</td>
                                <td>{{ $filter.currency(sale.doc.sub_total) }}</td>
                            </tr>
                            <tr>
                                <td>Grand Total</td>
                                <td>:</td>
                                <td>{{ $filter.currency(sale.doc.grand_total) }}</td>
                            </tr>
                            <tr>
                                <td>Total Paid</td>
                                <td>:</td>
                                <td>{{ $filter.currency(sale.doc.total_paid) }}</td>
                            </tr>
                        </table>
                    </div>
                </v-card>
           </v-card-text>
        </v-card>
        
    </v-dialog>
  </template>
  
<script setup>
    import { createDocumentResource, computed } from '@/plugin'
import ComToolbar from '../../components/ComToolbar.vue';
    const props = defineProps({selected: String, modelValue: Boolean, })
    const emit = defineEmits(['update:modelValue'])
    const open = computed(() => {
        return props.modelValue
    })
    let sale = createDocumentResource({
    url: 'frappe.client.get',
    doctype: 'Sale',
    name: props.selected,
    auto:true
    })
    function onClick() {
        emit('update:modelValue', false);
    }
    function onPrint() {
        if (localStorage.getItem("is_window")){
            
            window.chrome.webview.postMessage(JSON.stringify(sale));
        }else{
            alert("print on web")
        }
        
    }
</script>

