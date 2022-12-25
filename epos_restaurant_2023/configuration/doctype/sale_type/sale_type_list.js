frappe.listview_settings['Sale Type'] = {
    formatters: {
        color(val) {
            return `<div style="background-color:${val}; border-radius: 50%; height: 20px; width: 20px"></div>`;
        }
    }
}