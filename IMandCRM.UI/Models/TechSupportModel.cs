using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class TechSupportModel
    {
        [Required]
        public string FirmIdKod { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int SupportType { get; set; }
        [Required]
        public string Description { get; set; }
        public string Photo { get; set; }
        public bool Status { get; set; }
        public List<Firm> firms { get; set; }
    }
}
