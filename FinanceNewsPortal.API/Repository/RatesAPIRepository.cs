using FinanceNewsPortal.API.Models;
using FinanceNewsPortal.API.Repository.Contracts;
using System.Text.Json;

namespace FinanceNewsPortal.API.Repository
{
    public class RatesAPIRepository : IRatesRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public RatesAPIRepository(HttpClient httpClient, IConfiguration configuration)
        {
            this._httpClient = httpClient;
            this._configuration = configuration;
        }

        public async Task<Currency?> GetCurrencyExchangeRates(string baseCurrencyType)
        {
            Currency? currency;

            try
            {
                string path = $"https://anyapi.io/api/v1/exchange/rates?base={baseCurrencyType}&apiKey={this._configuration.GetConnectionString("ANYAPI_KEY")}";

                var responseMessage = await this._httpClient.GetAsync(path);

                var contentStream = await responseMessage.Content.ReadAsStreamAsync();

                currency = await JsonSerializer.DeserializeAsync<Currency>(contentStream);
            }
            catch(Exception e)
            {
                currency = null;
            }

            return currency;
        }

        public async Task<Stock> GetStockExchangeRates()
        {
            Stock? stock;

            try 
            {
                string path = $"https://api.polygon.io/v2/aggs/grouped/locale/us/market/stocks/2023-01-04?adjusted=false&apiKey={this._configuration.GetConnectionString("POLYGONAPI_KEY")}";

                var responseMessage = await this._httpClient.GetAsync(path);

                var contentStream = await responseMessage.Content.ReadAsStreamAsync();

                stock = await JsonSerializer.DeserializeAsync<Stock>(contentStream);
            }
            catch(Exception e)
            {
                stock = null;
            }

            return stock;
        }
    }
}