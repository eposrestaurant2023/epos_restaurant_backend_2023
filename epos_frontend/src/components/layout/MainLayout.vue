<template>
    <v-app>
		<v-main>
			<v-app-bar :elevation="2">
                <template #title>
                    <ComProductSearch/>
                </template>
                <template #append>
                    <v-menu :location="location">
                        <template v-slot:activator="{ props }">
                            <v-avatar image="https://pyxis.nymag.com/v1/imgs/51b/28a/622789406b8850203e2637d657d5a0e0c3-avatar-rerelease.rsquare.w700.jpg"  v-bind="props"></v-avatar>
                        </template>
                        <v-list>
                            <v-list-item
                                prepend-avatar="https://cdn.vuetifyjs.com/images/john.png"
                                title="John Leider"
                                subtitle="john@google.com"
                            >
                                <template v-slot:append>
                                <v-btn
                                    size="small"
                                    variant="text"
                                    icon="mdi-menu-down"
                                ></v-btn>
                                </template>
                            </v-list-item>
                        </v-list>
                        <v-divider></v-divider>
                        <v-list
                        :lines="false"
                        density="compact"
                        nav
                        >
                        <v-list-item>
                            <template v-slot:prepend>
                            <!-- <v-icon :icon="item.icon"></v-icon> -->
                            </template>
                            <v-list-item-title v-text="item.text"></v-list-item-title>
                        </v-list-item>
                        </v-list>
                    </v-menu>
                </template>
            </v-app-bar>
            <template></template>
        </v-main>
    </v-app>
</template>
<script>
import ComProductSearch from '../../views/sale/components/ComProductSearch.vue'
export default {
    name:"MainLayout",
    components: {
        ComProductSearch
    }
}
</script>
<style lang="">
    
</style>