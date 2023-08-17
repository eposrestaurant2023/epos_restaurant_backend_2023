<template>
    <div class="py-2">
        <div>
            <div class="px-2 pt-2 text-lg font-bold">Portion</div>
            <div class="text-xs mt-1 pl-2">Please select one.</div>
            <v-list lines="one"> 
                <v-list-item class="!pl-0" v-for="(p, index) in portions" :key="index" :value="p" @click="onPortionClick(p)">
                    <template v-slot:prepend="{ isActive }">
                        <v-list-item-action start>
                            <v-radio color="primary" :model-value="p.selected"></v-radio>
                        </v-list-item-action>
                    </template>
                    <v-list-item-title>
                        <div class="flex  justify-between" >
                            <div>{{ p.portion }}</div>
                            <div class="font-bold" :style="{ color: title_color }">
                                <CurrencyFormat :value="p.price" />
                            </div>
                        </div>
                    </v-list-item-title>
                </v-list-item>
            </v-list>
        </div>
    </div>
</template>
<script setup>
    import CurrencyFormat from '../../components/CurrencyFormat.vue';

    
    const props = defineProps({
        portions: Array,
        title_color:String
    })


    function onPortionClick(p){
        props.portions.forEach(e=>{
            e.selected = false;
        })
         p.selected = true;
    }
</script>
<style lang="">
    
</style>