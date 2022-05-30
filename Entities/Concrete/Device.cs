using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class Device: CommonFeature,IEntity
    {
        [Key]
        public int DeviceId { get; set; }
        public string IdKod { get; set; }
        public string DeviceName { get; set; }
        public string DeviceCode { get; set; }
        public string Standards { get; set; }
        public string Specification { get; set; }
        public string GeneralFeatures { get; set; }
        public string DevicePhoto { get; set; }
        public double? UnitPrice { get; set; }
        public double? UnitInStock { get; set; }
    }
}
