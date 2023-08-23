import './index.css';
import { createApp, reactive } from "vue";
import App from "./App.vue";
import router from './router';
// import call from "../../../doppio/libs/controllers/call";
import NumberFormat from 'number-format.js'

import Gv from "./providers/gv";
import Sale from './providers/sale';

import 'vuetify/styles';
import { createVuetify } from 'vuetify';
import * as components from 'vuetify/components';
import * as directives from 'vuetify/directives';
import '@mdi/font/css/materialdesignicons.css'
import { FrappeApp } from 'frappe-js-sdk';
import './registerServiceWorker'

const frappe = new FrappeApp()
const vuetify = createVuetify({
		components,
		directives,
		icons: {
			defaultSet: 'mdi',
		},
		display: {
			mobileBreakpoint: 'sm',
		},

	});

const app = createApp(App);
const gv = reactive(new Gv());
const sale = reactive(new Sale());

// Plugins
app.use(vuetify);
app.use(router);

// Global Properties,
// components can inject this

app.provide("$frappe", frappe);
app.provide("$gv", gv);
app.provide("$sale", sale);
app.provide("$numberFormat",NumberFormat)

// execute code
/* get global data */
const call = frappe.call()
await call.get('epos_restaurant_2023.api.emenu.get_emenu_settings')
.then((r)=> {
	gv.setting = r.message 
}).catch(er=> console.log(er))




// Configure route gaurds
router.beforeEach(async (to, from, next) => {
	next() 
});

app.mount("#app");
