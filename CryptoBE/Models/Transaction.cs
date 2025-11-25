using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICrypto.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string CryptoCode { get; set; }

        [Required]
        [StringLength(10)]
        public string Action { get; set; }

        [Required]
        [Range(0.00000001, double.MaxValue)]
        [Column(TypeName = "decimal(18,8)")]
        public decimal CryptoAmount { get; set; } 

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Money { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [ForeignKey("ClientId")]
        public int ClientId { get; set; } 
        public Client? Client { get; set; }
    }
}
