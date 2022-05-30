using Core.DataAccess.EfEntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class BidDal : EntityRepository<Bid, IMandCRMContext>, IBidDal
    {
        public async Task<List<BidListDto>> GetListByBidStatus(int bidStatus)
        {
            using (IMandCRMContext ctx = new IMandCRMContext())
            {
                var query = (from bid in ctx.Bids.Where(x => x.BidStatus == bidStatus && x.IsDelete == false)
                             join firm in ctx.Firms on bid.FirmIdKod equals firm.IdKod
                             join firmManager in ctx.FirmManagers on bid.FirmManagerIdKod equals firmManager.IdKod
                             join user in ctx.AspNetUsers on bid.CreatedPersonalId equals user.Id
                             select new BidListDto
                             {
                                 IdKod = bid.IdKod,
                                 FirmName = firm.FirmName,
                                 FirmManagerFullName = firmManager.FirstName + " " + firmManager.LastName,
                                 FirmManagerEmail = firmManager.EMail,
                                 BidDate = bid.BidDate,
                                 BidValidityDate = bid.BidValidityDate,
                                 CreatedPesonalFullName = user.FirstName + " " + user.LastName,
                                 BidNumber = bid.BidNumber,
                                 BidStatus = bid.BidStatus,
                                 ApprovalDate = bid.ApprovalDate,
                                 CancelledDate = bid.CancelledDate,
                                 GeneralRequirements = bid.GeneralRequirements,
                                 TotalPrice = bid.TotalPrice,
                                 TotalDiscount = bid.TotalDiscount,
                                 GeneralTotal = bid.GeneralTotal
                             });
                var list = await query.ToListAsync();
                return list;
            }
        }

        public async Task<List<BidListDto>> GetListByExpiredBids(DateTime today)
        {
            using (IMandCRMContext ctx = new IMandCRMContext())
            {
                var query = (from bid in ctx.Bids.Where(x => x.BidValidityDate < today && x.IsDelete == false)
                             join firm in ctx.Firms on bid.FirmIdKod equals firm.IdKod
                             join firmManager in ctx.FirmManagers on bid.FirmManagerIdKod equals firmManager.IdKod
                             join user in ctx.AspNetUsers on bid.CreatedPersonalId equals user.Id
                             select new BidListDto
                             {
                                 IdKod=bid.IdKod,
                                 FirmName = firm.FirmName,
                                 FirmManagerFullName = firmManager.FirstName + " " + firmManager.LastName,
                                 FirmManagerEmail = firmManager.EMail,
                                 BidDate = bid.BidDate,
                                 BidValidityDate = bid.BidValidityDate,
                                 CreatedPesonalFullName = user.FirstName + " " + user.LastName,
                                 BidNumber = bid.BidNumber,
                                 BidStatus = bid.BidStatus,
                                 ApprovalDate = bid.ApprovalDate,
                                 CancelledDate = bid.CancelledDate,
                                 GeneralRequirements = bid.GeneralRequirements,
                                 TotalPrice = bid.TotalPrice,
                                 TotalDiscount = bid.TotalDiscount,
                                 GeneralTotal = bid.GeneralTotal
                             });
                var list = await query.ToListAsync();
                return list;
            }
        }
    }
}

