using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class StockPoint:CommonFeature,IEntity
    {
        [Key]
        public int Id { get; set; }
        public string IdKod { get; set; }
        public string StockName { get; set; }
    }
}
