using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using Entities.Concrete;
using Entities.Dtos;
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
    [Authorize(Roles = "Admin,Teknik Destek")]
    public class TechSupportController : Controller
    {
        private ITechSupportService _techSupportService;
        private IFirmService _firmService;
        private IMapper _mapper;
        public TechSupportController(ITechSupportService techSupportService, IFirmService firmService, IMapper mapper)
        {
            _techSupportService = techSupportService;
            _firmService = firmService;
            _mapper = mapper;
        }
        public IActionResult TechSupportAdd()
        {
            IDataResult<List<Firm>> firmsResult = _firmService.GetList().Result;
            TechSupportModel techSupportModel = new TechSupportModel();
            techSupportModel.firms = firmsResult.Data;
            return View(techSupportModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TechSupportAdd(TechSupportModel techSupportModel, IFormFile Photo)
        {

            if (!ModelState.IsValid)
            {
                TempData["message"] = "Talep eklerken bir hata oluştu.|error";
                return RedirectToAction("TechSupports", "TechSupport", null);
            }
            TechSupport techSupport = _mapper.Map<TechSupportModel, TechSupport>(techSupportModel);
      

            if (Photo != null)
            {
                techSupport.Photo = await ImageUpload.Upload(Photo, "wwwroot\\assets\\media\\techsupports");
            }
            else
            {
                techSupport.Photo = Constants.Constants.DefaultTechSupportPhoto;
            }
            IResult result = await _techSupportService.Add(techSupport);
            if (result.Success)
            {
                TempData["message"] = result.Message + "|success";
            }
            else
            {
                TempData["message"] = "Bilinmeyen bir hata oluştu.|error";
            }

            return RedirectToAction("TechSupports", "TechSupport", null);
        }

        public IActionResult TechSupports()
        {
            IDataResult<List<TechSupportDto>> dataResult = _techSupportService.GetListTechSupportDto().Result;
            TechSupportListModel techSupportDetailModel = new TechSupportListModel();
            techSupportDetailModel.TechSupportDtos = dataResult.Data;
            return View(techSupportDetailModel);
        }

        [Route("TechSupport/TechSupportGetStatus/{idKod?}")]
        public JsonResult TechSupportGetStatus(string idKod)
        {
            IDataResult<TechSupport> dataResult = _techSupportService.GetByIdKod(idKod).Result;
            return Json(dataResult.Data);
        }

        [HttpPost]
        public async Task<JsonResult> TechSupportsDelete(string[] DeleteTechSupports)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                foreach (var idKod in DeleteTechSupports)
                {
                    var techSupport = await _techSupportService.GetByIdKod(idKod);
                    if (techSupport.Data == null)
                    {
                        alertMessage.ResponseStatus = false;
                        alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                        alertMessage.MessageType = "error";
                        return Json(alertMessage);
                    }
                    await _techSupportService.Delete(techSupport.Data);
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
