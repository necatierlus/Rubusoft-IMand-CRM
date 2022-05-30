using AutoMapper;
using Business.Abstract;
using Core.Utilities.Result;
using Entities.Concrete;
using IMandCRM.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Controllers
{
    public class ProductStockController : Controller
    {
        private IProductStockService _productStockService;
        private IProductService _productService;
        private IDeviceStockService _deviceStockService;
        private IMapper _mapper;
        public ProductStockController(IProductStockService productStockService, IProductService productService, IDeviceStockService deviceStockService,IMapper mapper)
        {
            _productStockService = productStockService;
            _deviceStockService = deviceStockService;
            _productService = productService;
            _mapper = mapper;
        }
        public async Task<IActionResult> ProductStocks()
        {
            var listProducStock = await _productStockService.GetListProductStockDto();
            var listDeviceStock = await _deviceStockService.GetListDeviceStockDto();
            var listProduct = await _productService.GetList();

            ProductStockListModel productStockListModel = new ProductStockListModel();
            productStockListModel.productStocks = listProducStock.Data;
            productStockListModel.deviceStocks = listDeviceStock.Data;
            productStockListModel.products = listProduct.Data;

            return View(productStockListModel);
        }
        public async Task<IActionResult> ProductStockAdd(ProductStockModel productStockModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "Stok noktası eklerken bir hata oluştu.|error";
                return RedirectToAction("DeviceStocks", "DeviceStock", null);
            }
            string deviceStockIdKods = "";
            foreach (var item in productStockModel.DeviceStockIdKod)
            {
                deviceStockIdKods += item+",";
                DeviceStock dataResult = _deviceStockService.GetByIdKod(item).Result.Data;
                if(dataResult!=null)
                {
                    dataResult.IsInProduct = true;
                    await _deviceStockService.Update(dataResult);
                }
            }
            ProductStock productStock = _mapper.Map<ProductStockModel, ProductStock>(productStockModel);
            productStock.DeviceStockIdKods = deviceStockIdKods;

            IResult result = await _productStockService.Add(productStock);
            if (result.Success)
            {
                TempData["message"] = result.Message + "|success";
            }
            else
            {
                TempData["message"] = "Bilinmeyen bir hata oluştu.|error";
            }
            return Redirect("/ProductStock/ProductStocks");
        }
    }
}
