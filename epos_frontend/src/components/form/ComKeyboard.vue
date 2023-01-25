<template>
  <div :class="keyboardClass"></div>
</template>

<script>
import Keyboard from "simple-keyboard";
import "simple-keyboard/build/css/index.css";

export default {
  name: "SimpleKeyboard",
  props: {
    keyboardClass: {
      default: "simple-keyboard",
      type: String
    },
    input: {
      type: String
    },
    lang: {
      type: String,
      default: 'en'
    }
  },
  data: () => ({
    keyboard: null,
    isCombineInput:false,
    layout_en: {
        'default': [
          '` 1 2 3 4 5 6 7 8 9 0 - = {bksp}',
          '{tab} q w e r t y u i o p [ ] \\',
          '{lock} a s d f g h j k l ; \' {enter}',
          '{shift} z x c v b n m , . / {shift}',
          '{ខ្មែរ} @ {space}'
        ],
        'shift': [
          '~ ! @ # $ % ^ &amp; * ( ) _ + {bksp}',
          '{tab} Q W E R T Y U I O P { } |',
          '{lock} A S D F G H J K L : " {enter}',
          '{shift} Z X C V B N M &lt; &gt; ? {shift}',
          '{ខ្មែរ} @ {space}'
        ]
      },
      layout_kh: {
        'default': [
          '« ១ ២ ៣ ៤ ៥ ៦ ៧ ៨ ៩ ០ ឥ ឲ {bksp}',
          '{tab} ឆ ឹ េ រ ត យ ុ ិ ោ ផ ៀ ឪ ឮ',
          '{lock} ា ស ដ ថ ង ហ ្ ក ល ើ ់ {enter}',
          '{shift} ឋ ខ ច វ ប ន ម​ ុំ​ ។ ៊ {shift}',
          '{English} @ {space}'
        ],
        'shift': [
          '»‍‌‌‌‌ ! ៗ @ " ៑ $ € ៙ ៚ * { } x ៎ ៛ % ៍ ័ ៏ () ៌ = {bksp}',
          '{tab} ឈ ឺ  ែ ឬ ទ ួ ូ ី ៅ ភ ឿ ឧ ឭ ៜ ៝ ឯ ឫ ឨ ឦ ឱ ឰ ឩ \ឳ​',
          '{lock} ាំ ឝ ៖ ៈ ​ៃ ឌ ធ អ ះ ញ គ ឡ ោះ ៉ {enter}',
          '{shift} ឍ ឞ , . / ឃ ជ​ េះ ព ណ ​ំ ុះ ៕ ? {shift}',
          '{English} @ {space}'
        ]
      }
  }),
  mounted() {
    this.keyboard = new Keyboard(this.keyboardClass, {
      onChange: this.onChange,
      onKeyPress: this.onKeyPress,
      layout: this.lang == 'en' ? this.layout_en : this.layout_kh
    });
  },
  methods: {
    onChange(data) {
      
      if(this.isCombineInput==false ){ 
        if(this.input != undefined)
          data = this.input + data
      this.isCombineInput = true;
      }
      this.$emit("onChange", data);
    },
    onKeyPress(button) {
      this.$emit("onKeyPress", button);
      

      /**
       * If you want to handle the shift and caps lock buttons
       */
      if (button === "{shift}" || button === "{lock}") this.handleShift();
    },
    handleShift() {
      let currentLayout = this.keyboard.options.layoutName;
      let shiftToggle = currentLayout === "default" ? "shift" : "default";

      this.keyboard.setOptions({
        layoutName: shiftToggle
      });
    }
  },
  watch: {
    input(input) {
      this.keyboard.setInput(input);
    }
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
