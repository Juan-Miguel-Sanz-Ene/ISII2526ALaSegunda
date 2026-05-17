using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.Web.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        Name = "Name" + DateTime.Now.ToString();
        Surname = "Surname";
        Address = "Address";
        AccountCreationDate = DateTime.Now;
    }
    

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
}

