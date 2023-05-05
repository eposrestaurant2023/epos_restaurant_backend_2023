<template>
    <ComModal :persistent="true" :width="categoryNoteName ? '1200px' : '800px'" @onClose="onClose()" @onOk="onOK()" title-ok-button="OK" :fullscreen="mobile">
        <template #title>
            <div>{{ params.title ?? "Discount" }}</div>
        </template>
        <template #content>
            <div>
                <v-row :class="categoryNoteName ? '' : '!m-0'">
                    <v-col cols="12" :md="categoryNoteName ? 6 : 12">
                        <div class="grid gap-2" :class="mobile ? 'grid-cols-1' : 'sm:grid-cols-2 md:grid-cols-2 lg:grid-cols-2'">
                            <div 
                                v-for="(item, index) in data" :key="index"
                                v-ripple
                                class="relative overflow-hidden h-full bg-cover bg-no-repeat rounded-lg cursor-pointer bg-gray-100 border-b border-gray-400"
                                @click="onClick(item)"> 
                                <div class="block relative p-2 w-full h-full"> 
                                        <div class="font-bold mb-2 pb-2 border-b">
                                            <v-icon icon="mdi-checkbox-marked-circle-outline" color="success" v-if="item.selected"></v-icon>
                                            <v-icon icon="mdi-checkbox-blank-circle-outline" color="gray" v-else></v-icon>
                                            <span class="pl-2">{{ item.tax_rule}}</span>
                                        </div>
                                        <v-chip size="small" style="margin: 5px;"><strong>{{ sale.setting.tax_1_name }}</strong>: {{ getTax(item.tax_rule_data).tax_1_rate }}% of {{ getTax(item.tax_rule_data).percentage_of_price_to_calculate_tax_1 }}% Revenue</v-chip>
                                        <v-chip size="small" style="margin: 5px;"><strong>{{ sale.setting.tax_2_name }}</strong>: {{ getTax(item.tax_rule_data).tax_2_rate }}% of {{ getTax(item.tax_rule_data).percentage_of_price_to_calculate_tax_2 }}% Revenue</v-chip>
                                        <v-chip size="small" style="margin: 5px;"><strong>{{ sale.setting.tax_3_name }}</strong>: {{ getTax(item.tax_rule_data).tax_3_rate }}% of {{ getTax(item.tax_rule_data).percentage_of_price_to_calculate_tax_3 }}% Revenue</v-chip>
                                      
                                        
                                </div>
                            </div>
                        </div>
                    </v-col>
                    <v-col cols="12" v-if="categoryNoteName" md="6">
                        <ComInlineNote :category_note="categoryNoteName" v-model="note"/>
                    </v-col>
                </v-row>
            </div>
        </template>
    </ComModal>
</template>
<script setup>
import { ref, defineEmits, createToaster, computed, inject,onMounted } from '@/plugin'
import ComInlineNote from '../../../components/ComInlineNote.vue';
import { useDisplay } from 'vuetify'
const emit = defineEmits(['resolve'])
const { mobile } = useDisplay()
const props = defineProps({
    params:Object
})
const toaster = createToaster({ position: "top" })
const sale = inject('$sale')
let note = ref(props.params.note) 
const data = ref([])
onMounted(() => {
    data.value = JSON.parse(JSON.stringify(sale.setting.tax_rules))  

    data.value.forEach(a => {
        a.selected = false;
        if(a.tax_rule==props.params.data.tax_rule){
            a.selected = true;
        }        
    }); 
})
const categoryNoteName = computed(()=>{
    return props.params.data?.category_note_name;
})
function onClick(item){
    data.value.forEach(((r)=>{
        if(r.selected){
            r.selected = false
        }
    }));
    
    item.selected = !item.selected
}
function onClose() {
    emit('resolve',false)
}
function onOK(){
 
    if(categoryNoteName.value && !note.value){
        toaster.warning("Please select note");
    }
    else{
        const result = {
            change_tax_setting_note: note.value,
            tax_rule: data.value.find(r=>r.selected == true)
        }
        emit('resolve', result)
    }
}


function getTax(tax_rule_data){
    return JSON.parse(tax_rule_data);

}
 
</script>