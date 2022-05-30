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
    public class BidProductService: IBidProductService
    {
        private IBidProductDal _bidProductDal;
        public BidProductService(IBidProductDal bidProductDal)
        {
            _bidProductDal = bidProductDal;
        }

        public async Task<IDataResult<BidProduct>> Add(BidProduct bidProduct)
        {
            bidProduct.IsDelete = false;
            bidProduct.IdKod = HelperMethods.HelperMethods.IdKod();
            BidProduct addedBidProduct=await _bidProductDal.Add(bidProduct);
            return new SuccessDataResult<BidProduct>(message: Messages.BidProductAdded,data: addedBidProduct);
        }
        public async Task<IDataResult<BidProduct>> Delete(BidProduct bidProduct)
        {
            bidProduct.IsDelete = true;
            bidProduct.DeleteTime = DateTime.Now;
            bidProduct.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _bidProductDal.Delete(bidProduct);
            return new SuccessDataResult<BidProduct>(message: Messages.BidProductDeleted);
        }
        public async Task<IDataResult<BidProduct>> Update(BidProduct bidProduct)
        {
            bidProduct.IsDelete = false;
            await _bidProductDal.Update(bidProduct);
            return new SuccessDataResult<BidProduct>(message: Messages.BidProductUpdated);
        }
        public async Task<IDataResult<BidProduct>> GetById(int bidProductId)
        {
            return new SuccessDataResult<BidProduct>(await _bidProductDal.Get(x => x.BidProductId == bidProductId && x.IsDelete == false));
        }
        public async Task<IDataResult<BidProduct>> GetByIdKod(string bidProductIdKod)
        {
            return new SuccessDataResult<BidProduct>(await _bidProductDal.Get(x => x.IdKod == bidProductIdKod && x.IsDelete == false));
        }   
        public async Task<IDataResult<List<BidProduct>>> GetListByBidIdKod(string bidIdKod)
        {
            return new SuccessDataResult<List<BidProduct>>(await _bidProductDal.GetList(x => x.BidIdKod == bidIdKod && x.IsDelete == false));
        }
        public async Task<IDataResult<List<BidProduct>>> GetList()
        {
            var value = await _bidProductDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<BidProduct>>(value.ToList());
        }
        public async Task<IDataResult<List<BidProductDetailDto>>> GetListBidProductDetail(string bidIdKod)
        {
            var value = await _bidProductDal.GetListBidProductDetail(bidIdKod);
            return new SuccessDataResult<List<BidProductDetailDto>>(value.ToList());
        }
    }
}
