using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class Bid:CommonFeature,IEntity
    {
        [Key]
        public int BidId { get; set; }
        public string IdKod { get; set; }
        public string BidNumber { get; set; }
        public string FirmIdKod { get; set; }
        public string FirmManagerIdKod { get; set; }
        public string CreatedPersonalId { get; set; }
        public DateTime BidDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime CancelledDate { get; set; }
        public DateTime BidValidityDate { get; set; }
        public int? BidPeriodOfValidity { get; set; }
        public int? BidStatus { get; set; }
        public string GeneralRequirements { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalDiscount { get; set; }
        public double? GeneralTotal { get; set; }
        public bool? UnitPriceIsShow { get; set; }
        public bool? GeneralTotalIsShow { get; set; }
        public bool? DiscountIsShow { get; set; }
        public bool? DiscountUnitPriceIsShow { get; set; }

    }
}
