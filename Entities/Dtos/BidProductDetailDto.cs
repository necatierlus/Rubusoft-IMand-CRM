using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class BidProductDetailDto
    {
        public string ProductIdKod { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string Standards { get; set; }
        public string Devices { get; set; }
        public string Specification { get; set; }
        public string GeneralFeatures { get; set; }
        public string ProductPhoto { get; set; }
        public int? Count { get; set; }
        public double? UnitPrice { get; set; }
        public double? Discount { get; set; }
        public double? DiscountUnitPrice { get; set; }
        public double? SubTotal { get; set; }
    }
}
