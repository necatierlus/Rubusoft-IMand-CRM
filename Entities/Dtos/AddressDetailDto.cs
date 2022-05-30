using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class AddressDetailDto
    {
        public int AddressId { get; set; }
        public string IdKod { get; set; }
        public string AddressTitle { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string AddressDescription { get; set; }
        public string FirmIdKod { get; set; }
    }
}
