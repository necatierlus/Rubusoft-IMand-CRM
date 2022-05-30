using Core.DataAccess.EfEntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class AddressDal : EntityRepository<Address, IMandCRMContext>, IAddressDal
    {
        public List<AddressDetailDto> GetAddressDetailListByFirmIdKod(string firmIdKod)
        {
            using (IMandCRMContext ctx =new IMandCRMContext())
            {
                var list =(from address in ctx.Addresses.Where(x=>x.FirmIdKod==firmIdKod&&x.IsDelete==false)
                           join country in ctx.Countries on address.CountryId equals country.CountryId
                           join city in ctx.Cities on address.CityId equals city.CityId
                           join district in ctx.Districts on address.DistrictId equals district.DistrictId
                           select new AddressDetailDto
                           {
                               AddressId = address.AddressId,
                               IdKod = address.IdKod,
                               AddressTitle = address.AddressTitle,
                               CountryId = country.CountryId,
                               CountryName = country.CountryName,
                               CityId = city.CityId,
                               CityName = city.CityName,
                               DistrictId = district.DistrictId,
                               DistrictName = district.DistrictName,
                               AddressDescription = address.AddressDescription,
                               FirmIdKod = address.FirmIdKod
                           }).ToList();
                return list;
            }
        }
    }
}
