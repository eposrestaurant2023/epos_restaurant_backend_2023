import Enumerable from 'linq'
import {keyboardDialog,createResource} from "@/plugin"
import { createToaster } from "@meforma/vue-toaster";

const setting = JSON.parse(localStorage.getItem("setting"))
const toaster = createToaster({ position:"top" });

export default class Product {
	constructor() {
		this.posMenu = []
        this.parentMenu="";
        this.searchProductKeyweord="";
        this.selectedProduct = {};
        this.prices = [];
        this.modifiers = [];
        this.keyword="";

      this.loadPOSMenu();
	}
    async loadPOSMenu(){
        if(this.posMenu.length==0){
            const data = createResource({
                 url: 'epos_restaurant_2023.api.product.get_product_by_menu',
             params:{
                 root_menu: setting.default_pos_menu
             },
             
             })
              
             await data.fetch()
             this.posMenu = data.data;
           

         }
    }
    getPOSMenu(){
        
    if (this.getString(this.searchProductKeyweord) == "") {
           
            if (this.parentMenu) {
                return this.posMenu.filter(r => r.parent == this.parentMenu)
            }
            else {
                let defaultMenu = setting.default_pos_menu;
    
                if (localStorage.getItem('default_menu')) {
                    defaultMenu = localStorage.getItem('default_menu')
                }
    
                return this.posMenu ? this.posMenu.filter(r => r.parent == defaultMenu) : null;
            }
        } else {
           
            return this.posMenu.filter((r) => {
                return String( r.name_en + ' ' + r.name_kh + ' ' + r.name ).toLocaleLowerCase().includes(this.searchProductKeyweord.toLocaleLowerCase())  && r.type =="product"
            })
    
     
        }
    }


    setSelectedProduct(p){

        this.selectedProduct = p;
        this.prices = [];
        this.modifiers = [];
        let prices=  JSON.parse( p.prices);
        prices.filter(r=>r.branch==setting.business_branch).forEach((p)=>{
            p.selected = false;
            this.prices.push(p)
        });
        if(this.prices.length>0){
            this.prices[0].selected = true;
        }
        
        let modifiers=  JSON.parse( p.modifiers);
        modifiers.forEach((r)=>{
            r.selected = false
        });
        this.modifiers =modifiers;
    }
    setSelectedProductByMenuID(id){
        const p = Enumerable.from( this.posMenu).where(`$.menu_product_name=='${id}'`).firstOrDefault();
        this.setSelectedProduct(p);
    }
    setModifierSelection(sp){
        Enumerable.from( this.prices).where("$.selected==true").forEach("$.selected = false");
        const portion = Enumerable.from( this.prices).where(`$.portion=='${sp.portion}'`).firstOrDefault();
        if(portion!=undefined){
            portion.selected = true;
        }
        const selectedModifiers = JSON.parse(sp.modifiers_data)
        const modfierItems = Enumerable.from(this.modifiers).selectMany("$.items");
        if(selectedModifiers!=undefined){  
                selectedModifiers.forEach((r)=>{
                    const modifierItem = modfierItems.where(`$.name=='${r.name}'`).firstOrDefault();
                    if(modifierItem!=undefined){
                        modifierItem.selected = true;
                    }
                })
        }
    }

    getModifierItem(category) {
        if (this.keyword == "") {
            return category.items.filter((r) => {
                return (r.branch == setting.business_branch || r.branch == '')
            });
        } else {

            return category.items.filter((r) => {
                    return (r.branch == setting.business_branch || r.branch == '') && String(r.modifier).toLocaleLowerCase().includes(this.keyword.toLocaleLowerCase());
                });
        }
    }



    onSelectPortion(p){
        Enumerable.from(this.prices).where("$.selected==true").forEach("$.selected=false");
        p.selected = true;
    }
    onSelectModifier(category, modifier){
        if(category.is_multiple==0){
            Enumerable.from(category.items).where(`$.selected==true`).forEach("$.selected=false");
        }
        modifier.selected = !modifier.selected;
       
    }

    getSelectedPortion(){
        if (this.prices.length == 1) {
            return this.prices[0];
        }
        let selected = Enumerable.from(this.prices).where("$.selected==true").firstOrDefault();
        if (selected == undefined) {
            selected = this.prices[0]
        }
        return selected;
    }
    getSelectedModierList(){
        return (Enumerable.from(this.modifiers).selectMany("$.items").where("$.selected==true").orderBy("$.modifier")).toArray();
    }

    getSelectedModifier() {
      
        const selected = (Enumerable.from(this.modifiers).selectMany("$.items").where("$.selected==true").orderBy("$.modifier"));
        let modifiers = selected.select("r=>r.prefix + ' ' + r.modifier").toJoinedString(", ");
        if(modifiers=="[]" || modifiers==undefined){
            modifiers = "";
        }
             
        return {
            modifiers_data:selected.select("x => {name:x['name'], modifier: x['modifier'], price: x['price'] }").toJSONString(),
            modifiers:modifiers,
            price:selected.sum("$.price")

        }

    }

    validateModifier(){
        return new Promise((resolve)=>{
            this.modifiers.forEach((c)=>{
                if (c.is_required==1){
                    if(c.items.filter(r=>r.selected==true).length==0){
                        toaster.warning("Please select a modifier of " + c.category);
                        resolve(false)
                    }
                }
            })
            resolve(true)
        })
    }
      

     
    getString(val) {
        val = (val = val == null ? "" : val)
        return val;
    }
}