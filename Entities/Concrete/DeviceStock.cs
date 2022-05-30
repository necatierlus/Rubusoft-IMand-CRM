using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class DeviceStock:CommonFeature,IEntity
    {
        [Key]
        public int Id { get; set; }
        public string IdKod { get; set; }
        public string DeviceIdKod { get; set; }
        public string StockPointIdKod { get; set; }
        public string SerialNumber { get; set; }
        public string Suplier { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsInProduct { get; set; }
    }
}
