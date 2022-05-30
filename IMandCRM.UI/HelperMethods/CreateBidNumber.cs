using Business.Abstract;
using Core.Utilities.Result;
using Entities.Concrete;
using IMandCRM.UI.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.HelperMethods
{
    public class CreateBidNumber
    {
        private IBidService _bidService;
        private IAppSettingService _appSettingService;
        public CreateBidNumber(IAppSettingService appSettingService, IBidService bidService)
        {
            _appSettingService = appSettingService;
            _bidService = bidService;
        }
        public async Task<string> GetBidNumber()
        {
            int year = DateTime.Now.Year;
            DateTime startDate = new DateTime(year, 1, 1);
            DateTime endDate = new DateTime(year, 12, 31);
            IDataResult<List<Bid>> bidList = await _bidService.GetList();
            List<Bid> filterBidList = bidList.Data.Where(x => x.BidStatus != (int)Enums.BidStatus.Draft && x.BidStatus != (int)Enums.BidStatus.PendingInternalApproval&&x.BidDate>= startDate &&x.BidDate<=endDate).ToList();
            IDataResult<List<AppSetting>> appSettingListResult = await _appSettingService.GetList();
            var result = appSettingListResult.Data.FirstOrDefault();
            if(result==null)
            {
                return "";
            }
            string bidNumber = result.BidCode+DateTime.Now.ToString("yy")+"10"+ Convert.ToString(50 + filterBidList.Count);
            return bidNumber;
        }
    }
}
