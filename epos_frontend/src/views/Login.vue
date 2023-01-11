<template class="h-full">
  <div class="h-full w-full py-16 px-4">
    <form @submit.prevent="login">
      <div class="flex flex-col items-center justify-center">
        <div class="bg-red-800 shadow rounded lg:w-1/3  md:w-1/2 w-full p-10 mt-16">

          <div class="mt-6  w-full">
            <img :src="ePOSSettings.epos_logo"/>
            <label for="pass" class="text-sm font-medium leading-none text-gray-800">
              Password   {{ ePOSSettings.epos_app_name }} {{ ePOSSettings.epos_logo }}
            </label>
            <div class="relative flex items-center justify-center">
              <input v-model="password" id="pass" type="password"
                class="bg-gray-200 border rounded  text-xs font-medium leading-none text-gray-800 py-3 w-full pl-3 mt-2" />
            </div>
          </div>
          <div class="mt-8">
            <v-btn @click="numpad_click('1')">
              1
            </v-btn>
            <v-btn @click="numpad_click('2')">
              2
            </v-btn>
            <v-btn @click="numpad_click('3')">
              3
            </v-btn>
            <v-btn @click="numpad_click('4')">
              4
            </v-btn>
            <v-btn @click="numpad_click('5')">
              5
            </v-btn>
            <v-btn @click="numpad_click('6')">
              6
            </v-btn>
            <v-btn @click="numpad_click('7')">
              7
            </v-btn>
            <v-btn @click="numpad_click('8')">
              8
            </v-btn>
            <v-btn @click="numpad_click('9')">
              9
            </v-btn>
            <v-btn @click="numpad_click('0')">
              0
            </v-btn>
            <v-btn @click="numpad_click('0')">

            </v-btn>
            <v-btn @click="clear_password">
              Clear
            </v-btn>
            <div id="x">xxx</div>
            <button role="button" type="submit" v-if="!this.$store.state.isLoading"
              class="mysubmit focus:ring-2 focus:ring-offset-2 focus:ring-indigo-700 text-sm font-semibold leading-none text-white focus:outline-none bg-indigo-700 border rounded hover:bg-indigo-600 py-4 w-full">
              Login
            </button>
            <button role="button" type="button" disabled v-else
              class="focus:ring-2 focus:ring-offset-2 focus:ring-indigo-700 text-sm font-semibold leading-none text-white focus:outline-none bg-indigo-700 border rounded hover:bg-indigo-600 py-4 w-full">
              Login
              <v-progress-circular indeterminate color="primary"></v-progress-circular>
            </button>

          </div>
        </div>
      </div>
    </form>
  </div>
</template>
<script>
export default {
  data() {
    return {
      username: "Administrator",
      password: "",
    };
  },
  inject: ["$auth", "$call"],
  async mounted() {
    if (this.$route?.query?.route) {
      this.redirect_route = this.$route.query.route;
      this.$router.replace({ query: null });
    }
  },
  methods: {
    numpad_click(n) {
      if (this.password == undefined) {
        this.password = "";
      }
      this.password = this.password + n;
    },
    clear_password() {
      this.password = "";
    },
    async login() {
      try {
        this.$store.state.isLoading = true;
        let res = await this.$call("epos_restaurant_2023.api.api.check_username", { "pin_code": this.password });
        this.username = res.username;
        if (this.username && this.password) {
          let res = await this.$auth.login(this.username, this.password);
          if (res) {
            this.$router.push({ name: "Home" });

          } else {
            this.$toast.warning(`Login fail. Invalid username or password.`);
          }
         
        }

      } catch (error) {
        this.$toast.error(error.messages ? error.messages.join("\n") : error)
      } finally {
        this.$store.state.isLoading = false;
      }
    },
  },
  computed:{
    ePOSSettings(){
      return this.$store.state.ePOSSettings
    }
  }
};
</script>
