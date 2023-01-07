<template lang="">
    <div>
        <div>
            <ComShortcut/>
        </div>
        <hr/>
        <div>
            <v-row v-if="posMenu">
                <v-col sm="4" v-for="(m, index) in posMenu" :key="index">
                    <v-btn variant="outlined" @click="onClickShortcut(m.name)">
                        {{m.pos_menu_name_en}}
                    </v-btn>
                </v-col>
                <v-col sm="4" v-for="(m, index) in posMenuProduct" :key="index">
                    <v-btn variant="outlined" @click="onClickProduct(m)">
                        {{m.product_name_en}}
                    </v-btn>
                </v-col>
            </v-row>
        </div>
    </div>
</template>
<script>
import ComShortcut from './ComShortcut.vue';
export default {
    name: "ComMenu",
    inject: ["$call"],
    components: {
        ComShortcut
    },
    data() {
        return {
            shortcut_menu: null
        }
    },
    computed: {
        posMenu(){
            return this.$store.state.posMenu;
        },
        posMenuProduct(){
            return this.$store.state.posMenuProduct;
        }
    },
    methods: {
        onClickShortcut(name) {
            this.$store.commit('getPosMenu',name)
            this.$store.commit('getPosMenuProduct',name)
        
        },
        onClickProduct(product){
            this.$store.commit('addPosProduct',product)
        }
    },
    mounted() {
        //
    }
}
</script>