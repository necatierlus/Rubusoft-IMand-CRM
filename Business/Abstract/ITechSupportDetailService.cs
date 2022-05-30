using Core.Utilities.Result;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ITechSupportDetailService
    {
        Task<IDataResult<TechSupportDetail>> GetById(int techSupportDetailId);
        Task<IDataResult<TechSupportDetail>> GetByIdKod(string techSupportIdKod);
        Task<IDataResult<List<TechSupportDetail>>> GetList();
        Task<IDataResult<TechSupportDetail>> Add(TechSupportDetail techSupportDetail);
        Task<IDataResult<TechSupportDetail>> Delete(TechSupportDetail techSupportDetail);
        Task<IDataResult<TechSupportDetail>> Update(TechSupportDetail techSupportDetail);
        Task<IDataResult<List<TechSupportDetailDto>>> GetListTechSupportDetailDto(string techSupportIdKod);
    }
}
