using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models.CustomerRequest
{
    public class FirmAddModel
    {
        [Required]
        public string IdKod { get; set; }
        [Required]
        public string FirmName { get; set; }
        [Required]
        public string FirmEmail { get; set; }
        [Required]
        public string ManagerFirstName { get; set; }
        [Required]
        public string ManagerLastName { get; set; }
        [Required]
        public string FirmPhoneNumber { get; set; }
    }
}
