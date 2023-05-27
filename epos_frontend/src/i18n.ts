// i18n
import { createI18n } from 'vue-i18n';

import en from './locales/en.json';
import kh from './locales/kh.json';
export const i18n = createI18n({
    legacy: false, // you must set `false`, to use Composition API
	locale: (localStorage.getItem('lang')||"en"),
	fallbackLocale: (localStorage.getItem('lang')||"en"),// set fallback locale
	globalInjection: true,
	messages: {  en,kh,  },
});

 