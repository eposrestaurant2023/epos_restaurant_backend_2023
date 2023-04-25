<template>
    <div class="py-2">
        <div>
            <div class="px-2 pt-2 text-lg font-bold">Modifier</div>
            <!-- <v-list lines="one"> 
                <v-list-item class="!pl-0" v-for="(p, index) in modifier" :key="index" :value="p">
                    <template v-slot:prepend="{ isActive }">
                        <v-list-item-action start>
                            <v-radio color="primary" :model-value="isActive"></v-radio>
                        </v-list-item-action>
                    </template>
                    <v-list-item-title>
                        <div class="flex justify-between">
                            <div>{{ p }}</div> 
                        </div>
                    </v-list-item-title>
                </v-list-item>
            </v-list> -->
            <div v-for="(item, index) in modifier" :key="index">
                {{ item }}
            </div>
        </div>
    </div>
</template>
<script setup>
const props = defineProps({
    modifier: Array
})

// function getModifierItem(category) {
//     return category.items.filter((r) => {
//         return (r.branch == '' || r.branch == '') && String(r.modifier).toLocaleLowerCase().includes(this.keyword.toLocaleLowerCase());
//     });
// }
</script>
<style lang="">
    
</style>