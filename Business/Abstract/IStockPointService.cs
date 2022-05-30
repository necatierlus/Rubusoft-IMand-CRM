using Core.Utilities.Result;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IStockPointService
    {
        Task<IDataResult<StockPoint>> GetById(int id);
        Task<IDataResult<StockPoint>> GetByIdKod(string idKod);
        Task<IDataResult<List<StockPoint>>> GetList();
        Task<IDataResult<StockPoint>> Add(StockPoint stockPoint);
        Task<IDataResult<StockPoint>> Delete(StockPoint stockPoint);
        Task<IDataResult<StockPoint>> Update(StockPoint stockPoint);
    }
}
