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
    public class CustomerRequestService: ICustomerRequestService
    {
        private ICustomerRequestDal _customerRequestDal;
        public CustomerRequestService(ICustomerRequestDal customerRequestDal)
        {
            _customerRequestDal = customerRequestDal;
        }
        public async Task<IDataResult<CustomerRequest>> Add(CustomerRequest customerRequest)
        {
            customerRequest.RequestDate = DateTime.Now;
            customerRequest.IsFirm = false;
            customerRequest.IsDelete = false;
            customerRequest.IdKod = HelperMethods.HelperMethods.IdKod();
            CustomerRequest addedCustomerRequest = await _customerRequestDal.Add(customerRequest);
            return new SuccessDataResult<CustomerRequest>(message: Messages.CustomerRequestAdded, data: addedCustomerRequest);
        }
        public async Task<IDataResult<CustomerRequest>> Delete(CustomerRequest customerRequest)
        {
            customerRequest.IsDelete = true;
            customerRequest.DeleteTime = DateTime.Now;
            customerRequest.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _customerRequestDal.Delete(customerRequest);
            return new SuccessDataResult<CustomerRequest>(message: Messages.CustomerRequestDeleted);
        }
        public async Task<IDataResult<CustomerRequest>> Update(CustomerRequest customerRequest)
        {
            customerRequest.IsDelete = false;
            await _customerRequestDal.Update(customerRequest);
            return new SuccessDataResult<CustomerRequest>(message: Messages.CustomerRequestUpdated);
        }
        public async Task<IDataResult<CustomerRequest>> GetById(int customerRequestId)
        {
            return new SuccessDataResult<CustomerRequest>(await _customerRequestDal.Get(x => x.Id == customerRequestId && x.IsDelete == false));
        }
        public async Task<IDataResult<CustomerRequest>> GetByIdKod(string customerRequestIdKod)
        {
            return new SuccessDataResult<CustomerRequest>(await _customerRequestDal.Get(x => x.IdKod == customerRequestIdKod && x.IsDelete == false));
        }
        public async Task<IDataResult<List<CustomerRequest>>> GetList()
        {
            var value = await _customerRequestDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<CustomerRequest>>(value.ToList());
        }

    }
}
