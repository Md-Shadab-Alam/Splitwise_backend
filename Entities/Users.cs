using System.ComponentModel.DataAnnotations;
namespace Splitwise.Entities
{
    public class Users 
    {
        [Key]
        public int UsersId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

    }
}
