using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using Entities.Concrete;
using IMandCRM.UI.HelperMethods;
using IMandCRM.UI.Messages;
using IMandCRM.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Controllers
{
    [Authorize]
    public class FirmController : Controller
    {
        private IFirmService _firmService;
        private IFirmManagerService _firmManagerService;
        private IAddressService _addressService;
        private ICountryService _countryService;
        private ICityService _cityService;
        private IDistrictService _districtService;
        private IMapper _mapper;
        public FirmController(IFirmService firmService, IFirmManagerService firmManagerService, IAddressService addressService, ICountryService countryService, ICityService cityService, IDistrictService districtService, IMapper mapper)
        {
            _firmService = firmService;
            _firmManagerService = firmManagerService;
            _addressService = addressService;
            _countryService = countryService;
            _cityService = cityService;
            _districtService = districtService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Firms()
        {
            var listFirmAsync = await _firmService.GetList();
            var listFirm = listFirmAsync.Data;
            var firmListModel = _mapper.Map<List<Firm>, List<FirmModel>>(listFirm);

            return View(firmListModel);
        }

        public IActionResult FirmAdd()
        {
            List<Country> countries = _countryService.GetList().Result.Data;
            List<City> cities = _cityService.GetList().Result.Data;
            List<District> districts = _districtService.GetList().Result.Data;
            FirmAddModel firmAddModel = new FirmAddModel();
            firmAddModel.Countries = countries;
            firmAddModel.Cities = cities;
            firmAddModel.Districts = districts;
            return View(firmAddModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FirmAdd(FirmAddModel firmAddModel, IFormFile FirmLogo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["message"] = "Firma eklerken bir hata oluştu.|error";
                    return View(firmAddModel);
                }
                Firm firm = _mapper.Map<FirmAddModel, Firm>(firmAddModel);
                FirmManager firmManager = _mapper.Map<FirmAddModel, FirmManager>(firmAddModel);
                Address firmAddress = _mapper.Map<FirmAddModel, Address>(firmAddModel);

                if (FirmLogo != null)
                {
                    firm.FirmLogo = await ImageUpload.Upload(FirmLogo, "wwwroot\\assets\\media\\firms");
                }
                else
                {
                    firm.FirmLogo = Constants.Constants.DefaultFirmLogo;
                }
                IDataResult<Firm> resultFirm = await _firmService.Add(firm);
                if (!resultFirm.Success)
                {
                    TempData["message"] = "Firma bilgisi eklerken bir hata oluştu.|error";
                    return View(firmAddModel);
                }
                firmManager.FirmIdKod = resultFirm.Data.IdKod;
                IResult resultFirmManager = await _firmManagerService.Add(firmManager);
                if (!resultFirmManager.Success)
                {
                    TempData["message"] = "Firma yönetici bilgisi eklerken bir hata oluştu.|error";
                    return View(firmAddModel);
                }
                firmAddress.FirmIdKod = resultFirm.Data.IdKod;
                IResult resultFirmAddress = await _addressService.Add(firmAddress);
                if (!resultFirmAddress.Success)
                {
                    TempData["message"] = "Firma adres bilgisi eklerken bir hata oluştu.|error";
                    return View(firmAddModel);
                }

                return RedirectToAction("Firms", "Firm", null);

            }
            catch (Exception)
            {

                TempData["message"] = "Hata oluştu.|error";
                return View(firmAddModel);
            }

        }

        public async Task<IActionResult> FirmDetail(string idKod)
        {
            var result = await _firmService.GetByIdKod(idKod);
            Firm firm = result.Data;
            FirmModel firmModel = _mapper.Map<Firm, FirmModel>(firm);

            return View(firmModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FirmManagerAdd(FirmManagerModel firmManagerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["message"] = "Firma yönetici bilgisi eklerken bir hata oluştu.|error";
                    return View(firmManagerModel);
                }

                FirmManager firmManager = _mapper.Map<FirmManagerModel, FirmManager>(firmManagerModel);


                IResult resultFirmManager = await _firmManagerService.Add(firmManager);
                if (!resultFirmManager.Success)
                {
                    TempData["message"] = "Firma yönetici bilgisi eklerken bir hata oluştu.|error";
                    return View(firmManagerModel);
                }
                return RedirectToAction("FirmManagers", "Firm", new { firmIdKod = firmManagerModel.FirmIdKod });

            }
            catch (Exception)
            {

                TempData["message"] = "Hata oluştu.|error";
                return View(firmManagerModel);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FirmAddressAdd(FirmAddressModel firmAddressModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["message"] = "Firma adres bilgisi eklerken bir hata oluştu.|error";
                    return View(firmAddressModel);
                }

                Address firmAddress = _mapper.Map<FirmAddressModel, Address>(firmAddressModel);


                IResult resultFirmAddress = await _addressService.Add(firmAddress);
                if (!resultFirmAddress.Success)
                {
                    TempData["message"] = "Firma adres bilgisi eklerken bir hata oluştu.|error";
                    return View(firmAddressModel);
                }
                return RedirectToAction("FirmAddresses", "Firm", new { firmIdKod = firmAddressModel.FirmIdKod });

            }
            catch (Exception)
            {

                TempData["message"] = "Hata oluştu.|error";
                return View(firmAddressModel);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FirmEdit(FirmModel firmModel, IFormFile FirmLogo)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "Firma güncellenirken bir hata oluştu.|error";
                return View(firmModel);
            }
            Firm firm = _firmService.GetByIdKod(firmModel.IdKod).Result.Data;
            if (firm == null)
            {
                TempData["message"] = "Cihaz bulunamadı.|error";
                return View(firmModel);
            }
            Firm editFirm = _mapper.Map<FirmModel, Firm>(firmModel);
            editFirm.FirmId = firm.FirmId;
            if (FirmLogo != null)
            {
                firm.FirmLogo = await ImageUpload.Upload(FirmLogo, "wwwroot\\assets\\media\\firms");
            }
            else
            {
                editFirm.FirmLogo = firm.FirmLogo;
            }

            IResult result = await _firmService.Update(editFirm);
            if (result.Success)
            {
                TempData["message"] = result.Message + "|success";
            }
            else
            {
                TempData["message"] = "Bilinmeyen bir hata oluştu.|error";
                return View(firmModel);
            }

            return RedirectToAction("FirmDetail", "Firm", new { idKod = firmModel.IdKod });


        }

        public IActionResult FirmAddresses(string firmIdKod)
        {
            var result = _addressService.GetAddressDetailListByFirmIdKod(firmIdKod);
            List<Country> countries = _countryService.GetList().Result.Data;
            List<City> cities = _cityService.GetList().Result.Data;
            List<District> districts = _districtService.GetList().Result.Data;
            FirmAddressesModel firmAddressesModel = new FirmAddressesModel();
            firmAddressesModel.Countries = countries;
            firmAddressesModel.Cities = cities;
            firmAddressesModel.Districts = districts;
            firmAddressesModel.addresses = result.Data;
            firmAddressesModel.firmIdKod = firmIdKod;
            return View(firmAddressesModel);
        }

        public async Task<IActionResult> FirmManagers(string firmIdKod)
        {
            var result = await _firmManagerService.GetListByFirmIdKod(firmIdKod);
            FirmManagersModel firmManagersModel = new FirmManagersModel();
            firmManagersModel.managers = result.Data;
            firmManagersModel.firmIdKod = firmIdKod;
            return View(firmManagersModel);
        }


        [HttpPost]
        public async Task<JsonResult> FirmsDelete(string[] DeleteFirms)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                foreach (var idKod in DeleteFirms)
                {
                    var device = await _firmService.GetByIdKod(idKod);
                    if (device.Data == null)
                    {
                        alertMessage.ResponseStatus = false;
                        alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                        alertMessage.MessageType = "error";
                        return Json(alertMessage);
                    }
                    await _firmService.Delete(device.Data);
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
