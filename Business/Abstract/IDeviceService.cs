using Core.Utilities.Result;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IDeviceService
    {
        Task<IDataResult<Device>> GetById(int deviceId);
        Task<IDataResult<Device>> GetByIdKod(string deviceIdKod);
        Task<IDataResult<List<Device>>> GetList();
        Task<IDataResult<Device>> Add(Device product);
        Task<IDataResult<Device>> Delete(Device product);
        Task<IDataResult<Device>> Update(Device product);
    }
}
