using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class DeviceModel
    {
        public string IdKod { get; set; }
        public string DeviceName { get; set; }
        public string DeviceCode { get; set; }
        public string Standards { get; set; }
        public string Specification { get; set; }
        public string GeneralFeatures { get; set; }
        public string DevicePhoto { get; set; }
        public double? UnitPrice { get; set; }
        public double? UnitInStock { get; set; }
    }
}
