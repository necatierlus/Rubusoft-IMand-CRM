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
    public class TechSupportDetailService : ITechSupportDetailService
    {
        private ITechSupportDetailDal _techSupportDetailDal;
        public TechSupportDetailService(ITechSupportDetailDal techSupportDetailDal)
        {
            _techSupportDetailDal = techSupportDetailDal;
        }

        public async Task<IDataResult<TechSupportDetail>> Add(TechSupportDetail techSupportDetail)
        {
            techSupportDetail.IsDelete = false;
            techSupportDetail.CreatedDate = DateTime.Now;
            techSupportDetail.IdKod = HelperMethods.HelperMethods.IdKod();
            TechSupportDetail added= await _techSupportDetailDal.Add(techSupportDetail);
            return new SuccessDataResult<TechSupportDetail>(message: Messages.TechSupportDetailAdded, data: added);
        }
        public async Task<IDataResult<TechSupportDetail>> Delete(TechSupportDetail techSupportDetail)
        {
            techSupportDetail.IsDelete = true;
            techSupportDetail.DeleteTime = DateTime.Now;
            techSupportDetail.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _techSupportDetailDal.Delete(techSupportDetail);
            return new SuccessDataResult<TechSupportDetail>(message: Messages.TechSupportDetailDeleted);
        }
        public async Task<IDataResult<TechSupportDetail>> Update(TechSupportDetail techSupportDetail)
        {
            techSupportDetail.IsDelete = false;
            TechSupportDetail updated = await _techSupportDetailDal.Update(techSupportDetail);
            return new SuccessDataResult<TechSupportDetail>(message: Messages.TechSupportDetailUpdated, data: updated);
        }
        public async Task<IDataResult<TechSupportDetail>> GetById(int techSupportDetailId)
        {
            return new SuccessDataResult<TechSupportDetail>(await _techSupportDetailDal.Get(x => x.Id == techSupportDetailId && x.IsDelete == false));
        }
        public async Task<IDataResult<TechSupportDetail>> GetByIdKod(string techSupportDetailIdKod)
        {
            return new SuccessDataResult<TechSupportDetail>(await _techSupportDetailDal.Get(x => x.IdKod == techSupportDetailIdKod && x.IsDelete == false));
        }
        public async Task<IDataResult<List<TechSupportDetail>>> GetList()
        {
            var value = await _techSupportDetailDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<TechSupportDetail>>(value.ToList());
        }
        public async Task<IDataResult<List<TechSupportDetailDto>>> GetListTechSupportDetailDto(string techSupportIdKod)
        {
            var value = await _techSupportDetailDal.GetListTechSupportDetailDto(techSupportIdKod);
            return new SuccessDataResult<List<TechSupportDetailDto>>(value.ToList());
        }
    }
}
