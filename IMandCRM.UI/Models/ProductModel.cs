using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class ProductModel
    {
        public string ProductName { get; set; }
        public string IdKod { get; set; }
        public string ProductCode { get; set; }
        public string Standards { get; set; }
        public string[] Devices { get; set; }
        public string Specification { get; set; }
        public string GeneralFeatures { get; set; }
        public string ProductPhoto { get; set; }
        public double? UnitPrice { get; set; }
        public double? UnitInStock { get; set; }
        public DateTime ProductionDate { get; set; }
    }
}
