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
    public class ProductStockService: IProductStockService
    {
        private IProductStockDal _productStockDal { get; set; }
        public ProductStockService(IProductStockDal productStockDal)
        {
            _productStockDal = productStockDal;
        }
        public async Task<IDataResult<ProductStock>> Add(ProductStock productStock)
        {
            productStock.IsDelete = false;
            productStock.CreatedAt = DateTime.Now;
            productStock.IsSold =false;
            productStock.IdKod = HelperMethods.HelperMethods.IdKod();
            ProductStock addedProductStock = await _productStockDal.Add(productStock);
            return new SuccessDataResult<ProductStock>(message: Messages.StockPointAdded, data: addedProductStock);
        }
        public async Task<IDataResult<ProductStock>> Delete(ProductStock productStock)
        {
            productStock.IsDelete = true;
            productStock.DeleteTime = DateTime.Now;
            productStock.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _productStockDal.Delete(productStock);
            return new SuccessDataResult<ProductStock>(message: Messages.ProductStockDeleted);
        }
        public async Task<IDataResult<ProductStock>> Update(ProductStock productStock)
        {
            productStock.IsDelete = false;
            await _productStockDal.Update(productStock);
            return new SuccessDataResult<ProductStock>(message: Messages.ProductStockUpdated);
        }
        public async Task<IDataResult<ProductStock>> GetById(int productStockId)
        {
            return new SuccessDataResult<ProductStock>(await _productStockDal.Get(x => x.Id == productStockId && x.IsDelete == false));
        }
        public async Task<IDataResult<ProductStock>> GetByIdKod(string productStockIdKod)
        {
            return new SuccessDataResult<ProductStock>(await _productStockDal.Get(x => x.IdKod == productStockIdKod && x.IsDelete == false));
        }
        public async Task<IDataResult<List<ProductStock>>> GetList()
        {
            var value = await _productStockDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<ProductStock>>(value.ToList());
        }       
        public async Task<IDataResult<List<ProductStockDto>>> GetListProductStockDto()
        {
            var value = await _productStockDal.GetListProductStockDto();
            return new SuccessDataResult<List<ProductStockDto>>(value.ToList());
        }
    }
}
