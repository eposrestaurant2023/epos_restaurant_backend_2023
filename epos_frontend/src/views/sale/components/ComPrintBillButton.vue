<template>
    <div class="cursor-pointer p-2 grow bg-blue-600 text-white" v-if="printFormatResource.loading">Print Bill</div>
    <template v-else>
        <div class="cursor-pointer p-2 grow bg-blue-600 text-white" v-if="printFormatResource.data.length==1" @click="onPrintReport(printFormatResource.data[0])" >
            <v-icon icon="mdi-printer"></v-icon>
        </div>
        <v-menu v-else>
        <template v-slot:activator="{ props }">
            <div class="cursor-pointer p-2 grow bg-blue-600 text-white" @click="$emit('onClose')" :loading="printFormatResource.loading" v-bind="props">
                Print Bill
            </div>
        </template>
        <v-list v-if="printFormatResource.data">
            <v-list-item v-for="(r, index) in printFormatResource.data" :key="index"  @click="onPrintReport(r)">
                <v-list-item-title>{{ r.name }}</v-list-item-title>
            </v-list-item>

        </v-list>
    </v-menu>
    </template>
       
</template>
<script setup>
    import {defineProps,defineEmits, createResource} from "@/plugin"
    const props = defineProps({
        doctype:String,
        title:{
            type:String,
            default:""
        }
    });
    const emit = defineEmits(["onPrint"])

    const printFormatResource = createResource({
        url: "epos_restaurant_2023.api.api.get_pos_print_format",
        params: {
            doctype:props.doctype
        },
        cache:"sale_get_pos_print_format",
        auto: true,
    })

    function onPrintReport(r){
         emit('onPrint', r);
    }

</script>