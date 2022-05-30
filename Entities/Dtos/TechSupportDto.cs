using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class TechSupportDto
    {
        public int Id { get; set; }
        public string IdKod { get; set; }
        public string FirmName { get; set; }
        public string CreatedPersonalName { get; set; }
        public string SupportTypeName { get; set; }
        public string Description { get; set; }
        public string DescriptionShort { get; set; }
        public string Photo { get; set; }
        public bool Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
