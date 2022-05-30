using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class AppSettingEditModel
    {
        public string IdKod { get; set; }

        [Required(ErrorMessage = "Firma  bilgisi boş geçilemez.")]
        public string FirmName { get; set; }

        [Required(ErrorMessage = "Host bilgisi boş geçilemez.")]
        public string TradeName { get; set; }
        public string Address { get; set; }

        [Required(ErrorMessage = "Host bilgisi boş geçilemez.")]
        public string EMail { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebAddress { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNumber { get; set; }
        public string Logo { get; set; }

        public string MailSenderHost { get; set; }
        public int? MailSenderPort { get; set; }
        public bool MailSenderEnableSSL { get; set; }
        public string MailSenderUserName { get; set; }
        public string MailSenderPassword { get; set; }

        [Required(ErrorMessage = "Teklif kodu bilgisi boş geçilemez.")]
        public string BidCode { get; set; }
        public bool IsActive { get; set; }
    }
}
