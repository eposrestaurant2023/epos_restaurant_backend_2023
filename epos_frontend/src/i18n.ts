// i18n
import { createI18n } from 'vue-i18n';
import { FrappeApp } from 'frappe-js-sdk';
import axios from 'axios';
import { log } from 'console';

async function loadLocaleMessages() {
	try {
		const response = await axios.get("http://192.168.10.103:1217/api/method/epos_restaurant_2023.configuration.doctype.pos_translation.pos_translation.get_translation?lang=en");
		console.log("rathaaaaaaaaaaaaaaaaaaaaaaaaaa")
	 } catch (error) {
		// Handle the error
	 }
	try{
		let messages ={};
		const frappe = new FrappeApp();
		const db = frappe.db();
		await db.getDoc('POS Translation', (localStorage.getItem('lang')||"en") )
		.then((docs) => {		 
			messages = JSON.parse(`{"${docs.name}":${docs.translate_text}}`);		 
		})  
		return messages;
	}
	catch (error) {
		console.log(error)
	}
	
  }


export const i18n = createI18n({
    legacy: false, 
	locale: (localStorage.getItem('lang')||"en"),
	fallbackLocale: (localStorage.getItem('lang')||"en"),
	globalInjection: true,
	messages:await loadLocaleMessages()
})

 