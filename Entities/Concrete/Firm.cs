using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class Firm: CommonFeature,IEntity
    {
        [Key]
        public int FirmId { get; set; }
        public string IdKod { get; set; }
        public string FirmName { get; set; }
        public string FirmLogo { get; set; }
        public string TradeName { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNumber { get; set; }
        public string FirmEMail { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string FaxNumber { get; set; }
        public string Gsm1 { get; set; }
        public string Gsm2 { get; set; }
    }
}
