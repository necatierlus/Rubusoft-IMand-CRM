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
    public class FirmManagerController : Controller
    {
        private IFirmManagerService _firmManagerService;
        private IMapper _mapper;
        public FirmManagerController(IFirmManagerService firmManagerService, IMapper mapper)
        {
            _firmManagerService = firmManagerService;
            _mapper = mapper;
        }
        public async Task<JsonResult> GetFirmManagerByIdKod(string managerIdKod)
        {
            var result = await _firmManagerService.GetByIdKod(managerIdKod);
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
        public async Task<IActionResult> FirmManagerEdit(FirmManagerModel firmManagerModel)
        {
            try
            {
                var result=await _firmManagerService.GetByIdKod(firmManagerModel.IdKod);
                FirmManager firmManager = result.Data;
                FirmManager editFirmManager = _mapper.Map<FirmManagerModel, FirmManager>(firmManagerModel);
                editFirmManager.FirmManagerId = firmManager.FirmManagerId;
                await _firmManagerService.Update(editFirmManager);
                TempData["message"] = "Yönetici başarıyla güncellenmiştir.|success";
                return RedirectToAction("FirmManagers", "Firm", new { firmIdKod = firmManagerModel.FirmIdKod });
            }
            catch (Exception)
            {
                TempData["message"] = "Firma yönetici bilgisi güncellenirken bir hata oluştu.|error";
                return RedirectToAction("FirmManagers", "Firm", new { firmIdKod = firmManagerModel.FirmIdKod });
            }


        }

        public async Task<JsonResult> FirmManagerDelete(string idKod)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                var device = await _firmManagerService.GetByIdKod(idKod);
                if (device.Data == null)
                {
                    alertMessage.ResponseStatus = false;
                    alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                    alertMessage.MessageType = "error";
                    return Json(alertMessage);
                }
                await _firmManagerService.Delete(device.Data);
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
