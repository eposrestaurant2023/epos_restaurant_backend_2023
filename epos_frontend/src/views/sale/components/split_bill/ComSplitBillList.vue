<template lang=""> 
    <div>
        <div class="flex -m-1 overflow-auto py-1">
            <v-btn variant="tonal" class="m-1" :color="!g.visibled ? 'error' : 'gray'" :prepend-icon="!(g.visibled) ? 'mdi-eye-off':'mdi-eye'" 
                    v-for="(g, index) in data.filter((x)=>x.deleted == false)" :key="index" @click="(()=>g.visibled = !g.visibled)" >
                    {{g.no}}
            </v-btn>
        </div>
        <hr/>

        <div class="grid gap-2 sm:grid-cols-4 md:grid-cols-4 lg:grid-cols-4 xl:grid-cols-5 mt-4">
            <v-card v-for="(g, index) in data.filter((x)=>x.deleted == false && x.visibled)" :key="index">
                <v-card-title class="!p-0">
                    <v-toolbar height="55">
                        <v-toolbar-title class="text">
                            <div class="text-lg">{{$t('No')}}: {{ g.no }}</div>
                            <div class="text-sm">#{{g.sale.name || "New"}}</div>                            
                        </v-toolbar-title>
                        <template v-slot:append>
                            <v-btn v-if="g.show_download" @click="onDownloadPressed(g)" style="height:35px; width:35px; margin-right:5px" variant="outlined" color="primary" icon="mdi-download-circle-outline">
                    
                            </v-btn>
                            <v-btn v-if="(!g.is_current)" @click="onDeleteBillPressed(g)" style="height:35px; width:35px; margin-left:5px" variant="outlined" color="error" icon="mdi-delete-outline">
                                
                            </v-btn> 
                        </template>
                    </v-toolbar>
                </v-card-title>
                <v-card-text class="!pt-0 !pr-0 !pb-14 !pl-0">       
                    <v-list>
                        <v-list-item class="!p-0" v-for="(sp, _index) in g.sale.sale_products" :key="_index"  @click="onSelected(sp)">                       
                            <div class="text-sm relative px-2 border-b" >
                                <v-badge :content="sp.total_selected" style="margin-top:-10px; margin-right:2px" color="success" class="absolute top-2 right-2" v-if="sp.total_selected > 0"></v-badge>
                        
                                <div class="flex" style="margin-top:10px;">
                                    <div class="grow">
                                        <div> {{ sp.product_name }}<v-chip class="ml-1" size="x-small"
                                                color="error" variant="outlined" v-if="sp.portion">{{ sp.portion }}</v-chip> <v-chip
                                                v-if="sp.is_free" size="x-small" color="success" variant="outlined">Free</v-chip>
                                        </div> 
                                        <div class="text-xs pt-1"> 
                                            <div v-if="sp.modifiers">
                                                <span>{{ sp.modifiers }} (<CurrencyFormat :value="sp.modifiers_price * sp.quantity" />)
                                                </span>
                                            </div>
                                            <div class="text-red-500" v-if="sp.discount > 0">
                                                {{$t('Discount')}} :
                                                <span v-if="sp.discount_type == 'Percent'">{{
                                                    sp.discount
                                                }}%</span>
                                                <CurrencyFormat v-else :value="parseFloat(sp.discount)" />
                                            </div>
                                            <v-chip color="blue" size="x-small" v-if="sp.seat_number">Seat# {{
                                                sp.seat_number
                                            }}</v-chip>
                                            <div class="text-gray-500" v-if="sp.note">
                                                {{$t('Note')}}: <span>{{ sp.note }}</span>
                                            </div> 
                                        </div>
                                    </div>
                                    <div class="flex-none text-right w-36"> 
                                        <div class="">
                                            <div class="text-sm"> 
                                                {{ sp.quantity }} x <CurrencyFormat :value="sp.price" />
                                            </div>
                                        </div>
                                    </div>
                                </div>                                
                            </div>     
                        </v-list-item>
                        <v-divider></v-divider>
                    </v-list>
                </v-card-text>
                <v-card-actions style="min-height:8px !important" class="!p-0 flex items-center justify-between absolute bottom-0 w-full bg-gray-300"></v-card-actions>
            </v-card>
            {{ showDownload()}}
        </div>
    </div>
</template>
<script setup> 


const props = defineProps({
    data: Object
});

function onSelected(sp) {
    if ((sp.total_selected || 0) >= sp.quantity) {
        sp.total_selected = 0;
    } else {
        sp.total_selected = (sp.total_selected || 0) + 1;
    }

    showDownload();
}


function showDownload() {
    const _sale_products = props.data.filter((d) => d.deleted == false).flatMap(a => (a.sale.sale_products || []).filter((r) => (r.total_selected || 0) > 0));
    if (_sale_products.length > 0) {
        props.data.filter((d) => d.deleted == false).forEach((g) => {
            g.show_download = true;
            const chk = (g.sale.sale_products || []).filter((r) => (r.total_selected || 0) > 0);
            if (chk.length > 0) {
                g.show_download = false;
            }
        });
    }
    else {
        props.data.filter((d) => d.deleted == false).forEach((g) => {
            g.show_download = false;
        });
    }
}

function onDownloadPressed(group) {
    const result = props.data.filter((d) => d.deleted == false).flatMap(a => (a.sale.sale_products || []).filter((r) => (r.total_selected || 0) > 0))
    const temp = [];
    result.forEach((sp) => {
        const _sp = JSON.parse(JSON.stringify(sp));
        sp.quantity -= sp.total_selected;
        if (sp.quantity <= 0) {
            temp.push(sp);
        }
        else {
            _sp.name = "";
            _sp.quantity = sp.total_selected;
        }

        //set sale product parent id
        _sp.parent = "";
        if (_sp.original_parent == group.sale.name) {
            _sp.parent = _sp.original_parent;
        }

        //check 
        let _sale_products = (group.sale.sale_products || []).filter((sp) => sp.original_name == _sp.original_name);

        if (_sale_products.length > 0) {
            if (_sale_products[0].quantity == _sp.original_quantity) {
                _sale_products[0].name = _sp.original_name;
                _sp.total_selected = sp.total_selected = 0;
                group.sale.sale_products.push(_sp);
            }
            else {

                if (sp.quantity == 0 && _sale_products[0].name == "") {
                    _sale_products[0].name = sp.original_name;
                }

                _sale_products[0].quantity += sp.total_selected;
                _sale_products[0].total_selected = sp.total_selected = 0;
            }
        }
        else {
            _sp.total_selected = sp.total_selected = 0;
            group.sale.sale_products.push(_sp);
        }

    });


    //remove sale product when qty equal zero
    temp.forEach((t) => {
        props.data.filter((x) => x.no != group.no && x.deleted == false).forEach((z) => {
            let sp = z.sale.sale_products;
            if (sp.filter((y) => y.quantity == 0).length > 0) {
                sp.splice(sp.indexOf(t), 1);
            }
        });
    });
}


//method delete bill 
function onDeleteBillPressed(group) {
    const _current_sale = props.data.filter((r) => r.is_current == true && r.deleted == false);
    if (_current_sale.length > 0) {
        const result = props.data.filter((x) => x.deleted == false).flatMap(a => (a.sale.sale_products || []));
        result.forEach((r) => r.total_selected = 0);
        (group.sale.sale_products || []).forEach((r) => r.total_selected = r.quantity);
        onDownloadPressed(_current_sale[0]);

        if (group.sale.name == "") {
            props.data.splice(props.data.indexOf(group), 1);
        }
        else {
            group.deleted = true;
        }
    }
}


</script>
<style>
.wrapper-badge-split-bill .v-badge__wrapper {
    display: block !important;
    padding: 6px;
}
</style>