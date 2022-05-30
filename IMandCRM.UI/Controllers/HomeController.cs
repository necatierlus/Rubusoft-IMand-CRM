using Business.Abstract;
using Core.Utilities.Result;
using Entities.Dtos;
using IMandCRM.UI.Constants;
using IMandCRM.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IBidService _bidService;
        public HomeController(IBidService bidService)
        {
            _bidService = bidService;
        }
        public async Task<IActionResult> Index()
        {
            IDataResult<List<BidListDto>> bidListResult = await _bidService.GetListByBidStatus((int)Enums.BidStatus.CustomerApproved);
            List<BidListDto> bids = bidListResult.Data.OrderBy(x=>x.ApprovalDate).Take(10).ToList();
            HomeModel homeModel = new HomeModel();
            homeModel.bids = bids;
            return View(homeModel);
        }

    }
}
