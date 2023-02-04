<template>
    <div class="cursor-pointer p-2 grow bg-blue-600 text-white  hover:bg-blue-700" v-if="printFormatResource.loading">Print Bill</div>
    <template v-else>
        <div class="cursor-pointer p-2 grow bg-blue-600 text-white hover:bg-blue-700" v-if="printFormatResource.data.length==1" @click="onPrintReport(printFormatResource.data[0])" >
            <v-icon icon="mdi-printer"></v-icon>
        </div>
        <v-menu v-else>
        <template v-slot:activator="{ props }">
            <div class="cursor-pointer p-2 grow bg-blue-600 text-white hover:bg-blue-700" @click="$emit('onClose')" :loading="printFormatResource.loading" v-bind="props">
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
        cache:["print_format",props.doctype],
        auto: true,
    })

    function onPrintReport(r){
         emit('onPrint', r);
    }

</script>