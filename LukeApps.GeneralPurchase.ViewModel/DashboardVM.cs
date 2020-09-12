using LukeApps.AlertHandling;
using LukeApps.GeneralPurchase.Models;
using System.Collections.Generic;

namespace LukeApps.GeneralPurchase.ViewModel
{
    public class DashboardVM
    {
        public DashboardVM()
        {
            MRAlerts = new List<AlertNotification>();
            ExpiringPurchaseOrders = new List<PurchaseOrder>();
            ExpiringInvoices = new List<Invoice>();
        }

        public IEnumerable<AlertNotification> MRAlerts { get; set; }
        public int TotalExpiringPurchaseOrders { get;  set; }
        public int TotalExpiringInvoices { get;  set; }
        public List<PurchaseOrder> ExpiringPurchaseOrders { get;  set; }
        public List<Invoice> ExpiringInvoices { get;  set; }
    }
}