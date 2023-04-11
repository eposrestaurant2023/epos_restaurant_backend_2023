frappe.views.calendar['Hotel Booking'] = {
    field_map: {
        start: 'arrival_date',
        end: 'departure_date',
        id: 'name',
        title: 'hotel_name',
    },
    style_map: {
        Public: 'success',
        Private: 'info'
    },
    order_by: 'arrival_date',
    on_event_move:null
}
 
frappe.views.calendar['Hotel Booking'].on_event_move = null;