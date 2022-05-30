using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class GeneralRequirementListModel
    {
        public List<GeneralRequirement> generalRequirements { get; set; }
        public GeneralRequirement generalRequirement { get; set; }
        
    }
}
