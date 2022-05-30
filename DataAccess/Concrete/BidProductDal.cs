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
    public class BidProductDal:EntityRepository<BidProduct,IMandCRMContext>, IBidProductDal
    {
        public async Task<List<BidProductDetailDto>> GetListBidProductDetail(string bidIdKod)
        {
            using (IMandCRMContext ctx = new IMandCRMContext())
            {
                var query = (from bidProduct in ctx.BidProducts.Where(x => x.BidIdKod == bidIdKod && x.IsDelete == false)
                             join product in ctx.Products on bidProduct.ProductIdKod equals product.IdKod
                             select new BidProductDetailDto
                             {
                                 ProductIdKod=product.IdKod,
                                 ProductName=product.ProductName,
                                 ProductCode=product.ProductCode,
                                 Standards=product.Standards,
                                 Devices=product.Devices,
                                 Specification=product.Specification,
                                 GeneralFeatures=product.GeneralFeatures,
                                 ProductPhoto=product.ProductPhoto,
                                 Count=bidProduct.Count,
                                 UnitPrice=bidProduct.UnitPrice,
                                 Discount=bidProduct.Discount,
                                 DiscountUnitPrice=bidProduct.DiscountUnitPrice,
                                 SubTotal=bidProduct.SubTotal
                             });
                var list = await query.ToListAsync();
                return list;
            }
        }
    }
}
