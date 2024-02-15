using Requiem.PetProject.DTOs.Stock;
using Requiem.PetProject.Models;

namespace Requiem.PetProject.Interfaces;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync();

    Task<Stock?> GetByIdAsync(Guid id);

    Task<Stock> CreateAsync(Stock stockModel);

    Task<Stock> UpdateAsync(Guid id, UpdateStockRequestDto stockDto);

    Task<Stock> DeleteAsync(Guid id);

    Task<bool> StockExists(Guid id);
}