// i18n
import { createI18n } from 'vue-i18n';
import { FrappeApp } from 'frappe-js-sdk';
 
async function loadLocaleMessages() {
	let messages ={};
	const frappe = new FrappeApp();
	const db = frappe.db();
	await	db.getDoc('POS Translation', (localStorage.getItem('lang')||"en") )
	.then((docs) => {
		console.log(messages)
		messages = JSON.parse(`{"${docs.name}":${docs.translate_text}}`);
		console.log(JSON.stringify( messages))
	})  
	return messages;
  }


export const i18n = createI18n({
    legacy: false, // you must set `false`, to use Composition API
	locale: (localStorage.getItem('lang')||"en"),
	fallbackLocale: (localStorage.getItem('lang')||"en"),// set fallback locale
	globalInjection: true,
	messages:await loadLocaleMessages()// {  en,kh,  },
	// message: {"en":{"Address and Note":"Address & Note","Working Day and Cashier Shift Report":"Working Day & Cashier Shift Report","Submit and New":"Submit & New","Change or Merge Table":"Change/Merge Table","msg":{"You do not have permission to perform this action":"You don't have permission to perform this action","Your working day is to long please close your working day":"Your working day is to long. Please close your working day","There is no cashier shift opened":"There's no cashier shift opened","are you sure to close cashier shift":"Are you sure to close cashier shift?","are you sure to close working day":"Are you sure to close working day?","are you sure to delete this sale order":"Are you sure to delete this sale order?","There are pending orders":"There are {0} pending orders","are you sure to start shift":"Are you sure to start shift?","Add is Successfully":"Add {0} is Successfully","please save or submit your current order first":"Please {0} your current order first","You cannot add other payment type with":"You cannot add other payment type with {0}","this sale order is already print bill please cancel print bill first":"This sale order is already print bill. Please cancel print bill first.","are you sure to process payment and close order":"Are you sure to process payment and close order?","are you sure to process quick pay and close order":"Are you sure to process quick pay and close order?","Login fail Invalid username or password":"Login fail. Invalid username or password","are your sure to cancel this sale order":"Are you sure to cancel this sale order?"}}}

})

 