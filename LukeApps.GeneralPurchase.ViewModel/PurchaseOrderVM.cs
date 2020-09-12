using LukeApps.AccountingDocumentor.Enums;
using LukeApps.CurrencyRates.Enums;
using LukeApps.GeneralPurchase.Models;
using PhilApprovalFlow.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LukeApps.GeneralPurchase.ViewModel
{
    public class PurchaseOrderVM
    {
        public PurchaseOrderVM(PurchaseOrder agreement)
        {
            OrderNumber = agreement.PurchaseOrderNumber;
            PODate = agreement.PurchaseOrderDate.ToString("dd/MM/yyyy");

            Revision = agreement.Revision.ToString("00");
            VendorID = agreement.CompanyID;
            VendorName = agreement.Company.CompanyName;
            VendorSection = agreement.CompanySection;
            VendorContactName = agreement.Company.DefaultFocalPoint.ContactName;
            VendorReference = agreement.Company.CompanyRegistration ?? agreement.Company.CompanyNumber;

            ShippingSection = agreement.ShippingSection;

            var ProcurementContact = agreement.Originator;

            if (ProcurementContact != null)
            {
                ProcurementContactName = agreement.Originator.DisplayName;
                ProcurementContactNumber = agreement.Originator.TelephoneNumber;
            }

            ApprovedBy = agreement.Approver.DisplayName;
            var Approver = agreement.Approver;
            var ApproverTransition = agreement.Transitions.Where(p => p.ApproverDecision != DecisionType.Invalidated).Where(t => t.ApproverID == agreement.ApproverID).FirstOrDefault();
            ApprovedES = ApproverTransition == null ? null : ApproverTransition.ApproverDecision == DecisionType.Approved ? Approver?.ElectronicSignature : null;

            ApprovedPosition = agreement.Approver.JobTitle;
            InspectedBy = agreement.TechnicalEvaluatorName;

            DeliveryTerms = agreement.DeliveryTerms ?? "-";
            DeliveryDate = agreement.DeliveryDate?.ToString("dd/MM/yyyy");

            var items = agreement.PurchaseOrderItems.Where(o => o.ScopeItemType == ScopeItemType.Main || o.ScopeItemType == ScopeItemType.Additional);

            OrderTotal = agreement.Total.ToString();
            OrderTotalWords = priceToWords(agreement.Total.CurrencyCode, agreement.Total.Value);
            DeliveryTerms = agreement.DeliveryTerms;
            DeliveryDate = agreement.DeliveryDate?.ToString("dd/MM/yyyy");

            PaymentTerm = agreement.PaymentTerms.Term?.ToString("00") + " Days";
            PaymentMethod = agreement.PaymentTerms.Method;
            Entitlement = agreement.PaymentTerms.Entitlement;
            AuthorisedPersonApprovedOn = ApproverTransition == null ? null : ApproverTransition.ApproverDecision == DecisionType.Approved ? ApproverTransition.AcknowledgementDate?.ToString("dd/MM/yyyy") : null;
            ContentOwnerApprovedOn = agreement.PurchaseOrderDate.ToString("dd/MM/yyyy");

            var ContentOwnerTransition = agreement.Transitions.Where(p => p.ApproverDecision != DecisionType.Invalidated).Where(t => t.ApproverID == agreement.ReviewerID).FirstOrDefault();
            ContentOwner = agreement.Reviewer.DisplayName;
            ContentOwnerES = ContentOwnerTransition == null ? null : ContentOwnerTransition.ApproverDecision == DecisionType.Approved ? agreement.Reviewer.ElectronicSignature : null;
            ContentOwnerPosition = agreement.Reviewer.JobTitle;

            VendorPosition = "________";

            ScopeItemsCategory = agreement.Budget.BudgetName;

            IsCancelled = agreement.IsPurchaseOrderCancelled;
        }

        public string OrderNumber { get; private set; }
        public string PODate { get; private set; }

        public string Revision { get; private set; }
        public long VendorID { get; private set; }
        public string VendorName { get; private set; }
        public string VendorSection { get; private set; }
        public string VendorContactName { get; private set; }
        public string VendorReference { get; private set; }
        public string ShippingSection { get; private set; }
        public string ProcurementContactName { get; private set; }
        public string ProcurementContactNumber { get; private set; }
        public string DeliveryTerms { get; private set; }
        public string DeliveryDate { get; private set; }

        public string InspectedBy { get; private set; }
        public List<ScopeItem> OrderItems { get; private set; }
        public string OrderTotal { get; private set; }
        public string OrderTotalWords { get; private set; }
        public string PaymentTerm { get; private set; }
        public string PaymentMethod { get; private set; }
        public string Entitlement { get; private set; }

        public string ContentOwner { get; private set; }
        public string ContentOwnerPosition { get; private set; }
        public string ContentOwnerES { get; private set; }
        public string VendorPosition { get; private set; }

        public string ApprovedBy { get; private set; }
        public string ApprovedES { get; private set; }
        public string ApprovedPosition { get; private set; }
        public string Currency { get; set; }
        public string AuthorisedPersonApprovedOn { get; private set; }
        public string ContentOwnerApprovedOn { get; private set; }
        public string ScopeItemsCategory { get; private set; }
        public bool IsCancelled { get; set; }

        private string priceToWords(CurrencyCode currency, double number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + priceToWords(currency, Math.Abs(number));

            string words = "";
            string[] CurrencyProperty = getCurrencyProperty(currency).Split(',');

            int intPortion = (int)number;
            double fraction = (number - intPortion) * int.Parse(CurrencyProperty[2]);
            int decPortion = (int)fraction;

            words = numberToWords(intPortion);
            if (decPortion > 0)
            {
                words += " " + CurrencyProperty[0] + " and ";
                words += numberToWords(decPortion) + " " + CurrencyProperty[1] + ".";
            }
            else
            {
                words += " " + CurrencyProperty[0];
            }
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(words);
        }

        private string getCurrencyProperty(CurrencyCode currency)
        {
            switch (currency)
            {
                case CurrencyCode.OMR:
                    return "Omani Rial,Baisa,100";

                case CurrencyCode.AED:
                    return "UAE Dirham,Fils,10";

                case CurrencyCode.EUR:
                    return "Euros,Centime,10";

                case CurrencyCode.USD:
                    return "US Dollar,Cent,10";

                case CurrencyCode.GBP:
                    return "Britsh Pound,Pence,10";

                default:
                    return ",,0";
            }
        }

        private string numberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + numberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += numberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += numberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += numberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
    }
}