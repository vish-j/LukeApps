using LukeApps.CurrencyRates.DAL;
using System.Linq;

namespace LukeApps.CurrencyRates
{
    public class CurrencyProvider : CurrencyHandler
    {
        private static readonly CurrencyProvider _instance =
          new CurrencyProvider();

        private CurrencyProvider()
        {
            using (CurrencyEntities db = new CurrencyEntities())
            {
                // Load list of Currencies
                SetCurrency(db.Currencies.ToList());
            }
        }

        public static CurrencyProvider GetCurrencyProvider()
        {
            return _instance;
        }
    }
}