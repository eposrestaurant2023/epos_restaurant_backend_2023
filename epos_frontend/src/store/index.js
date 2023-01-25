import { createStore } from "vuex";
import SaleModule from "./modules/sale";
import AppModel from "./modules/app";
export default createStore({
  namespaced: true,
  modules: {
    sale: SaleModule,
    app: AppModel
  },
  state: {
    isLoading: false,
    drawer: true,
    setting: null
  },
  mutations: {
    isLoading(state, new_value) {
      state.isLoading = new_value;
    },
    drawer(state, new_value){
      state.drawer = new_value;
    },
    drawer(state, new_value){
      state.setting = new_value;
    }
  },
  actions: {
    startLoading({ commit }) {
      commit("isLoading", true);
    },
    endLoading({ commit }) {
      commit("isLoading", false);
    },
  },
  
});
