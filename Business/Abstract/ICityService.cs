using Core.Utilities.Result;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICityService
    {
        Task<IDataResult<City>> GetById(int cityId);
        Task<IDataResult<List<City>>> GetList();
    }
}
