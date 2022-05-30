using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class StockPointModel
    {
        public string IdKod { get; set; }

        [Required]
        public string StockName { get; set; }
    }
}
