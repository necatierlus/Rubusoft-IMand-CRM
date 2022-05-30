using Core.DataAccess.EfEntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class DeviceStockDal : EntityRepository<DeviceStock, IMandCRMContext>, IDeviceStockDal
    {
        public async Task<List<DeviceStockDto>> GetListDeviceStockDto()
        {
            using (IMandCRMContext ctx = new IMandCRMContext())
            {
                var query = (from deviceStock in ctx.DeviceStocks.Where(x => x.IsDelete == false)
                             join device in ctx.Devices on deviceStock.DeviceIdKod equals device.IdKod
                             join stockPoint in ctx.StockPoints on deviceStock.StockPointIdKod equals stockPoint.IdKod
                             select new DeviceStockDto
                             {
                                 IdKod = deviceStock.IdKod,
                                 DeviceIdKod = deviceStock.DeviceIdKod,
                                 DeviceName= device.DeviceName,
                                 StockPointIdKod = deviceStock.StockPointIdKod,
                                 StockPointName=stockPoint.StockName,
                                 SerialNumber = deviceStock.SerialNumber,
                                 Suplier = deviceStock.Suplier,
                                 Description = deviceStock.Description,
                                 CreatedAt = deviceStock.CreatedAt,
                                 IsInProduct=deviceStock.IsInProduct
                             });
                var list = await query.ToListAsync();
                return list;
            }
        }
    }
}
