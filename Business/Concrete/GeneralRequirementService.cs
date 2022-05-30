using Business.Abstract;
using Business.Constants;
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
    public class GeneralRequirementService: IGeneralRequirementService
    {
        private IGeneralRequirementDal _generalRequirementDal;
        public GeneralRequirementService(IGeneralRequirementDal generalRequirementDal)
        {
            _generalRequirementDal = generalRequirementDal;
        }

        public async Task<IDataResult<GeneralRequirement>> Add(GeneralRequirement generalRaquirement)
        {
            generalRaquirement.IsDelete = false;
            generalRaquirement.IdKod = HelperMethods.HelperMethods.IdKod();
            GeneralRequirement addedGeneralRequirement = await _generalRequirementDal.Add(generalRaquirement);
            return new SuccessDataResult<GeneralRequirement>(message: Messages.GeneralRequirementAdded, data: addedGeneralRequirement);
        }
        public async Task<IDataResult<GeneralRequirement>> Delete(GeneralRequirement generalRaquirement)
        {
            generalRaquirement.IsDelete = true;
            generalRaquirement.DeleteTime = DateTime.Now;
            generalRaquirement.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _generalRequirementDal.Delete(generalRaquirement);
            return new SuccessDataResult<GeneralRequirement>(message: Messages.GeneralRequirementDeleted);
        }
        public async Task<IDataResult<GeneralRequirement>> Update(GeneralRequirement generalRaquirement)
        {
            generalRaquirement.IsDelete = false;
            GeneralRequirement updatedGeneralRaquirement = await _generalRequirementDal.Update(generalRaquirement);
            return new SuccessDataResult<GeneralRequirement>(message: Messages.GeneralRequirementUpdated, data: updatedGeneralRaquirement);
        }
        public async Task<IDataResult<GeneralRequirement>> GetById(int generalRaquirementId)
        {
            return new SuccessDataResult<GeneralRequirement>(await _generalRequirementDal.Get(x => x.GeneralRequirementId == generalRaquirementId && x.IsDelete == false));
        }
        public async Task<IDataResult<GeneralRequirement>> GetByIdKod(string generalRaquirementIdKod)
        {
            return new SuccessDataResult<GeneralRequirement>(await _generalRequirementDal.Get(x => x.IdKod == generalRaquirementIdKod && x.IsDelete == false));
        }
        public async Task<IDataResult<List<GeneralRequirement>>> GetList()
        {
            var value = await _generalRequirementDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<GeneralRequirement>>(value.ToList());
        }
    }
}
