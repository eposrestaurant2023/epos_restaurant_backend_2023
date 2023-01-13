import { createStore } from "vuex";
import saleModule from "./modules/sale";
export default createStore({
  modules: {
    sale: saleModule
  },
  state: {
    isLoading: false,
    drawer: true,
    ePOSSettings:{},
  },
  mutations: {
    isLoading(state, new_value) {
      state.isLoading = new_value;
    },
   ePOSSettings(state, new_value) {
      state.ePOSSettings = new_value;
    },
    drawer(state, new_value){
      state.drawer = new_value;
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
