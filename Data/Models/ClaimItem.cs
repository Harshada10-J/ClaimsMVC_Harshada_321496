using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimsMVC.Data.Models
{
    public class ClaimItem
    {
        [Key]
        public int ClaimItemId { get; set; }
        public int ClaimId { get; set; }
        public required string ItemName { get; set; }
        public decimal Cost { get; set; }

        [ForeignKey("ClaimId")]
        public virtual Claim? Claim { get; set; }
    }
}
