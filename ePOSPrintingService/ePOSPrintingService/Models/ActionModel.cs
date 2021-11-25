using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePOSPrintingService.Models
{
    class ActionModel
    {
        public string action { get; set; }
        public string sale_id { get; set; }
        public string receipt_name { get; set; }
        public int copies { get; set; } = 1;

    }
}
