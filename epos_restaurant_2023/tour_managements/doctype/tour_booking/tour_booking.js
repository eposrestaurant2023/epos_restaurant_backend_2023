// Copyright (c) 2023, Tes Pheakdey and contributors
// For license information, please see license.txt

// frappe.ui.form.on("Tour Booking", {
// 	refresh(frm) {

// 	},
// });

// Copyright (c) 2022, Tes Pheakdey and contributors
// For license information, please see license.txt

frappe.ui.form.on("Tour Booking", {
    refresh(frm){
     
        set_indicator(frm);

         if(!frm.doc.__islocal){
            var iframe = document.createElement('iframe');
            iframe.height="1122";
            iframe.width="100%";
            iframe.style="border:none"
            iframe.src = '/printview?doctype=Tour%20Packages&name=' + frm.doc.tour_package +  '&format=' + frappe.get_meta("Tour Booking").fields.find(r=>r.fieldname=='tour_package_detail_frame').default + '&no_letterhead=1&settings=%7B%7D&_lang=en';
            document.getElementById('frame').appendChild(iframe);
        }
    },
    setup(frm){
        frm.set_query("document_type", "guides_and_drivers", function(doc, cdt, cdn) {
            return {
                filters: {
                    name: ["in",["Tour Guides","Drivers"]]
                    
                }
            };
        });

       
    },  
    start_date(frm){   
        if (frm.doc.start_date && frm.doc.end_date){
            frm.doc.duration = frappe.datetime.get_diff( frm.doc.end_date, frm.doc.start_date )
            refresh_field('duration');
        }  
    },
    end_date(frm){    
        if (frm.doc.start_date && frm.doc.end_date){
            frm.doc.duration = frappe.datetime.get_diff( frm.doc.end_date, frm.doc.start_date )
            refresh_field('duration');
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

frappe.ui.form.on('Tour Booking Hotels', {
   departure:function (frm,cdt, cdn) {
    let doc = locals[cdt][cdn];
    if (doc.arrival && doc.departure){
        doc.total_night = frappe.datetime.get_diff( doc.departure , doc.arrival )
        refresh_field('hotels');
    }
  },
  arrival:function (frm,cdt, cdn) {
    let doc = locals[cdt][cdn];
    if (doc.arrival && doc.departure){
        doc.total_night = frappe.datetime.get_diff( doc.departure , doc.arrival )
        refresh_field('hotels');
    }
  },
  total_night:function(frm,cdt, cdn){
        let doc = locals[cdt][cdn];
        const from_date = doc.arrival;
        const to_date = frappe.datetime.add_days(from_date, doc.total_night);
        doc.departure = to_date;

        refresh_field('hotels');
    },
})

frappe.ui.form.on('Tour Booking Discount', {

    discounts_remove:function(frm){
        frm.set_value('total_discount', frm.doc.discounts.reduce((n, d) => n + (d.discount_amount || 0),0));
        frm.refresh_field('total_discount')
    },
    discount_on:function (frm,cdt, cdn) {
        calculate_discount(frm,locals[cdt][cdn])
   },
    discount_type:function (frm,cdt, cdn) {
        calculate_discount(frm,locals[cdt][cdn])
   },
   
   discount:function (frm,cdt, cdn) {
        calculate_discount(frm,locals[cdt][cdn])
   },

 
 })
 frappe.ui.form.on('Tour Booking Payments', {

    tour_booking_payment_remove:function(frm){
        frm.set_value('total_paid', frm.doc.tour_booking_payment.reduce((n, d) => n + (d.payment_amount || 0),0));
        frm.refresh_field('total_paid')
    }
 })

 function calculate_discount(frm,doc){
    
    if (doc.discount_on=="Tour Package"){
       
        doc.amount = frm.doc.total_tour_package_price
    }else if(doc.discount_on=="Hotel"){
        doc.amount = frm.doc.total_hotel_amount
    }else if(doc.discount_on=="Restaurant"){
        doc.amount = frm.doc.total_restaurant_amount
    }
    else if(doc.discount_on=="Tour Guide"){
        doc.amount = frm.doc.total_tour_guide_amount
    }else if(doc.discount_on=="Transportation"){
        doc.amount = frm.doc.total_transportation_amount
    }else if(doc.discount_on=="Additional Charge"){
        doc.amount = frm.doc.total_additional_charge
    }else{
        doc.amount = 0
    }
    
    
    
        if (doc.discount_type=="Percent"){
           doc.discount_amount = (doc.amount || 0) * (doc.discount || 0)/100;
        }else {
           doc.discount_amount =doc.discount || 0;
        }
        

        refresh_field('discounts');
        frm.set_value('total_discount', frm.doc.discounts.reduce((n, d) => n + (d.discount_amount || 0),0));
        refresh_field('total_discount');
     
 }

 
function set_indicator(frm){
    if(frm.doc.__islocal)
			return;
  
    if (frm.doc.price > 0 ) frm.dashboard.add_indicator(__("Tour Package Price: {0}",[format_currency(frm.doc.total_tour_package_price)]) ,"blue");

    if (frm.doc.total_hotel_amount > 0 )  frm.dashboard.add_indicator(__("Hotel: {0}",[format_currency(frm.doc.total_hotel_amount)]) ,"blue");
    if (frm.doc.total_restaurant_amount > 0 )  frm.dashboard.add_indicator(__("Restaurant: {0}",[format_currency(frm.doc.total_restaurant_amount)]) ,"blue");
    if (frm.doc.total_tour_guide_amount > 0 )  frm.dashboard.add_indicator(__("Tour Guide: {0}",[format_currency(frm.doc.total_tour_guide_amount)]) ,"blue");
    if (frm.doc.total_transportation_amount > 0 )  frm.dashboard.add_indicator(__("Transportation: {0}",[format_currency(frm.doc.total_transportation_amount)]) ,"blue");

    if (frm.doc.total_additional_charge > 0 )  frm.dashboard.add_indicator(__("Additional Charge: {0}",[format_currency(frm.doc.total_additional_charge)]) ,"blue");

    if (  frm.doc.price > 0 && (frm.doc.total_additional_charge   + frm.doc.total_hotel_amount + frm.doc.total_restaurant_amount + frm.doc.total_transportation_amount + frm.doc.total_tour_guide_amount)>0)  frm.dashboard.add_indicator(__("Total Charge: {0}",[format_currency(frm.doc.total_additional_charge + frm.doc.total_tour_package_price + frm.doc.total_restaurant_amount + frm.doc.total_hotel_amount + frm.doc.total_transportation_amount)]) ,"blue");
    if (frm.doc.total_expense > 0 )  frm.dashboard.add_indicator(__("Additional Expense: {0}",[format_currency(frm.doc.total_expense)]) ,"red");

    if (frm.doc.total_paid > 0 )  frm.dashboard.add_indicator(__("Total Paid: {0}",[format_currency(frm.doc.total_paid)]) ,"green");
    if (frm.doc.balance > 0 )  frm.dashboard.add_indicator(__("Balance: {0}",[format_currency(frm.doc.balance)]) ,"red");
}

function set_query(frm,field_name, filters){ 
	 
    frm.set_query(field_name, function() {
        return {
            filters: filters
        }
    });
 
}

function updateTotalRate(frm){
    frm.doc.total_tour_package_price =frm.doc.price * (frm.doc.adult || 1) + frm.doc.child_rate * (frm.doc.child || 0);
    
    refresh_field('total_tour_package_price');
}

function updateTourPackagePrice(frm){
    frm.call({
        method: 'get_tour_price',
        doc:frm.doc,
        callback:function(r){
            if(r.message){
                 frm.doc.price = r.message;
                 frm.doc.child_rate = r.message;

                 frm.doc.total_tour_package_price =frm.doc.price * (frm.doc.adult || 1) + frm.doc.child_rate * (frm.doc.child || 0);
                 refresh_field('price');
                 refresh_field('child_rate');
                 
                 refresh_field('total_tour_package_price');
                 //set_indicator(frm);
                 //console.log(frm.dashboard)

            }
            
        },
        async: true,
    });
}