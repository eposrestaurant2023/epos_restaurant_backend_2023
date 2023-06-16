<template>
    <div v-if="!current_working_day.loading">
        <ComButton icon-class="text-white" class="bg-red-600 text-gray-100" v-if="current_working_day.data"
            :title="$t('Close Working Day')" icon="mdi-calendar"  @click="onCloseWorkingDay" />
        <ComButton v-else :title="$t('Start Working Day')" icon-color="#e99417" icon="mdi-calendar" @click="onStartWorkingDay" />
    </div>
    <div v-else
        class="shadow-md text-center bg-white p-4 rounded-md cursor-pointer flex items-center justify-center h-full">
        <div>
            <v-icon class="m-2" icon="mdi-spin mdi-loading" size="x-large"></v-icon>
            <div class="text-gray-400">{{ $t('Loading') }}</div>
        </div>
    </div>
</template>
<script setup>
import ComButton from '../../../components/ComButton.vue';
import { createResource, useRouter, inject ,i18n} from "@/plugin"
import { createToaster } from '@meforma/vue-toaster'; 
const { t: $t } = i18n.global;   
 

const gv = inject("$gv")
const toaster = createToaster({ position: "top" });
const router = useRouter();

const current_working_day = createResource({
    url: "epos_restaurant_2023.api.api.get_current_working_day",
    params: {
        business_branch: gv.setting.business_branch
    },
    onSuccess(data) {
        if(data){
            gv.workingDay = data?.name
        }
        
    },
    auto: true,
})


function onStartWorkingDay() {
    gv.authorize("start_working_day_required_password", "start_working_day").then(async (v) => {
        if (v) {
            router.push({ name: "StartWorkingDay" });
        }
    })

}

async function onCloseWorkingDay() {  
    gv.authorize("close_working_day_required_password", "close_working_day").then(async (v) => {
        if (v) {
            const cashierShiftResource = createResource({
                url: "epos_restaurant_2023.api.api.get_current_cashier_shift",
                params: {
                    pos_profile: localStorage.getItem("pos_profile")
                }
            });

            await cashierShiftResource.fetch().then(async (v) => {
                if (v) {
                    toaster.warning($t('msg.Please close shift first'))
                } else {
                    router.push({ name: "CloseWorkingDay" });
                }

            })
        }
    });


}

</script>