using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeApps.GeneralPurchase.ViewModel
{
    public class CommitmentLineVM
    {
        public string CompanyName { get; set; }
        public string PurchaseOrders { get; set; }
        public string InvoiceReceivedTotal { get; set; }

        public string InvoiceApprovedTotal { get; set; }
        public string InvoicePaidTotal { get; set; }

        public string TotalValue { get; set; }
        public string InvoiceTotalAmount { get; set; }
        public string InvoiceTotalAmountPaid { get; set; }
        public string TotalBalance { get; set; }
    }
}
