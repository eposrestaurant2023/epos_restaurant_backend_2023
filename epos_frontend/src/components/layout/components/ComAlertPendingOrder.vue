<template>
    <div v-if="countPendingSaleListResource.data > 0 && !countPendingSaleListResource.loading" class="mx-4">
        <v-alert
            class="mb-3"
            :type="type"
            variant="tonal">
            <template #title>
                <span>Pending Orders</span>
            </template>
            <template #text>
                <div :class="mobile ? '' : 'flex justify-between items-center'">
                    <div>There are <span class="font-bold underline cursor-pointer" @click="onViewPendingOrder">{{ countPendingSaleListResource.data }}</span> pending orders.</div>
                    <div class="p-2"><v-btn color="primary" @click="onViewPendingOrder">View Pending Order</v-btn></div>
                </div>
            </template>
        </v-alert>
    </div>
</template>
<script setup>
import {defineProps,ref,createResource,pendingSaleListDialog,defineEmits} from '@/plugin'
import {useDisplay} from 'vuetify'
const {mobile} = useDisplay()
const emit = defineEmits(['getPendingOrder'])
const props = defineProps({
    total: String,
    type: {
        type: String,
        default: 'info'
    },
    cashier_shift: '',
    working_day: '',
})
let filters = ref({
    pos_profile: localStorage.getItem("pos_profile"),
    cashier_shift: props.cashier_shift,
    working_day: props.working_day,
    docstatus: 0
})

if (!props.cashier_shif)
  delete filters.value.cashier_shift
if (!props.working_day)
  delete filters.value.working_day

const countPendingSaleListResource = createResource({
    url: "frappe.client.get_count",
    auto:true,
    params: {
        doctype: "Sale",
        filters: filters.value
    },
    onSuccess(doc){
        emit('getPendingOrder', doc)
    }
})

async function onViewPendingOrder() {
    const result = await pendingSaleListDialog({
        data: {
            working_day:props.working_day, cashier_shift: props.cashier_shift
        }
    })
}

</script>