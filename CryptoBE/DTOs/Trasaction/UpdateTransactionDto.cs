using System.ComponentModel.DataAnnotations;

namespace APICrypto.DTOs.Trasaction
{
    public class UpdateTransactionDto
    {
        public string? Exchange { get; set; }
        public string? CryptoCode { get; set; }
        public string? Action { get; set; }
        public decimal? CryptoAmount { get; set; }
        public decimal? Money { get; set; } 
        public DateTime? DateTime { get; set; }
    }
}