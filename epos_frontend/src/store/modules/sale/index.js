import {createResource, createStore} from "@/plugin";
import Enumerable from 'linq'

const state = {
    keyword: '',
    posMenu: null,
    parentMenu: null,
    posMenuProduct: [],
    posProduct:[],
    sale:{
      customer:'C2022-0007',
      business_branch: 'SR Branch',
      outlet: 'Main Outlet - SR',
      stock_location:'SR - Main Stock',
      sale_products:[]
    }
  }
  const mutations = {
    keyword(state, new_value){
      state.keyword = new_value
    },
    sale(state, new_value){
      state.sale = new_value
    },
    ADD_SALE_PRODUCT(state, new_value){
      state.sale.sale_products.push(new_value)
    },
    POS_MENU(state, new_value){
      state.posMenu = new_value;
    },
    PARENT_MENU(state, new_value){
      state.posMenu = new_value;
    },
    POS_MENU_PRODUCT(state, new_value){
      state.posMenuProduct = new_value;
    },
    UPDATE_SALE_PRODUCT(state, new_value){
      new_value.sub_total = new_value.price * new_value.quantity
    },
    UPDATE_SUMMARY(state, new_value){
      const sp = state.sale.sale_products ;
      state.sale.total_quantity = getNumber(Enumerable.from(sp).sum("$.quantity"));
      state.sale.sub_total =  getNumber(Enumerable.from(sp).sum("$.sub_total"));
      state.sale.product_discount =  getNumber(Enumerable.from(sp).sum("$.discount_amount"));
      state.sale.total_discount = getNumber(state.sale.product_discount + state.sale.sale_discount);

      //tax
      state.sale.tax_1_amount =  getNumber(Enumerable.from(sp).sum("$.tax_1_amount"));
      state.sale.tax_2_amount =  getNumber(Enumerable.from(sp).sum("$.tax_2_amount"));
      state.sale.tax_3_amount =  getNumber(Enumerable.from(sp).sum("$.tax_3_amount"));
      state.sale.total_tax =getNumber(  Enumerable.from(sp).sum("$.total_tax"));

      //grand_total
      state.sale.grand_total =  (state.sale.sub_total - state.sale.total_discount) + state.sale.total_tax 
    },
    UPDATE_SALE_PRODUCT_QUANTITY(state, sp){
      sp.quantity = sp.quantity + 1;
    },
  }
  const actions = {
    filterMenu({dispatch}, name){
        dispatch('filterPosMenu',name)
    },
    addMenuProduct({commit}, product){
        commit('posMenuProduct',product)
    },
    addPosProduct({commit}, new_value){
    let sp = Enumerable.from(state.sale.sale_products)
      .where(`$.product_code=='${new_value.name}'`)
      .firstOrDefault()
      
    if (sp!=undefined){ 
      commit("UPDATE_SALE_PRODUCT_QUANTITY", sp)

    }else { 
      var saleProduct = {
        product_code: new_value.name,
        product_name: new_value.name_en,
        product_name_kh: new_value.name_kh,
        unit: new_value.unit,
        quantity: 1,
        price: new_value.price,
        amount: 1 * new_value.price,
        is_inventory_product:new_value.is_inventory_product,
        product_photo:new_value.photo
      }

      commit('UPDATE_SALE_PRODUCT',saleProduct);
      commit('ADD_SALE_PRODUCT', saleProduct)
    }
      commit('UPDATE_SUMMARY')
      
  },
    async onGetPosMenu({commit}){ 
        const setting = JSON.parse(localStorage.getItem('setting'))
        let defaultMenu = setting.default_pos_menu;
 
        if(localStorage.getItem('default_menu')){
          defaultMenu = localStorage.getItem('default_menu')
        }
        
        await createResource({
            url: 'epos_restaurant_2023.api.product.get_product_by_menu',
            auto: true,
            params:{
                root_menu: defaultMenu
            },
            auto:true,
            async onSuccess(doc) {
                commit('POS_MENU',doc)
                // store.state.app.isLoading = false
            },
            onError(x) {
              store.dispatch('app/endLoading');
            }
          })
    },
    
  }



// function updateSaleProduct(sp){
//   sp.sub_total = sp.price * sp.quantity

// }

function getNumber(val) {
  val =  (val = val == null ? 0 : val)
  if (isNaN(val)){
    return 0;
  }
  return val;
}



export default {
    namespaced: true,
    state,
    mutations,
    actions
}