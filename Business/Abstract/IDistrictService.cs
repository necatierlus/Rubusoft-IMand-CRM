using Core.Utilities.Result;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IDistrictService
    {
        Task<IDataResult<District>> GetById(int districtId);
        Task<IDataResult<List<District>>> GetList();
    }
}
