// Copyright (c) 2023, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Vehicle Rental", {
    refresh(frm){
     
        //set_indicator(frm);

       
    },
    setup(frm){
        
       
    },  
    start_date(frm){   
        if (frm.doc.start_date && frm.doc.end_date){
            frm.doc.total_days = frappe.datetime.get_diff( frm.doc.end_date, frm.doc.start_date )
            refresh_field('total_days');
        }  
    },
    end_date(frm){    
        if (frm.doc.start_date && frm.doc.end_date){
            frm.doc.total_days = frappe.datetime.get_diff( frm.doc.end_date, frm.doc.start_date )
            refresh_field('total_days');
        }
    } ,
    adult(frm){
        frm.doc.total_pax = (frm.doc.adult || 1) + (frm.doc.child || 0) 
        refresh_field('total_pax');
        updateTourPackagePrice(frm);
    },

    child(frm){
        frm.doc.total_pax = (frm.doc.adult || 1) + (frm.doc.child || 0) 
        refresh_field('total_pax');
        frm.doc.total_tour_package_price =frm.doc.price * (frm.doc.adult || 1) + frm.doc.child_rate * (frm.doc.child || 0);
        refresh_field('total_tour_package_price');
        
    },
    price(frm){
        updateTotalRate(frm);
    },
    child_rate(frm){
        updateTotalRate(frm);
    },
    tour_package(frm){
        updateTourPackagePrice(frm);
    },

});
