using Core.Utilities.Result;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IFirmManagerService
    {
        Task<IDataResult<FirmManager>> GetById(int firmManagerId);
        Task<IDataResult<FirmManager>> GetByIdKod(string firmManagerIdKod);
        Task<IDataResult<List<FirmManager>>> GetList();
        Task<IDataResult<List<FirmManager>>> GetListByFirmIdKod(string firmIdKod);
        Task<IDataResult<FirmManager>> Add(FirmManager firm);
        Task<IDataResult<FirmManager>> Delete(FirmManager firm);
        Task<IDataResult<FirmManager>> Update(FirmManager firm);
    }
}
