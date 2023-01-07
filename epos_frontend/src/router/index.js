import { createRouter, createWebHistory } from "vue-router";
import Home from "../views/Home.vue";
import AddSale from "../views/sale/AddSale.vue";
import authRoutes from './auth';

const routes = [
  {
    path: "/",
    name: "Home",
    component: Home,
  },
  {
    path: "/add-sale/:name?",
    name: "AddSale",
    component: AddSale,
  },

  ...authRoutes,
];

const router = createRouter({
  base: "/epos_frontend/",
  history: createWebHistory(),
  routes,
});

export default router;
