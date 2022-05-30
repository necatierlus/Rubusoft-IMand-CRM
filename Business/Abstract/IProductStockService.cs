using Core.Utilities.Result;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductStockService
    {
        Task<IDataResult<ProductStock>> GetById(int id);
        Task<IDataResult<ProductStock>> GetByIdKod(string idKod);
        Task<IDataResult<List<ProductStock>>> GetList();
        Task<IDataResult<List<ProductStockDto>>> GetListProductStockDto();
        Task<IDataResult<ProductStock>> Add(ProductStock productStock);
        Task<IDataResult<ProductStock>> Delete(ProductStock productStock);
        Task<IDataResult<ProductStock>> Update(ProductStock productStock);
    }
}
