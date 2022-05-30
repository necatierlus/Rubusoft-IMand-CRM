using AutoMapper;
using Business.Abstract;
using Entities.Concrete;
using IMandCRM.UI.Messages;
using IMandCRM.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Controllers
{
    [Authorize]
    public class AddressController : Controller
    {
        private IAddressService _adressService;
        private IMapper _mapper;
        public AddressController(IAddressService adressService, IMapper mapper)
        {
            _adressService = adressService;
            _mapper = mapper;
        }
        public async Task<JsonResult> GetFirmAddressByIdKod(string addressIdKod)
        {
            var result = await _adressService.GetByIdKod(addressIdKod);
            if (result.Success)
            {
                return Json(new { Data = result.Data, Result = true });
            }
            else
            {
                return Json(new { Result = false });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FirmAddressEdit(FirmAddressModel firmAddressModel)
        {
            try
            {
                var result = await _adressService.GetByIdKod(firmAddressModel.IdKod);
                Address firmManager = result.Data;
                Address editAddress = _mapper.Map<FirmAddressModel, Address>(firmAddressModel);
                editAddress.AddressId = firmManager.AddressId;
                await _adressService.Update(editAddress);
                TempData["message"] = "Adres başarıyla güncellenmiştir.|success";
                return RedirectToAction("FirmAddresses", "Firm", new { firmIdKod = firmAddressModel.FirmIdKod });
            }
            catch (Exception)
            {
                TempData["message"] = "Firma adres bilgisi güncellenirken bir hata oluştu.|error";
                return RedirectToAction("FirmAddresses", "Firm", new { firmIdKod = firmAddressModel.FirmIdKod });
            }


        }

        public async Task<JsonResult> AddressDelete(string idKod)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                var device = await _adressService.GetByIdKod(idKod);
                if (device.Data == null)
                {
                    alertMessage.ResponseStatus = false;
                    alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                    alertMessage.MessageType = "error";
                    return Json(alertMessage);
                }
                await _adressService.Delete(device.Data);
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
    }
}
