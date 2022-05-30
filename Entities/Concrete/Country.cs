using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Country:CommonFeature,IEntity
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
