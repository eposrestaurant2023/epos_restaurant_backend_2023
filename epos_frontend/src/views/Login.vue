<template>
  <v-row class="mt-0 mb-0 h-screen">
    <v-col md="6" lg="8" class="pa-0 d-sm-none d-none d-md-block">
      <div class="h-screen bg-cover bg-no-repeat bg-center"
        v-bind:style="{ 'background-image': 'url(' + setting?.login_background + ')' }">
        <div class="h-full w-full p-10 flex justify-center items-center">
          <div>
            <div
              class="app-info w-96 inline-block text-center rounded-lg pa-4 bg-gradient-to-t from-yellow-900 to-yellow-700 text-white shadow-sm">
              <div class="mb-3">
                <img class="my-0 mx-auto" :src="setting?.logo" />
              </div>
              <h1 class="font-bold mb-3">{{ setting?.app_name }}</h1>
              <v-divider></v-divider>
              <div class="py-3">
                <v-list lines="one" bg-color="transparent">
                  <v-list-item class="mb-2" :title="setting?.business_branch" :subtitle="$t('Business')" ></v-list-item>
                  <v-list-item class="mb-2" :title="setting?.pos_profile" :subtitle='$t("POS Profile")'></v-list-item>
                  <v-list-item class="mb-2" :title="setting?.phone_number"  :subtitle='$t("Phone Number")'></v-list-item>
                  <v-list-item :title="setting?.address" :subtitle="$t('Address')"></v-list-item>
                </v-list>
              </div> 
            </div>
          </div>
        </div>
      </div>
    </v-col>
    <v-col sm="12" md="6" lg="4" class="pa-0 relative">
      <div class="h-full flex items-center justify-center bg-gray-100">
        <form @submit.prevent="onLogin">
          <div class="w-73">
            <div>
              <div class="d-block d-md-none mt-4">
                <div class="mb-3">
                  <img class="my-0 mx-auto w-16" :src="setting?.logo" />
                </div>
                <div class="text-center mb-3">
                  <h1 class="font-bold mb-1 text-2xl">{{ setting?.app_name }}</h1>
                  <p class="text-sm">{{ setting?.business_branch }}</p>
                  <v-divider></v-divider>
                </div>
              </div>
              <div class="mb-3">
                <div class="relative">
                  <v-text-field :readonly="mobile" type="password" density="compact" variant="solo" autofocus :label="$t('Password')"
                    append-inner-icon="mdi-arrow-left" single-line hide-details v-model="state.password" height="200"
                    @click:append-inner="onDeleteBack()"></v-text-field>
                </div>
              </div>
              <div>
                <div class="grid grid-cols-3 gap-3">
                  <v-btn @click="onNumPressed('1')" size="x-large">
                  1
                  </v-btn>
                  <v-btn @click="onNumPressed('2')" size="x-large">
                    2
                  </v-btn>
                  <v-btn @click="onNumPressed('3')" size="x-large">
                    3
                  </v-btn>
                  <v-btn @click="onNumPressed('4')" size="x-large">
                    4
                  </v-btn>
                  <v-btn @click="onNumPressed('5')" size="x-large">
                    5
                  </v-btn>
                  <v-btn @click="onNumPressed('6')" size="x-large">
                    6
                  </v-btn>
                  <v-btn @click="onNumPressed('7')" size="x-large">
                    7
                  </v-btn>
                  <v-btn @click="onNumPressed('8')" size="x-large">
                    8
                  </v-btn>
                  <v-btn @click="onNumPressed('9')" size="x-large">
                    9
                  </v-btn>
                  <v-btn @click="onNumPressed('0')" size="x-large">
                    0
                  </v-btn>
                  <v-btn class="col-span-2" color="error" @click="onClearPressed" size="x-large">
                    {{ $t("Clear") }}
                  </v-btn>

                </div>
              </div>
              <div class="mt-6">
                <v-btn type="submit" :loading="isLoading" size="x-large" class="w-full" color="primary">{{ $t("Login") }}</v-btn>
              </div> 
              <div class="mt-2">
                <v-btn size="x-large" class="w-full" color="light"  @click="(()=>{ 
                    $i18n.locale=($i18n.locale=='kh'?'en':'kh');
                    onChangeLang( $i18n.locale) 

                })">{{ $i18n.locale=="en"?"ខ្មែរ":"English" }}</v-btn>
              </div> 

              
              <div class="mt-4 text-center">
                <p class="text-sm text-green-700">{{ setting?.pos_profile }}</p>
              </div> 
            </div>
          </div>
        </form>

        <div class="fixed bottom-8 " v-if="isWindow()">
          <v-btn block  class="w-full" prepend-icon="mdi-window-close"  size="x-large" color="error" @click="onExitWindow()"> {{ $t("Exit") }}</v-btn>
        </div>
      </div>
    </v-col>
  </v-row>

</template>
<script setup>
import { reactive, inject, computed, useStore, useRouter, createResource, createToaster,i18n } from '@/plugin';
import { onMounted } from 'vue';
import { useDisplay } from 'vuetify'; 

const { t: $t } = i18n.global; 

const auth = inject("$auth");
const gv = inject('$gv');
const sale = inject('$sale');
const { mobile } = useDisplay();
const toast = createToaster()
const router = useRouter();
const store = useStore();

store.state.isLoading = false;


let state = reactive({
  username: "",
  password: "",
})

const setting = computed(() => {
  return JSON.parse(localStorage.getItem('setting'));
})
const isLoading = computed(() => {
  return store.state.isLoading;
})


//on init
onMounted(()=>{
  localStorage.removeItem('current_user');
  localStorage.removeItem('make_order_auth');
})



function onChangeLang(code){
  localStorage.setItem('lang', code);
  location.reload(); 
}

function onNumPressed(n) {
  if (state.password == undefined) {
    state.password = "";
  }
  state.password = state.password + n;
}


function onClearPressed() {
  state.password = "";

}

function isWindow() {
  return localStorage.getItem("is_window") == "1"; 
} 

function onDeleteBack() {
  state.password = state.password.substring(0, state.password.length - 1);
}
const onLogin = async () => {
  if (!state.password) {
    toast.warning($t('msg.Invalid PIN Codex'), { position: 'top' });
    return
  }
  store.dispatch('startLoading');

  let is_error = false;
  createResource({
    url: 'epos_restaurant_2023.api.api.check_username',
    auto: true,
    params: {
      "pin_code": state.password
    },
    async onSuccess(doc) {
      store.dispatch('startLoading');
      state.username = doc.username;
      
      if (state.username && state.password) {
        let res = await auth.login(state.username, state.password);
        if (res) {
          getCurrentUserInfo(doc)
          checkPromotionDay()
        } else {
          toast.warning(`msg.Login fail Invalid username or password`);
          store.dispatch('endLoading');
        }
      }
    },
    onError(x) {  
      if(!is_error){
        is_error = true;
        store.dispatch('endLoading');
        toast.warning($t('msg.Invalid PIN Code'), { position: 'top' });
      } 
    }
  })

}

function getCurrentUserInfo(user) {
  createResource({
    url: 'epos_restaurant_2023.api.api.get_user_information',
    auto: true,
    async onSuccess(doc) {
      doc.permission = user.permission;
      localStorage.setItem('current_user', JSON.stringify(doc));
      router.push({ name: "Home" });
      store.dispatch('endLoading');
    },
    onError(x) {
      store.dispatch('endLoading');
    }
  })
}

function checkPromotionDay(){ 
  // check promotion 
	createResource({
		url: 'epos_restaurant_2023.api.promotion.check_promotion',
		cache: "check_promotion",
		auto: true,
		params: {
			business_branch: gv.setting.business_branch
		},
		onSuccess(doc) {
			gv.promotion = doc;
			sale.promotion = doc;
		}
	});
}
function onExitWindow() {
  const data = {
    action: "exit",
  }
  window.chrome.webview.postMessage(JSON.stringify(data));
}
</script>
<style scoped>
.app-info h1 {
  font-size: 26px;
}
</style>