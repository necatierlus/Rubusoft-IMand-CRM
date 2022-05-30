using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using Entities.Concrete;
using Entities.Dtos;
using IMandCRM.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Controllers
{
    [Authorize(Roles = "Admin,Teknik Destek")]
    public class TechSupportDetailController : Controller
    {
        private ITechSupportService _techSupportService;
        private ITechSupportDetailService _techSupportDetailService;
        private IMapper _mapper;
        public TechSupportDetailController(ITechSupportService techSupportService, ITechSupportDetailService techSupportDetailService, IMapper mapper)
        {
            _techSupportService = techSupportService;
            _techSupportDetailService = techSupportDetailService;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> TechSupportDetailAdd(TechSupportDetailModel techSupportDetailModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "Talep eklerken bir hata oluştu.|error";
                return RedirectToAction("TechSupports", "TechSupport", null);
            }
            TechSupportDetail techSupportDetail = _mapper.Map<TechSupportDetailModel, TechSupportDetail>(techSupportDetailModel);

            IResult result = await _techSupportDetailService.Add(techSupportDetail);

            if (result.Success)
            {
                TempData["message"] = result.Message + "|success";
            }
            else
            {
                TempData["message"] = "Bilinmeyen bir hata oluştu.|error";
            }

            TechSupport techSupport = _techSupportService.GetByIdKod(techSupportDetailModel.TechSupportIdKod).Result.Data;

            if(techSupportDetailModel.Status)
            {
                techSupport.Status = true;
            }else
            {
                techSupport.Status = false;
            }
            TechSupport techSupportUpdate = _techSupportService.Update(techSupport).Result.Data;

            return RedirectToAction("TechSupports", "TechSupport", null);
        }

        [Route("TechSupportDetail/TechSupportDetails/{techSupportIdKod?}")]
        public async Task<JsonResult> TechSupportDetails(string techSupportIdKod)
        {
            IDataResult<List<TechSupportDetailDto>> dataTechSupportDetailResult = await  _techSupportDetailService.GetListTechSupportDetailDto(techSupportIdKod);
            IDataResult<TechSupportDto> dataTechSupportResult =await _techSupportService.GetTechSupportDtoByIdKod(techSupportIdKod);
            TechSupportDetailListModel techSupportDetailModel = new TechSupportDetailListModel();
            techSupportDetailModel.TechSupportDetailDtos = dataTechSupportDetailResult.Data;
            techSupportDetailModel.TechSupportDto = dataTechSupportResult.Data;
            return Json(techSupportDetailModel);
        }
    }
}
