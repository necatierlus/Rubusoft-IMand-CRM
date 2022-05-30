using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class DeviceStockModel
    {
        public string IdKod { get; set; }
        [Required]
        public string DeviceIdKod { get; set; }
        [Required]
        public string StockPointIdKod { get; set; }
        [Required]
        public string SerialNumber { get; set; }
        public string Suplier { get; set; }
        public string Description { get; set; }
    }
}
