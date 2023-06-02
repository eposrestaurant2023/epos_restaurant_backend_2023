<template>    
    <v-dialog :scrollable="false" v-model="open" :fullscreen="mobile"
        :style="mobile ? '' : 'width: 100%;max-width:800px'">
        <v-card>
            <ComToolbar @onClose="onClose">
                <template #title>
                   {{ $t('Change or Merge Table') }}
                </template>
            </ComToolbar>
            <div class="overflow-auto p-3 h-full">
                <v-tabs align-tabs="center"
                    v-if="tableLayout.table_groups && tableLayout.table_groups.length > 1"
                    v-model="tableLayout.tab">
                    <v-tab v-for="g in tableLayout.table_groups" :key="g.key" :value="g.key">
                        {{ g.table_group }}
                    </v-tab>
                </v-tabs>
                <template v-if="tableLayout.table_groups">
                    <v-window v-model="tableLayout.tab">
                        <template v-for="g in tableLayout.table_groups">
                            <v-window-item :value="g.key">
                                <div class="pa-4">
                                    <ComInput 
                                    v-model="g.search_table_keyword"
                                    autofocus
                                    ref="searchTextField"
                                    keyboard
                                    class="my-2 mb-4"
                                    :placeholder="$t('Search')" />
                                    <div class="grid gap-4 grid-cols-3 sm:grid-cols-4 md:grid-cols-5 lg:grid-cols-5 xl:grid-cols-5 2xl:grid-cols-6">
                                    <template v-for="(t, index) in getTable(g.tables, g.search_table_keyword)" :key="index">
                                        <v-badge :content="t.sales?.length" color="error" v-if="t.sales?.length>0">
                                            <v-btn  :color="t.background_color" @click="onSelectTable(t)" width="100%" height="100">
                                            {{ t.tbl_no }}
                                            </v-btn>
                                    </v-badge>
                                    <v-btn v-else   :color="t.background_color" @click="onSelectTable(t)" width="100%" height="100">
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
import { ref, defineEmits, inject,changeTableSelectSaleOrderDialog,i18n } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';
import { createToaster } from '@meforma/vue-toaster';
import ComInput from '../../../components/form/ComInput.vue';
import { useDisplay } from 'vuetify';
const { t: $t } = i18n.global;  

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



async function onSelectTable(t) {
   if(t.sales?.length==0){    
        sale.sale.table_id = t.id;
        sale.sale.tbl_number = t.tbl_no;
        toaster.success($t('msg.Change to table')+": " + t.tbl_no);
        emit("resolve", true)
   }
   else {
    const result = await changeTableSelectSaleOrderDialog({data:t});
    if(result){
        if(result.action=="create_new_bill"){
            sale.sale.table_id = t.id;
            sale.sale.tbl_number = t.tbl_no;
            toaster.success($t('msg.Change to table')+": " + t.tbl_no);
            emit("resolve", true)
        }else if(result.action=="reload_sale"){
            emit("resolve", result)
        }
    }
   }
}



</script>