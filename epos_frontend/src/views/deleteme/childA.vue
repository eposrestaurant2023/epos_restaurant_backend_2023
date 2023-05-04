<template lang="">
    <div>
        <div class="bg-slate-600 h-20">
            header
            <slot name="header"></slot>
        </div>
        <div>
            <slot name="defualt"></slot>
        </div>
        <div class="bg-green-300 p-3">
            <slot name="footer"></slot>
            <slot name="header"></slot>
        </div>
    </div>
</template>
<script>
 
</script>
<style lang="">
    
</style>