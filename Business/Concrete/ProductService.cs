using Business.Abstract;
using Business.Constants;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Linq;

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductService : IProductService
    {
        private IProductDal _productDal;
        public ProductService(IProductDal productDal)
        {
            _productDal = productDal;
        }
        public async Task<IDataResult<Product>> Add(Product product)
        {
            product.IsDelete = false;
            product.IdKod = HelperMethods.HelperMethods.IdKod();
            Product addedProduct=await _productDal.Add(product);
            return new SuccessDataResult<Product>(message: Messages.ProductAdded,data: addedProduct);
        }

        public async Task<IDataResult<Product>> Delete(Product product)
        {
            product.IsDelete = true;
            product.DeleteTime = DateTime.Now;
            product.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _productDal.Delete(product);
            return new SuccessDataResult<Product>(message: Messages.ProductDeleted);
        }

        public async Task<IDataResult<Product>> Update(Product product)
        {
            product.IsDelete = false;
            await _productDal.Update(product);
            return new SuccessDataResult<Product>(message: Messages.ProductUpdated);
        }

        public async Task<IDataResult<Product>> GetById(int productId)
        {
            return new SuccessDataResult<Product>(await _productDal.Get(x => x.ProductId == productId));
        }
        public async Task<IDataResult<Product>> GetByIdKod(string productIdKod)
        {
            return new SuccessDataResult<Product>(await _productDal.Get(x => x.IdKod == productIdKod&&x.IsDelete==false));
        }

        public async Task<IDataResult<List<Product>>> GetList()
        {
            var value = await _productDal.GetList(x=>x.IsDelete==false);
            return new SuccessDataResult<List<Product>>(value.ToList());
        }


    }
}
