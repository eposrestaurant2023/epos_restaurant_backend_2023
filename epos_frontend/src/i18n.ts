// i18n
import { createI18n } from 'vue-i18n';
import { FrappeApp } from 'frappe-js-sdk';

// import en from './locales/en.json';
// import kh from './locales/kh.json';
 
 
async function loadLocaleMessages() {
	let messages ={};
	const frappe = new FrappeApp();
	const db = frappe.db();
	await	db.getDoc('POS Translation', (localStorage.getItem('lang')||"en") )
	.then((docs) => {
		messages = JSON.parse(`{"${docs.name}":${docs.translate_text}}`);
	})  
	return messages;
  }


export const i18n = createI18n({
    legacy: false, // you must set `false`, to use Composition API
	locale: (localStorage.getItem('lang')||"en"),
	fallbackLocale: (localStorage.getItem('lang')||"en"),// set fallback locale
	globalInjection: true,
	messages:await loadLocaleMessages()// {  en,kh,  },
})

 