using Core.Utilities.Result;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductService
    {
        Task <IDataResult<Product>> GetById(int productId);
        Task <IDataResult<Product>> GetByIdKod(string productIdKod);
        Task<IDataResult<List<Product>>> GetList();
        Task<IDataResult<Product>> Add(Product product);
        Task<IDataResult<Product>> Delete(Product product);
        Task<IDataResult<Product>> Update(Product product);
    }
}
