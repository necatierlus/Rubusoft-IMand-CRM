using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class ProductListModel
    {
        public List<Product> products { get; set; }
        public ProductModel product { get; set; }
    }
}
