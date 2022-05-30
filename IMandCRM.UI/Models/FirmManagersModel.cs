using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class FirmManagersModel
    {
        public string firmIdKod { get; set; }
        public List<FirmManager> managers { get; set; }
        public FirmManagerModel manager { get; set; }
    }
}
