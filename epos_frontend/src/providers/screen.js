
export default class Screen {
    constructor(){
        this.heightAppBar = 0,
        this.heightShortcutMenu = 0
    }
    onResizeHandle() {
        // console.log(window.innerWidth)
        this.heightShortcutMenu = document.querySelector('#shortcut_menu').offsetHeight;
    }
}