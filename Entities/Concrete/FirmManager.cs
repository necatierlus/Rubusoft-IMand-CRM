using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class FirmManager : CommonFeature, IEntity
    {
        [Key]
        public int FirmManagerId { get; set; }
        public string IdKod { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string EMail { get; set; }
        public string FirmIdKod { get; set; }
    }
}
