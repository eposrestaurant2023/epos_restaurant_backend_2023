import { onKeyStroke } from '@vueuse/core'


export default function KeyStroke(app) {
    app.config.globalProperties.$onKeyStroke = onKeyStroke
}


  