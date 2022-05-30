using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class District:CommonFeature,IEntity
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int CityId { get; set; }
    }
}
