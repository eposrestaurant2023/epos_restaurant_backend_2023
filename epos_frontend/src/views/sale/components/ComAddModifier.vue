<template>
    <v-dialog v-model="open" persistent max-width="800px">
        <div class="bg-white rounded-md overflow-hidden">
            <ComToolbar @onClose="onClose">
                <template #title>
                    Choose Product Portion & Modifier
                </template>
            </ComToolbar>
            <div class="p-4">
                <div>
                    <v-row>
                        <v-col :md="priceList.length > 0 ? '6' : '12'" v-if="prices.length > 0">
                            <v-list>
                                <v-list-subheader>Portions</v-list-subheader>
                                <v-list-item
                                    v-for="(item, i) in priceList"
                                    :key="i"
                                    :value="item"
                                    active-color="primary"
                                    rounded="lg"
                                    :title="item.portion"
                                    :subtitle="item.price_rule"
                                > 
                                    <template v-slot:append>
                                        <v-chip color="success">{{ item.price }}</v-chip>
                                    </template>
                                </v-list-item>
                                </v-list>
                        </v-col>
                        <v-col :md="modifierList.length > 0 ? '6' : '12'" v-if="modifierList.length > 0">
                            <v-list>
                                <v-list-subheader>Modifiers</v-list-subheader>
                                <v-list-item
                                    v-for="(item, i) in modifierList"
                                    :key="i"
                                    :value="item"
                                    active-color="primary"
                                    rounded="lg"
                                    :title="item.modifier"
                                    :subtitle="item.category"
                                    >
                                    <template v-slot:append>
                                        <v-chip color="success">{{ item.price }}</v-chip>
                                    </template>
                                </v-list-item>
                            </v-list>
                        </v-col>
                    </v-row>
                </div>
            </div>
            <v-divider></v-divider>
            <div class="text-right p-2">
                <v-btn color="error" @click="onClose" class="mr-2">Close</v-btn>
                <v-btn color="primary" @click="onConfirm">OK</v-btn>
            </div>
        </div>
    </v-dialog>
</template>
  
<script setup>
import { ref,closeDialog,computed } from '@/plugin'
import ComToolbar from '@/components/ComToolbar.vue';

const props = defineProps({ prices: Array, modifiers: Array})
const priceList = computed(()=>{
    return JSON.parse(props.prices)
})
const modifierList = computed(()=>{
    return JSON.parse(props.modifiers)
})
const open = ref(true);
 
function onClose() {
    closeDialog(false);
}
</script>

