<template>
    <div class="py-2">
   
            <div class="px-2 pt-2 text-lg font-bold">Modifiers</div> 
            <v-list-item class="!pl-0 px-2" v-for="(item, index) in modifiers" :key="index" :value="item"> 
                <v-list-item-title v-if="getModifierItem(item).length>0">
                    <div class="flex font-bold" >
                        <span>{{  item.category  }}</span>
                        <span v-if="item.is_required" class="text-red-500 mx-2 text-xs">( *{{'Required' }} )</span>
                    </div>
                    <hr/>
                    <v-list lines="one"> 
                        <v-list-item class="!pl-0" v-for="(m, index) in getModifierItem(item)" :key="index" :value="m" @click="onSelectModifier(item, m)">
                            <template v-slot:prepend="{ isActive }">
                                <v-list-item-action start>
                                    <v-radio color="primary" :model-value="m.selected"></v-radio>
                                </v-list-item-action>
                            </template>
                            <v-list-item-title>
                                <div class="flex  justify-between" >
                                    <div>{{m.modifier }}</div>
                                    <div class="font-bold" :style="{ color: title_color }">
                                        <CurrencyFormat :value="m.price" />
                                    </div>
                                </div>
                            </v-list-item-title>
                        </v-list-item>
                    </v-list>
                </v-list-item-title>
            </v-list-item>
 
    </div>
</template>
<script setup>
    import CurrencyFormat from '../../components/CurrencyFormat.vue';  
    import Enumerable from 'linq';
    
    const props = defineProps({
        modifiers: Array,
        title_color:String
    }) 

    function getModifierItem(category){
        return category.items.filter((r) => {
                return (r.branch == this.setting?.business_branch || r.branch == '');
            });
    }

   function onSelectModifier(category, modifier) {
 
        if (category.is_multiple == 0) {
            Enumerable.from(category.items).where(`$.selected==true`).forEach("$.selected=false");
        }
        modifier.selected = !modifier.selected;
    }

</script>
<style lang="">
    
</style>