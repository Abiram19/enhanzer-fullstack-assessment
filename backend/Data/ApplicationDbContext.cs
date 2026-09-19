using Microsoft.EntityFrameworkCore;
using EnhanzerAssessment.Api.Models;

namespace EnhanzerAssessment.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LocationDetail> Location_Details { get; set; }
        public DbSet<PurchaseBill> PurchaseBills { get; set; }
        public DbSet<PurchaseBillItem> PurchaseBillItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LocationDetail>()
                .HasKey(ld => new { ld.Company_Code, ld.Location_Code });
        }
    }
}
