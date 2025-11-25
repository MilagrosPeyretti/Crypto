namespace APICrypto.Services
{
    public interface ICryptoYaService
    {
        Task<decimal?> GetPriceAsync(string exchange, string cryptoCode);
        Task<decimal?> GetPriceSumary(string cryptoCode);
    }
}
