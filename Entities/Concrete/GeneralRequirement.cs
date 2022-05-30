using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class GeneralRequirement : CommonFeature, IEntity
    {
        [Key]
        public int GeneralRequirementId { get; set; }
        public string IdKod { get; set; }
        public string RequirementTitle { get; set; }
        public string RequirementContent { get; set; }
    }
}
