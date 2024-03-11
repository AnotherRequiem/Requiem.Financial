using Newtonsoft.Json;
using Requiem.PetProject.DTOs.Stock;
using Requiem.PetProject.Interfaces;
using Requiem.PetProject.Mappers;
using Requiem.PetProject.Models;

namespace Requiem.PetProject.Services;

public class FmpService : IFmpService
{
    private HttpClient _httpClient;
    private IConfiguration _configuration;
    
    
    public FmpService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    
    public async Task<Stock> FindStockSymbolAsync(string symbol)
    {
        try
        {
            var result = await _httpClient.GetAsync(
                $"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_configuration["FMPKey"]}");
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<FmpStock[]>(content);
                var stock = tasks[0];
                
                if (stock != null)
                    return stock.ToStockFromFmp();
                
                return null;
            }

            return null;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return null;
        }
    }
}