using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using Entities.Concrete;
using IMandCRM.UI.HelperMethods;
using IMandCRM.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AppSettingController : Controller
    {
        private IAppSettingService _appSettingService;
        private readonly IConfiguration _config;
        private IMapper _mapper;
        public AppSettingController(IAppSettingService appSettingService, IMapper mapper, IConfiguration config)
        {
            _appSettingService = appSettingService;
            _mapper = mapper;
            _config = config;
        }
        public async Task<IActionResult> AppSettingEdit()
        {
            IDataResult<List<AppSetting>> appSettingListResult = await _appSettingService.GetList();
            var result = appSettingListResult.Data.FirstOrDefault();
            if (result == null)
            {
                AppSettingEditModel appSettingEditModel = new AppSettingEditModel();
                appSettingEditModel.Logo = Constants.Constants.DefaultAppLogo;
                appSettingEditModel.MailSenderHost = _config.GetValue<string>("EmailSender:Host");
                appSettingEditModel.MailSenderPort = _config.GetValue<int>("EmailSender:Port");
                appSettingEditModel.MailSenderEnableSSL = _config.GetValue<bool>("EmailSender:EnableSSL");
                appSettingEditModel.MailSenderUserName = _config.GetValue<string>("EmailSender:UserName");
                appSettingEditModel.MailSenderPassword = _config.GetValue<string>("EmailSender:Password");

                AppSetting addAppSetting = _mapper.Map<AppSettingEditModel, AppSetting>(appSettingEditModel);
                await _appSettingService.Add(addAppSetting);
                return View(appSettingEditModel);
            }
            AppSettingEditModel appSetting = _mapper.Map<AppSetting, AppSettingEditModel>(result);
            appSetting.MailSenderHost = _config.GetValue<string>("EmailSender:Host");
            appSetting.MailSenderPort = _config.GetValue<int>("EmailSender:Port");
            appSetting.MailSenderEnableSSL = _config.GetValue<bool>("EmailSender:EnableSSL");
            appSetting.MailSenderUserName = _config.GetValue<string>("EmailSender:UserName");
            appSetting.MailSenderPassword = _config.GetValue<string>("EmailSender:Password");
            return View(appSetting);
        }

        [HttpPost]
        public async Task<IActionResult> AppSettingEdit(AppSettingEditModel appSettingEditModel, IFormFile Logo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["message"] = "Ayarlar güncellenirken bir hata oluştu.|error";
                    return View(appSettingEditModel);
                }
                AppSetting appSetting = _mapper.Map<AppSettingEditModel, AppSetting>(appSettingEditModel);
                appSetting.MailSenderHost = _config.GetValue<string>("EmailSender:Host");
                appSetting.MailSenderPort = _config.GetValue<int>("EmailSender:Port");
                appSetting.MailSenderEnableSSL = _config.GetValue<bool>("EmailSender:EnableSSL");
                appSetting.MailSenderUserName = _config.GetValue<string>("EmailSender:UserName");
                appSetting.MailSenderPassword = _config.GetValue<string>("EmailSender:Password");

                IDataResult<List<AppSetting>> appSettingListResult = await _appSettingService.GetList();
                var result = appSettingListResult.Data.FirstOrDefault();

                if (Logo != null)
                {
                    appSetting.Logo = await ImageUpload.Upload(Logo, "wwwroot\\assets\\media\\appsettings");
                }
                else
                {
                    appSetting.Logo = result.Logo;
                }


                if (result == null)
                {
                    IDataResult<AppSetting> resultAppSettingAdd = await _appSettingService.Add(appSetting);
                    if (!resultAppSettingAdd.Success)
                    {
                        TempData["message"] = "Ayarlar güncellenirken bir hata oluştu.|error";
                        return View(appSettingEditModel);
                    }
                }
                else
                {
                    appSetting.AppSettingId = result.AppSettingId;
                    IDataResult<AppSetting> resultAppSettingUpdate = await _appSettingService.Update(appSetting);
                    if (!resultAppSettingUpdate.Success)
                    {
                        TempData["message"] = "Ayarlar güncellenirken bir hata oluştu.|error";
                        return View(appSettingEditModel);
                    }
                }

                return RedirectToAction("AppSettingEdit", "AppSetting", null);
            }
            catch (Exception)
            {
                return View(appSettingEditModel);
            }

        }
    }
}
