using Core.DataAccess.EfEntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IDeviceStockDal:IEntityRepository<DeviceStock>
    {
        Task<List<DeviceStockDto>> GetListDeviceStockDto();
    }
}
