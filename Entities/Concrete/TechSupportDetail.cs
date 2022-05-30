using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class TechSupportDetail:CommonFeature,IEntity
    {
        [Key]
        public int Id { get; set; }
        public string IdKod { get; set; }
        public string TechSupportIdKod { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
