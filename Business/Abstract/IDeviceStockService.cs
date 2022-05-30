using Core.Utilities.Result;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IDeviceStockService
    {
        Task<IDataResult<DeviceStock>> GetById(int id);
        Task<IDataResult<DeviceStock>> GetByIdKod(string idKod);
        Task<IDataResult<List<DeviceStock>>> GetList();
        Task<IDataResult<List<DeviceStockDto>>> GetListDeviceStockDto();
        Task<IDataResult<DeviceStock>> Add(DeviceStock deviceStock);
        Task<IDataResult<DeviceStock>> Delete(DeviceStock deviceStock);
        Task<IDataResult<DeviceStock>> Update(DeviceStock deviceStock);
    }
}
