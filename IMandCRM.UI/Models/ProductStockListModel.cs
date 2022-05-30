using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class ProductStockListModel
    {
        public List<ProductStockDto> productStocks { get; set; }
        public List<Product> products { get; set; }
        public List<DeviceStockDto> deviceStocks { get; set; }
    }
}
