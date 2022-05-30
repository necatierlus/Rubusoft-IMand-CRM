using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class FirmAddModel
    {
        //Firm
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

        //FirmAddress
        public string AddressTitle { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public string AddressDescription { get; set; }

        public List<Country> Countries { get; set; }
        public List<City> Cities { get; set; }
        public List<District> Districts { get; set; }

        //Firm manager
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string EMail { get; set; }
        public string FirmIdKod { get; set; }
    }
}
