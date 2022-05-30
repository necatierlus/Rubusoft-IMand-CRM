using Business.Abstract;
using Business.Constants;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class FirmService: IFirmService
    {
        private IFirmDal _firmDal;
        public FirmService(IFirmDal firmDal)
        {
            _firmDal = firmDal;
        }
        public async Task<IDataResult<Firm>> Add(Firm firm)
        {
            firm.IsDelete = false;
            firm.IdKod = HelperMethods.HelperMethods.IdKod();
            Firm addedFirm=await _firmDal.Add(firm);
            return new SuccessDataResult<Firm>(data: addedFirm, message: Messages.FirmAdded);
        }
        public async Task<IDataResult<Firm>> Delete(Firm firm)
        {
            firm.IsDelete = true;
            firm.DeleteTime = DateTime.Now;
            firm.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _firmDal.Delete(firm);
            return new SuccessDataResult<Firm>(message: Messages.FirmDeleted);
        }
        public async Task<IDataResult<Firm>> Update(Firm firm)
        {
            firm.IsDelete = false;
            await _firmDal.Update(firm);
            return new SuccessDataResult<Firm>(message: Messages.FirmUpdated);
        }
        public async Task<IDataResult<Firm>> GetById(int firmId)
        {
            return new SuccessDataResult<Firm>(await _firmDal.Get(x => x.FirmId == firmId && x.IsDelete == false));
        }
        public async Task<IDataResult<Firm>> GetByIdKod(string firmIdKod)
        {
            return new SuccessDataResult<Firm>(await _firmDal.Get(x => x.IdKod == firmIdKod && x.IsDelete == false));
        }
        public async Task<IDataResult<List<Firm>>> GetList()
        {
            var value = await _firmDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<Firm>>(value.ToList());
        }
    }
}
