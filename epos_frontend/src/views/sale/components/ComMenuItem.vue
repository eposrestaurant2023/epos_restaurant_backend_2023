<template>
    <div v-if="data.type == 'back'" class="w-36 h-36 rounded-lg shadow-lg cursor-pointer bg-gray-500">
        <div class="relative p-2 w-full h-full flex justify-center items-center" @click="onBack(data.parent)">
            <div>
                <v-icon color="white" size="large">mdi-reply</v-icon>
                <div class="text-white">Back</div>
            </div>
        </div>
    </div>

    <div v-if="data.type == 'menu'" class="w-36 h-36 bg-cover bg-no-repeat rounded-lg shadow-lg cursor-pointer" v-bind:style="{
        'background-color': data.background_color,
        'color': data.text_color,
        'background-image': 'url(' + data.photo + ')'
    }"

        @click="onClickMenu(data.name)"
    >

        <div class="block relative p-2 w-full h-full">
           
            <div class="absolute right-1 top-1">
                <v-icon color="white">mdi-folder-open</v-icon>
            </div>
            <avatar :name="data.name_en" size="100" :rounded="false"></avatar>
            <div class="p-1 rounded-md absolute bottom-1 right-1 left-1 bg-gray-50 bg-opacity-70 text-sm text-center">
                <span>{{ data.name_en }}</span>
                
            </div>
        </div>
    </div>
    <!-- Product -->
    <div v-else-if="data.type == 'product'" class="w-36 h-36 bg-cover bg-no-repeat rounded-lg shadow-lg cursor-pointer bg-gray-300 " v-bind:style="{
        'background-image': 'url(' + data.photo + ')'
    }"
        @click="onClickProduct()"
    >
        <div class="block relative p-2 w-full h-full">
            <avatar v-if="!data.photo" :name="data.name_en" size="125" :rounded="false"></avatar>
            <div class="absolute left-0 top-0 bg-red-700 text-white p-1 rounded-tl-lg rounded-br-lg text-sm">
                {{ data.price }}$
            </div>
            <div class="p-1 rounded-md absolute bottom-1 right-1 left-1 bg-gray-50 bg-opacity-70 text-sm text-center">
                {{ data.name_en }}
            </div>
        </div>
    </div>
</template>
<script setup>
    import { computed, useStore, addModifierDialog,inject } from '@/plugin'
 
    const store = useStore()
    const props = defineProps({data: Object })
    const sale = inject("$sale");
    const product = inject("$product");

    const data = computed(() => {
        return props.data
    })

    const styleObject = computed(()=>{
        return {
            backgroundColor: `'${data.text_color}'`,
            color: data.text_color
        }
    })

    const image = computed(()=>{
        return "'" + data.photo +"'"
    })

    function onClickMenu(menu){
        store.state.sale.parentMenu = menu;
    }

    function onBack(parent) { 
        const parent_menu = store.state.sale.posMenu.find(r=>r.name==parent).parent;
        store.state.sale.parentMenu = parent_menu ;
    }
    async function onClickProduct(){
        const p =   JSON.parse( JSON.stringify(data.value));

        if(JSON.parse(p.modifiers).length > 0 || JSON.parse(p.prices).length>1 ){
            const dt = { 
                modifiers: p.modifiers, 
                prices: p.prices
            }
            product.setSelectedProduct(data.value);

            let result = await addModifierDialog({
                data: dt
            });

            if(result){
               if(result.portion!=undefined){
                p.price =result.portion.price;
                p.portion =result.portion.portion;
               }
                p.modifiers = result.modifiers.modifiers;
                p.modifiers_data = result.modifiers.modifiers_data;
                p.modifiers_price = result.modifiers.price
                
            }else{
                return;
            }
        }else {
            p.modifiers="";
            data.modifiers_data = "[]";
        }

        sale.addSaleProduct(p);
        

    }
</script>