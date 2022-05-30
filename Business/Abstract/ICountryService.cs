using Core.Utilities.Result;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICountryService
    {
        Task<IDataResult<Country>> GetById(int countryId);
        Task<IDataResult<List<Country>>> GetList();

    }
}
