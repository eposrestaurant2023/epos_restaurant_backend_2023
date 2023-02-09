<template>
    <v-btn class="text-none" stacked :size="mobile ? 'x-small' : 'small'">
        <v-badge content="9" color="success">
            <v-icon>mdi-cart</v-icon>
        </v-badge>
    </v-btn>
</template>
<script setup>
import { useDisplay } from 'vuetify'
const { mobile } = useDisplay()
</script>