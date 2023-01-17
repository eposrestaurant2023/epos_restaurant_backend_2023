<template>
    <div class="p-5">
        <h2>Hrllo world</h2>
        <div>{{ var1 }}</div>
        <div>{{ data.age }}</div>
        <button @click="onTextClick">click me</button>
        <ComDemo v-model="var1">

                <div>
                    askldfhsdaf
                    fashdfjhaskdfsadf
                    jkahfsdfhkljsdhf
                </div>
        </ComDemo>
       <br/>
        <button @click="open">xxx</button>
    </div>
</template>
<script setup>
    import { ref, reactive } from 'vue'
    import ComConfirm from '../components/ComConfirm.vue';
    import { openDialog } from 'vue3-promise-dialog';
    import ComDemo from '../components/ComDemo.vue'
    const var1 = ref("xx")
    const ar = [1,2,3,4]
    let data = reactive({
        name: 'Jon',
        age: 90
    })
    function onClick(value, msg) {
        alert(msg)
    }
    function onTextClick(msg) {
        alert(msg)
    }
    async function open() {
        let result = await openDialog(ComConfirm, {
            title:'title',
            data:data}
        )
        if(result == false)
            alert('Cancel')
        else
            alert('OK')
    }
</script>
<style scoped>
    .h2{
        background: red;
    }
</style>