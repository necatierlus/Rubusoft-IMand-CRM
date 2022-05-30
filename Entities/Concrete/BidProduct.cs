using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class BidProduct:CommonFeature,IEntity
    {
        [Key]
        public int BidProductId { get; set; }
        public string IdKod { get; set; }
        public string BidIdKod { get; set; }
        public string ProductIdKod { get; set; }
        public int? Count { get; set; }
        public double? UnitPrice { get; set; }
        public double? Discount { get; set; }
        public double? DiscountUnitPrice { get; set; }
        public double? SubTotal { get; set; }
    }
}
