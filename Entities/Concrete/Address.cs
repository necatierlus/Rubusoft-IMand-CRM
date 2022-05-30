using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class Address:CommonFeature,IEntity
    {
        [Key]
        public int AddressId { get; set; }
        public string IdKod { get; set; }
        public string AddressTitle { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public string AddressDescription { get; set; }
        public string FirmIdKod { get; set; }
    }
}
