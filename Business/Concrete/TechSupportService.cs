using Business.Abstract;
using Business.Constants;
using Core.Utilities.Result;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class TechSupportService: ITechSupportService
    {
        private ITechSupportDal _techSupportDal;
        public TechSupportService(ITechSupportDal techSupportDal)
        {
            _techSupportDal = techSupportDal;
        }

        public async Task<IDataResult<TechSupport>> Add(TechSupport techSupport)
        {
            techSupport.Status = false;
            techSupport.StartDate = DateTime.Now;
            techSupport.IsDelete = false;
            techSupport.IdKod = HelperMethods.HelperMethods.IdKod();
            TechSupport addedTechSupport = await _techSupportDal.Add(techSupport);
            return new SuccessDataResult<TechSupport>(message: Messages.TechSupportAdded, data: addedTechSupport);
        }
        public async Task<IDataResult<TechSupport>> Delete(TechSupport techSupport)
        {
            techSupport.IsDelete = true;
            techSupport.DeleteTime = DateTime.Now;
            techSupport.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _techSupportDal.Delete(techSupport);
            return new SuccessDataResult<TechSupport>(message: Messages.TechSupportDeleted);
        }
        public async Task<IDataResult<TechSupport>> Update(TechSupport techSupport)
        {
            techSupport.IsDelete = false;
            TechSupport updated = await _techSupportDal.Update(techSupport);
            return new SuccessDataResult<TechSupport>(message: Messages.TechSupportUpdated, data: updated);
        }
        public async Task<IDataResult<TechSupport>> GetById(int techSupportId)
        {
            return new SuccessDataResult<TechSupport>(await _techSupportDal.Get(x => x.Id == techSupportId && x.IsDelete == false));
        }
        public async Task<IDataResult<TechSupport>> GetByIdKod(string techSupportIdKod)
        {
            return new SuccessDataResult<TechSupport>(await _techSupportDal.Get(x => x.IdKod == techSupportIdKod && x.IsDelete == false));
        }
        public async Task<IDataResult<List<TechSupport>>> GetList()
        {
            var value = await _techSupportDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<TechSupport>>(value.ToList());
        }       
        public async Task<IDataResult<List<TechSupportDto>>> GetListTechSupportDto()
        {
            var value = await _techSupportDal.GetListTechSupportDto();
            return new SuccessDataResult<List<TechSupportDto>>(value.ToList());
        }        
        public async Task<IDataResult<TechSupportDto>> GetTechSupportDtoByIdKod(string techSupportIdKod)
        {
            return new SuccessDataResult<TechSupportDto>(await _techSupportDal.GetTechSupportDtoByIdKod(techSupportIdKod));
        }
      
    }
}
