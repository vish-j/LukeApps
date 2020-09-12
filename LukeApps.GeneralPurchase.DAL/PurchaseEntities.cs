using LukeApps.GeneralPurchase.Models;
using LukeApps.TrackingExtended;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TrackerEnabledDbContext.Common.Extensions;

namespace LukeApps.GeneralPurchase.DAL
{
    public class PurchaseEntities : ExtendedEntities
    {
        public PurchaseEntities(string username)
       : base(username, "PurchaseEntities")
        {
        }

        public PurchaseEntities()
            : base("", "PurchaseEntities")
        {
        }

        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<Budget> Budgets { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyFocalPoint> CompanyFocalPoints { get; set; }
        public virtual DbSet<Enquiry> Enquiries { get; set; }

        public virtual DbSet<ExpenseClaim> ExpenseClaims { get; set; }

        public virtual DbSet<ExpenseClaimItem> ExpenseClaimItem { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItemss { get; set; }

        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<ScopeItem> ScopeItems { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            var BankAccountEntity = modelBuilder.Entity<BankAccount>();
            BankAccountEntity.TrackAllProperties();
            BankAccountEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);

            var BudgetEntity = modelBuilder.Entity<Budget>();
            BudgetEntity.TrackAllProperties();
            BudgetEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);

            var CompanyEntity = modelBuilder.Entity<Company>();
            CompanyEntity.TrackAllProperties();
            CompanyEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);

            var CompanyFocalPointEntity = modelBuilder.Entity<CompanyFocalPoint>();
            CompanyFocalPointEntity.TrackAllProperties();
            CompanyFocalPointEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);

            var EnquiryEntity = modelBuilder.Entity<Enquiry>();
            EnquiryEntity.TrackAllProperties();
            EnquiryEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);

            var EnquiryTransitionEntity = modelBuilder.Entity<EnquiryTransition>();
            EnquiryTransitionEntity.HasKey(f => f.TransitionID);
            EnquiryTransitionEntity.Property(f => f.TransitionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            EnquiryTransitionEntity.TrackAllProperties();
            EnquiryTransitionEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);

            var OfferEntity = modelBuilder.Entity<Offer>();
            OfferEntity.HasOptional(p => p.PurchaseOrder).WithOptionalDependent(p => p.Offer);
            OfferEntity.TrackAllProperties();
            OfferEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);

            var ScopeItemEntity = modelBuilder.Entity<ScopeItem>();
            ScopeItemEntity.TrackAllProperties();
            ScopeItemEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);

            var PurchaseOrderEntity = modelBuilder.Entity<PurchaseOrder>();
            PurchaseOrderEntity.TrackAllProperties();
            PurchaseOrderEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                     .Ignore(m => m.IsDeleted);

            var PurchaseOrderItemEntity = modelBuilder.Entity<PurchaseOrderItem>();
            PurchaseOrderItemEntity.TrackAllProperties();
            PurchaseOrderItemEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);

            var PurchaseOrderTransitionEntity = modelBuilder.Entity<PurchaseOrderTransition>();
            PurchaseOrderTransitionEntity.HasKey(f => f.TransitionID);
            PurchaseOrderTransitionEntity.Property(f => f.TransitionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            PurchaseOrderTransitionEntity.TrackAllProperties();
            PurchaseOrderTransitionEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);

            var ExpenseClaimEntity = modelBuilder.Entity<ExpenseClaim>();
            ExpenseClaimEntity.TrackAllProperties();
            ExpenseClaimEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                     .Ignore(m => m.IsDeleted);

            var ExpenseClaimItemEntity = modelBuilder.Entity<ExpenseClaimItem>();
            ExpenseClaimItemEntity.TrackAllProperties();
            ExpenseClaimItemEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);

            var ExpenseClaimTransitionEntity = modelBuilder.Entity<ExpenseClaimTransition>();
            ExpenseClaimTransitionEntity.HasKey(f => f.TransitionID);
            ExpenseClaimTransitionEntity.Property(f => f.TransitionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ExpenseClaimTransitionEntity.TrackAllProperties();
            ExpenseClaimTransitionEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);

            var InvoiceEntity = modelBuilder.Entity<Invoice>();
            InvoiceEntity.TrackAllProperties();
            InvoiceEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                     .Ignore(m => m.IsDeleted);

            var InvoiceItemEntity = modelBuilder.Entity<InvoiceItem>();
            InvoiceItemEntity.TrackAllProperties();
            InvoiceItemEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);

            var InvoiceTransitionEntity = modelBuilder.Entity<InvoiceTransition>();
            InvoiceTransitionEntity.HasKey(f => f.TransitionID);
            InvoiceTransitionEntity.Property(f => f.TransitionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            InvoiceTransitionEntity.TrackAllProperties();
            InvoiceTransitionEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);
        }
    }
}