<template>
    <div v-if="data.type == 'back'" class="h-full rounded-lg shadow-lg cursor-pointer bg-gray-500">
        <div v-ripple class="relative p-2 w-full h-full flex justify-center items-center" @click="onBack(data.parent)">
            <div>
                <v-icon color="white" size="large">mdi-reply</v-icon>
                <div class="text-white">{{ $t('Back') }}</div>
            </div>
        </div>
    </div>

    <div v-if="data.type == 'menu'" v-ripple
        class="relative h-full bg-cover bg-no-repeat rounded-lg shadow-lg cursor-pointer overflow-auto" v-bind:style="{
                'background-color': data.background_color,
                'color': data.text_color,
                'background-image': 'url(' + encodeURIComponent(data.photo).replace(/%2F/g, '/') + ')'
            }" @click="onClickMenu(data.name)">
        <div class="absolute top-0 bottom-0 right-0 left-0">
            <avatar class="!h-full !w-full" :name="data.name_en || ''" :rounded="false" :color="data.background_color"
                v-if="!data.photo"></avatar>
        </div>
        <div class="block relative p-2 w-full h-full">
            <div class="absolute right-1 top-1">
                <v-icon color="white">mdi-folder-open</v-icon>
            </div>

            <div class="p-1 rounded-md absolute bottom-1 right-1 left-1 bg-gray-50 bg-opacity-70 text-sm text-center">
                <span class="text-black" v-if="!sale.load_menu_lang">{{ getMenuName(data) }}</span>
            </div>
        </div>
    </div>
    <!-- Product -->
    <div v-else-if="data.type == 'product'" v-ripple
        class="relative overflow-hidden h-full bg-cover bg-no-repeat rounded-lg shadow-lg cursor-pointer bg-gray-300 "
        v-bind:style="{ 'background-image': 'url(' +  encodeURIComponent(image).replace(/%2F/g, '/') + ')' }" @click="onClickProduct()">
        <div class="absolute top-0 bottom-0 right-0 left-0" v-if="!image">
            <avatar class="!h-full !w-full" :name="data.name_en||''" :rounded="false" background="#f1f1f1"></avatar>
        </div> 
        <div class="block relative p-2 w-full h-full">
            <div class="absolute left-0 top-0 bg-red-700 text-white p-1 rounded-tl-lg rounded-br-lg text-sm">
                <span>
                    <span v-if="productPrices.length > 1">
                        <span>
                            <CurrencyFormat :value="minPrice" />
                        </span> <v-icon icon="mdi-arrow-right" size="x-small" /> <span>
                            <CurrencyFormat :value="maxPrice" />
                        </span>
                    </span>
                    <CurrencyFormat v-else :value="showPrice" />
                </span>
            </div>
            <div class="p-1 rounded-md absolute bottom-1 right-1 left-1 bg-gray-50 bg-opacity-90 text-sm text-center">
                <span v-if="!sale.load_menu_lang">{{ data.name }} - {{ getMenuName(data) }}</span>                
            </div>
        </div>
    </div>
</template>
<script setup>
import { computed, addModifierDialog, inject, keypadWithNoteDialog,SaleProductComboMenuGroupModal, createToaster } from '@/plugin'
import Enumerable from 'linq'
// import ComPriceOnMenu from '../ComPriceOnMenu.vue';
const props = defineProps({ data: Object })
const sale = inject("$sale");
const product = inject("$product");
const toaster = createToaster({position: 'top'})
const frappe = inject("$frappe")
const db = frappe.db();

// get image
const image = computed(() => {
    return props.data.photo
})
// price menu
const productPrices = computed(() => {
    if (props.data.prices) {
        const r = JSON.parse(props.data.prices)
        return r.filter(r => (r.branch == sale.sale.business_branch || r.branch == '') && r.price_rule == sale.sale.price_rule)
    }
    return []
})
const showPrice = computed(() => {
    if (props.data.is_combo_menu) {
        return props.data.price || 0
    }
    if (productPrices.value.length == 1) {
        return productPrices.value[0].price
    }
    else if (productPrices.value.length == 0) {
        return props.data.price || 0
    }
    return 0
})
const maxPrice = computed(() => {
    if (productPrices.value.length > 1) {
        return Enumerable.from(productPrices.value).max("$.price")
    }
    return 0
})
const minPrice = computed(() => {
    if (productPrices.value.length > 1) {
        return Enumerable.from(productPrices.value).min("$.price")
    }
    return 0
})

function getMenuName(menu) {
    const mlang = localStorage.getItem('mLang');
    if(mlang != null){
        if(mlang=="en"){
            return menu.name_en;
        }else{
            return menu.name_kh;
        }
        
    }else{
        localStorage.setItem('mLang','en');
        return menu.name_en;
    }
}


// end price menu

function onClickMenu(menu) {
    if(sale.setting.pos_menus.length>0){
        product.parentMenu = menu;
    } else{    
        product.getProductMenuByProductCategory(db,menu)
    }
    
}




function onBack(parent) {
    const parent_menu = product.posMenuResource.data?.find(r => r.name == parent).parent;
    product.parentMenu = parent_menu;
}
async function onClickProduct() {
    if (!sale.isBillRequested()) {
      
        const p = JSON.parse(JSON.stringify(props.data));
        product.is_open_price = p.is_open_price
        if (p.is_open_product == 1) {
         
            let productPrices = await keypadWithNoteDialog({
                data: {
                    title: `${p.name_en}`,
                    label_input: 'Enter Price',
                    note: "Open Menu Note",
                    category_note_name: "Open Menu Note",
                    number: 0,
                    product_code: p.name
                }
            });
           
            if (productPrices) {
                
                p.name_en = productPrices.note;
                p.price = productPrices.number;
                p.modifiers = '';
                sale.addSaleProduct(p);
                return
            }else{
                return
            }

        }
        else if (p.is_combo_menu) {
            await onComboMenu(p)
            p.modifiers = "";
            p.portion = "";
            p.modifiers_data = "[]";
        }
        else {
            const portions = JSON.parse(p.prices)?.filter(r => (r.branch == sale.sale.business_branch || r.branch == '') && r.price_rule == sale.sale.price_rule);
            const check_modifiers = product.onCheckModifier(JSON.parse(p.modifiers));
            if (portions?.length == 1) {
                p.price = portions[0].price
                p.unit = portions[0].unit
            }

            if (check_modifiers || portions?.length > 1 || p.is_open_price) {
                const pro_data = props.data
                if (p.is_open_price && portions.length==0){
                    pro_data.prices = JSON.stringify( [{"price":p.price,"branch":"","price_rule":sale.sale.price_rule,"portion":"Normal","unit":p.unit,"default_discount":0}])                
                }

                product.setSelectedProduct(pro_data);

                let productPrices = await addModifierDialog();

                if (productPrices) {
                    if (productPrices.portion != undefined) {
                        p.price = productPrices.portion.price;
                        p.portion = productPrices.portion.portion;
                        p.unit = productPrices.portion.unit
                    }
                    p.modifiers = productPrices.modifiers.modifiers;
                    p.modifiers_data = productPrices.modifiers.modifiers_data;
                    p.modifiers_price = productPrices.modifiers.price

                } else {
                    return;
                }
            } else {
                p.modifiers = "";
                p.modifiers_data = "[]";
                p.portion = "";
            }
        }
        sale.addSaleProduct(p);

    }

}

async function onComboMenu(p){
    if (p.is_combo_menu && p.use_combo_group) {
        product.setSelectedComboMenu(p)
        const result = await SaleProductComboMenuGroupModal();
        if(result){
            
            if(result.combo_groups.length > 0){
                p.combo_menu = getSeperateNameComboGroup(p,result.combo_groups)
                
                p.combo_group_data = JSON.stringify(result.combo_groups)

            }else{
                p.combo_menu = ''
                p.combo_group_data = "[]"
            }
        }
    } else { 
        if(p.is_combo_menu && p.combo_menu_data){
            const combo_menu_data = JSON.parse(p.combo_menu_data)
            p.combo_menu = getSeperateName(combo_menu_data)
            
        }        
    }
}

function getSeperateNameComboGroup(p,list){
    let combo_groups = JSON.parse(p.combo_group_data)
    let combo_menu_items = []
    combo_groups.forEach((x)=>{
        combo_menu_items.push('***' + x.pos_title + '***')
        let combo_menus = []
        list.forEach(r=> {
        
            if(r.group == x.combo_group){
                combo_menus.push( r.product_name + ' x' + r.quantity)
            }
        })
        
        combo_menu_items.push(combo_menus.join(", "))
    })
    return combo_menu_items.join("|")
}
function getSeperateName(list){
    let combo_menus = []
        list.forEach(r=> {
            combo_menus.push( r.product_name + ' x' + r.quantity)
        })
        return combo_menus.join(", ")
}
</script>

 