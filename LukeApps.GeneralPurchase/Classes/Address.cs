using LukeApps.GeneralPurchase.Enums;
using LukeApps.Common.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LukeApps.GeneralPurchase.Classes
{
    public class Address
    {
        [Display(Name = "PO Box")]
        public int? POBox { get; set; }

        [Required]
        [Display(Name = "Address Line 1")]
        [DataType(DataType.MultilineText)]
        [Column(TypeName = "ntext")]
        public string Line1 { get; set; }

        [Display(Name = "Address Line 2")]
        [DataType(DataType.MultilineText)]
        [Column(TypeName = "ntext")]
        public string Line2 { get; set; }

        public string Street { get; set; }

        [Display(Name = "Postal Code")]
        public int? PostalCode { get; set; }

        [Required]
        public string City { get; set; }

        public Country Country { get; set; }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(Line1);

            if (POBox != null)
            {
                sb.Append(",\nPOBox: ");
                sb.Append(POBox);
            }

            if (Line2 != string.Empty && Line2 != null)
            {
                sb.Append(",\n");
                sb.Append(Line2);
            }

            if (Street != string.Empty && Street != null)
            {
                sb.Append(",\n");
                sb.Append(Street);
            }
            sb.Append(",\n");
            sb.Append(City);

            if (PostalCode != null)
            {
                sb.Append(", PC: ");
                sb.Append(PostalCode);
            }

            sb.Append(",\n");
            sb.Append(Country.GetDisplay());

            return sb.ToString();
        }
    }
}