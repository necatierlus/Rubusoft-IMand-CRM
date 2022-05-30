using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class FirmAddressesModel
    {
        public string firmIdKod { get; set; }
        public List<AddressDetailDto> addresses { get; set; }
        public FirmAddressModel address { get; set; }
        public List<Country> Countries { get; set; }
        public List<City> Cities { get; set; }
        public List<District> Districts { get; set; }
    }
}
