using Business.Abstract;
using Business.Constants;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class DeviceStockService: IDeviceStockService
    {
        private IDeviceStockDal _deviceStockDal;
        public DeviceStockService(IDeviceStockDal deviceStockDal)
        {
            _deviceStockDal = deviceStockDal;
        }
        public async Task<IDataResult<DeviceStock>> Add(DeviceStock deviceStock)
        {
            deviceStock.IsDelete = false;
            deviceStock.CreatedAt = DateTime.Now;
            deviceStock.IdKod = HelperMethods.HelperMethods.IdKod();
            DeviceStock addedDeviceStock = await _deviceStockDal.Add(deviceStock);
            return new SuccessDataResult<DeviceStock>(message: Messages.StockPointAdded, data: addedDeviceStock);
        }
        public async Task<IDataResult<DeviceStock>> Delete(DeviceStock deviceStock)
        {
            deviceStock.IsDelete = true;
            deviceStock.DeleteTime = DateTime.Now;
            deviceStock.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _deviceStockDal.Delete(deviceStock);
            return new SuccessDataResult<DeviceStock>(message: Messages.DeviceStockDeleted);
        }
        public async Task<IDataResult<DeviceStock>> Update(DeviceStock deviceStock)
        {
            deviceStock.IsDelete = false;
            await _deviceStockDal.Update(deviceStock);
            return new SuccessDataResult<DeviceStock>(message: Messages.DeviceStockUpdated);
        }
        public async Task<IDataResult<DeviceStock>> GetById(int deviceStockId)
        {
            return new SuccessDataResult<DeviceStock>(await _deviceStockDal.Get(x => x.Id == deviceStockId && x.IsDelete == false));
        }
        public async Task<IDataResult<DeviceStock>> GetByIdKod(string deviceStockIdKod)
        {
            return new SuccessDataResult<DeviceStock>(await _deviceStockDal.Get(x => x.IdKod == deviceStockIdKod && x.IsDelete == false));
        }
        public async Task<IDataResult<List<DeviceStock>>> GetList()
        {
            var value = await _deviceStockDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<DeviceStock>>(value.ToList());
        }   
        public async Task<IDataResult<List<DeviceStockDto>>> GetListDeviceStockDto()
        {
            var value = await _deviceStockDal.GetListDeviceStockDto();
            var valList = value.Where(x => x.IsInProduct == false).ToList();
            return new SuccessDataResult<List<DeviceStockDto>>(valList);
        }

    }
}
