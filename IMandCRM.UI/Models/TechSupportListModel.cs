using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class TechSupportListModel
    {
        public List<TechSupportDto> TechSupportDtos { get; set; }
        public TechSupport  TechSupport { get; set; }
    }
}
