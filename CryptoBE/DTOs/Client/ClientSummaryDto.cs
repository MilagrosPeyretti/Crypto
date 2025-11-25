using APICrypto.DTOs.Trasaction;    
namespace APICrypto.DTOs.Client
{
    public class ClientSummaryDto
    {
        public List<CryptoSummaryDto> Cryptos { get; set; } = new();
        public decimal TotalARS { get; set; }
    }
}
