<template>
   
    <v-tabs align-tabs="center" v-if="tableLayout.table_groups && tableLayout.table_groups.length > 1 && !mobile"
        v-model="tableLayout.tab">
        <v-tab v-for="g in tableLayout.table_groups" :key="g.key" :value="g.key">
            {{ g.table_group }}
        </v-tab>
    </v-tabs>
    <v-bottom-navigation  v-model="tableLayout.tab"  v-if="tableLayout.table_groups && tableLayout.table_groups.length > 1 && mobile">
        <v-btn   v-for="g in tableLayout.table_groups" :key="g.key" :value="g.key" >
             
            {{ g.table_group }}
        </v-btn>
    </v-bottom-navigation>
</template>
<script setup>
import { inject } from '@/plugin';
import { useDisplay } from 'vuetify'
const tableLayout = inject("$tableLayout");
const { mobile } = useDisplay()
</script>