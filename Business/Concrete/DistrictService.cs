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
    public class DistrictService: IDistrictService
    {
        private IDistrictDal _districtDal;
        public DistrictService(IDistrictDal districtDal)
        {
            _districtDal = districtDal;
        }
        public async Task<IDataResult<District>> GetById(int districtId)
        {
            return new SuccessDataResult<District>(await _districtDal.Get(x => x.DistrictId == districtId && x.IsDelete == false));
        }
        public async Task<IDataResult<List<District>>> GetList()
        {
            var value = await _districtDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<District>>(value.ToList());
        }
    }
}
