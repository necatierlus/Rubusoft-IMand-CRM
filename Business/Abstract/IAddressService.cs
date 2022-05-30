using Core.Utilities.Result;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAddressService
    {
        Task<IDataResult<Address>> GetById(int addressId);
        Task<IDataResult<Address>> GetByIdKod(string addressIdKod);
        Task<IDataResult<List<Address>>> GetList();
        Task<IDataResult<List<Address>>> GetListByFirmIdKod(string firmIdKod);
        IDataResult<List<AddressDetailDto>> GetAddressDetailListByFirmIdKod(string firmIdKod);
        Task<IDataResult<Address>> Add(Address address);
        Task<IDataResult<Address>> Delete(Address address);
        Task<IDataResult<Address>> Update(Address address);
    }
}
