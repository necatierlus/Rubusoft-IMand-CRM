using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Constants
{
    public class Enums
    {
        public enum BidStatus
        {
            Draft = 1, //Taslak
            PendingInternalApproval = 2, //İç Onay Bekleyen
            InternallyApproved = 3, //İç Onaylı
            Sent = 4, //Gönderilen
            CustomerApproved = 5, //Müşteri Onaylı
            Rejected = 6, //Reddedilmiş
            RequestedRevision = 7, //Revize Talep Edilen
            Revised = 8, //Revize Edilen
            Expired = 9 //Süresi Biten
        }

        public enum RequestStatus
        {
            Application = 1, //Başvuru
            Call = 2, //Arama
            Bid = 3, //Teklif
            Sales = 4, //Satış
            Cancel = 5, //İptal
        }
    }
}
