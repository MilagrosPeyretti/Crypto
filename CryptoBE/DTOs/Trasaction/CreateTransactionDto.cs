using System.ComponentModel.DataAnnotations;

namespace APICrypto.DTOs.Trasaction
{
    public class CreateTransactionDto
    {
        [Required]
        [StringLength(20)]
        public string Exchange { get; set; } = null!;
        [Required]
        public string CryptoCode { get; set; } 

        [Required]
        
        [RegularExpression("^(purchase|sale)$", ErrorMessage = "La acción debe ser 'purchase' o 'sale'.")]
        public string Action { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        [Range(0.00000001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public decimal CryptoAmount { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
    }
}