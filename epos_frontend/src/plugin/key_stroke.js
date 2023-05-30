import { onKeyStroke,useMagicKeys,whenever } from '@vueuse/core'


export default function KeyStroke(app) {
    app.config.globalProperties.$onKeyStroke = onKeyStroke
    app.config.globalProperties.$magicKey = {ctrlP,ctrlU,ctrlS,ctrlQ}
}

const { ctrlP } = useMagicKeys({
    passive: false,
    onEventFired(e) {
      if (e.ctrlKey && e.key === 'p' && e.type === 'keydown')
        e.preventDefault()
    },
})
const { ctrlU } = useMagicKeys({
    passive: false,
    onEventFired(e) {
      if (e.ctrlKey && e.key === 'u' && e.type === 'keydown')
        e.preventDefault()
    },
})
const { ctrlS } = useMagicKeys({
    passive: false,
    onEventFired(e) {
      if (e.ctrlKey && e.key === 's' && e.type === 'keydown')
        e.preventDefault()
    },
})
const { ctrlQ } = useMagicKeys({
    passive: false,
    onEventFired(e) {
      if (e.ctrlKey && e.key === 'q' && e.type === 'keydown')
        e.preventDefault()
    },
})
  