using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class DeviceListModel
    {
        public List<Device> devices { get; set; }
        public DeviceModel device { get; set; }
    }
}
