using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using Entities.Concrete;
using Entities.Dtos;
using IMandCRM.UI.Constants;
using IMandCRM.UI.EmailServices;
using IMandCRM.UI.HelperMethods;
using IMandCRM.UI.Identity;
using IMandCRM.UI.Messages;
using IMandCRM.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Controllers
{
    public class BidController : Controller
    {
        private IFirmService _firmService;
        private IProductService _productService;
        private IFirmManagerService _firmManagerService;
        private IBidService _bidService;
        private IBidProductService _bidProductService;
        private IGeneralRequirementService _generalRequirementService;
        private IAppSettingService _appSettingService;
        private IDeviceService _deviceService;
        private UserManager<User> _userManager;
        private IEmailSender _emailSender;
        private IMapper _mapper;
        public BidController(IFirmService firmService, IProductService productService, IFirmManagerService firmManagerService, IBidService bidService, IBidProductService bidProductService, IGeneralRequirementService generalRequirementService, IAppSettingService appSettingService, IDeviceService deviceService, UserManager<User> userManager, IEmailSender emailSender, IMapper mapper)
        {
            _firmService = firmService;
            _productService = productService;
            _firmManagerService = firmManagerService;
            _bidService = bidService;
            _bidProductService = bidProductService;
            _generalRequirementService = generalRequirementService;
            _appSettingService = appSettingService;
            _deviceService = deviceService;
            _userManager = userManager;
            _emailSender = emailSender;
            _mapper = mapper;
        }
        [Authorize]
        public async Task<IActionResult> BidAdd()
        {
            IDataResult<List<Firm>> firmListResult = await _firmService.GetList();
            IDataResult<List<Product>> productListResult = await _productService.GetList();
            IDataResult<List<FirmManager>> firmManagerListResult = await _firmManagerService.GetList();
            IDataResult<List<GeneralRequirement>> generalRequirementListResult = await _generalRequirementService.GetList();
            BidAddModel bidAddModel = new BidAddModel();
            bidAddModel.firms = firmListResult.Data;
            bidAddModel.products = productListResult.Data;
            bidAddModel.firmManagers = firmManagerListResult.Data;
            bidAddModel.generalRequirements = generalRequirementListResult.Data;
            return View(bidAddModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> BidAdd(BidAddModel bidAddModel)
        {
            Bid bid = _mapper.Map<BidAddModel, Bid>(bidAddModel);
            if (bidAddModel.BidStatusStr == "Draft")
            {
                bid.BidStatus = (int)Enums.BidStatus.Draft;
            }
            else
            {
                bid.BidStatus = (int)Enums.BidStatus.PendingInternalApproval;
            }
            bid.BidValidityDate = bidAddModel.BidDate.AddDays(bidAddModel.BidPeriodOfValidity);
            bid.TotalPrice = Convert.ToDouble(bidAddModel.StrTotalPrice.Replace(",", "#").Replace(".", ",").Replace("#", "."));
            bid.TotalDiscount = Convert.ToDouble(bidAddModel.StrTotalDiscount.Replace(",", "#").Replace(".", ",").Replace("#", "."));
            bid.GeneralTotal = Convert.ToDouble(bidAddModel.StrGeneralTotal.Replace(",", "#").Replace(".", ",").Replace("#", "."));
            IDataResult<Bid> addedBid = await _bidService.Add(bid);
            for (int i = 0; i < bidAddModel.ProductIdKods.Length; i++)
            {
                if (bidAddModel.ProductIdKods[i] != null)
                {
                    BidProduct bidProduct = new BidProduct();
                    bidProduct.BidIdKod = addedBid.Data.IdKod;
                    bidProduct.ProductIdKod = bidAddModel.ProductIdKods[i];
                    bidProduct.Count = Convert.ToInt32(bidAddModel.Counts[i]);
                    bidProduct.UnitPrice = Convert.ToDouble(bidAddModel.UnitPrices[i] == null ? "0.00" : bidAddModel.UnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                    bidProduct.Discount = Convert.ToDouble(bidAddModel.Discounts[i]);
                    bidProduct.DiscountUnitPrice = Convert.ToDouble(bidAddModel.DiscountUnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                    bidProduct.SubTotal = Convert.ToDouble(bidAddModel.SubTotals[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                    await _bidProductService.Add(bidProduct);

                }
            }
            if (bidAddModel.BidStatusStr == "Draft")
            {
                return RedirectToAction("DraftBids", "Bid");
            }
            else
            {
                return RedirectToAction("PendingInternalApprovalBids", "Bid");
            }

        }

        [Authorize]
        public async Task<IActionResult> DraftBids()
        {
            //Taslak teklif

            IDataResult<List<BidListDto>> bidListResult = await _bidService.GetListByBidStatus((int)Enums.BidStatus.Draft);
            List<BidListDto> bids = bidListResult.Data;
            BidListModel bidListModel = new BidListModel();
            bidListModel.Bids = bids;

            return View(bidListModel);
        }

        [Authorize(Roles = "Admin,Admin Plus")]
        public async Task<IActionResult> PendingInternalApprovalBids()
        {
            //İç onay bekleyen teklifler

            IDataResult<List<BidListDto>> bidListResult = await _bidService.GetListByBidStatus((int)Enums.BidStatus.PendingInternalApproval);
            List<BidListDto> bids = bidListResult.Data;
            BidListModel bidListModel = new BidListModel();
            bidListModel.Bids = bids;

            return View(bidListModel);
        }

        [Authorize(Roles = "Admin,Admin Plus")]
        public async Task<IActionResult> InternallyApprovedBids()
        {
            //İç onaylı teklifler

            IDataResult<List<BidListDto>> bidListResult = await _bidService.GetListByBidStatus((int)Enums.BidStatus.InternallyApproved);
            List<BidListDto> bids = bidListResult.Data;
            BidListModel bidListModel = new BidListModel();
            bidListModel.Bids = bids;

            return View(bidListModel);
        }

        [Authorize]
        public async Task<IActionResult> SentBids()
        {
            //Mail gönderilen teklifler

            IDataResult<List<BidListDto>> bidListResult = await _bidService.GetListByBidStatus((int)Enums.BidStatus.Sent);
            List<BidListDto> bids = bidListResult.Data.Where(x => x.BidValidityDate >= DateTime.Now).ToList();
            BidListModel bidListModel = new BidListModel();
            bidListModel.Bids = bids;

            return View(bidListModel);
        }

        [Authorize]
        public async Task<IActionResult> CustomerApprovedBids()
        {
            //Müşteri onaylı teklifler

            IDataResult<List<BidListDto>> bidListResult = await _bidService.GetListByBidStatus((int)Enums.BidStatus.CustomerApproved);
            List<BidListDto> bids = bidListResult.Data;
            BidListModel bidListModel = new BidListModel();
            bidListModel.Bids = bids;

            return View(bidListModel);
        }

        [Authorize]
        public async Task<IActionResult> ExpiredBids()
        {
            //Süresi geçen teklifler

            IDataResult<List<BidListDto>> bidListResult = await _bidService.GetListByExpiredBids();
            List<BidListDto> bids = bidListResult.Data.Where(x => x.BidValidityDate < DateTime.Now && x.BidStatus == (int)Enums.BidStatus.Sent).ToList();
            BidListModel bidListModel = new BidListModel();
            bidListModel.Bids = bids;

            return View(bidListModel);
        }

        [Authorize]
        public async Task<IActionResult> DraftBidEdit(string bidIdKod)
        {
            IDataResult<List<Firm>> firmListResult = await _firmService.GetList();
            IDataResult<List<Product>> productListResult = await _productService.GetList();
            IDataResult<List<FirmManager>> firmManagerListResult = await _firmManagerService.GetList();
            IDataResult<List<BidProduct>> bidProductListResult = await _bidProductService.GetListByBidIdKod(bidIdKod);
            IDataResult<List<GeneralRequirement>> generalRequirementListResult = await _generalRequirementService.GetList();


            IDataResult<Bid> bidResult = await _bidService.GetByIdKod(bidIdKod);
            BidEditModel bidEditModel = _mapper.Map<Bid, BidEditModel>(bidResult.Data);
            bidEditModel.firms = firmListResult.Data;
            bidEditModel.products = productListResult.Data;
            bidEditModel.firmManagers = firmManagerListResult.Data;
            bidEditModel.bidProducts = bidProductListResult.Data;
            bidEditModel.requirements = generalRequirementListResult.Data;

            bidEditModel.StrGeneralTotal = bidResult.Data.GeneralTotal?.ToString("N2").Replace(".", "#").Replace(",", ".").Replace("#", ",");
            bidEditModel.StrTotalPrice = bidResult.Data.TotalPrice?.ToString("N2").Replace(".", "#").Replace(",", ".").Replace("#", ",");
            bidEditModel.StrTotalDiscount = bidResult.Data.TotalDiscount?.ToString("N2").Replace(".", "#").Replace(",", ".").Replace("#", ",");

            return View(bidEditModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DraftBidEdit(BidEditModel bidEditModel)
        {
            try
            {
                Bid bid = _bidService.GetByIdKod(bidEditModel.IdKod).Result.Data;
                List<BidProduct> bidProducts = _bidProductService.GetList().Result.Data.Where(x => x.BidIdKod == bidEditModel.IdKod).ToList();

                Bid bidUpdate = _mapper.Map<BidEditModel, Bid>(bidEditModel);

                bidUpdate.BidId = bid.BidId;
                if (bidEditModel.BidStatusStr == "Draft")
                {
                    bidUpdate.BidStatus = (int)Enums.BidStatus.Draft;
                }
                else
                {
                    bidUpdate.BidStatus = (int)Enums.BidStatus.PendingInternalApproval;
                }
                bidUpdate.BidValidityDate = bidEditModel.BidDate.AddDays(bidEditModel.BidPeriodOfValidity);

                bidUpdate.TotalPrice = Convert.ToDouble(bidEditModel.StrTotalPrice.Replace(",", "#").Replace(".", ",").Replace("#", "."));
                bidUpdate.TotalDiscount = Convert.ToDouble(bidEditModel.StrTotalDiscount.Replace(",", "#").Replace(".", ",").Replace("#", "."));
                bidUpdate.GeneralTotal = Convert.ToDouble(bidEditModel.StrGeneralTotal.Replace(",", "#").Replace(".", ",").Replace("#", "."));

                IDataResult<Bid> updatedBid = await _bidService.Update(bidUpdate);

                for (int i = 0; i < bidEditModel.ProductIdKods.Length; i++)
                {
                    if (bidEditModel.ProductIdKods[i] != null)
                    {
                        BidProduct editBidProduct = bidProducts.Where(x => x.ProductIdKod == bidEditModel.ProductIdKods[i]).FirstOrDefault();
                        if (editBidProduct == null)
                        {
                            //Yeni eklenen ürünler
                            BidProduct bidProduct = new BidProduct();
                            bidProduct.BidIdKod = updatedBid.Data.IdKod;
                            bidProduct.ProductIdKod = bidEditModel.ProductIdKods[i];
                            bidProduct.Count = Convert.ToInt32(bidEditModel.Counts[i]);
                            bidProduct.UnitPrice = Convert.ToDouble(bidEditModel.UnitPrices[i] == null ? "0.00" : bidEditModel.UnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            bidProduct.Discount = Convert.ToDouble(bidEditModel.Discounts[i]);
                            bidProduct.DiscountUnitPrice = Convert.ToDouble(bidEditModel.DiscountUnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            bidProduct.SubTotal = Convert.ToDouble(bidEditModel.SubTotals[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            await _bidProductService.Add(bidProduct);
                        }
                        else
                        {
                            //Güncellenmiş ürünler
                            editBidProduct.BidIdKod = updatedBid.Data.IdKod;
                            editBidProduct.ProductIdKod = bidEditModel.ProductIdKods[i];
                            editBidProduct.Count = Convert.ToInt32(bidEditModel.Counts[i]);
                            editBidProduct.UnitPrice = Convert.ToDouble(bidEditModel.UnitPrices[i] == null ? "0.00" : bidEditModel.UnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            editBidProduct.Discount = Convert.ToDouble(bidEditModel.Discounts[i]);
                            editBidProduct.DiscountUnitPrice = Convert.ToDouble(bidEditModel.DiscountUnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            editBidProduct.SubTotal = Convert.ToDouble(bidEditModel.SubTotals[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            await _bidProductService.Update(editBidProduct);

                        }
                    }
                }


                //Silinen ürünler
                List<BidProduct> updateBidProducts = _bidProductService.GetList().Result.Data.Where(x => x.BidIdKod == bidEditModel.IdKod).ToList();
                foreach (var item in updateBidProducts)
                {
                    bool isDeleted = bidEditModel.ProductIdKods.Any(x => x == item.ProductIdKod);
                    if (!isDeleted)
                    {
                        await _bidProductService.Delete(item);
                    }
                }
                if (bidEditModel.BidStatusStr == "Draft")
                {
                    return RedirectToAction("DraftBids", "Bid");
                }
                else
                {
                    return RedirectToAction("PendingInternalApprovalBids", "Bid");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Hata oluştu.|error";
                return RedirectToAction("DraftBids", "Bid");
            }

        }

        [Authorize]
        public async Task<IActionResult> PendingInternalApprovalEdit(string bidIdKod)
        {
            IDataResult<List<Firm>> firmListResult = await _firmService.GetList();
            IDataResult<List<Product>> productListResult = await _productService.GetList();
            IDataResult<List<FirmManager>> firmManagerListResult = await _firmManagerService.GetList();
            IDataResult<List<BidProduct>> bidProductListResult = await _bidProductService.GetListByBidIdKod(bidIdKod);
            IDataResult<List<GeneralRequirement>> generalRequirementListResult = await _generalRequirementService.GetList();

            IDataResult<Bid> bidResult = await _bidService.GetByIdKod(bidIdKod);
            BidEditModel bidEditModel = _mapper.Map<Bid, BidEditModel>(bidResult.Data);
            bidEditModel.firms = firmListResult.Data;
            bidEditModel.products = productListResult.Data;
            bidEditModel.firmManagers = firmManagerListResult.Data;
            bidEditModel.bidProducts = bidProductListResult.Data;
            bidEditModel.requirements = generalRequirementListResult.Data;

            bidEditModel.StrGeneralTotal = bidResult.Data.GeneralTotal?.ToString("N2").Replace(".", "#").Replace(",", ".").Replace("#", ",");
            bidEditModel.StrTotalPrice = bidResult.Data.TotalPrice?.ToString("N2").Replace(".", "#").Replace(",", ".").Replace("#", ",");
            bidEditModel.StrTotalDiscount = bidResult.Data.TotalDiscount?.ToString("N2").Replace(".", "#").Replace(",", ".").Replace("#", ",");

            return View(bidEditModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PendingInternalApprovalEdit(BidEditModel bidEditModel)
        {
            try
            {
                Bid bid = _bidService.GetByIdKod(bidEditModel.IdKod).Result.Data;

                List<BidProduct> bidProducts = _bidProductService.GetList().Result.Data.Where(x => x.BidIdKod == bidEditModel.IdKod).ToList();
                IDataResult<Firm> firmResult = await _firmService.GetByIdKod(bid.FirmIdKod);
                IDataResult<FirmManager> firmManagerResult = await _firmManagerService.GetByIdKod(bid.FirmManagerIdKod);
                Firm firm = firmResult.Data;
                FirmManager firmManager = firmManagerResult.Data;
                IDataResult<List<AppSetting>> appSettingListResult = await _appSettingService.GetList();
                var resultAppSetting = appSettingListResult.Data.FirstOrDefault();

                Bid bidUpdate = _mapper.Map<BidEditModel, Bid>(bidEditModel);

                bidUpdate.BidId = bid.BidId;
                if (bidEditModel.BidStatusStr == "PendingInternalApproval")
                {
                    bidUpdate.BidStatus = (int)Enums.BidStatus.PendingInternalApproval;
                }
                else if (bidEditModel.BidStatusStr == "InternallyApproved")
                {
                    CreateBidNumber createBidNumber = new CreateBidNumber(_appSettingService, _bidService);
                    string bidNumber = await createBidNumber.GetBidNumber();
                    if (bidNumber == "")
                    {
                        TempData["message"] = "Ayarlar sekmesinden teklif kodunu girmeden teklif onaylayamazsınız!!!|warning";
                        return RedirectToAction("PendingInternalApprovalBids", "Bid");
                    }
                    bidUpdate.BidNumber = bidNumber;
                    bidUpdate.BidStatus = (int)Enums.BidStatus.InternallyApproved;
                }
                else
                {
                    if (firm == null)
                    {
                        TempData["message"] = "Ayarlar sekmesindeki bilgiler eksik olduğundan teklif gönderilemedi.|warning";
                        return RedirectToAction("InternallyApprovedBids", "Bid");
                    }
                    if (firmManager == null)
                    {
                        TempData["message"] = "Firma yönetici bilgisi hatalı olduğundan teklif gönderilemedi.|error";
                        return RedirectToAction("InternallyApprovedBids", "Bid");
                    }


                    if (resultAppSetting == null)
                    {
                        TempData["message"] = "Ayarlar sekmesinden firma bilgileri girmeden mail gönderemezsiniz.|warning";
                        return RedirectToAction("PendingInternalApprovalBids", "Bid");
                    }

                    CreateBidNumber createBidNumber = new CreateBidNumber(_appSettingService, _bidService);
                    string bidNumber = await createBidNumber.GetBidNumber();
                    if (bidNumber == "")
                    {
                        TempData["message"] = "Ayarlar sekmesinden teklif kodunu girmeden teklif onaylayamazsınız!!!|warning";
                        return View(bidEditModel);
                    }
                    bidUpdate.BidNumber = bidNumber;
                    bidUpdate.BidStatus = (int)Enums.BidStatus.Sent;
                }

                bidUpdate.BidValidityDate = bidEditModel.BidDate.AddDays(bidEditModel.BidPeriodOfValidity);

                bidUpdate.TotalPrice = Convert.ToDouble(bidEditModel.StrTotalPrice.Replace(",", "#").Replace(".", ",").Replace("#", "."));
                bidUpdate.TotalDiscount = Convert.ToDouble(bidEditModel.StrTotalDiscount.Replace(",", "#").Replace(".", ",").Replace("#", "."));
                bidUpdate.GeneralTotal = Convert.ToDouble(bidEditModel.StrGeneralTotal.Replace(",", "#").Replace(".", ",").Replace("#", "."));

                IDataResult<Bid> updatedBid = await _bidService.Update(bidUpdate);

                for (int i = 0; i < bidEditModel.ProductIdKods.Length; i++)
                {
                    if (bidEditModel.ProductIdKods[i] != null)
                    {
                        BidProduct editBidProduct = bidProducts.Where(x => x.ProductIdKod == bidEditModel.ProductIdKods[i]).FirstOrDefault();
                        if (editBidProduct == null)
                        {
                            //Yeni eklenen ürünler
                            BidProduct bidProduct = new BidProduct();
                            bidProduct.BidIdKod = updatedBid.Data.IdKod;
                            bidProduct.ProductIdKod = bidEditModel.ProductIdKods[i];
                            bidProduct.Count = Convert.ToInt32(bidEditModel.Counts[i]);
                            bidProduct.UnitPrice = Convert.ToDouble(bidEditModel.UnitPrices[i] == null ? "0.00" : bidEditModel.UnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            bidProduct.Discount = Convert.ToDouble(bidEditModel.Discounts[i]);
                            bidProduct.DiscountUnitPrice = Convert.ToDouble(bidEditModel.DiscountUnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            bidProduct.SubTotal = Convert.ToDouble(bidEditModel.SubTotals[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            await _bidProductService.Add(bidProduct);
                        }
                        else
                        {
                            //Güncellenmiş ürünler
                            editBidProduct.BidIdKod = updatedBid.Data.IdKod;
                            editBidProduct.ProductIdKod = bidEditModel.ProductIdKods[i];
                            editBidProduct.Count = Convert.ToInt32(bidEditModel.Counts[i]);
                            editBidProduct.UnitPrice = Convert.ToDouble(bidEditModel.UnitPrices[i] == null ? "0.00" : bidEditModel.UnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            editBidProduct.Discount = Convert.ToDouble(bidEditModel.Discounts[i]);
                            editBidProduct.DiscountUnitPrice = Convert.ToDouble(bidEditModel.DiscountUnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            editBidProduct.SubTotal = Convert.ToDouble(bidEditModel.SubTotals[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            await _bidProductService.Update(editBidProduct);

                        }
                    }
                }


                //Silinen ürünler
                List<BidProduct> updateBidProducts = _bidProductService.GetList().Result.Data.Where(x => x.BidIdKod == bidEditModel.IdKod).ToList();
                foreach (var item in updateBidProducts)
                {
                    bool isDeleted = bidEditModel.ProductIdKods.Any(x => x == item.ProductIdKod);
                    if (!isDeleted)
                    {
                        await _bidProductService.Delete(item);
                    }
                }
                if (bidEditModel.BidStatusStr == "PendingInternalApproval")
                {
                    return RedirectToAction("PendingInternalApprovalBids", "Bid");
                }
                else if (bidEditModel.BidStatusStr == "InternallyApproved")
                {

                    return RedirectToAction("InternallyApprovedBids", "Bid");
                }
                else
                {
                    //Teklifi mail gönderme
                    AppSetting appSetting = appSettingListResult.Data.FirstOrDefault();
                    string footer = $"Adres:{appSetting.Address} Tel:+90 {appSetting.PhoneNumber} E-mail:{appSetting.EMail}";
                    string url = "/Bid/BidPdf?bidIdKod=" + bid.IdKod;

                    bool result = CreatePdf.SelectCreatePdf(updatedBid.Data.BidNumber, url, footer);
                    string attachmentFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\assets\\media\\bid\\pdf\\" + updatedBid.Data.BidNumber + ".pdf");

                    string subject = $"Ürün Teklifi Hk.";
                    string htmlMessage = $"Sayın: {firmManagerResult.Data.FirstName} {firmManagerResult.Data.LastName}<br/><br/>Firmamızdan teklif isteyerek göstermiş olduğunuz ilgi ve güven için teşekkür ederiz. Aşağıdaki bağlantıyı tıklayarak teklifimizi inceleyebilirsiniz.<br/><br/> Teklifi incelemek için <a href='{Constants.Constants.Url}/Bid/BidCustomerApproval?bidIdKod={bid.IdKod}'><b>tıklayınız.<b></a><br/><br/>Ürün detayları için <a href='{Constants.Constants.Url}/Bid/BidCustomerApprovalProductDetail?bidIdKod={bid.IdKod}'><b>tıklayınız.</b></a><br/><br/>Uygun görmeniz durumunda TEKLİFİ ONAYLA butonuna basınız.<br/><br/><br/>Saygılarımızla,<br/>İyi Çalışmalar.<br/><br/><br/><img style='width:200px;' src='{Constants.Constants.Url}/assets/media/appsettings/{resultAppSetting.Logo}'/><br/><b>{resultAppSetting.FirmName}</b><br/>{resultAppSetting.Address}<br/><b>T:</b>+90 {resultAppSetting.PhoneNumber}";
                    await _emailSender.SendEmailAttachmentAsync(firmManager.EMail, subject, htmlMessage, attachmentFile, appSetting.EMail,resultAppSetting.FirmName);

                    TempData["message"] = "Teklif başarıyla gönderilmiştir.|success";

                    return RedirectToAction("SentBids", "Bid");
                }
            }
            catch (Exception)
            {
                TempData["message"] = "Hata oluştu.|error";
                return RedirectToAction("PendingInternalApprovalBids", "Bid");
            }


        }

        [Authorize]
        public async Task<IActionResult> InternallyApprovedEdit(string bidIdKod)
        {
            IDataResult<List<Firm>> firmListResult = await _firmService.GetList();
            IDataResult<List<Product>> productListResult = await _productService.GetList();
            IDataResult<List<FirmManager>> firmManagerListResult = await _firmManagerService.GetList();
            IDataResult<List<BidProduct>> bidProductListResult = await _bidProductService.GetListByBidIdKod(bidIdKod);
            IDataResult<List<GeneralRequirement>> generalRequirementListResult = await _generalRequirementService.GetList();

            IDataResult<Bid> bidResult = await _bidService.GetByIdKod(bidIdKod);
            BidEditModel bidEditModel = _mapper.Map<Bid, BidEditModel>(bidResult.Data);
            bidEditModel.firms = firmListResult.Data;
            bidEditModel.products = productListResult.Data;
            bidEditModel.firmManagers = firmManagerListResult.Data;
            bidEditModel.bidProducts = bidProductListResult.Data;
            bidEditModel.requirements = generalRequirementListResult.Data;

            bidEditModel.StrGeneralTotal = bidResult.Data.GeneralTotal?.ToString("N2").Replace(".", "#").Replace(",", ".").Replace("#", ",");
            bidEditModel.StrTotalPrice = bidResult.Data.TotalPrice?.ToString("N2").Replace(".", "#").Replace(",", ".").Replace("#", ",");
            bidEditModel.StrTotalDiscount = bidResult.Data.TotalDiscount?.ToString("N2").Replace(".", "#").Replace(",", ".").Replace("#", ",");

            return View(bidEditModel);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> InternallyApprovedEdit(BidEditModel bidEditModel)
        {
            try
            {

                Bid bid = _bidService.GetByIdKod(bidEditModel.IdKod).Result.Data;
                List<BidProduct> bidProducts = _bidProductService.GetList().Result.Data.Where(x => x.BidIdKod == bidEditModel.IdKod).ToList();

                IDataResult<Firm> firmResult = await _firmService.GetByIdKod(bid.FirmIdKod);
                IDataResult<FirmManager> firmManagerResult = await _firmManagerService.GetByIdKod(bid.FirmManagerIdKod);
                Firm firm = firmResult.Data;
                FirmManager firmManager = firmManagerResult.Data;
                if (firm == null)
                {
                    TempData["message"] = "Firma bilgisi hatalı olduğundan teklif gönderilemedi.|error";
                    return RedirectToAction("InternallyApprovedBids", "Bid");
                }
                if (firmManager == null)
                {
                    TempData["message"] = "Firma yönetici bilgisi hatalı olduğundan teklif gönderilemedi.|error";
                    return RedirectToAction("InternallyApprovedBids", "Bid");
                }

                IDataResult<List<AppSetting>> appSettingListResult = await _appSettingService.GetList();
                var resultAppSetting = appSettingListResult.Data.FirstOrDefault();
                if (resultAppSetting == null)
                {
                    TempData["message"] = "Ayarlar sekmesinden firma bilgileri girmeden mail gönderemezsiniz.|warning";
                    return RedirectToAction("InternallyApprovedBids", "Bid");
                }

                Bid bidUpdate = _mapper.Map<BidEditModel, Bid>(bidEditModel);

                bidUpdate.BidId = bid.BidId;
                bidUpdate.BidNumber = bid.BidNumber;
                bidUpdate.BidValidityDate = bidEditModel.BidDate.AddDays(bidEditModel.BidPeriodOfValidity);
                if (bidEditModel.BidStatusStr == "Sent")
                {
                    bidUpdate.BidStatus = (int)Enums.BidStatus.Sent;
                }
                else
                {
                    TempData["message"] = "Teklif durumu hatalı...|error";
                    return View(bidEditModel);
                }

                bidUpdate.TotalPrice = Convert.ToDouble(bidEditModel.StrTotalPrice.Replace(",", "#").Replace(".", ",").Replace("#", "."));
                bidUpdate.TotalDiscount = Convert.ToDouble(bidEditModel.StrTotalDiscount.Replace(",", "#").Replace(".", ",").Replace("#", "."));
                bidUpdate.GeneralTotal = Convert.ToDouble(bidEditModel.StrGeneralTotal.Replace(",", "#").Replace(".", ",").Replace("#", "."));

                IDataResult<Bid> updatedBid = await _bidService.Update(bidUpdate);

                for (int i = 0; i < bidEditModel.ProductIdKods.Length; i++)
                {
                    if (bidEditModel.ProductIdKods[i] != null)
                    {
                        BidProduct editBidProduct = bidProducts.Where(x => x.ProductIdKod == bidEditModel.ProductIdKods[i]).FirstOrDefault();
                        if (editBidProduct == null)
                        {
                            //Yeni eklenen ürünler
                            BidProduct bidProduct = new BidProduct();
                            bidProduct.BidIdKod = updatedBid.Data.IdKod;
                            bidProduct.ProductIdKod = bidEditModel.ProductIdKods[i];
                            bidProduct.Count = Convert.ToInt32(bidEditModel.Counts[i]);
                            bidProduct.UnitPrice = Convert.ToDouble(bidEditModel.UnitPrices[i] == null ? "0.00" : bidEditModel.UnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            bidProduct.Discount = Convert.ToDouble(bidEditModel.Discounts[i]);
                            bidProduct.DiscountUnitPrice = Convert.ToDouble(bidEditModel.DiscountUnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            bidProduct.SubTotal = Convert.ToDouble(bidEditModel.SubTotals[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            await _bidProductService.Add(bidProduct);
                        }
                        else
                        {
                            //Güncellenmiş ürünler
                            editBidProduct.BidIdKod = updatedBid.Data.IdKod;
                            editBidProduct.ProductIdKod = bidEditModel.ProductIdKods[i];
                            editBidProduct.Count = Convert.ToInt32(bidEditModel.Counts[i]);
                            editBidProduct.UnitPrice = Convert.ToDouble(bidEditModel.UnitPrices[i] == null ? "0.00" : bidEditModel.UnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            editBidProduct.Discount = Convert.ToDouble(bidEditModel.Discounts[i]);
                            editBidProduct.DiscountUnitPrice = Convert.ToDouble(bidEditModel.DiscountUnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            editBidProduct.SubTotal = Convert.ToDouble(bidEditModel.SubTotals[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            await _bidProductService.Update(editBidProduct);

                        }
                    }
                }


                //Silinen ürünler
                List<BidProduct> updateBidProducts = _bidProductService.GetList().Result.Data.Where(x => x.BidIdKod == bidEditModel.IdKod).ToList();
                foreach (var item in updateBidProducts)
                {
                    bool isDeleted = bidEditModel.ProductIdKods.Any(x => x == item.ProductIdKod);
                    if (!isDeleted)
                    {
                        await _bidProductService.Delete(item);
                    }
                }

                //Teklifi mail gönderme
                AppSetting appSetting = appSettingListResult.Data.FirstOrDefault();
                string footer = $"Adres:{appSetting.Address} Tel:+90 {appSetting.PhoneNumber} E-mail:{appSetting.EMail}";
                string url = "/Bid/BidPdf?bidIdKod=" + bid.IdKod;
   
                bool result = CreatePdf.SelectCreatePdf(bid.BidNumber, url, footer);
                string attachmentFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\assets\\media\\bid\\pdf\\" + bid.BidNumber + ".pdf");

                string subject = $"Ürün Teklifi Hk.";
                string htmlMessage = $"Sayın: {firmManager.FirstName} {firmManager.LastName}<br/><br/>Firmamızdan teklif isteyerek göstermiş olduğunuz ilgi ve güven için teşekkür ederiz. Aşağıdaki bağlantıyı tıklayarak teklifimizi inceleyebilirsiniz.<br/><br/> Teklifi incelemek için <a href='{Constants.Constants.Url}/Bid/BidCustomerApproval?bidIdKod={bid.IdKod}'><b>tıklayınız.<b></a><br/><br/>Ürün detayları için <a href='{Constants.Constants.Url}/Bid/BidCustomerApprovalProductDetail?bidIdKod={bid.IdKod}'><b>tıklayınız.</b></a><br/><br/>Uygun görmeniz durumunda TEKLİFİ ONAYLA butonuna basınız.<br/><br/><br/>Saygılarımızla,<br/>İyi Çalışmalar.<br/><br/><br/><img style='width:200px;' src='{Constants.Constants.Url}/assets/media/appsettings/{resultAppSetting.Logo}'/></br><b>{resultAppSetting.FirmName}</b><br/>{resultAppSetting.Address}<br/><b>T:</b>+90 {resultAppSetting.PhoneNumber}";
                await _emailSender.SendEmailAttachmentAsync(firmManager.EMail, subject, htmlMessage, attachmentFile, resultAppSetting.EMail, resultAppSetting.FirmName);

                TempData["message"] = "Teklif başarıyla gönderilmiştir.|success";
                return RedirectToAction("SentBids", "Bid");

            }
            catch (Exception)
            {
                TempData["message"] = "Hata oluştu.|error";
                return RedirectToAction("InternallyApprovedBids", "Bid");
            }

        }

        [Authorize]
        public async Task<IActionResult> ExpiredBidEdit(string bidIdKod)
        {
            IDataResult<List<Firm>> firmListResult = await _firmService.GetList();
            IDataResult<List<Product>> productListResult = await _productService.GetList();
            IDataResult<List<FirmManager>> firmManagerListResult = await _firmManagerService.GetList();
            IDataResult<List<BidProduct>> bidProductListResult = await _bidProductService.GetListByBidIdKod(bidIdKod);
            IDataResult<List<GeneralRequirement>> generalRequirementListResult = await _generalRequirementService.GetList();


            IDataResult<Bid> bidResult = await _bidService.GetByIdKod(bidIdKod);
            BidEditModel bidEditModel = _mapper.Map<Bid, BidEditModel>(bidResult.Data);
            bidEditModel.firms = firmListResult.Data;
            bidEditModel.products = productListResult.Data;
            bidEditModel.firmManagers = firmManagerListResult.Data;
            bidEditModel.bidProducts = bidProductListResult.Data;
            bidEditModel.requirements = generalRequirementListResult.Data;

            bidEditModel.StrGeneralTotal = bidResult.Data.GeneralTotal?.ToString("N2").Replace(".", "#").Replace(",", ".").Replace("#", ",");
            bidEditModel.StrTotalPrice = bidResult.Data.TotalPrice?.ToString("N2").Replace(".", "#").Replace(",", ".").Replace("#", ",");
            bidEditModel.StrTotalDiscount = bidResult.Data.TotalDiscount?.ToString("N2").Replace(".", "#").Replace(",", ".").Replace("#", ",");

            return View(bidEditModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ExpiredBidEdit(BidEditModel bidEditModel)
        {
            try
            {

                Bid bid = _bidService.GetByIdKod(bidEditModel.IdKod).Result.Data;
                List<BidProduct> bidProducts = _bidProductService.GetList().Result.Data.Where(x => x.BidIdKod == bidEditModel.IdKod).ToList();

                IDataResult<Firm> firmResult = await _firmService.GetByIdKod(bid.FirmIdKod);
                IDataResult<FirmManager> firmManagerResult = await _firmManagerService.GetByIdKod(bid.FirmManagerIdKod);
                Firm firm = firmResult.Data;
                FirmManager firmManager = firmManagerResult.Data;
                if (firm == null)
                {
                    TempData["message"] = "Firma bilgisi hatalı olduğundan teklif gönderilemedi.|error";
                    return RedirectToAction("InternallyApprovedBids", "Bid");
                }
                if (firmManager == null)
                {
                    TempData["message"] = "Firma yönetici bilgisi hatalı olduğundan teklif gönderilemedi.|error";
                    return RedirectToAction("InternallyApprovedBids", "Bid");
                }

                IDataResult<List<AppSetting>> appSettingListResult = await _appSettingService.GetList();
                var resultAppSetting = appSettingListResult.Data.FirstOrDefault();
                if (resultAppSetting == null)
                {
                    TempData["message"] = "Ayarlar sekmesinden firma bilgileri girmeden mail gönderemezsiniz.|warning";
                    return RedirectToAction("InternallyApprovedBids", "Bid");
                }

                Bid bidUpdate = _mapper.Map<BidEditModel, Bid>(bidEditModel);

                bidUpdate.BidId = bid.BidId;
                bidUpdate.BidNumber = bid.BidNumber;
                bidUpdate.BidValidityDate = bidEditModel.BidDate.AddDays(bidEditModel.BidPeriodOfValidity);
                if (bidEditModel.BidStatusStr == "Sent")
                {
                    bidUpdate.BidStatus = (int)Enums.BidStatus.Sent;
                }
                else
                {
                    TempData["message"] = "Teklif durumu hatalı...|error";
                    return View(bidEditModel);
                }

                bidUpdate.TotalPrice = Convert.ToDouble(bidEditModel.StrTotalPrice.Replace(",", "#").Replace(".", ",").Replace("#", "."));
                bidUpdate.TotalDiscount = Convert.ToDouble(bidEditModel.StrTotalDiscount.Replace(",", "#").Replace(".", ",").Replace("#", "."));
                bidUpdate.GeneralTotal = Convert.ToDouble(bidEditModel.StrGeneralTotal.Replace(",", "#").Replace(".", ",").Replace("#", "."));

                IDataResult<Bid> updatedBid = await _bidService.Update(bidUpdate);

                for (int i = 0; i < bidEditModel.ProductIdKods.Length; i++)
                {
                    if (bidEditModel.ProductIdKods[i] != null)
                    {
                        BidProduct editBidProduct = bidProducts.Where(x => x.ProductIdKod == bidEditModel.ProductIdKods[i]).FirstOrDefault();
                        if (editBidProduct == null)
                        {
                            //Yeni eklenen ürünler
                            BidProduct bidProduct = new BidProduct();
                            bidProduct.BidIdKod = updatedBid.Data.IdKod;
                            bidProduct.ProductIdKod = bidEditModel.ProductIdKods[i];
                            bidProduct.Count = Convert.ToInt32(bidEditModel.Counts[i]);
                            bidProduct.UnitPrice = Convert.ToDouble(bidEditModel.UnitPrices[i] == null ? "0.00" : bidEditModel.UnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            bidProduct.Discount = Convert.ToDouble(bidEditModel.Discounts[i]);
                            bidProduct.DiscountUnitPrice = Convert.ToDouble(bidEditModel.DiscountUnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            bidProduct.SubTotal = Convert.ToDouble(bidEditModel.SubTotals[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            await _bidProductService.Add(bidProduct);
                        }
                        else
                        {
                            //Güncellenmiş ürünler
                            editBidProduct.BidIdKod = updatedBid.Data.IdKod;
                            editBidProduct.ProductIdKod = bidEditModel.ProductIdKods[i];
                            editBidProduct.Count = Convert.ToInt32(bidEditModel.Counts[i]);
                            editBidProduct.UnitPrice = Convert.ToDouble(bidEditModel.UnitPrices[i] == null ? "0.00" : bidEditModel.UnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            editBidProduct.Discount = Convert.ToDouble(bidEditModel.Discounts[i]);
                            editBidProduct.DiscountUnitPrice = Convert.ToDouble(bidEditModel.DiscountUnitPrices[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            editBidProduct.SubTotal = Convert.ToDouble(bidEditModel.SubTotals[i].Replace(",", "#").Replace(".", ",").Replace("#", "."));
                            await _bidProductService.Update(editBidProduct);

                        }
                    }
                }


                //Silinen ürünler
                List<BidProduct> updateBidProducts = _bidProductService.GetList().Result.Data.Where(x => x.BidIdKod == bidEditModel.IdKod).ToList();
                foreach (var item in updateBidProducts)
                {
                    bool isDeleted = bidEditModel.ProductIdKods.Any(x => x == item.ProductIdKod);
                    if (!isDeleted)
                    {
                        await _bidProductService.Delete(item);
                    }
                }

                //Teklifi mail gönderme
                AppSetting appSetting = appSettingListResult.Data.FirstOrDefault();
                string footer = $"Adres:{appSetting.Address} Tel:+90 {appSetting.PhoneNumber} E-mail:{appSetting.EMail}";
                string url = "/Bid/BidPdf?bidIdKod=" + bid.IdKod;

                bool result = CreatePdf.SelectCreatePdf(bid.BidNumber, url, footer);
                string attachmentFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\assets\\media\\bid\\pdf\\" + bid.BidNumber + ".pdf");

                string subject = $"Ürün Teklifi Hk.";
                string htmlMessage = $"Sayın: {firmManager.FirstName} {firmManager.LastName}<br/><br/>Firmamızdan teklif isteyerek göstermiş olduğunuz ilgi ve güven için teşekkür ederiz. Aşağıdaki bağlantıyı tıklayarak teklifimizi inceleyebilirsiniz.<br/><br/> Teklifi incelemek için <a href='{Constants.Constants.Url}/Bid/BidCustomerApproval?bidIdKod={bid.IdKod}'><b>tıklayınız.<b></a><br/><br/>Ürün detayları için <a href='{Constants.Constants.Url}/Bid/BidCustomerApprovalProductDetail?bidIdKod={bid.IdKod}'><b>tıklayınız.</b></a><br/><br/>Uygun görmeniz durumunda TEKLİFİ ONAYLA butonuna basınız.<br/><br/><br/>Saygılarımızla,<br/>İyi Çalışmalar.<br/><br/><br/><img style='width:200px;' src='{Constants.Constants.Url}/assets/media/appsettings/{resultAppSetting.Logo}'/><br/><b>{resultAppSetting.FirmName}</b><br/>{resultAppSetting.Address}<br/><b>T:</b>+90 {resultAppSetting.PhoneNumber}";
                await _emailSender.SendEmailAttachmentAsync(firmManager.EMail, subject, htmlMessage, attachmentFile, resultAppSetting.EMail, resultAppSetting.FirmName);

                TempData["message"] = "Teklif başarıyla gönderilmiştir.|success";
                return RedirectToAction("SentBids", "Bid");

            }
            catch (Exception)
            {
                TempData["message"] = "Hata oluştu.|error";
                return RedirectToAction("InternallyApprovedBids", "Bid");
            }

        }

        [Authorize]
        public async Task<IActionResult> SentBidDetail(string bidIdKod)
        {
            IDataResult<List<Firm>> firmListResult = await _firmService.GetList();
            IDataResult<List<Product>> productListResult = await _productService.GetList();
            IDataResult<List<FirmManager>> firmManagerListResult = await _firmManagerService.GetList();
            IDataResult<List<BidProduct>> bidProductListResult = await _bidProductService.GetListByBidIdKod(bidIdKod);
            IDataResult<List<GeneralRequirement>> generalRequirementListResult = await _generalRequirementService.GetList();

            IDataResult<Bid> bidResult = await _bidService.GetByIdKod(bidIdKod);
            BidEditModel bidEditModel = _mapper.Map<Bid, BidEditModel>(bidResult.Data);
            bidEditModel.firms = firmListResult.Data;
            bidEditModel.products = productListResult.Data;
            bidEditModel.firmManagers = firmManagerListResult.Data;
            bidEditModel.bidProducts = bidProductListResult.Data;
            bidEditModel.requirements = generalRequirementListResult.Data;

            bidEditModel.StrGeneralTotal = bidResult.Data.GeneralTotal?.ToString("N2").Replace(".", "#").Replace(",", ".").Replace("#", ",");
            bidEditModel.StrTotalPrice = bidResult.Data.TotalPrice?.ToString("N2").Replace(".", "#").Replace(",", ".").Replace("#", ",");
            bidEditModel.StrTotalDiscount = bidResult.Data.TotalDiscount?.ToString("N2").Replace(".", "#").Replace(",", ".").Replace("#", ",");

            return View(bidEditModel);
        }


        public async Task<IActionResult> BidCustomerApproval(string bidIdKod)
        {
            IDataResult<Bid> bidResult = await _bidService.GetByIdKod(bidIdKod);
            BidCustomerApprovalModel bidCustomerApprovalModel = new BidCustomerApprovalModel();

            Bid bid = bidResult.Data;
            IDataResult<Firm> firmResult = await _firmService.GetByIdKod(bid.FirmIdKod);
            IDataResult<FirmManager> firmManagerResult = await _firmManagerService.GetByIdKod(bid.FirmManagerIdKod);
            IDataResult<List<BidProductDetailDto>> bidProductDetailListResult = await _bidProductService.GetListBidProductDetail(bidIdKod);
            IDataResult<List<Device>> deviceListResult = await _deviceService.GetList();
            IDataResult<List<AppSetting>> appSettingListResult = await _appSettingService.GetList();

            User user = await _userManager.FindByIdAsync(bid.CreatedPersonalId);
            bidCustomerApprovalModel.bid = bid;
            bidCustomerApprovalModel.firm = firmResult.Data;
            bidCustomerApprovalModel.firmManager = firmManagerResult.Data;
            bidCustomerApprovalModel.appSetting = appSettingListResult.Data.FirstOrDefault();
            bidCustomerApprovalModel.devices = deviceListResult.Data;
            bidCustomerApprovalModel.bidProductDetailList = bidProductDetailListResult.Data;
            bidCustomerApprovalModel.createdPersonal = user;

            return View(bidCustomerApprovalModel);
        }

        public async Task<IActionResult> BidCustomerApprovalProductDetail(string bidIdKod)
        {
            IDataResult<Bid> bidResult = await _bidService.GetByIdKod(bidIdKod);
            BidCustomerApprovalModel bidCustomerApprovalModel = new BidCustomerApprovalModel();

            Bid bid = bidResult.Data;
            IDataResult<Firm> firmResult = await _firmService.GetByIdKod(bid.FirmIdKod);
            IDataResult<FirmManager> firmManagerResult = await _firmManagerService.GetByIdKod(bid.FirmManagerIdKod);
            IDataResult<List<BidProductDetailDto>> bidProductDetailListResult = await _bidProductService.GetListBidProductDetail(bidIdKod);
            IDataResult<List<Device>> deviceListResult = await _deviceService.GetList();
            IDataResult<List<AppSetting>> appSettingListResult = await _appSettingService.GetList();

            User user = await _userManager.FindByIdAsync(bid.CreatedPersonalId);
            bidCustomerApprovalModel.bid = bid;
            bidCustomerApprovalModel.firm = firmResult.Data;
            bidCustomerApprovalModel.firmManager = firmManagerResult.Data;
            bidCustomerApprovalModel.appSetting = appSettingListResult.Data.FirstOrDefault();
            bidCustomerApprovalModel.devices = deviceListResult.Data;
            bidCustomerApprovalModel.bidProductDetailList = bidProductDetailListResult.Data;
            bidCustomerApprovalModel.createdPersonal = user;

            return View(bidCustomerApprovalModel);
        }

        public async Task<JsonResult> BidCustomerApprovaled(string bidIdKod, string bidStatus)
        {
            try
            {
                IDataResult<Bid> bidResult = await _bidService.GetByIdKod(bidIdKod);
                Bid bid = bidResult.Data;
                AlertMessage alertMessage = new AlertMessage();
                if (bidResult.Data != null)
                {
                    if (bidStatus == "CustomerApproved")
                    {
                        bid.BidStatus = (int)Enums.BidStatus.CustomerApproved;
                        bid.ApprovalDate = DateTime.Now;
                        await _bidService.Update(bid);
                        alertMessage.ResponseStatus = true;
                        alertMessage.MessageText = "Teklif başarıyla onaylandı.";
                        alertMessage.MessageType = "success";

                        //Teklifi mail gönderme
                        IDataResult<List<AppSetting>> appSettingListResult = await _appSettingService.GetList();
                        var resultAppSetting = appSettingListResult.Data.FirstOrDefault();
                        IDataResult<Firm> firm = await _firmService.GetByIdKod(bid.FirmIdKod);

                        string subject = $"Ürün Teklif Onay Hk.";
                        string htmlMessage = $"Sayın: {resultAppSetting.FirmName};<br/><br/>{firm.Data.FirmName} firmasına vermiş olduğunuz <b>{bid.BidNumber}</b> numaralı teklif onaylanmıştır.";
                        await _emailSender.SendEmailAsync(resultAppSetting.EMail, subject, htmlMessage);
                    }
                    else
                    {
                        alertMessage.ResponseStatus = false;
                        alertMessage.MessageText = "Hata oluştu.";
                        alertMessage.MessageType = "error";
                    }
                }
                else
                {
                    alertMessage.ResponseStatus = false;
                    alertMessage.MessageText = "Hata oluştu.";
                    alertMessage.MessageType = "error";
                }



                return Json(alertMessage);
            }
            catch (Exception)
            {
                AlertMessage alertMessage = new AlertMessage();
                alertMessage.ResponseStatus = false;
                alertMessage.MessageText = "Hata oluştu.";
                alertMessage.MessageType = "error";
                return Json(alertMessage);

            }

        }

        public async Task<bool> HtmlToPdfSingle(string bidIdKod)
        {
            IDataResult<Bid> bid = await _bidService.GetByIdKod(bidIdKod);
            IDataResult<List<AppSetting>> appSettingListResult = await _appSettingService.GetList();
            AppSetting appSetting = appSettingListResult.Data.FirstOrDefault();
            string footer = $"Adres:{appSetting.Address} Tel:+90 {appSetting.PhoneNumber} E-mail:{appSetting.EMail}";
            string url = "/Bid/BidPdf?bidIdKod=" + bidIdKod;
            string fileName;
            if(bid.Data.BidNumber==null)
            {
                fileName = bid.Data.IdKod;
            }else
            {
                fileName = bid.Data.BidNumber;
            }
            bool result = CreatePdf.SelectCreatePdf(fileName, url, footer);
            return result;
        }
        public async Task<bool> HtmlToPdfProductDetail(string bidIdKod)
        {
            IDataResult<Bid> bid = await _bidService.GetByIdKod(bidIdKod);
            IDataResult<List<AppSetting>> appSettingListResult = await _appSettingService.GetList();
            AppSetting appSetting = appSettingListResult.Data.FirstOrDefault();
            string footer = $"Adres:{appSetting.Address} Tel:+90 {appSetting.PhoneNumber} E-mail:{appSetting.EMail}";
            string url = "/Bid/BidProductDetailPdf?bidIdKod=" + bidIdKod;

            bool result = CreatePdf.SelectCreatePdf(bid.Data.BidNumber, url, footer);
            return result;
        }

        public async Task<IActionResult> BidPdf(string bidIdKod)
        {
            IDataResult<Bid> bidResult = await _bidService.GetByIdKod(bidIdKod);
            BidCustomerApprovalModel bidCustomerApprovalModel = new BidCustomerApprovalModel();

            Bid bid = bidResult.Data;
            IDataResult<Firm> firmResult = await _firmService.GetByIdKod(bid.FirmIdKod);
            IDataResult<FirmManager> firmManagerResult = await _firmManagerService.GetByIdKod(bid.FirmManagerIdKod);
            IDataResult<List<BidProductDetailDto>> bidProductDetailListResult = await _bidProductService.GetListBidProductDetail(bidIdKod);
            IDataResult<List<Device>> deviceListResult = await _deviceService.GetList();
            IDataResult<List<AppSetting>> appSettingListResult = await _appSettingService.GetList();

            User user = await _userManager.FindByIdAsync(bid.CreatedPersonalId);
            bidCustomerApprovalModel.bid = bid;
            bidCustomerApprovalModel.firm = firmResult.Data;
            bidCustomerApprovalModel.firmManager = firmManagerResult.Data;
            bidCustomerApprovalModel.appSetting = appSettingListResult.Data.FirstOrDefault();
            bidCustomerApprovalModel.devices = deviceListResult.Data;
            bidCustomerApprovalModel.bidProductDetailList = bidProductDetailListResult.Data;
            bidCustomerApprovalModel.createdPersonal = user;

            return View(bidCustomerApprovalModel);
        }

        public async Task<IActionResult> SentBidEmail(string bidIdKod)
        {
            Bid bid = _bidService.GetByIdKod(bidIdKod).Result.Data;
            IDataResult<FirmManager> firmManagerResult = await _firmManagerService.GetByIdKod(bid.FirmManagerIdKod);
            FirmManager firmManager = firmManagerResult.Data;
            IDataResult<List<AppSetting>> appSettingListResult = await _appSettingService.GetList();
            var resultAppSetting = appSettingListResult.Data.FirstOrDefault();

            //Teklifi mail gönderme
            AppSetting appSetting = appSettingListResult.Data.FirstOrDefault();
            string footer = $"Adres:{appSetting.Address} Tel:+90 {appSetting.PhoneNumber} E-mail:{appSetting.EMail}";
            string url = "/Bid/BidPdf?bidIdKod=" + bid.IdKod;

            bool result = CreatePdf.SelectCreatePdf(bid.BidNumber, url, footer);
            string attachmentFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\assets\\media\\bid\\pdf\\" + bid.BidNumber + ".pdf");

            string subject = $"Ürün Teklifi Hk.";
            string htmlMessage = $"Sayın: {firmManager.FirstName} {firmManager.LastName}<br/><br/>Firmamızdan teklif isteyerek göstermiş olduğunuz ilgi ve güven için teşekkür ederiz. Aşağıdaki bağlantıyı tıklayarak teklifimizi inceleyebilirsiniz.<br/><br/> Teklifi incelemek için <a href='{Constants.Constants.Url}/Bid/BidCustomerApproval?bidIdKod={bid.IdKod}'><b>tıklayınız.<b></a><br/><br/>Ürün detayları için <a href='{Constants.Constants.Url}/Bid/BidCustomerApprovalProductDetail?bidIdKod={bid.IdKod}'><b>tıklayınız.</b></a><br/><br/>Uygun görmeniz durumunda TEKLİFİ ONAYLA butonuna basınız.<br/><br/><br/>Saygılarımızla,<br/>İyi Çalışmalar.<br/><br/><br/><img style='width:200px;' src='{Constants.Constants.Url}/assets/media/appsettings/{resultAppSetting.Logo}'/><br/><b>{resultAppSetting.FirmName}</b><br/>{resultAppSetting.Address}<br/><b>T:</b>+90 {resultAppSetting.PhoneNumber}";
            await _emailSender.SendEmailAttachmentAsync(firmManager.EMail, subject, htmlMessage, attachmentFile, resultAppSetting.EMail, resultAppSetting.FirmName);

            TempData["message"] = "Teklif başarıyla gönderilmiştir.|success";
            return RedirectToAction("SentBids", "Bid");
        }
        public async Task<IActionResult> BidProductDetailPdf(string bidIdKod)
        {
            IDataResult<Bid> bidResult = await _bidService.GetByIdKod(bidIdKod);
            BidCustomerApprovalModel bidCustomerApprovalModel = new BidCustomerApprovalModel();

            Bid bid = bidResult.Data;
            IDataResult<Firm> firmResult = await _firmService.GetByIdKod(bid.FirmIdKod);
            IDataResult<FirmManager> firmManagerResult = await _firmManagerService.GetByIdKod(bid.FirmManagerIdKod);
            IDataResult<List<BidProductDetailDto>> bidProductDetailListResult = await _bidProductService.GetListBidProductDetail(bidIdKod);
            IDataResult<List<Device>> deviceListResult = await _deviceService.GetList();
            IDataResult<List<AppSetting>> appSettingListResult = await _appSettingService.GetList();

            User user = await _userManager.FindByIdAsync(bid.CreatedPersonalId);
            bidCustomerApprovalModel.bid = bid;
            bidCustomerApprovalModel.firm = firmResult.Data;
            bidCustomerApprovalModel.firmManager = firmManagerResult.Data;
            bidCustomerApprovalModel.appSetting = appSettingListResult.Data.FirstOrDefault();
            bidCustomerApprovalModel.devices = deviceListResult.Data;
            bidCustomerApprovalModel.bidProductDetailList = bidProductDetailListResult.Data;
            bidCustomerApprovalModel.createdPersonal = user;

            return View(bidCustomerApprovalModel);
        }

        public async Task<ActionResult> DownloadPdf(string bidIdKod)
        {
            IDataResult<Bid> bid = await _bidService.GetByIdKod(bidIdKod);

            string fileName;
            if (bid.Data.BidNumber == null)
            {
                fileName = bid.Data.IdKod;
            }
            else
            {
                fileName = bid.Data.BidNumber;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\assets\\media\\bid\\pdf\\" + bid.Data.BidNumber + ".pdf");

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/pdf", fileName + ".pdf");

        }
        public async Task<JsonResult> BidDelete(string idKod)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                var bid = await _bidService.GetByIdKod(idKod);
                if (bid.Data == null)
                {
                    alertMessage.ResponseStatus = false;
                    alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                    alertMessage.MessageType = "error";
                    return Json(alertMessage);
                }
                await _bidService.Delete(bid.Data);
                alertMessage.ResponseStatus = true;
                alertMessage.MessageText = "Kayıt başarıyla silindi.";
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

        [HttpPost]
        public async Task<JsonResult> BidsDelete(string[] DeleteBids)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                foreach (var idKod in DeleteBids)
                {
                    var bid = await _bidService.GetByIdKod(idKod);
                    if (bid.Data == null)
                    {
                        alertMessage.ResponseStatus = false;
                        alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                        alertMessage.MessageType = "error";
                        return Json(alertMessage);
                    }
                    await _bidService.Delete(bid.Data);
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
    }
}
