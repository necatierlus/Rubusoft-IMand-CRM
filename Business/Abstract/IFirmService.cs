using Core.Utilities.Result;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IFirmService
    {
        Task<IDataResult<Firm>> GetById(int firmId);
        Task<IDataResult<Firm>> GetByIdKod(string firmIdKod);
        Task<IDataResult<List<Firm>>> GetList();
        Task<IDataResult<Firm>> Add(Firm firm);
        Task<IDataResult<Firm>> Delete(Firm firm);
        Task<IDataResult<Firm>> Update(Firm firm);
    }
}
