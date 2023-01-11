import './index.css';
import { createApp, reactive } from "vue";
import App from "./App.vue";
import { FrappeUI } from 'frappe-ui'
import 'vuetify/styles';
import { createVuetify } from 'vuetify';
import * as components from 'vuetify/components';
import * as directives from 'vuetify/directives';
import '@mdi/font/css/materialdesignicons.css'

import router from './router';
import resourceManager from "../../../doppio/libs/resourceManager";
import API from "./api/index";
import call from "../../../doppio/libs/controllers/call";
import socket from "../../../doppio/libs/controllers/socket";
import Auth from "../../../doppio/libs/controllers/auth";
import store from "./store";
import Toaster from "@meforma/vue-toaster";
import { resourcesPlugin } from 'frappe-ui'

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
app.use(resourceManager);
app.use(vuetify);
app.use(FrappeUI)
app.use(resourcesPlugin)
app.use(store);
app.use(Toaster, {
	position: "top",
})

// Global Properties,
// components can inject this
app.provide("$auth", auth);
app.provide("$call", call);
app.provide("$api",API);
app.provide("$socket", socket);


// Configure route gaurds
router.beforeEach(async (to, from, next) => {
	if (to.matched.some((record) => !record.meta.isLoginPage)) {
		// this route requires auth, check if logged in
		// if not, redirect to login page.
		console.log(auth.isLoggedIn)
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
});

app.mount("#app");
