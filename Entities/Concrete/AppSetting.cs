using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class AppSetting:CommonFeature,IEntity
    {
        [Key]
        public int AppSettingId { get; set; }
        public string IdKod { get; set; }
        public string FirmName { get; set; }
        public string TradeName { get; set; }
        public string Address { get; set; }
        public string EMail { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebAddress { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNumber { get; set; }
        public string Logo { get; set; }
        public string MailSenderHost { get; set; }
        public int MailSenderPort { get; set; }
        public bool MailSenderEnableSSL { get; set; }
        public string MailSenderUserName { get; set; }
        public string MailSenderPassword { get; set; }
        public string BidCode { get; set; }
        public bool? IsActive { get; set; }
    }
}
