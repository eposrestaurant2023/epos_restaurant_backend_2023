using eModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eAdmin.Pages.PageCustomers
{
    public class PageAddCustomers:PageCore
    {
        public CustomerModel model { get; set; }
        public async Task Save_Click()
        {
            var res = await http.ApiPost("customer/save",model);
        }
    }
}
