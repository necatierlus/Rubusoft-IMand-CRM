using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class DeviceStockListModel
    {
        public List<DeviceStockDto> deviceStocks { get; set; }
        public List<Device> devices { get; set; }
        public List<StockPoint> stockPoints { get; set; }
    }
}
