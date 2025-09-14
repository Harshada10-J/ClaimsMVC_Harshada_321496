using System.ComponentModel.DataAnnotations;

namespace ClaimsMVC.Data.Models
{
    public class Designation
    {
        [Key]
        public int DesignationId { get; set; }
        public required string LevelName { get; set; }
        public decimal ClaimLimit { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
