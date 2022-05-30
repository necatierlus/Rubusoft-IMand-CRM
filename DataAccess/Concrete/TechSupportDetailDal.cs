using Core.DataAccess.EfEntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class TechSupportDetailDal:EntityRepository<TechSupportDetail, IMandCRMContext>, ITechSupportDetailDal
    {
        public async Task<List<TechSupportDetailDto>> GetListTechSupportDetailDto(string techSupportIdKod)
        {
            using (IMandCRMContext ctx = new IMandCRMContext())
            {
                var query = (from techSupportDetail in ctx.TechSupportDetails.Where(x =>x.TechSupportIdKod== techSupportIdKod && x.IsDelete == false)
                             join user in ctx.AspNetUsers on techSupportDetail.UserId equals user.Id
                             select new TechSupportDetailDto
                             {
                                 Id = techSupportDetail.Id,
                                 IdKod = techSupportDetail.IdKod,
                                 TechSupportIdKod = techSupportDetail.TechSupportIdKod,
                                 UserFullName = user.FirstName + " " + user.LastName,
                                 Description = techSupportDetail.Description,
                                 DescriptionShort = techSupportDetail.Description.Length > 50 ? techSupportDetail.Description.Substring(0, 50) + " ..." : techSupportDetail.Description,
                                 CreatedDate =techSupportDetail.CreatedDate
                             });
                var list = await query.ToListAsync();
                return list;
            }
        }
    }
}
