import { createRouter, createWebHistory } from "vue-router";
import Home from "../views/Home.vue";
import CategoryDetail from "../views/category/CategoryDetail.vue";
 
// import authRoutes from './auth';
const pos_profile = "UE9TX1Byb2ZpbGU=";
const category = "Q2F0ZWdvcnk=";  
const table ="VGFibGU="
const routes = [ 
  { path: "/emenu", redirect: `/emenu/${pos_profile}= /${table}= `}, 
  { path: `/emenu/${pos_profile}=:pos_profile/${table}=:table_name/`, name: "Home", component: Home },
  { path: `/emenu/${pos_profile}=:pos_profile/${table}=:table_name/category/${category}=:category?`, name: "CategoryDetail", component: CategoryDetail },
];

const router = createRouter({
  base: "/emenu/",
  history: createWebHistory(),
  routes,
});

export default router;
