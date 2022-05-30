using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
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
    public class GeneralRequirementController : Controller
    {
        private IGeneralRequirementService _generalRequirementService;
        private IMapper _mapper;
        public GeneralRequirementController(IGeneralRequirementService generalRequirementService, IMapper mapper)
        {
            _generalRequirementService = generalRequirementService;
            _mapper = mapper;
        }
        public async Task<IActionResult> GeneralRequirements()
        {
            var listAsync = await _generalRequirementService.GetList();
            var list = listAsync.Data;
            GeneralRequirementListModel generalRequirementListModel = new GeneralRequirementListModel();
            generalRequirementListModel.generalRequirements = list;
            return View(generalRequirementListModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GeneralRequirementAdd(GeneralRequirementModel generalRequirementModel)
        {

            if (!ModelState.IsValid)
            {
                TempData["message"] = "Ürün eklerken bir hata oluştu.|error";
                return RedirectToAction("Products", "Product", null);
            }
            GeneralRequirement generalRequirement = _mapper.Map<GeneralRequirementModel, GeneralRequirement>(generalRequirementModel);
            IResult result = await _generalRequirementService.Add(generalRequirement);
            if (result.Success)
            {
                TempData["message"] = result.Message + "|success";
            }
            else
            {
                TempData["message"] = "Bilinmeyen bir hata oluştu.|error";
            }

            return RedirectToAction("GeneralRequirements", "GeneralRequirement", null);
        }
        public async Task<IActionResult> GeneralRequirementEdit(string idKod)
        {
            var result = await _generalRequirementService.GetByIdKod(idKod);
            GeneralRequirement generalRequirement = result.Data;
            GeneralRequirementModel generalRequirementModel = _mapper.Map<GeneralRequirement, GeneralRequirementModel>(generalRequirement);

            return View(generalRequirementModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GeneralRequirementEdit(GeneralRequirementModel generalRequirementModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "Teklif şartı güncellenirken bir hata oluştu.|error";
                return View(generalRequirementModel);
            }
            GeneralRequirement generalRequirement = _generalRequirementService.GetByIdKod(generalRequirementModel.IdKod).Result.Data;
            if (generalRequirement == null)
            {
                TempData["message"] = "Teklif şartı bulunamadı.|error";
                return View(generalRequirementModel);
            }
            GeneralRequirement editGeneralRequirement = _mapper.Map<GeneralRequirementModel, GeneralRequirement>(generalRequirementModel);
            editGeneralRequirement.GeneralRequirementId = generalRequirement.GeneralRequirementId;

            IResult result = await _generalRequirementService.Update(editGeneralRequirement);
            if (result.Success)
            {
                TempData["message"] = result.Message + "|success";
            }
            else
            {
                TempData["message"] = "Bilinmeyen bir hata oluştu.|error";
                return View(generalRequirementModel);
            }

            return RedirectToAction("GeneralRequirements", "GeneralRequirement", null);
        }

        public async Task<JsonResult> GeneralRequirementDelete(string idKod)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                var generalRequirement = await _generalRequirementService.GetByIdKod(idKod);
                if (generalRequirement.Data == null)
                {
                    alertMessage.ResponseStatus = false;
                    alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                    alertMessage.MessageType = "error";
                    return Json(alertMessage);
                }
                await _generalRequirementService.Delete(generalRequirement.Data);
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
        public async Task<JsonResult> GeneralRequirementsDelete(string[] DeleteGeneralrequirements)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                foreach (var idKod in DeleteGeneralrequirements)
                {
                    var generalRequirement = await _generalRequirementService.GetByIdKod(idKod);
                    if (generalRequirement.Data == null)
                    {
                        alertMessage.ResponseStatus = false;
                        alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                        alertMessage.MessageType = "error";
                        return Json(alertMessage);
                    }
                    await _generalRequirementService.Delete(generalRequirement.Data);
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
