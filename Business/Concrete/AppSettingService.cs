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
    public class AppSettingService: IAppSettingService
    {
        private IAppSettingDal _appSettingDal;
        public AppSettingService(IAppSettingDal appSettingDal)
        {
            _appSettingDal = appSettingDal;
        }

        public async Task<IDataResult<AppSetting>> Add(AppSetting appSetting)
        {
            appSetting.IsDelete = false;
            appSetting.IdKod = HelperMethods.HelperMethods.IdKod();
            AppSetting addedAppSettingawait=await _appSettingDal.Add(appSetting);
            return new SuccessDataResult<AppSetting>(message: Messages.BidProductAdded,data: addedAppSettingawait);
        }
        public async Task<IDataResult<AppSetting>> Delete(AppSetting appSetting)
        {
            appSetting.IsDelete = true;
            appSetting.DeleteTime = DateTime.Now;
            appSetting.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _appSettingDal.Delete(appSetting);
            return new SuccessDataResult<AppSetting>(message: Messages.AppSettingDeleted);
        }
        public async Task<IDataResult<AppSetting>> Update(AppSetting appSetting)
        {
            appSetting.IsDelete = false;
            await _appSettingDal.Update(appSetting);
            return new SuccessDataResult<AppSetting>(message: Messages.AppSettingUpdated);
        }
        public async Task<IDataResult<AppSetting>> GetById(int appSettingId)
        {
            return new SuccessDataResult<AppSetting>(await _appSettingDal.Get(x => x.AppSettingId == appSettingId && x.IsDelete == false));
        }
        public async Task<IDataResult<AppSetting>> GetByIdKod(string appSettingIdKod)
        {
            return new SuccessDataResult<AppSetting>(await _appSettingDal.Get(x => x.IdKod == appSettingIdKod && x.IsDelete == false));
        }
        public async Task<IDataResult<List<AppSetting>>> GetList()
        {
            var value = await _appSettingDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<AppSetting>>(value.ToList());
        }
    }
}
