using Core.Utilities.Result;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICustomerRequestService
    {
        Task<IDataResult<CustomerRequest>> GetById(int id);
        Task<IDataResult<CustomerRequest>> GetByIdKod(string idKod);
        Task<IDataResult<List<CustomerRequest>>> GetList();
        Task<IDataResult<CustomerRequest>> Add(CustomerRequest customerRequest);
        Task<IDataResult<CustomerRequest>> Delete(CustomerRequest customerRequest);
        Task<IDataResult<CustomerRequest>> Update(CustomerRequest customerRequest);
    }
}
