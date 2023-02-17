<template>
    <v-dialog :scrollable="false" v-model="open" :fullscreen="mobile"
        :style="mobile ? '' : 'width: 100%;max-width:800px'">
        <v-card>
            <ComToolbar @onClose="onClose">
                <template #title>
                    Change / Merge Table
                </template>
            </ComToolbar>
            <div class="overflow-auto p-3 h-full">
                <v-tabs align-tabs="center"
                    v-if="tableLayout.table_groups && tableLayout.table_groups.length > 1"
                    v-model="tableLayout.tab">
                    <v-tab v-for="g in tableLayout.table_groups" :key="g.key" :value="g.key">
                        {{ g.table_group }}   {{ g.search_table_keyword }}
                    </v-tab>
                </v-tabs>
                <template v-if="tableLayout.table_groups">
                    <v-window v-model="tableLayout.tab">
                        <template v-for="g in tableLayout.table_groups">
                            <v-window-item :value="g.key">
                                <div class="pa-1">
                                    <ComInput 
                                    v-model="g.search_table_keyword"
                                    autofocus
                                    ref="searchTextField"
                                    keyboard
                                    class="my-2"
                                    placeholder="Search Table" />
                                    <div class="grid gap-2 grid-cols-3 sm:grid-cols-4 md:grid-cols-5 lg:grid-cols-5 xl:grid-cols-5 2xl:grid-cols-6">
                                    <template v-for="(t, index) in getTable(g.tables, g.search_table_keyword)" :key="index">
                                        <v-btn :disabled="sale.sale.table_id==t.id" :color="t.background_color" @click="onSelectTable(t)" width="100%" height="100">
                                        {{ t.tbl_no }}
                                        </v-btn>
                                    </template>
                                </div>
                            </div>
                            </v-window-item>
                        </template>
                    </v-window>
                </template>
            </div>
        </v-card>
    </v-dialog>
</template>
  
<script setup>
import { ref, defineEmits, inject } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import { createToaster } from '@meforma/vue-toaster';
import ComInput from '../../../components/form/ComInput.vue';
import { useDisplay } from 'vuetify'
const { mobile } = useDisplay()

const tableLayout = inject("$tableLayout");
const sale = inject("$sale");
const toaster = createToaster({ position: "top" })

const props = defineProps({
    params: {
        type: Object,
        require: true
    }
})

const emit = defineEmits(["resolve", "reject"])

let open = ref(true);
tableLayout.getSaleList();


function onClose() {
    emit("resolve", false)
}



function getTable(tables,keyword) {
        if (keyword == "") {
            return tables;
        } else {

            return tables.filter((r) => {
                    return   String(r.tbl_no).toLocaleLowerCase().includes(keyword.toLocaleLowerCase());
                });
        }
    }



function onSelectTable(t) {
   if(t.sales?.length==0){

    
        sale.sale.table_id = t.id;
        sale.sale.tbl_number = t.tbl_no;
        toaster.success("Change to table: " + t.tbl_no);

        emit("resolve", true)
 
   }
   else {
    alert("popup select sale")
   }
}



</script>