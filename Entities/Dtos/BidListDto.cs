using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class BidListDto
    {
        public int BidId { get; set; }
        public string IdKod { get; set; }
        public string BidNumber { get; set; }
        public string FirmName { get; set; }
        public string FirmManagerFullName { get; set; }
        public string FirmManagerEmail { get; set; }
        public string CreatedPesonalFullName { get; set; }
        public DateTime BidDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime CancelledDate { get; set; }
        public DateTime BidValidityDate { get; set; }
        public int? BidStatus { get; set; }
        public string GeneralRequirements { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalDiscount { get; set; }
        public double? GeneralTotal { get; set; }
    }
}
