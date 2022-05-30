using Entities.Concrete;
using Entities.Dtos;
using IMandCRM.UI.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class BidCustomerApprovalModel
    {
        public Bid bid { get; set; }
        public Firm firm { get; set; }
        public FirmManager firmManager { get; set; }
        public List<BidProductDetailDto> bidProductDetailList { get; set; }
        public List<Device> devices { get; set; }
        public AppSetting appSetting { get; set; }
        public User createdPersonal { get; set; }
    }
}
