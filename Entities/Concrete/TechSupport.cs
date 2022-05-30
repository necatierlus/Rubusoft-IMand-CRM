using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class TechSupport: CommonFeature,IEntity
    {
        [Key]
        public int Id { get; set; }
        public string IdKod { get; set; }
        public string FirmIdKod { get; set; }
        public string UserId { get; set; }
        public int SupportType { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public bool Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
