<template>
    <v-dialog v-model="open" persistent :max-width="800">
        <v-card>
            <v-toolbar color="default" title="Change Price Rule">
                <v-toolbar-items>
                    <v-btn icon @click="onClose()">
                        <v-icon>mdi-close</v-icon>
                    </v-btn>
                </v-toolbar-items>
            </v-toolbar>
            <v-card-text class="p-0">
                <div>
                    {{ setting }}
                    <ComPlaceholder :is-not-empty="setting.price_rules.length > 0">
                        <div>
                            <v-card
                                v-for="(item, index) in setting.price_rules" 
                                :key="index"
                                @click="onSelect(item)"
                                :title="item">
                            </v-card>
                        </div>
                    </ComPlaceholder>
                </div>
            </v-card-text>
            <v-card-actions class="justify-end">
                <v-btn variant="flat" @click="onClose(false)" color="error">
                    Close
                </v-btn>
                <v-btn variant="flat" @click="onOK()" color="primary">
                    OK
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
<script setup>
import { ref, defineEmits, createToaster, inject } from '@/plugin'
import ComPlaceholder from '../../../components/layout/components/ComPlaceholder.vue';
import { useDisplay } from 'vuetify'
const {mobile} = useDisplay()
const emit = defineEmits(['resolve'])
const props = defineProps({
    params:Object
})
const toaster = createToaster({ position: "top" })

const sale  = inject('$sale')
const setting  = inject('$setting')
let open = ref(true)
let selectedPriceRule = ref(sale.setting.price_rule)
 
function onClose() {
    emit('resolve',false)
}
function onOK(){
    sale.sale.price_rule = result;
    sale.setting.price_rule = result;

    emit('resolve', true)
}
 
</script>