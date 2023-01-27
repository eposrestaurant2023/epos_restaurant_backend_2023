import Enumerable from 'linq'
import {keyboardDialog,createResource} from "@/plugin"
import { createToaster } from "@meforma/vue-toaster";

const setting = JSON.parse(localStorage.getItem("setting"))
const toaster = createToaster({ position:"top" });

export default class Product {
	constructor() {
		this.posMenu = []
        this.selectedProduct = {};
        this.prices = [];
        this.modifiers = [];
        this.keyword="";

      this.getPOSMenu();
	}
    async getPOSMenu(){
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
        const p = Enumerable.from( this.posMenu).where(`$.menu_product_id=='${id}'`).firstOrDefault();
        setSelectedProduct(p);
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

    getSelectedModifier() {
      
        const selected = (Enumerable.from(this.modifiers).selectMany("$.items").where("$.selected==true").orderBy("$.modifier"));
        let modifiers = selected.select("r=>r.prefix + ' ' + r.modifier").toJoinedString(", ");
        if(modifiers=="[]" || modifiers==undefined){
            modifiers = "";
        }
             
        return {
            modifiers_data:selected.select("x => { modifier: x['modifier'], price: x['price'] }").toJSONString(),
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
      

     
     
}