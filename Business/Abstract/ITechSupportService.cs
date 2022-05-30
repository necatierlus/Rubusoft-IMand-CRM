using Core.Utilities.Result;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ITechSupportService
    {
        Task<IDataResult<TechSupport>> GetById(int techSupportId);
        Task<IDataResult<TechSupport>> GetByIdKod(string techSupportIdKod);
        Task<IDataResult<List<TechSupport>>> GetList();
        Task<IDataResult<TechSupport>> Add(TechSupport techSupport);
        Task<IDataResult<TechSupport>> Delete(TechSupport techSupport);
        Task<IDataResult<TechSupport>> Update(TechSupport techSupport);
        Task<IDataResult<List<TechSupportDto>>> GetListTechSupportDto();
        Task<IDataResult<TechSupportDto>> GetTechSupportDtoByIdKod(string techSupportIdKod);
    }
}
