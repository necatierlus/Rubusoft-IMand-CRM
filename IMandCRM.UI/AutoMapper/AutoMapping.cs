using AutoMapper;
using Entities.Concrete;
using IMandCRM.UI.Models;
using IMandCRM.UI.Models.CustomerRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.AutoMapper
{
    public class AutoMapping:Profile
    {
        public AutoMapping()
        {
            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel,Product>();

            CreateMap<Device, DeviceModel>();
            CreateMap<DeviceModel, Device>();

            CreateMap<Models.FirmAddModel, Firm>();
            CreateMap<Models.FirmAddModel, FirmManager>();
            CreateMap<Models.FirmAddModel, Address>();

            CreateMap<Firm, FirmModel>();
            CreateMap<FirmModel,Firm>();

            CreateMap<FirmManagerModel, FirmManager>();
            CreateMap<FirmAddressModel, Address>();

            CreateMap<BidAddModel, Bid>();

            CreateMap<Bid, BidEditModel>();
            CreateMap<BidEditModel,Bid>();

            CreateMap<GeneralRequirementModel, GeneralRequirement>();
            CreateMap<GeneralRequirement, GeneralRequirementModel>();

            CreateMap<AppSettingEditModel, AppSetting>();
            CreateMap<AppSetting, AppSettingEditModel>();          
            
            CreateMap<CustomerRequestModel, CustomerRequest>();
            CreateMap<CustomerRequest, CustomerRequestModel>();

            CreateMap<CreateBidModel, Bid>();
            CreateMap<Bid,CreateBidModel>();

            CreateMap<TechSupport, TechSupportModel>().ReverseMap();

            CreateMap<TechSupport, TechSupportListModel>().ReverseMap();

            CreateMap<TechSupportDetail, TechSupportDetailModel>().ReverseMap();

            CreateMap<StockPoint, StockPointModel>().ReverseMap();

            CreateMap<DeviceStock, DeviceStockModel>().ReverseMap();

            CreateMap<ProductStock, ProductStockModel>().ReverseMap();


        }
    }
}
