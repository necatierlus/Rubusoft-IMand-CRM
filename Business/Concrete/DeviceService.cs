using Business.Abstract;
using Business.Constants;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class DeviceService : IDeviceService
    {
        private IDeviceDal _deviceDal;
        public DeviceService(IDeviceDal deviceDal)
        {
            _deviceDal = deviceDal;
        }
        public async Task<IDataResult<Device>> Add(Device device)
        {
            device.IsDelete = false;
            device.IdKod = HelperMethods.HelperMethods.IdKod();
            Device addedDevice= await _deviceDal.Add(device);
            return new SuccessDataResult<Device>(message: Messages.DeviceAdded,data: addedDevice);
        }
        public async Task<IDataResult<Device>> Delete(Device device)
        {
            device.IsDelete = true;
            device.DeleteTime = DateTime.Now;
            device.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _deviceDal.Delete(device);
            return new SuccessDataResult<Device>(message: Messages.DeviceDeleted);
        }
        public async Task<IDataResult<Device>> Update(Device device)
        {
            device.IsDelete = false;
            await _deviceDal.Update(device);
            return new SuccessDataResult<Device>(message: Messages.DeviceUpdated);
        }
        public async Task<IDataResult<Device>> GetById(int deviceId)
        {
            return new SuccessDataResult<Device>(await _deviceDal.Get(x => x.DeviceId == deviceId&&x.IsDelete==false));
        }       
        public async Task<IDataResult<Device>> GetByIdKod(string deviceIdKod)
        {
            return new SuccessDataResult<Device>(await _deviceDal.Get(x => x.IdKod == deviceIdKod && x.IsDelete==false));
        }
        public async Task<IDataResult<List<Device>>> GetList()
        {
            var value = await _deviceDal.GetList(x=>x.IsDelete==false);
            return new SuccessDataResult<List<Device>>(value.ToList());
        }
    }
}
