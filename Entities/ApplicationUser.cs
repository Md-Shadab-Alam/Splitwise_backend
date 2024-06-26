using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Splitwise.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Key]
        public string Name {  get; set; }
    }
}
