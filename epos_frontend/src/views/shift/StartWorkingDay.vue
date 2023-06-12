<template>
    <PageLayout :title="$t('Start Working Day')" icon="mdi-calendar-clock" class="p-4">
        <div class="mb-3">
            <v-row>
                <v-col cols="12" sm="6">
                    <v-text-field :label="$t('Working Date')" v-model="current_date" variant="solo" readonly :hide-details="true"></v-text-field>
                </v-col>
            <v-col cols="12" sm="6">
                    <v-text-field :label="$t('POS Profile')" v-model="pos_profile" variant="solo" readonly :hide-details="true"></v-text-field>
                </v-col>
            </v-row>
            <div class="mt-4">
                <ComInput :title="$t('Enter Note')" keyboard :label="$t('Note')" v-model="note" type="textarea"></ComInput>
            </div>
        </div>
        <div class="flex items-center justify-between mt-8 mb-3">
        <v-btn @click="onStartWorking" color="primary" class="mt-4">{{ $t('Start Working Day') }}</v-btn>
        <v-btn @click="router.push({ name: 'Home' })" color="error" class="ml-4">{{ $t('Cancel') }}</v-btn>
        </div>
    </PageLayout>
</template>
 
<script setup>
import moment from '@/utils/moment.js'
import { ref, createResource,useRouter,createToaster,inject,confirm ,i18n} from '@/plugin'
import PageLayout from '../../components/layout/PageLayout.vue';
import ComInput from '../../components/form/ComInput.vue';
const gv = inject("$gv")
const router = useRouter()
const pos_profile = localStorage.getItem("pos_profile");
const note = ref("")
const current_date = moment(new Date).format('DD-MM-YYYY');
const toaster = createToaster({ /* options */ });

const { t: $t } = i18n.global; 

 
createResource({
    url: "epos_restaurant_2023.api.api.get_current_working_day",
    params: {
        business_branch: gv.setting.business_branch
    },
    auto: true,
    onSuccess(data){
        if(data){
            toaster.warning($t('msg.Working day is already started'),{position:"top"});
            router.push({name:"Home"});
        }
    }
    
})

async function onStartWorking() {
    
    if(await confirm({title:$t('Start Working Day'), text:$t('msg.are you sure to start working day')})){
        createResource({
            url:"frappe.client.insert",
            params:{
                doc:{
                    doctype:"Working Day",
                    pos_profile:pos_profile,
                    posting_date:moment(new Date).format('YYYY-MM-DD'),
                    note:note.value
                }
            },
            onSuccess(data) {
                toaster.success($t('msg.Open Working Day successfully'),{position:"top"});
                router.push({name:"Home"});
            },

            auto:true
     })
    }
}


</script>