using Core.Utilities.Result;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBidService
    {
        Task<IDataResult<Bid>> GetById(int bidId);
        Task<IDataResult<Bid>> GetByIdKod(string bidIdKod);
        Task<IDataResult<List<Bid>>> GetList();
        Task<IDataResult<List<BidListDto>>> GetListByBidStatus(int bidStatus);
        Task<IDataResult<List<BidListDto>>> GetListByExpiredBids();
        Task<IDataResult<Bid>> Add(Bid bid);
        Task<IDataResult<Bid>> Delete(Bid bid);
        Task<IDataResult<Bid>> Update(Bid bid);
    }
}
