using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class TechSupportDetailListModel
    {
        public List<TechSupportDetailDto> TechSupportDetailDtos { get; set; }
        public TechSupportDto TechSupportDto { get; set; }
    }
}
