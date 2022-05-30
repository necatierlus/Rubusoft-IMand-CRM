using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class FirmAddressModel
    {
        public string IdKod { get; set; }
        public string AddressTitle { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public string AddressDescription { get; set; }
        public string FirmIdKod { get; set; }
    }
}
