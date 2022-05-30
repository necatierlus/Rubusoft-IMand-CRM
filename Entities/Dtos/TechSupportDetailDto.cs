using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class TechSupportDetailDto
    {
        public int Id { get; set; }
        public string IdKod { get; set; }
        public string TechSupportIdKod { get; set; }
        public string Description { get; set; }
        public string DescriptionShort { get; set; }
        public string UserFullName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
