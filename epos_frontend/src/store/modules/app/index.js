
const state = {
    isLoading: false,
    drawer: true,
}
const mutations = {
    isLoading(state, new_value) {
        state.isLoading = new_value;
    },
    drawer(state, new_value){
        state.drawer = new_value;
    }
}
const actions = {
    startLoading({ commit }) {
        commit("isLoading", true);
    },
    endLoading({ commit }) {
        commit("isLoading", false);
    },
}

export default {
    namespaced: true,
    state,
    mutations,
    actions
}