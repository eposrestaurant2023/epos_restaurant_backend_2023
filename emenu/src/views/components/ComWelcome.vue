<template>
    <div>
        <h1 :style="{color:gv.setting?.template_style?.title_color}" :class="gv.setting?.template_style?.title_class">{{ gv.setting?.welcome_title }}</h1>
        <div v-if="gv.setting?.welcome_description">
            <div :style="{color:gv.setting?.template_style?.subtitle_color}" v-html="gv.setting?.welcome_description"></div>
        </div>
    </div>
</template>
<script setup>
import {inject} from 'vue'
const gv = inject('$gv')

</script>