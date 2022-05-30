using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using Entities.Concrete;
using IMandCRM.UI.Messages;
using IMandCRM.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Controllers
{
    public class StockPointController : Controller
    {
        private IStockPointService _stockPointService;
        private IMapper _mapper;
        public StockPointController(IStockPointService stockPointService, IMapper mapper)
        {
            _stockPointService = stockPointService;
            _mapper = mapper;
        }
        public async Task<IActionResult> StockPoints()
        {
            var listAsync = await _stockPointService.GetList();
            var list = listAsync.Data;
            StockPointListModel stockPointListModel = new StockPointListModel();
            stockPointListModel.stockPoints = list;
            return View(stockPointListModel);
        }

        [HttpPost]
        public async Task<IActionResult> StockPointAdd(StockPointModel stockPointModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "Stok noktası eklerken bir hata oluştu.|error";
                return RedirectToAction("StockPoints", "StockPoint", null);
            }
            StockPoint stockPoint = _mapper.Map<StockPointModel, StockPoint>(stockPointModel);

            IResult result = await _stockPointService.Add(stockPoint);
            if (result.Success)
            {
                TempData["message"] = result.Message + "|success";
            }
            else
            {
                TempData["message"] = "Bilinmeyen bir hata oluştu.|error";
            }

            return RedirectToAction("StockPoints", "StockPoint", null);
        }

        [HttpPost]
        public async Task<JsonResult> StockPointsDelete(string[] DeleteStockPoints)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                foreach (var idKod in DeleteStockPoints)
                {
                    var stockPoint = await _stockPointService.GetByIdKod(idKod);
                    if (stockPoint.Data == null)
                    {
                        alertMessage.ResponseStatus = false;
                        alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                        alertMessage.MessageType = "error";
                        return Json(alertMessage);
                    }
                    await _stockPointService.Delete(stockPoint.Data);
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
