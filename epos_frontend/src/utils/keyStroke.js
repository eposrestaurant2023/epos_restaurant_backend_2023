
import { onKeyStroke,useKeyModifier  } from '@vueuse/core'
const control = useKeyModifier('Control')
// myFunction.js
export default function KeyStroke (keyname, method) {
    
    onKeyStroke(keyname, (e) => {
        e.preventDefault()
        method()
    })
}