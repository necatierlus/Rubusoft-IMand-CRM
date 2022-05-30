using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class ProductStockModel
    {
        public string[] DeviceStockIdKod { get; set; }
        public string ProductIdKod { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
    }
}
