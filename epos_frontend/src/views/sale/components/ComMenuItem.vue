<template>
    <div class="w-28 h-28" >
        hello
    </div>
</template>
<script setup>
    import { computed } from '@/plugin'
    const props = defineProps({data: Object })
    const data = computed(() => {
        return props.data
    })
</script>