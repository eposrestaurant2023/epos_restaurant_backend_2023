<template>
    <div class="container">
        <ComProductSearch/>
        <v-row>
            <v-col sm="16">
                <h2>MENU</h2>
                <ComMenu/>
            </v-col>
            <v-col sm="16">
                <ComSelectedCustomer/>
                <hr/>
                <table>
                    <thead>
                        <tr>
                            <th>Product</th>
                            <th>Unit</th>
                            <th>Price</th>
                            <th>Amount</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(p, index) in sale.sale_products" :key="index">
                            <td>{{ p.product_code }} - {{ p.product_name }}</td>
                            <td><input type="text" v-model="p.unit"/></td>
                            <td><input type="number" v-model="p.quantity"/></td>
                            <td><input type="number" v-model="p.price"/></td>
                            <td>{{ p.amount }}</td>
                        </tr>
                    </tbody>
                </table>
            </v-col>
        </v-row>
        <div class="pa-4">
            <button @click="onSave()">Save</button>
            {{ sale }}
        </div>
    </div>
</template>
<script>
import ComProductSearch from './components/ComProductSearch.vue';
import ComSelectedCustomer from './components/ComSelectedCustomer.vue';
import ComMenu from './components/ComMenu.vue';

export default {
    name: "AddSale",
    inject: ['$api'],
    components:{
        ComProductSearch,
        ComSelectedCustomer,
        ComMenu
    },
    computed: {
        sale(){
            return this.$store.state.sale;
        }
    },
    methods:{
        async onSave(){
            await this.$api('Sale', this.sale).then((res) => {
                alert( atob('aGVsbG8gbWFkYSBmYWth'));
                alert('Success')
            })
        },
        
    },
    async mounted(){
        if(this.$route.params.name){
            await this.$api('Sale', {
                name: this.$route.params.name
            }).then((res) => {
                if(res.data)
                    this.$store.commit('sale',res.data)
            })
        }
        
    }
}
</script>
<style lang="">
    
</style>