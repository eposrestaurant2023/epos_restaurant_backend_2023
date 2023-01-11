import call from "../../../../doppio/libs/controllers/call"
export default {
    install: (app, options) => {
      app.prototype.$call = call
    }
  }