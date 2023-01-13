import './index.css';
import { createApp, reactive } from "vue";

import App from "./App.vue";
import 'vuetify/styles';
import { createVuetify } from 'vuetify';
import * as components from 'vuetify/components';
import * as directives from 'vuetify/directives';
import '@mdi/font/css/materialdesignicons.css'
import { vue3Debounce } from 'vue-debounce'

import router from './router';
import call from "./utils/call";
// import socket from "./controllers/socket";
import Auth from "./utils/auth";
import store from "./store";
import Toaster from "@meforma/vue-toaster";
import {resourcesPlugin} from "./resources"
import { setConfig, frappeRequest } from './resource'
setConfig('resourceFetcher', frappeRequest)



const app = createApp(App);
const auth = reactive(new Auth());

const vuetify = createVuetify({
	components,
	directives,
	icons: {
		defaultSet: 'mdi',
	},
  });

// Plugins
app.use(router);
app.use(resourcesPlugin);
app.use(vuetify);
app.use(store);
app.use(Toaster, {
	position: "top",
})

// Global Properties,
// components can inject this
app.provide("$auth", auth);
app.provide("$call", call);
// app.provide("$socket", socket);

app.config.globalProperties.$filter = {
    currency(_value) {
        let value = _value === undefined ? 0 : _value;
        return '$ ' + value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    },
    isEmpty(str) {
        return (!str || str.trim().length === 0);
    },
}
app.directive('debounce', vue3Debounce({ lock: true }))
// Configure route gaurds
router.beforeEach(async (to, from, next) => {
	if(!localStorage.getItem("pos_profile"))
	{
		
		if (to.matched.some((record) => !record.meta.isStartupConfig)){
			next({name:"StartupConfig", query: { route: to.path }})
		}else{
			next()
		}
	}
	else{
		if (to.matched.some((record) => !record.meta.isLoginPage)) {
			// this route requires auth, check if logged in
			// if not, redirect to login page.
			if (!auth.isLoggedIn) {
				next({ name: 'Login', query: { route: to.path } });
			} else {
				next();
			}

		} else {
			if (auth.isLoggedIn) {
				next({ name: 'Home' });
			} else {
				next();
			}
		}
	}
});

app.mount("#app");
