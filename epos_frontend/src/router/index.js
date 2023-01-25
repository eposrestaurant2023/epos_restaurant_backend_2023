import { createRouter, createWebHistory } from "vue-router";
import Home from "../views/Home.vue";
import AddSale from "../views/sale/AddSale.vue";
import Sale from "../views/sale/Sale.vue";
import TableLayout from "../views/sale/TableLayout.vue";
import OpenShift from "../views/shift/OpenShift.vue";
import CloseShift from "../views/shift/CloseShift.vue";
import StartWorkingDay from "../views/shift/StartWorkingDay.vue";
import CustomerDetail from "../views/customer/CustomerDetail.vue";
import Customer from "../views/customer/Customer.vue";
import TestPage from "../views/TestPage.vue";
import ReceiptList from "@/views/receipt_list/ReceiptList.vue"
import StartupConfig from "@/views/checking_system/StartupConfig.vue"
 
import authRoutes from './auth';

const routes = [
  { path: "/", name: "Home", component: Home, meta: { layout: 'main_layout' } },
  {path: '/startup-config', name: 'StartupConfig',component: StartupConfig, meta: { isStartupConfig: true }, props: true},
  { path: "/add-sale/:name?", name: "AddSale", component: AddSale, meta: { layout: 'sale_layout' }},
  { path: "/customer/:name?", name: "CustomerDetail", component: CustomerDetail, },
  { path: "/customer", name: "Customer", component: Customer, },
  { path: "/sale", name: "Sale", component: Sale, },
  { path: "/table", name: "TableLayout", component: TableLayout, meta: { layout: 'main_layout' } },
  { path: "/receipt-list", name: "ReceiptList", component: ReceiptList, meta: { layout: 'main_layout' } },
  { path: "/open-shift", name: "OpenShift", component: OpenShift, meta: { layout: "main_layout" } },
  { path: "/close-shift", name: "CloseShift", component: CloseShift, meta: { layout: "main_layout" } },
  { path: "/start-working-day", name: "StartWorkingDay", component: StartWorkingDay, meta: { layout: "main_layout" } },
  { path: "/test-page", name: "TestPage", component: TestPage, meta: { layout: "main_layout" } },

  ...authRoutes,
];

const router = createRouter({
  base: "/epos_frontend/",
  history: createWebHistory(),
  routes,
});

export default router;
