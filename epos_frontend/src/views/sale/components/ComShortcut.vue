<template lang="">
    <div>
        <div>
            <v-row v-if="shortcut_menu">
                <v-col sm="4" v-for="(m, index) in shortcut_menu" :key="index">
                    <v-btn @click="onClickShortcut(m.name)">
                        {{m.pos_menu_name_en}}
                    </v-btn>
                </v-col>
            </v-row>
        </div>
    </div>
</template>
<script>
export default {
    name: "ComShortcut",
    inject: ["$call"],
    data() {
        return {
            shortcut_menu: null,
            menu: null
        }
    },
    methods: {
        async onLoadData() {
            await this.$call('frappe.client.get_list', {
                doctype: 'POS Menu',
                fields: ['*'],
                filters: {
                    "shortcut_menu": 1,
                }
            }).then((res) => {
                if(res.length > 0){
                    this.shortcut_menu = res;
                }
                
            })
        },
        onClickShortcut(name) {
            this.$store.commit('getPosMenu',name)
            this.$store.commit('getPosMenuProduct',name)
        }
    },
    mounted() {
        this.onLoadData()
    }
}
</script>