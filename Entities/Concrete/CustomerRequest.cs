using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class CustomerRequest : CommonFeature, IEntity
    {
        [Key]
        public int Id { get; set; }
        public string IdKod { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? UsingSpace { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime RequestDate { get; set; }
        public string Description { get; set; }
        public int? RequestStatus { get; set; }
        public bool? IsFirm { get; set; }
        public string FirmIdKod { get; set; }
        public bool? IsReadClarificationText { get; set; }
        public bool? ReciveEmail { get; set; }
    }
}
