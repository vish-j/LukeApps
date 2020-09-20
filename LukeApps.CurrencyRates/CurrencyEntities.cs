using LukeApps.CurrencyRates.Models;
using LukeApps.TrackingExtended;
using System.Data.Entity;
using TrackerEnabledDbContext.Common.Extensions;

namespace LukeApps.CurrencyRates.DAL
{
    public class CurrencyEntities : ExtendedEntities
    {
        public CurrencyEntities(string username)
            : base(username, "name=CurrencyEntities")
        {
            Database.SetInitializer(new CurrencyInitializer());
        }

        public CurrencyEntities()
            : base("", "name=CurrencyEntities")
        {
            Database.SetInitializer(new CurrencyInitializer());
        }

        public virtual DbSet<Currency> Currencies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var CurrencyEntity = modelBuilder.Entity<Currency>();
            CurrencyEntity.TrackAllProperties();
            CurrencyEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);
        }
    }
}