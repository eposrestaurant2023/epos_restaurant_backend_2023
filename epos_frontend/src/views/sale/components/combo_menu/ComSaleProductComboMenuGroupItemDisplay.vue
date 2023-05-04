<template>
    <div>
        <div class="mb-1" v-for="(g, index) in data" :key="index">
            <div class="font-bold underline">{{ g.group_title }}:</div>
            <span v-for="(i, index) in g.products" :key="index">
                <span>{{ i.product_name }} <span class="text-red-600">x{{ i.quantity }}</span></span>
                <span class="pr-1" v-if="index < (g.products.length - 1)">,</span>
            </span>
        </div>
    </div>
</template>
<script setup>
    import {computed} from 'vue'
    import Enumerable from 'linq'
    const props = defineProps({
        comboMenuData: Array
    })
    const data = computed(()=>{
        const list = JSON.parse(props.comboMenuData)
        const groups = Enumerable.from(list).groupBy(
        "{group:$.group,group_title:$.group_title}",
        "{selected:$.selected}",
        "{group:$.group,group_title:$.group_title, selected: $$.count('$.selected')}",
        "$.group+','+$.group_title+','+$.selected"
        ).toArray()
 
        return groups.map(r=>{
            return {
                ...r,
                products: list.filter(x=>x.group == r.group)
            }
        })
    })
</script>
<style lang="">
    
</style>