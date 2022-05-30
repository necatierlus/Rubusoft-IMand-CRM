using Core.Utilities.Result;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IGeneralRequirementService
    {
        Task<IDataResult<GeneralRequirement>> GetById(int generalRequirementId);
        Task<IDataResult<GeneralRequirement>> GetByIdKod(string generalRequirementIdKod);
        Task<IDataResult<List<GeneralRequirement>>> GetList();
        Task<IDataResult<GeneralRequirement>> Add(GeneralRequirement generalRequirement);
        Task<IDataResult<GeneralRequirement>> Delete(GeneralRequirement generalRequirement);
        Task<IDataResult<GeneralRequirement>> Update(GeneralRequirement generalRequirement);
    }
}
