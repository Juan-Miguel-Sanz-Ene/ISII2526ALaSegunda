using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<ReturnPurchaseOrder> Returns { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<ReturnProduct> ReturnProducts { get; set; }

    public DbSet<DeliveryAssignment> DeliveryAssignments { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseProduct> PurchaseProducts { get; set; }
    public DbSet<PurchaseDelivery> PurchaseDeliveries { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<CreditCard> CreditCards { get; set; }
    public DbSet<PayPal> PayPals { get; set; }
    public DbSet<Bizum> Bizums { get; set; }
    public DbSet<DeliveryDriver> DeliveryDrivers { get; set; }
    public DbSet<BanReport> BanReports { get; set; }
    public DbSet<Product> Products { get; set; }

    public DbSet<Complaint> Complaints { get; set; }

    public DbSet<ApplicationUser> ApplicationUsers{ get; set;}
    public DbSet<ComplaintType> ComplaintTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.Entity<ReturnProduct>()
            .HasOne(rp => rp.PurchaseProduct)
            .WithOne(pp => pp.ReturnProduct)
            .HasForeignKey<ReturnProduct>(rp => new { rp.PurchaseOrderId, rp.ProductId })
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<PurchaseOrder>()
            .HasOne(po => po.PaymentMethod)
            .WithMany(pm => pm.PurchaseOrders)       
            .HasForeignKey(po => po.PaymentMethodId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Complaint>()
            .HasOne(c => c.BanReport)
            .WithMany(br => br.Complaints)
            .HasForeignKey(c => c.BanReportId)
             .OnDelete(DeleteBehavior.SetNull);


        builder.Entity<ReportCustomer>(entity =>

        {  
            entity.HasKey(rc => new { rc.BanReportId, rc.CustomerId });
            
            entity.HasOne(rc => rc.BanReport)
                .WithMany(br => br.ReportCustomers)
                .HasForeignKey(rc => rc.BanReportId)
                .OnDelete(DeleteBehavior.NoAction);
            

            entity.HasOne(rc => rc.Customer)
                .WithMany(u => u.ReportCustomers)
                .HasForeignKey(rc => rc.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

        }
        );
            
    }



    public DbSet<ReportCustomer> ReportCustomers { get; set; }
}
