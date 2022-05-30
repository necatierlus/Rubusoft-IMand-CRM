using Business.Abstract;
using Business.Constants;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AddressService: IAddressService
    {
        private IAddressDal _addressDal;
        public AddressService(IAddressDal addressDal)
        {
            _addressDal = addressDal;
        }
        public async Task<IDataResult<Address>> Add(Address address)
        {
            address.IsDelete = false;
            address.IdKod = HelperMethods.HelperMethods.IdKod();
            Address addedAddress=await _addressDal.Add(address);
            return new SuccessDataResult<Address>(message: Messages.AddressAdded,data:addedAddress);
        }
        public async Task<IDataResult<Address>> Delete(Address address)
        {
            address.IsDelete = true;
            address.DeleteTime = DateTime.Now;
            address.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _addressDal.Delete(address);
            return new SuccessDataResult<Address>(message: Messages.AddressDeleted);
        }
        public async Task<IDataResult<Address>> Update(Address address)
        {
            address.IsDelete = false;
            await _addressDal.Update(address);
            return new SuccessDataResult<Address>(message: Messages.AddressUpdated);
        }
        public async Task<IDataResult<Address>> GetById(int addressId)
        {
            return new SuccessDataResult<Address>(await _addressDal.Get(x => x.AddressId == addressId && x.IsDelete == false));
        }
        public async Task<IDataResult<Address>> GetByIdKod(string addressIdKod)
        {
            return new SuccessDataResult<Address>(await _addressDal.Get(x => x.IdKod == addressIdKod && x.IsDelete == false));
        }
        public async Task<IDataResult<List<Address>>> GetList()
        {
            var value = await _addressDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<Address>>(value.ToList());
        }

        public async Task<IDataResult<List<Address>>> GetListByFirmIdKod(string firmIdKod)
        {
            var value = await _addressDal.GetList(x =>x.FirmIdKod==firmIdKod&& x.IsDelete == false);
            return new SuccessDataResult<List<Address>>(value.ToList());
        }
        public IDataResult<List<AddressDetailDto>> GetAddressDetailListByFirmIdKod(string firmIdKod)
        {
            var value =  _addressDal.GetAddressDetailListByFirmIdKod(firmIdKod);
            return new SuccessDataResult<List<AddressDetailDto>>(value.ToList());
        }
    }
}
