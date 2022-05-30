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
    public class ProductController : Controller
    {
        private IProductService _productService;
        private IDeviceService _deviceService;
        private IMapper _mapper;
        public ProductController(IProductService productService, IDeviceService deviceService, IMapper mapper)
        {
            _productService = productService;
            _deviceService = deviceService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Products()
        {
            var listAsync = await _productService.GetList();
            var list = listAsync.Data;
            ProductListModel productListModel = new ProductListModel();
            productListModel.products = list;
            return View(productListModel);
        }

        public IActionResult ProductAdd()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductAdd(ProductModel productModel, IFormFile ProductPhoto)
        {
           
            if (!ModelState.IsValid)
            {
                TempData["message"] = "Ürün eklerken bir hata oluştu.|error";
                return RedirectToAction("Products", "Product", null);
            }
            Product product = _mapper.Map<ProductModel, Product>(productModel);
            product.Devices = productModel.Devices==null?null:string.Join(",", productModel.Devices);

            if (ProductPhoto != null)
            {
                product.ProductPhoto = await ImageUpload.Upload(ProductPhoto, "wwwroot\\assets\\media\\products");
            }
            else
            {
                product.ProductPhoto = Constants.Constants.DefaultProductPhoto;
            }
            IResult result = await _productService.Add(product);
            if (result.Success)
            {
                TempData["message"] = result.Message + "|success";
            }
            else
            {
                TempData["message"] = "Bilinmeyen bir hata oluştu.|error";
            }

            return RedirectToAction("Products", "Product", null);
        }

        public async Task<IActionResult> ProductEdit(string idKod)
        {
            var resultProduct = await _productService.GetByIdKod(idKod);
            Product product = resultProduct.Data;
            var resultDeviceList = await _deviceService.GetList();

            ProductModel productModel = _mapper.Map<Product, ProductModel>(product);
            ProductEditModel productEditModel = new ProductEditModel();
            productEditModel.product = productModel;
            productEditModel.productDevices = product.Devices==null? new string[] { } : product.Devices.Split(",");
            productEditModel.devices = resultDeviceList.Data;
            return View(productEditModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductModel productModel, IFormFile ProductPhoto)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "Ürün güncellenirken bir hata oluştu.|error";
                return View(productModel);
            }
            Product product = _productService.GetByIdKod(productModel.IdKod).Result.Data;
            if(product==null)
            {
                TempData["message"] = "Ürün bulunamadı.|error";
                return View(productModel);
            }
            Product editProduct= _mapper.Map<ProductModel, Product>(productModel);
            editProduct.Devices = productModel.Devices==null?null:string.Join(",", productModel.Devices);
            editProduct.ProductId = product.ProductId;
            if (ProductPhoto != null)
            {
                editProduct.ProductPhoto = await ImageUpload.Upload(ProductPhoto, "wwwroot\\assets\\media\\products");
            }else
            {
                editProduct.ProductPhoto = product.ProductPhoto;
            }
           
            IResult result = await _productService.Update(editProduct);
            if (result.Success)
            {
                TempData["message"] = result.Message + "|success";
            }
            else
            {
                TempData["message"] = "Bilinmeyen bir hata oluştu.|error";
                return View(productModel);
            }

            return RedirectToAction("Products", "Product", null);

           
        }

        public async Task<JsonResult> ProductDelete(string idKod)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                var product = await _productService.GetByIdKod(idKod);
                if (product.Data == null)
                {
                    alertMessage.ResponseStatus = false;
                    alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                    alertMessage.MessageType = "error";
                    return Json(alertMessage);
                }
                await _productService.Delete(product.Data);
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
        public async Task<JsonResult> ProductsDelete(string[] DeleteProducts)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                foreach (var idKod in DeleteProducts)
                {
                    var product = await _productService.GetByIdKod(idKod);
                    if (product.Data == null)
                    {
                        alertMessage.ResponseStatus = false;
                        alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                        alertMessage.MessageType = "error";
                        return Json(alertMessage);
                    }
                    await _productService.Delete(product.Data);
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
