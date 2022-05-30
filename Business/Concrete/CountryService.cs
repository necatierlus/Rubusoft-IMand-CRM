using Business.Abstract;
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
    public class CountryService: ICountryService
    {
        private ICountryDal _countryDal;
        public CountryService(ICountryDal countryDal)
        {
            _countryDal = countryDal;
        }
        public async Task<IDataResult<Country>> GetById(int countryId)
        {
            return new SuccessDataResult<Country>(await _countryDal.Get(x => x.CountryId == countryId && x.IsDelete == false));
        }
        public async Task<IDataResult<List<Country>>> GetList()
        {
            var value = await _countryDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<Country>>(value.ToList());
        }
    }
}
