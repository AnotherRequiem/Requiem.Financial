using Requiem.PetProject.Models;

namespace Requiem.PetProject.Interfaces;

public interface IFmpService
{
    Task<Stock> FindStockSymbolAsync(string symbol);
}