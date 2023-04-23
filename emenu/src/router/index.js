import { createRouter, createWebHistory } from "vue-router";
import Home from "../views/Home.vue";
import CategoryDetail from "../views/category_detail/CategoryDetail.vue";
import TestPage from "../views/TestPage.vue";
// import authRoutes from './auth';

const routes = [
  { path: "/", redirect: '/emenu/dashboard'},
  { path: "/emenu/:branch?/dashboard", name: "Home", component: Home },
  { path: "/emenu/category/:category?", name: "CategoryDetail", component: CategoryDetail },
  { path: "/emenu/test-page", name: "TestPage", component: TestPage},
  // ...authRoutes,
];

const router = createRouter({
  base: "/emenu/",
  history: createWebHistory(),
  routes,
});

export default router;
