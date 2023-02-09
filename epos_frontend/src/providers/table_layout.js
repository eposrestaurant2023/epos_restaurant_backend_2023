import Enumerable from 'linq'
import moment from '@/utils/moment.js';
import {  keyboardDialog, createResource, createDocumentResource, addModifierDialog, useRouter, confirmDialog,saleProductDiscountDialog } from "@/plugin"
import { createToaster } from "@meforma/vue-toaster";

 
const toaster = createToaster({ position: "top" });
export default class TableLayout {
    constructor() {
        this.isLoading = false;
        this.setting = null;
        this.tab = null;
        this.currentView = "table_group";
        this.table_groups = [];
        this.canArrangeTable = false;
        this.getTableGroups();
        this.saleListResource = null;
        this.saveTablePositionResource = null;
        this.initSaveTablePositionResource()

    }
    //end constructor
    initSaveTablePositionResource(){
        const parent = this;
        this.saveTablePositionResource = createResource({
            url: "epos_restaurant_2023.api.api.save_table_position",
            onSuccess(d) {
                toaster.success("Save table position successfully");
                this.onEnableArrangeTable(false);
                localStorage.setItem("table_groups", JSON.stringify(table_groups));
        
            }
        })
    }
    getSaleList(){
        const parent = this;
        this.saleListResource = createResource({
            url: "frappe.client.get_list",
            params: {
                doctype: "Sale",
                fields: ["name", "creation", "grand_total", "total_quantity", "tbl_group", "tbl_number", "guest_cover", "grand_total", "sale_status", "sale_status_color", "sale_status_priority", "customer", "customer_name", "phone_number"],
                filters: {
                    pos_profile: localStorage.getItem("pos_profile"),
                    docstatus: 0
                },
            },
            auto:true,
            onSuccess(data) {
                parent.table_groups.forEach(function (g) {
                    g.tables.forEach(function (t) {
                        t.sales = data.filter(r => r.tbl_group == g.table_group && r.tbl_number == t.tbl_no)
                        if (t.sales.length > 0) {
                            t.guest_cover = t.sales.reduce((n, r) => n + r.guest_cover, 0)
                            t.grand_total = t.sales.reduce((n, r) => n + r.grand_total, 0)
                            t.background_color = t.sales.sort((a, b) => a.sale_status_priority - b.sale_status_priority)[0].sale_status_color;
                            t.creation = Enumerable.from(t.sales).orderBy("$.creation").select("$.creation").toArray()[0]
                        }

                    })
                })
                //loading()
            }
        });
    }
    


    getTableGroups(){
        this.table_groups = JSON.parse(localStorage.getItem("table_groups"));
         
    }
    
    

    onEnableArrangeTable(status) {
        if (status) {
            this.isLoading = true;
        }
        this.canArrangeTable =!this.canArrangeTable ;// status;
        // this.table_groups.forEach((d) => {

        //     d.tables.forEach((t) => {
        //         alert(t.w)
        //         t.dragable = status;
        //         t.resizable = status;
        //     })
        // });
        // if (status) {
        //     setTimeout(() => {
        //         this.isLoading= false;

        //     }, 100);
        // }

    }
    
    


}


