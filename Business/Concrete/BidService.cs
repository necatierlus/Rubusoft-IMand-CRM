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
    public class BidService: IBidService
    {
        private IBidDal _bidDal;
        public BidService(IBidDal bidDal)
        {
            _bidDal = bidDal;
        }

        public async Task<IDataResult<Bid>> Add(Bid bid)
        {
            bid.IsDelete = false;
            bid.IdKod = HelperMethods.HelperMethods.IdKod();
            Bid addedBid=await _bidDal.Add(bid);
            return new SuccessDataResult<Bid>(message: Messages.BidAdded,data:addedBid);
        }
        public async Task<IDataResult<Bid>> Delete(Bid bid)
        {
            bid.IsDelete = true;
            bid.DeleteTime = DateTime.Now;
            bid.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _bidDal.Delete(bid);
            return new SuccessDataResult<Bid>(message: Messages.BidDeleted);
        }
        public async Task<IDataResult<Bid>> Update(Bid bid)
        {
            bid.IsDelete = false;
            Bid updatedBid=await _bidDal.Update(bid);
            return new SuccessDataResult<Bid>(message: Messages.BidUpdated, data: updatedBid);
        }
        public async Task<IDataResult<Bid>> GetById(int bidId)
        {
            return new SuccessDataResult<Bid>(await _bidDal.Get(x => x.BidId == bidId && x.IsDelete == false));
        }
        public async Task<IDataResult<Bid>> GetByIdKod(string bidIdKod)
        {
            return new SuccessDataResult<Bid>(await _bidDal.Get(x => x.IdKod == bidIdKod && x.IsDelete == false));
        }
        public async Task<IDataResult<List<Bid>>> GetList()
        {
            var value = await _bidDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<Bid>>(value.ToList());
        }    
        public async Task<IDataResult<List<BidListDto>>> GetListByBidStatus(int bidStatus)
        {
            var value = await _bidDal.GetListByBidStatus(bidStatus);
            return new SuccessDataResult<List<BidListDto>>(value.ToList());
        }        
        public async Task<IDataResult<List<BidListDto>>> GetListByExpiredBids()
        {
            var value = await _bidDal.GetListByExpiredBids(DateTime.Now);
            return new SuccessDataResult<List<BidListDto>>(value.ToList());
        }

    }
}
