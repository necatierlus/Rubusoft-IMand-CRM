using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class TechSupportDetailModel
    {
        public string IdKod { get; set; }
        [Required]
        public string TechSupportIdKod { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
