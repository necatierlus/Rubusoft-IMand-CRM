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
    public class ProductStockDal : EntityRepository<ProductStock, IMandCRMContext>, IProductStockDal
    {
        public async Task<List<ProductStockDto>> GetListProductStockDto()
        {
            using (IMandCRMContext ctx = new IMandCRMContext())
            {
                var query = (from productStock in ctx.ProductStocks.Where(x => x.IsDelete == false)
                             join product in ctx.Products on productStock.ProductIdKod equals product.IdKod
                             select new ProductStockDto
                             {
                                 IdKod = productStock.IdKod,
                                 DeviceStockIdKods = productStock.DeviceStockIdKods,
                                 ProductIdKod = productStock.ProductIdKod,
                                 SerialNumber= productStock.SerialNumber,
                                 ProductName = product.ProductName,
                                 Description = productStock.Description,
                                 CreatedAt = productStock.CreatedAt,
                                 IsSold=productStock.IsSold
                             });
                var list = await query.ToListAsync();
                return list;
            }
        }
    }
}
