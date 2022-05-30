using Core.Utilities.Result;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBidProductService
    {
        Task<IDataResult<BidProduct>> GetById(int bidIdProduct);
        Task<IDataResult<BidProduct>> GetByIdKod(string bidProductIdKod);
        Task<IDataResult<List<BidProduct>>> GetListByBidIdKod(string bidIdKod);
        Task<IDataResult<List<BidProduct>>> GetList();
        Task<IDataResult<List<BidProductDetailDto>>> GetListBidProductDetail(string bidIdKod);
        Task<IDataResult<BidProduct>> Add(BidProduct bidProduct);
        Task<IDataResult<BidProduct>> Delete(BidProduct bidProduct);
        Task<IDataResult<BidProduct>> Update(BidProduct bidProduct);
    }
}
