namespace APICrypto.DTOs.Trasaction
{
    public class CryptoSummaryDto
    {
        public string CryptoCode { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal ValueARS { get; set; }
    }
}

