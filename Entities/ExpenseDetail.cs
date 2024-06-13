using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Splitwise.Entities
{
    public class ExpenseDetail
    {
        public int Id { get; set; }
        [ForeignKey("ExpenseId")]
        public int ExpenseId { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public String Description { get; set; }
        public DateTime CreatedOn = DateTime.Now;
        
    }
}
