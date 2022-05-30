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
    public class DeviceStockController : Controller
    {
        private IDeviceStockService _deviceStockService;
        private IDeviceService _deviceService;
        private IStockPointService _stockPointService;
        private IMapper _mapper;
        public DeviceStockController(IDeviceStockService deviceStockService, IDeviceService deviceService, IStockPointService stockPointService, IMapper mapper)
        {
            _deviceStockService = deviceStockService;
            _deviceService = deviceService;
            _stockPointService = stockPointService;
            _mapper = mapper;
        }
        public async Task<IActionResult> DeviceStocks()
        {
            var listAsyncDeviceStock = await _deviceStockService.GetListDeviceStockDto();
            var listAsyncDevice = await _deviceService.GetList();
            var listAsyncStockPoint = await _stockPointService.GetList();
            var deviceStocks = listAsyncDeviceStock.Data;
            var devices = listAsyncDevice.Data;
            var stockPoints = listAsyncStockPoint.Data;
            DeviceStockListModel deviceStockListModel = new DeviceStockListModel();
            deviceStockListModel.deviceStocks = deviceStocks;
            deviceStockListModel.devices = devices;
            deviceStockListModel.stockPoints = stockPoints;
            return View(deviceStockListModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeviceStockAdd(DeviceStockModel deviceStockModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "Stok noktası eklerken bir hata oluştu.|error";
                return RedirectToAction("DeviceStocks", "DeviceStock", null);
            }
            DeviceStock deviceStock = _mapper.Map<DeviceStockModel, DeviceStock>(deviceStockModel);

            IResult result = await _deviceStockService.Add(deviceStock);
            if (result.Success)
            {
                TempData["message"] = result.Message + "|success";
            }
            else
            {
                TempData["message"] = "Bilinmeyen bir hata oluştu.|error";
            }

            return RedirectToAction("DeviceStocks", "DeviceStock", null);
        }

        [HttpPost]
        public async Task<JsonResult> DeviceStocksDelete(string[] DeleteDeviceStocks)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                foreach (var idKod in DeleteDeviceStocks)
                {
                    var deviceStock = await _deviceStockService.GetByIdKod(idKod);
                    if (deviceStock.Data == null)
                    {
                        alertMessage.ResponseStatus = false;
                        alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                        alertMessage.MessageType = "error";
                        return Json(alertMessage);
                    }
                    await _deviceStockService.Delete(deviceStock.Data);
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
