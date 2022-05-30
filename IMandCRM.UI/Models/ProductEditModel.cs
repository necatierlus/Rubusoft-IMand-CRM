using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class ProductEditModel
    {
        public ProductModel product { get; set; }
        public string[] productDevices { get; set; }
        public List<Device> devices { get; set; }
    }
}
