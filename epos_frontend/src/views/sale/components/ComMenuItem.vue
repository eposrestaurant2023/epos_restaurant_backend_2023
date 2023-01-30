<template>
    <div v-if="data.type == 'back'" class="w-36 h-36 rounded-lg shadow-lg cursor-pointer bg-gray-500">
        <div class="relative p-2 w-full h-full flex justify-center items-center" @click="onBack(data.parent)">
            <div>
                <v-icon color="white" size="large">mdi-reply</v-icon>
                <div class="text-white">Back</div>
            </div>
        </div>
    </div>

    <div v-if="data.type == 'menu'" class="relative w-36 h-36 bg-cover bg-no-repeat rounded-lg shadow-lg cursor-pointer overflow-auto" v-bind:style="{
        'background-color': data.background_color,
        'color': data.text_color,
        'background-image': 'url(' + data.photo + ')'
    }"

        @click="onClickMenu(data.name)"
    >
        <div class="absolute top-0 bottom-0 right-0 left-0">
            <avatar class="!h-full !w-full" :name="data.name_en" :rounded="false" :color="data.background_color" v-if="!data.photo"></avatar>
        </div>
        <div class="block relative p-2 w-full h-full">
            <div class="absolute right-1 top-1">
                <v-icon color="white">mdi-folder-open</v-icon>
            </div>
           
            <div class="p-1 rounded-md absolute bottom-1 right-1 left-1 bg-gray-50 bg-opacity-70 text-sm text-center">
                <span>{{ data.name_en }}</span>
                
            </div>
        </div>
    </div>
    <!-- Product -->
    <div v-else-if="data.type == 'product'" class="relative overflow-hidden w-36 h-36 bg-cover bg-no-repeat rounded-lg shadow-lg cursor-pointer bg-gray-300 " v-bind:style="{'background-image': 'url(' + data.photo + ')'}" @click="onClickProduct()">
        <div class="absolute top-0 bottom-0 right-0 left-0" v-if="!data.photo">
            <avatar class="!h-full !w-full" :name="data.name_en" :rounded="false"></avatar>
        </div>
        <div class="block relative p-2 w-full h-full">
            <div class="absolute left-0 top-0 bg-red-700 text-white p-1 rounded-tl-lg rounded-br-lg text-sm">
                <CurrencyFormat :value="data.price"></CurrencyFormat>
            </div>
            <div class="p-1 rounded-md absolute bottom-1 right-1 left-1 bg-gray-50 bg-opacity-70 text-sm text-center">
                {{ data.name_en }}
            </div>
        </div>
    </div>
</template>
<script setup>
    import { computed, addModifierDialog,inject } from '@/plugin'
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
        product.parentMenu = menu;
    }

    function onBack(parent) { 
        const parent_menu = product.posMenu.find(r=>r.name==parent).parent;
        product.parentMenu = parent_menu ;
    }
async function onClickProduct() {
    if (!sale.isBillRequested()) {
        const p = JSON.parse(JSON.stringify(data.value));

        if (JSON.parse(p.modifiers).length > 0 || JSON.parse(p.prices).length > 1) {
            const dt = {
                modifiers: p.modifiers,
                prices: p.prices
            }
            product.setSelectedProduct(data.value);

            let result = await addModifierDialog({
                data: dt
            });

            if (result) {
                if (result.portion != undefined) {
                    p.price = result.portion.price;
                    p.portion = result.portion.portion;
                }
                p.modifiers = result.modifiers.modifiers;
                p.modifiers_data = result.modifiers.modifiers_data;
                p.modifiers_price = result.modifiers.price

            } else {
                return;
            }
        } else {
            p.modifiers = "";
            data.modifiers_data = "[]";
        }

        sale.addSaleProduct(p);
    }

    }
</script>