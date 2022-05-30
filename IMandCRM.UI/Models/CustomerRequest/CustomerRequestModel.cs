using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models.CustomerRequest
{
    public class CustomerRequestModel
    {
        public int Id { get; set; }
        public string IdKod { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public int UsingSpace { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        public DateTime RequestDate { get; set; }

        [EmailAddress(ErrorMessage = "Mail adresinizi email formatında giriniz.")]
        public string Email { get; set; }
        public string Description { get; set; }
        public int? RequestStatus { get; set; }
        public bool IsFirm { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "You gotta tick the box!")]
        public bool IsReadClarificationText { get; set; }

        public bool ReciveEmail { get; set; }
    }
}
