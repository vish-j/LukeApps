using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LukeApps.GeneralPurchase.Classes
{
    public class PaymentTerms
    {
        public PaymentTerms()
        {
        }

        public PaymentTerms(PaymentTerms paymentTerms)
        {
            Term = paymentTerms.Term;
            Method = paymentTerms.Method;
            Entitlement = paymentTerms.Entitlement;
        }

        [Display(Name = "Term (Days)")]
        public int? Term { get; set; }

        public string Method { get; set; }

        [Display(Name = "Entitlement")]
        [DataType(DataType.MultilineText)]
        [Column(TypeName = "ntext")]
        public string Entitlement { get; set; }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (Term == null)
            {
                sb.Append("-");
            }
            else
            {
                sb.Append("Within ");
                sb.Append(Term);
                sb.Append(" days,");

                if (!(Method == null || Method == string.Empty))
                {
                    sb.Append(" via ");
                    sb.Append(Method);
                    sb.Append(".");
                }

                if (!(Entitlement == null || Entitlement == string.Empty))
                {
                    sb.Append("\n\nEntitlement\n");
                    sb.Append(Entitlement);
                }
            }

            return sb.ToString();
        }
    }
}