using LukeApps.AlertHandling.Models;
using System.Data.Entity;

namespace LukeApps.AlertHandling.DAL
{
    internal partial class AlertEntities : DbContext
    {
        public AlertEntities()
            : base("name=AlertsEntities")
        {
        }

        public virtual DbSet<Alert> Alerts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}