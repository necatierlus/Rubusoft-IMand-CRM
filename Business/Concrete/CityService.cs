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
    public class CityService: ICityService
    {
        private ICityDal _cityDal;
        public CityService(ICityDal cityDal)
        {
            _cityDal = cityDal;
        }
        public async Task<IDataResult<City>> GetById(int cityId)
        {
            return new SuccessDataResult<City>(await _cityDal.Get(x => x.CityId == cityId && x.IsDelete == false));
        }
        public async Task<IDataResult<List<City>>> GetList()
        {
            var value = await _cityDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<City>>(value.ToList());
        }
    }
}
