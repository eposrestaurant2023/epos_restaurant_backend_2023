<template lang="">
    <div>
        <div>
            <ComShortcut/>
        </div>
        <div class="mt-4">
            <v-row>
                <!-- <v-col sm="4" v-for="(m, index) in posMenu" :key="index">
                    <v-btn variant="outlined" @click="onClickMenu(m.name)">
                        {{m.pos_menu_name_en}}
                    </v-btn>
                </v-col> -->
                <v-col sm="4" v-for="(m, index) in menuProduct" :key="index">
                    <ComMenuItem :data="m"></ComMenuItem>
                    <v-btn variant="outlined" @click="onClickProduct(m)">
                        {{m.pos_menu_name_en}}
                    </v-btn>
                </v-col>
            </v-row>
        </div>
    </div>
</template>
<script setup>
import ComShortcut from './ComShortcut.vue';
import ComMenuItem from './ComMenuItem.vue';
import { useStore, computed } from '@/plugin';
const store = useStore()

const menuProduct = computed(()=>{
    return store.state.sale.posMenuProduct
})
// import { mapState } from 'vuex';
//
// export default {
//     name: "ComMenu",
//     inject: ["$call"],
//     components: {
//         ComShortcut
//     },
//     data() {
//         return {
//             shortcut_menu: null
//         }
//     },
//     computed: {
//         ...mapState("sale",["POS_MENU",'POS_MENU_PRODUCT'])
//     },
//     methods: {
//         onClickMenu(name) {
//             this.$store.dispatch('sale/searchMenu',name)
//         },
//         onClickProduct(product){
//             this.$store.commit('sale/addPosProduct',product)
//         }
//     },
//     mounted() {
//         //
//     }
// }
</script>