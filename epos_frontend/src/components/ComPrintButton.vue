<template>
    <v-btn v-if="printFormatResource.loading" class="ma-2"  icon="mdi-printer" :loading="printFormatResource.loading" ></v-btn>
    <template v-else>
        <v-btn v-if="printFormatResource.data.length==1" class="ma-2"  icon="mdi-printer" :loading="printFormatResource.loading" @click="onPrintReport(printFormatResource.data[0])" ></v-btn>
        <v-menu v-else>
        <template v-slot:activator="{ props }">

            <v-btn icon @click="$emit('onClose')" :loading="printFormatResource.loading" v-bind="props">
                <v-icon>mdi-printer</v-icon>
            </v-btn>

            
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
    import {defineProps,defineEmits, createResource,inject} from "@/plugin"
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