using Requiem.PetProject.DTOs.Stock;
using Requiem.PetProject.Helpers;
using Requiem.PetProject.Models;

namespace Requiem.PetProject.Interfaces;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync(QueryObject query);

    Task<Stock?> GetByIdAsync(Guid id);

    Task<Stock?> GetBySymbolAsync(string symbol);

    Task<Stock> CreateAsync(Stock stockModel);

    Task<Stock> UpdateAsync(Guid id, UpdateStockRequestDto stockDto);

    Task<Stock> DeleteAsync(Guid id);

    Task<bool> StockExists(Guid id);
}