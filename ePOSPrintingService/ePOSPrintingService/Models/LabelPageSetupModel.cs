using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePOSPrintingService.Models
{
    public class LabelPageSetupModel
    {
        public decimal PageWidth { get; set; }
        public decimal PageHeight { get; set; }
        public decimal MarginTop { get; set; }
        public decimal MarginRight { get; set; }
        public decimal MarginBottom { get; set; }
        public decimal MarginLeft { get; set; }
    }
}
