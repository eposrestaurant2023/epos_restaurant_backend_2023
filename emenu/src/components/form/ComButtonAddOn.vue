<template>
    <div class="flex justify-end">
        <template v-if="qty > 0">
            <button type="button" class="btn-delete h-7 w-7 rounded-sm mr-2" @click.stop="onDelete()">
                <v-icon>mdi-delete</v-icon>
            </button>
            <button type="button" class="btn-add h-7 w-7 rounded-sm" @click.stop="onMinus()">
                <v-icon>mdi-minus</v-icon>
            </button>
            <div class="h-7 px-2 flex items-center justify-center">{{ qty }}</div>
        </template>
        <button type="button" class="btn-add h-7 w-7 rounded-sm" @click.stop="onAdd()">
            <v-icon>mdi-plus</v-icon>
        </button>
    </div>
</template>
<script setup>
    import {ref} from 'vue'
    let qty = ref(0)
    function onAdd() {
        qty.value++
    }
    function onMinus() {
        qty.value--
    }
    function onDelete() {
        qty.value = 0
    }
</script>
<style scoped>
.btn-add {
    border: 1px solid rgb(6, 180, 6);
    color: rgb(6, 180, 6);
}
.btn-delete {
    border: 1px solid rgb(219, 24, 10);
    color: rgb(219, 24, 10);
}
</style>