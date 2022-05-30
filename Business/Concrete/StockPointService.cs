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
    public class StockPointService: IStockPointService
    {
        private IStockPointDal _stockPointDal;
        public StockPointService(IStockPointDal stockPointDal)
        {
            _stockPointDal = stockPointDal;
        }
        public async Task<IDataResult<StockPoint>> Add(StockPoint  stockPoint)
        {
            stockPoint.IsDelete = false;
            stockPoint.IdKod = HelperMethods.HelperMethods.IdKod();
            StockPoint addedStockPoint = await _stockPointDal.Add(stockPoint);
            return new SuccessDataResult<StockPoint>(message: Messages.StockPointAdded, data: addedStockPoint);
        }
        public async Task<IDataResult<StockPoint>> Delete(StockPoint stockPoint)
        {
            stockPoint.IsDelete = true;
            stockPoint.DeleteTime = DateTime.Now;
            stockPoint.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _stockPointDal.Delete(stockPoint);
            return new SuccessDataResult<StockPoint>(message: Messages.StockPointDeleted);
        }
        public async Task<IDataResult<StockPoint>> Update(StockPoint stockPoint)
        {
            stockPoint.IsDelete = false;
            await _stockPointDal.Update(stockPoint);
            return new SuccessDataResult<StockPoint>(message: Messages.StockPointUpdated);
        }
        public async Task<IDataResult<StockPoint>> GetById(int stockPointId)
        {
            return new SuccessDataResult<StockPoint>(await _stockPointDal.Get(x => x.Id == stockPointId && x.IsDelete == false));
        }
        public async Task<IDataResult<StockPoint>> GetByIdKod(string stockPointIdKod)
        {
            return new SuccessDataResult<StockPoint>(await _stockPointDal.Get(x => x.IdKod == stockPointIdKod && x.IsDelete == false));
        }
        public async Task<IDataResult<List<StockPoint>>> GetList()
        {
            var value = await _stockPointDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<StockPoint>>(value.ToList());
        }

    }
}
