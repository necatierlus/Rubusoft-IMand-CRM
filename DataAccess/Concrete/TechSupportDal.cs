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
    public class TechSupportDal : EntityRepository<TechSupport, IMandCRMContext>, ITechSupportDal
    {
        public async Task<List<TechSupportDto>> GetListTechSupportDto()
        {
            using (IMandCRMContext ctx = new IMandCRMContext())
            {
                var query = (from techSupport in ctx.TechSupports.Where(x => x.IsDelete == false)
                             join firm in ctx.Firms on techSupport.FirmIdKod equals firm.IdKod
                             join user in ctx.AspNetUsers on techSupport.UserId equals user.Id
                             select new TechSupportDto
                             {
                                 Id = techSupport.Id,
                                 IdKod = techSupport.IdKod,
                                 FirmName = firm.FirmName,
                                 CreatedPersonalName = user.FirstName + " " + user.LastName,
                                 SupportTypeName = techSupport.SupportType == 1 ? "Elektrik" : techSupport.SupportType == 2 ? "İnternet" : techSupport.SupportType == 3 ? "Ekran" : "Diğer",
                                 Description = techSupport.Description,
                                 DescriptionShort = techSupport.Description.Length > 50 ? techSupport.Description.Substring(0, 50) + " ..." : techSupport.Description,
                                 Photo = techSupport.Photo,
                                 Status = techSupport.Status,
                                 StartDate = techSupport.StartDate,
                                 EndDate = techSupport.EndDate
                             });
                var list = await query.ToListAsync();
                return list;
            }
        }

        public async Task<TechSupportDto> GetTechSupportDtoByIdKod(string techSupportIdKod)
        {
            using (IMandCRMContext ctx = new IMandCRMContext())
            {
                var query = (from techSupport in ctx.TechSupports.Where(x => x.IdKod == techSupportIdKod && x.IsDelete == false)
                             join firm in ctx.Firms on techSupport.FirmIdKod equals firm.IdKod
                             join user in ctx.AspNetUsers on techSupport.UserId equals user.Id
                             select new TechSupportDto
                             {
                                 Id = techSupport.Id,
                                 IdKod = techSupport.IdKod,
                                 FirmName = firm.FirmName,
                                 CreatedPersonalName = user.FirstName + " " + user.LastName,
                                 SupportTypeName = techSupport.SupportType == 1 ? "Elektrik" : techSupport.SupportType == 2 ? "İnternet" : techSupport.SupportType == 3 ? "Ekran" : "Diğer",
                                 Description = techSupport.Description,
                                 DescriptionShort = techSupport.Description.Length > 50 ? techSupport.Description.Substring(0, 50) + " ..." : techSupport.Description,
                                 Photo = techSupport.Photo,
                                 Status = techSupport.Status,
                                 StartDate = techSupport.StartDate,
                                 EndDate = techSupport.EndDate
                             });
                var techSupportDto = await query.FirstOrDefaultAsync();
                return techSupportDto;
            }
        }
    }
}
