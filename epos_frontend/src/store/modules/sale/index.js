import {createResource, createStore} from "@/plugin";
const store = createStore()
const state = {
    posMenu: [],
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
      alert(name)
        // dispatch('getPosMenu',name)
        dispatch('filterPosMenuProduct',name)
    },
    addMenuProduct({commit}, product){
        commit('posMenuProduct',product)
    },
    async filterPosMenuProduct({commit}, new_value){
        if(!new_value) return;
        createResource({
            url: 'epos_restaurant_2023.api.product.get_product_by_menu',
            auto: true,
            params:{
                root_menu: new_value
            },
            auto:true,
            async onSuccess(doc) {
                commit('POS_MENU_PRODUCT',doc)
            },
            onError(x) {
              //store.dispatch('endLoading');
            }
          })
    },
    // async getPosMenu({commit}, new_value){
    //     createResource({
    //         url: 'frappe.client.get_list',
    //         auto: true,
    //         params:{
    //             doctype:"Product",
    //             fields:["*"],
    //             filters: {
    //                 "parent_pos_menu": new_value
    //             }
    //         },
    //         auto:true,
    //         async onSuccess(doc) {
    //             commit('POS_MENU_PRODUCT',doc)
    //         },
    //         onError(x) {
    //           //store.dispatch('endLoading');
    //         }
    //       })
    // },
  }

export default {
    namespaced: true,
    state,
    mutations,
    actions
}