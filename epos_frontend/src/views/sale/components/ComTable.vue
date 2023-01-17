<template>
  <interact
    draggable
    :dragOption="dragOption"
    resizable
    :resizeOption="resizeOption"
    class="resize-drag"
    :style="style"
    @dragmove="dragmove"
    @resizemove="resizemove"
  >
    Drag and drop, resize from any edge or corner
  </interact>
</template>

<script>
  import Vue from "vue";
  import VueInteractJs from "vue-interactjs";
  import interact from "interactjs";

  export default {
    name: "resizeDrag",
    data: () => ({
      resizeOption: {
        edges: { left: true, right: true, bottom: true, top: true }
      },
      dragOption: {
        modifiers: [
          interact.modifiers.restrictRect({
            restriction: "parent",
            endOnly: true
          })
        ],
      },
      // values for interact.js transformation
      x: 0,
      y: 0,
      w: 200,
      h: 200,
    }),

    computed: {
      style() {
        return {
          height: `${this.h}px`,
          width: `${this.w}px`,
          transform: `translate(${this.x}px, ${this.y}px)`,
        };
      }
    },

    methods: {
      dragmove(event) {
        this.x += event.dx;
        this.y += event.dy;
      },
      resizemove(event) {
        this.w = event.rect.width;
        this.h = event.rect.height;

        this.x += event.deltaRect.left;
        this.y += event.deltaRect.top;
      }
    }
  };
</script>

<style scoped>
.resize-drag {
  box-sizing: border-box;
  background: #41b883;

  /* To prevent interact.js warnings */
  user-select: none;
  -ms-touch-action: none;
  touch-action: none;
}
</style>