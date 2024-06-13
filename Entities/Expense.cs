using System.ComponentModel.DataAnnotations.Schema;

namespace Splitwise.Entities
{
    public class Expense 
    {
        public int ExpenseId { get; set; }

       // public bool IsSettle = false;
       // public decimal Amount { get; set; }
        public int GroupId { get; set; }
    //    [ForeignKey("GroupId")]
     //   public Group Group { get; set; }
    //    public int UsersId { get; set; }

        public ICollection<Users> UsersPaid { get; set; }
        public ICollection<Users> UsersInvolved { get; set; }
        public ExpenseDetail ExpenseDetails { get; set; }
       // public Users Users { get; set; }
       // public string? Title { get; set; }
       // public DateTime TimeStamp = DateTime.UtcNow;
    }
}
