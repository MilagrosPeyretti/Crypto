namespace APICrypto.DTOs.Trasaction
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }
        public string CryptoCode { get; set; }
        public string Action { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public decimal CryptoAmount { get; set; }
        public decimal Money { get; set; } 
        public DateTime DateTime { get; set; }
    }
}