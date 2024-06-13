using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Splitwise.Entities
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        // public string Name { get; set; }

        public GroupDetail GroupDetails { get; set; }
        public ICollection<Users> Users { get; set; }
        //public string Description { get; set; }
        //public string? Category { get; set; }
    }
}
