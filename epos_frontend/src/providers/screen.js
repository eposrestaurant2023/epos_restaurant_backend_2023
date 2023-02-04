
export default class Screen {
    constructor(){
        this.heightAppBar = 0,
        this.widthWrapMenu = 0,
        this.width = 0,
        this.height = 0,
        this.small = false,
        this.chipSize = 'small'
    }
    onResizeHandle() {
        this.width = window.innerWidth
        this.height = window.innerHeight
        if(this.width <= 1024){
            this.small = true
            this.chipSize = 'x-small'
        }else{
            this.small = false
            this.chipSize = 'small'
        }
        const width_wrap_menu = document.querySelector('#wrap_menu')?.offsetWidth
        this.widthWrapMenu = width_wrap_menu ? width_wrap_menu : 0;

    }
}