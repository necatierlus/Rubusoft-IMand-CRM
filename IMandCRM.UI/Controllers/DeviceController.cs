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
    public class DeviceController : Controller
    {
        private IDeviceService _deviceService;
        private IMapper _mapper;
        public DeviceController(IDeviceService deviceService, IMapper mapper)
        {
            _mapper = mapper;
            _deviceService = deviceService;
        }
        public async Task<IActionResult> Devices()
        {
            var listAsync = await _deviceService.GetList();
            var list = listAsync.Data;
            DeviceListModel deviceListModel = new DeviceListModel();
            deviceListModel.devices = list;
            return View(deviceListModel);
        }

        public IActionResult DeviceAdd()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeviceAdd(DeviceModel deviceModel, IFormFile DevicePhoto)
        {

            if (!ModelState.IsValid)
            {
                TempData["message"] = "Parça eklerken bir hata oluştu.|error";
                return RedirectToAction("Devices", "Device", null);
            }
            Device device = _mapper.Map<DeviceModel, Device>(deviceModel);
            if (DevicePhoto != null)
            {
                device.DevicePhoto = await ImageUpload.Upload(DevicePhoto, "wwwroot\\assets\\media\\devices");
            }
            else
            {
                device.DevicePhoto = Constants.Constants.DefaultDevicePhoto;
            }
            IResult result = await _deviceService.Add(device);
            if (result.Success)
            {
                TempData["message"] = result.Message + "|success";
            }
            else
            {
                TempData["message"] = "Bilinmeyen bir hata oluştu.|error";
            }

            return RedirectToAction("Devices", "Device", null);
        }

        public async Task<IActionResult> DeviceEdit(string idKod)
        {
            var result = await _deviceService.GetByIdKod(idKod);
            Device device = result.Data;
            DeviceModel deviceModel = _mapper.Map<Device, DeviceModel>(device);

            return View(deviceModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeviceEdit(DeviceModel deviceModel, IFormFile DevicePhoto)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "Parça güncellenirken bir hata oluştu.|error";
                return View(deviceModel);
            }
            Device device = _deviceService.GetByIdKod(deviceModel.IdKod).Result.Data;
            if (device == null)
            {
                TempData["message"] = "Parça bulunamadı.|error";
                return View(deviceModel);
            }
            Device editDevice = _mapper.Map<DeviceModel, Device>(deviceModel);
            editDevice.DeviceId = device.DeviceId;
            if (DevicePhoto != null)
            {
                editDevice.DevicePhoto = await ImageUpload.Upload(DevicePhoto, "wwwroot\\assets\\media\\devices");
            }
            else
            {
                editDevice.DevicePhoto = device.DevicePhoto;
            }

            IResult result = await _deviceService.Update(editDevice);
            if (result.Success)
            {
                TempData["message"] = result.Message + "|success";
            }
            else
            {
                TempData["message"] = "Bilinmeyen bir hata oluştu.|error";
                return View(deviceModel);
            }

            return RedirectToAction("Devices", "Device", null);


        }


        public async Task<JsonResult> DeviceDelete(string idKod)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                var device = await _deviceService.GetByIdKod(idKod);
                if (device.Data == null)
                {
                    alertMessage.ResponseStatus = false;
                    alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                    alertMessage.MessageType = "error";
                    return Json(alertMessage);
                }
                await _deviceService.Delete(device.Data);
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
        public async Task<JsonResult> DevicesDelete(string[] DeleteDevices)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                foreach (var idKod in DeleteDevices)
                {
                    var device = await _deviceService.GetByIdKod(idKod);
                    if (device.Data == null)
                    {
                        alertMessage.ResponseStatus = false;
                        alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                        alertMessage.MessageType = "error";
                        return Json(alertMessage);
                    }
                    await _deviceService.Delete(device.Data);
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

        public JsonResult GetDevices()
        {
            List<Device> devices = _deviceService.GetList().Result.Data;
            return Json(devices);
        }

    }
}
