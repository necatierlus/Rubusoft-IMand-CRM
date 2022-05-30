using Core.Utilities.Result;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAppSettingService
    {
        Task<IDataResult<AppSetting>> GetById(int appSettingId);
        Task<IDataResult<AppSetting>> GetByIdKod(string appSettingIdKod);
        Task<IDataResult<List<AppSetting>>> GetList();
        Task<IDataResult<AppSetting>> Add(AppSetting appSetting);
        Task<IDataResult<AppSetting>> Delete(AppSetting appSetting);
        Task<IDataResult<AppSetting>> Update(AppSetting appSetting);
    }
}
