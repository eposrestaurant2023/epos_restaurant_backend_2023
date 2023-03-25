

<template>
  hello : {{ message }} === world
  
  <button @click="sendMessage()">Test</button>
</template>
<script>
import { ref, onMounted } from 'vue'
import { useSocket } from 'vue-3-socket.io'

export default {
  setup() {
    const message = ref('')
    const { socket } = useSocket()

    const sendMessage = () => {
      socket.emit('message', message.value)
      message.value = ''
    }

    onMounted(() => {
      socket.value.on('message', (data) => {
        console.log(data)
      })
    })

    return {
      message,
      sendMessage
    }
  }
}

</script>