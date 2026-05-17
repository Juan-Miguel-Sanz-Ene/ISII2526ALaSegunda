using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class

[Index(nameof(Name), IsUnique = true)]

public class ApplicationUser : IdentityUser {
    [PersonalData]
    [StringLength(50)]
    public string Name { get; set; }

    [PersonalData]
    [StringLength(50)]
    public string Surname { get; set; }

    [PersonalData]
    [StringLength(200)]
    public string Address { get; set; }

    public DateTime AccountCreationDate { get; set; }
    public IList<PurchaseOrder>? PurchaseOrders { get; set; }
    
    public List<Complaint> Complaints { get; set; }

    public List<ReportCustomer> ReportCustomers { get; set; } = new();


    public List<ReturnPurchaseOrder> ReturnOrders { get; set; }
}