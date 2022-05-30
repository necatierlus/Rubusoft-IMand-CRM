using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class ProductStockDto
    {
        public string IdKod { get; set; }
        public string DeviceStockIdKods { get; set; }
        public string ProductIdKod { get; set; }
        public string SerialNumber { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public bool IsSold { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
