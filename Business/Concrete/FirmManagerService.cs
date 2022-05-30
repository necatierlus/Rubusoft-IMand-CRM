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
    public class FirmManagerService: IFirmManagerService
    {
        private IFirmManagerDal _firmManagerDal;
        public FirmManagerService(IFirmManagerDal firmManagerDal)
        {
            _firmManagerDal = firmManagerDal;
        }
        public async Task<IDataResult<FirmManager>> Add(FirmManager firmManager)
        {
            firmManager.IsDelete = false;
            firmManager.IdKod = HelperMethods.HelperMethods.IdKod();
            FirmManager addedFirmManager=await _firmManagerDal.Add(firmManager);
            return new SuccessDataResult<FirmManager>(message: Messages.FirmManagerAdded,data: addedFirmManager);
        }

        public async Task<IDataResult<FirmManager>> Delete(FirmManager firmManager)
        {
            firmManager.IsDelete = true;
            firmManager.DeleteTime = DateTime.Now;
            firmManager.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _firmManagerDal.Delete(firmManager);
            return new SuccessDataResult<FirmManager>(message: Messages.FirmManagerDeleted);
        }

        public async Task<IDataResult<FirmManager>> Update(FirmManager firmManager)
        {
            firmManager.IsDelete = false;
            await _firmManagerDal.Update(firmManager);
            return new SuccessDataResult<FirmManager>(message: Messages.FirmManagerUpdated);
        }

        public async Task<IDataResult<FirmManager>> GetById(int firmManagerId)
        {
            return new SuccessDataResult<FirmManager>(await _firmManagerDal.Get(x => x.FirmManagerId == firmManagerId && x.IsDelete == false));
        }
        public async Task<IDataResult<FirmManager>> GetByIdKod(string firmManagerIdKod)
        {
            return new SuccessDataResult<FirmManager>(await _firmManagerDal.Get(x => x.IdKod == firmManagerIdKod && x.IsDelete == false));
        }
        public async Task<IDataResult<List<FirmManager>>> GetList()
        {
            var value = await _firmManagerDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<FirmManager>>(value.ToList());
        }  
        public async Task<IDataResult<List<FirmManager>>> GetListByFirmIdKod(string firmIdKod)
        {
            var value = await _firmManagerDal.GetList(x =>x.FirmIdKod==firmIdKod&& x.IsDelete == false);
            return new SuccessDataResult<List<FirmManager>>(value.ToList());
        }
    }
}
