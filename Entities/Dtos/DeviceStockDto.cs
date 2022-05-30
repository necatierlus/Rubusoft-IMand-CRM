using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class DeviceStockDto
    {
        public int Id { get; set; }
        public string IdKod { get; set; }
        public string DeviceIdKod { get; set; }
        public string DeviceName { get; set; }
        public string StockPointIdKod { get; set; }
        public string StockPointName { get; set; }
        public string SerialNumber { get; set; }
        public string Suplier { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsInProduct { get; set; }
    }
}
