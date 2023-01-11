import { createStore } from "vuex";
import call from "../../../../doppio/libs/controllers/call";
export default createStore({
  state: {

    isLoading: false,
    drawer: true,
    ePOSSettings:{},
    gv: {},
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
  },
  mutations: {
    loadingStatus(state, new_value) {
      state.isLoading = new_value;
    },
   ePOSSettings(state, new_value) {
      state.ePOSSettings = new_value;
    },
    gv(state, new_value) {
      state.gv = new_value;
    },
    sale(state, new_value){
      state.sale = new_value
    },
    posMenu(state, new_value){
      state.posMenu = new_value;
    },
    posMenuProduct(state, new_value){
      state.posMenuProduct = new_value;
    },
    drawer(state, new_value){
      state.drawer = new_value;
    },
    async getPosMenu(state, new_value){
      await call('frappe.client.get_list', {
          doctype: 'POS Menu',
          fields: ['*'],
          filters: {
              "parent_pos_menu": new_value
          }
      }).then((res) => {
          state.posMenu = res;
      })
    },
    async getPosMenuProduct(state,new_value){
      await call('epos_restaurant_2023.api.api.get_product_menu', {
        pos_menu: new_value,
      }).then((res) => {
          state.posMenuProduct = res;
      })
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
  },
  actions: {
    loadingStatus({ commit }) {
      commit("loadingStatus");
    },
    posMenu({ commit }){
      commit("posMenu")
    },
    posMenuProduct({ commit }){
      commit("posMenuProduct")
    },
    sale({ commit }){
      commit("sale")
    },
  },
  
});
