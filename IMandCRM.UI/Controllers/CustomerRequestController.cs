using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using Entities.Concrete;
using IMandCRM.UI.Constants;
using IMandCRM.UI.EmailServices;
using IMandCRM.UI.Messages;
using IMandCRM.UI.Models.CustomerRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Controllers
{

    public class CustomerRequestController : Controller
    {
        private readonly ICustomerRequestService _customerRequestService;
        private readonly IFirmService _firmService;
        private readonly IFirmManagerService _firmManagerService;
        private readonly IProductService _productService;
        private readonly IBidService _bidService;
        private readonly IBidProductService _bidProductService;
        private readonly IGeneralRequirementService _generalRequirementService;

        private readonly IMapper _mapper;
        private IEmailSender _emailSender;
        public CustomerRequestController(ICustomerRequestService customerRequestService, IMapper mapper, IEmailSender emailSender, IFirmService firmService, IFirmManagerService firmManagerService, IProductService productService, IBidService bidService, IBidProductService bidProductService, IGeneralRequirementService generalRequirementService)
        {
            _customerRequestService = customerRequestService;
            _firmService = firmService;
            _firmManagerService = firmManagerService;
            _productService = productService;
            _bidService = bidService;
            _bidProductService = bidProductService;
            _generalRequirementService = generalRequirementService;
            _mapper = mapper;
            _emailSender = emailSender;
        }
        public IActionResult CreateCustomerRequest()
        {
            return View();
        } 
        

        [HttpPost]
        public async Task<IActionResult> CreateCustomerRequest(CustomerRequestModel customerRequestModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "Zorunlu alanları eksiksiz doldurunuz.|warning";
                return View(customerRequestModel);
            }
            customerRequestModel.ReciveEmail = true;
            CustomerRequest customerRequest = _mapper.Map<CustomerRequestModel, CustomerRequest>(customerRequestModel);
            customerRequest.RequestStatus = (int)Enums.RequestStatus.Application;
            IDataResult<CustomerRequest> addedBid = await _customerRequestService.Add(customerRequest);
            if (addedBid.Success)
            {
                TempData["message"] = "Talebiniz başarıyla iletildi.|success";
            }
            else
            {
                TempData["message"] = "Talebiniz gönderilirken bir hata meydana geldi.|error";
            }

            return RedirectToAction("CreateCustomerRequest");
        }

        [Authorize]
        public IActionResult AddRequest()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRequest(CustomerRequestModel customerRequestModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "Zorunlu alanları eksiksiz doldurunuz.|warning";
                return View(customerRequestModel);
            }
            customerRequestModel.ReciveEmail = true;
            CustomerRequest customerRequest = _mapper.Map<CustomerRequestModel, CustomerRequest>(customerRequestModel);
            customerRequest.RequestStatus = (int)Enums.RequestStatus.Application;
            IDataResult<CustomerRequest> addedBid = await _customerRequestService.Add(customerRequest);
            if (addedBid.Success)
            {
                TempData["message"] = "Talebiniz başarıyla iletildi.|success";
            }
            else
            {
                TempData["message"] = "Talebiniz gönderilirken bir hata meydana geldi.|error";
            }

            return RedirectToAction("ApplicationRequests");
        }

        [Authorize]
        public async Task<IActionResult> ApplicationRequests()
        {
            var customerRequests = await _customerRequestService.GetList();
            var requestList = customerRequests.Data.Where(x=>x.RequestStatus==(int)Enums.RequestStatus.Application).OrderByDescending(x => x.RequestDate).ToList();

            List<CustomerRequestModel> customerRequestModels = _mapper.Map<List<CustomerRequest>, List<CustomerRequestModel>>(requestList);

            return View(customerRequestModels);
        }

        [Authorize]
        public async Task<IActionResult> CallRequests()
        {
            var customerRequests = await _customerRequestService.GetList();
            var requestList = customerRequests.Data.Where(x => x.RequestStatus == (int)Enums.RequestStatus.Call).OrderByDescending(x => x.RequestDate).ToList();

            List<CustomerRequestModel> customerRequestModels = _mapper.Map<List<CustomerRequest>, List<CustomerRequestModel>>(requestList);

            return View(customerRequestModels);
        }

        [Authorize]
        public async Task<IActionResult> BidRequests()
        {
            var customerRequests = await _customerRequestService.GetList();
            var requestList = customerRequests.Data.Where(x => x.RequestStatus == (int)Enums.RequestStatus.Bid).OrderByDescending(x => x.RequestDate).ToList();

            List<CustomerRequestModel> customerRequestModels = _mapper.Map<List<CustomerRequest>, List<CustomerRequestModel>>(requestList);

            return View(customerRequestModels);
        }

        [Authorize]
        public async Task<IActionResult> SalesRequests()
        {
            var customerRequests = await _customerRequestService.GetList();
            var requestList = customerRequests.Data.Where(x => x.RequestStatus == (int)Enums.RequestStatus.Sales).OrderByDescending(x => x.RequestDate).ToList();

            List<CustomerRequestModel> customerRequestModels = _mapper.Map<List<CustomerRequest>, List<CustomerRequestModel>>(requestList);

            return View(customerRequestModels);
        }

        [Authorize]
        public async Task<IActionResult> CancelRequests()
        {
            var customerRequests = await _customerRequestService.GetList();
            var requestList = customerRequests.Data.Where(x => x.RequestStatus == (int)Enums.RequestStatus.Cancel).OrderByDescending(x => x.RequestDate).ToList();

            List<CustomerRequestModel> customerRequestModels = _mapper.Map<List<CustomerRequest>, List<CustomerRequestModel>>(requestList);

            return View(customerRequestModels);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> CustomerRequestsDelete(string[] DeleteCustomerRequests)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                foreach (var idKod in DeleteCustomerRequests)
                {
                    var product = await _customerRequestService.GetByIdKod(idKod);
                    if (product.Data == null)
                    {
                        alertMessage.ResponseStatus = false;
                        alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                        alertMessage.MessageType = "error";
                        return Json(alertMessage);
                    }
                    await _customerRequestService.Delete(product.Data);
                }


                alertMessage.ResponseStatus = true;
                alertMessage.MessageText = "Kayıtlar başarıyla silindi.";
                alertMessage.MessageType = "success";

                return Json(alertMessage);
            }
            catch (Exception)
            {
                AlertMessage alertMessage = new AlertMessage();
                alertMessage.ResponseStatus = false;
                alertMessage.MessageText = "Kayıt silinirken hata oluştu.";
                alertMessage.MessageType = "error";
                return Json(alertMessage);

            }

        }

        [Authorize]
        public async Task<IActionResult> DescriptionAdd(string IdKod, string Description)
        {
            var customerRequest = _customerRequestService.GetByIdKod(IdKod);
            CustomerRequest updateCustomerRequest = customerRequest.Result.Data;
            updateCustomerRequest.Description = Description;

            var updated = await _customerRequestService.Update(updateCustomerRequest);

            return RedirectToAction("ApplicationRequests");
        }

        [Authorize]
        public async Task<JsonResult> GetRequestDescription(string IdKod)
        {
            IDataResult<CustomerRequest> customerRequest = await _customerRequestService.GetByIdKod(IdKod);
            CustomerRequest updateCustomerRequest = customerRequest.Data;

            return Json(updateCustomerRequest);
        }



        [Authorize]
        [HttpPost]
        public async Task<JsonResult> ChangeRequestStatus([FromBody] RequestChangeModel requestChangeModel)
        {
            var customerRequest = _customerRequestService.GetByIdKod(requestChangeModel.IdKod);
            CustomerRequest updateCustomerRequest = customerRequest.Result.Data;
            updateCustomerRequest.RequestStatus = Convert.ToInt32(requestChangeModel.RequestStatus);

            var updated = await _customerRequestService.Update(updateCustomerRequest);

            AlertMessage alertMessage = new AlertMessage();
            if (updated.Success)
            {
                alertMessage.ResponseStatus = true;
                alertMessage.MessageText = "Başarıyla güncellendi.";
                alertMessage.MessageType = "success";
                return Json(alertMessage);
            }
            else
            {

                alertMessage.ResponseStatus = false;
                alertMessage.MessageText = "İstek durumu değiştirilirken hata oluştu.";
                alertMessage.MessageType = "error";
                return Json(alertMessage);
            }
        }

        [Authorize]
        public JsonResult GetCustomer(string idKod)
        {
            var customerRequest = _customerRequestService.GetByIdKod(idKod);
            CustomerRequest updateCustomerRequest = customerRequest.Result.Data;


            return Json(updateCustomerRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> FirmAdd(FirmAddModel firmAddModel)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                if (!ModelState.IsValid)
                {
                    alertMessage.ResponseStatus = false;
                    alertMessage.MessageText = "Firma eklenirken hata oluştu.";
                    alertMessage.MessageType = "error";
                    return Json(alertMessage);
                }

                var customerRequest = _customerRequestService.GetByIdKod(firmAddModel.IdKod);
                CustomerRequest updateCustomerRequest = customerRequest.Result.Data;

                Firm firm = new Firm();
                firm.TradeName = firmAddModel.FirmName;
                firm.FirmName = firmAddModel.FirmName;
                firm.FirmEMail = firmAddModel.FirmEmail;
                firm.Gsm1 = firmAddModel.FirmPhoneNumber;
                IDataResult<Firm> addedFirm = await _firmService.Add(firm);

                FirmManager firmManager = new FirmManager();
                firmManager.FirstName = firmAddModel.ManagerFirstName;
                firmManager.LastName = firmAddModel.ManagerLastName;
                firmManager.EMail = firmAddModel.FirmEmail;
                firmManager.Title = "Yönetici";
                firmManager.FirmIdKod = addedFirm.Data.IdKod;

                IDataResult<FirmManager> addedFirmManaer = await _firmManagerService.Add(firmManager);
                updateCustomerRequest.FirmIdKod = addedFirm.Data.IdKod;
                updateCustomerRequest.IsFirm = true;
                var updated = await _customerRequestService.Update(updateCustomerRequest);

                alertMessage.ResponseStatus = true;
                alertMessage.MessageText = "Firma başarıyla eklendi.";
                alertMessage.MessageType = "success";
                return Json(alertMessage);
            }
            catch (Exception)
            {
                AlertMessage alertMessage = new AlertMessage();
                alertMessage.ResponseStatus = false;
                alertMessage.MessageText = "Firma eklenirken hata oluştu.";
                alertMessage.MessageType = "error";
                return Json(alertMessage);
            }

        }

        [Authorize]
        public async Task<IActionResult> CreateBid(string idKod)
        {
            var customerRequest = _customerRequestService.GetByIdKod(idKod);
            CustomerRequest customerRequestResult = customerRequest.Result.Data;

            IDataResult<Firm> firmResult = await _firmService.GetByIdKod(customerRequestResult.FirmIdKod);
            IDataResult<List<Product>> productListResult = await _productService.GetList();
            IDataResult<List<FirmManager>> firmManagersResult = await _firmManagerService.GetListByFirmIdKod(firmResult.Data.IdKod);
            FirmManager firmManager = firmManagersResult.Data.FirstOrDefault();
            IDataResult<List<GeneralRequirement>> generalRequirementListResult = await _generalRequirementService.GetList();
            CreateBidModel createBidModel = new CreateBidModel();
            createBidModel.FirmName = firmResult.Data.FirmName;
            createBidModel.FirmIdKod = firmResult.Data.IdKod;
            createBidModel.products = productListResult.Data;
            createBidModel.FirmManagerFullName = firmManager.FirstName + " " + firmManager.LastName;
            createBidModel.FirmManagerIdKod = firmManager.IdKod;
            createBidModel.generalRequirements = generalRequirementListResult.Data;
            return View(createBidModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBid(CreateBidModel createBidModel)
        {
            Bid bid = _mapper.Map<CreateBidModel, Bid>(createBidModel);
            if (createBidModel.BidStatusStr == "Draft")
            {
                bid.BidStatus = (int)Enums.BidStatus.Draft;
            }
            else
            {
                bid.BidStatus = (int)Enums.BidStatus.PendingInternalApproval;
            }
            bid.BidValidityDate = createBidModel.BidDate.AddDays(createBidModel.BidPeriodOfValidity);
            bid.TotalPrice = Convert.ToDouble(createBidModel.StrTotalPrice.Replace(",", "#").Replace(".", ",").Replace("#", "."));
            bid.TotalDiscount = Convert.ToDouble(createBidModel.StrTotalDiscount.Replace(",", "#").Replace(".", ",").Replace("#", "."));
            bid.GeneralTotal = Convert.ToDouble(createBidModel.StrGeneralTotal.Replace(",", "#").Replace(".", ",").Replace("#", "."));
            IDataResult<Bid> addedBid = await _bidService.Add(bid);
            for (int i = 0; i < createBidModel.ProductIdKods.Length; i++)
            {
                if (createBidModel.ProductIdKods[i] != null)
                {
                    BidProduct bidProduct = new BidProduct();
                    bidProduct.BidIdKod = addedBid.Data.IdKod;
                    bidProduct.ProductIdKod = createBidModel.ProductIdKods[i];
                    bidProduct.Count = Convert.ToInt32(createBidModel.Counts[i]);
                    bidProduct.UnitPrice = Convert.ToDouble(createBidModel.UnitPrices[i] == null ? "0.00" : createBidModel.UnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                    bidProduct.Discount = Convert.ToDouble(createBidModel.Discounts[i]);
                    bidProduct.DiscountUnitPrice = Convert.ToDouble(createBidModel.DiscountUnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                    bidProduct.SubTotal = Convert.ToDouble(createBidModel.SubTotals[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                    await _bidProductService.Add(bidProduct);

                }
            }
            if (createBidModel.BidStatusStr == "Draft")
            {
                return RedirectToAction("DraftBids", "Bid");
            }
            else
            {
                return RedirectToAction("PendingInternalApprovalBids", "Bid");
            }

        }
    }
}
