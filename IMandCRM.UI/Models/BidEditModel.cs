using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class BidEditModel
    {
        public List<Firm> firms { get; set; }
        public List<Product> products { get; set; }
        public List<FirmManager> firmManagers { get; set; }
        public List<BidProduct> bidProducts { get; set; }
        public List<GeneralRequirement> requirements { get; set; }

        public string IdKod { get; set; }
        public string FirmIdKod { get; set; }
        public string FirmManagerIdKod { get; set; }
        public string CreatedPersonalId { get; set; }
        public DateTime BidDate { get; set; }
        public int BidPeriodOfValidity { get; set; }
        public string BidStatusStr { get; set; }
        public string GeneralRequirements { get; set; }

        public string StrTotalPrice { get; set; }
        public string StrTotalDiscount { get; set; }
        public string StrGeneralTotal { get; set; }

        public bool UnitPriceIsShow { get; set; }
        public bool GeneralTotalIsShow { get; set; }
        public bool DiscountIsShow { get; set; }
        public bool DiscountUnitPriceIsShow { get; set; }

        public string[] ProductIdKods { get; set; }
        public string[] Counts { get; set; }
        public string[] UnitPrices { get; set; }
        public string[] Discounts { get; set; }
        public string[] DiscountUnitPrices { get; set; }
        public string[] SubTotals { get; set; }
    }
}
