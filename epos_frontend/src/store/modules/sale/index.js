import {createResource, createStore} from "@/plugin";
const state = {
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
    sale(state, new_value){
      state.sale = new_value
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
    addPosProduct(state, new_value){
      if(state.sale.sale_products.filter(r=>r.product_code == new_value.name).length > 0)
        state.sale.sale_products = state.sale.sale_products.map(function(r){
          if(r.product_code === new_value.product_code){
              return r.quantity++
          }    
      })
      else
        var saleProduct = {
          product_code: new_value.product_code,
          product_name_en: new_value.product_name_en,
          product_name_kh: new_value.product_name_kh,
          unit: new_value.unit,
          quantity: 1,
          price: new_value.price,
          amount: 1 * new_value.price
        }
        state.sale.sale_products.push(saleProduct)
    }
  }
  const actions = {
    filterMenu({dispatch}, name){
        dispatch('filterPosMenu',name)
    },
    addMenuProduct({commit}, product){
        commit('posMenuProduct',product)
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

export default {
    namespaced: true,
    state,
    mutations,
    actions
}