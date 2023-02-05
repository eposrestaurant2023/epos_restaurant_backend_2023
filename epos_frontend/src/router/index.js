import { createRouter, createWebHistory } from "vue-router";
import Home from "../views/Home.vue";
import AddSale from "../views/sale/AddSale.vue";
import Sale from "../views/sale/Sale.vue";
import TableLayout from "../views/sale/TableLayout.vue";
import OpenShift from "../views/shift/OpenShift.vue";
import CloseShift from "../views/shift/CloseShift.vue";
import StartWorkingDay from "../views/shift/StartWorkingDay.vue";
import CloseWorkingDay from "../views/shift/CloseWorkingDay.vue";
import CustomerDetail from "../views/customer/CustomerDetail.vue";
import Customer from "../views/customer/Customer.vue";
import TestPage from "../views/TestPage.vue";
import ReceiptList from "@/views/receipt_list/ReceiptList.vue"
import StartupConfig from "@/views/checking_system/StartupConfig.vue"
 
import authRoutes from './auth';

const routes = [
  { path: "/epos_frontend", name: "Home", component: Home, meta: { layout: 'main_layout' } },
  {path: '/epos_frontend/startup-config', name: 'StartupConfig',component: StartupConfig, meta: { isStartupConfig: true }, props: true},
  { path: "/epos_frontend/add-sale/:name?", name: "AddSale", component: AddSale, meta: { layout: 'sale_layout' }},
  { path: "/epos_frontend/customer-detail/:name?", name: "CustomerDetail", component: CustomerDetail, },
  { path: "/epos_frontend/customer", name: "Customer", component: Customer,meta: { layout: 'main_layout' }},
  { path: "/epos_frontend/sale", name: "Sale", component: Sale, },
  { path: "/epos_frontend/table", name: "TableLayout", component: TableLayout, meta: { layout: 'main_layout' } },
  { path: "/epos_frontend/receipt-list", name: "ReceiptList", component: ReceiptList, meta: { layout: 'main_layout' } },
  { path: "/epos_frontend/open-shift", name: "OpenShift", component: OpenShift, meta: { layout: "main_layout" } },
  { path: "/epos_frontend/close-shift", name: "CloseShift", component: CloseShift, meta: { layout: "main_layout" } },
  { path: "/epos_frontend/start-working-day", name: "StartWorkingDay", component: StartWorkingDay, meta: { layout: "main_layout" } },
  { path: "/epos_frontend/close-working-day", name: "CloseWorkingDay", component: CloseWorkingDay, meta: { layout: "main_layout" } },
  { path: "/epos_frontend/test-page", name: "TestPage", component: TestPage, meta: { layout: "main_layout" } },

  ...authRoutes,
];

const router = createRouter({
  base: "/epos_frontend/",
  history: createWebHistory(),
  routes,
});

export default router;
