<template>
    <v-row>

        <v-col cols="12" sm="6" v-for="(c, index) in categories" :key="index">
            <v-card @click="onClick(c)">
                <v-img :src="c.background_image"  class="align-end" gradient="to bottom, rgba(0,0,0,.1), rgba(0,0,0,.5)"
                height="150px" cover >
                    <v-card-title class="text-white" v-text="c.title_en"></v-card-title>
                </v-img>

                <v-card-actions v-if="c.show_description"> 
                    <div v-html="c.description"></div>
                </v-card-actions>
            </v-card>
        </v-col> 
    </v-row>
</template>
<script setup>
    import {useRouter} from 'vue-router'
    const router = useRouter();
    const props = defineProps({
        categories: []
    })

    function onClick(category) {
        console.log(category)
        router.push({name: 'CategoryDetail', params:{category: category.name}})
    }
</script>
 