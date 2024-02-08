using Requiem.PetProject.DTOs.Stock;
using Requiem.PetProject.Models;

namespace Requiem.PetProject.Interfaces;

public interface IStockRepository
{
    public Task<List<Stock>> GetAllAsync();

    public Task<Stock?> GetByIdAsync(Guid id);

    public Task<Stock> CreateAsync(Stock stockModel);

    public Task<Stock> UpdateAsync(Guid id, UpdateStockRequestDto stockDto);

    public Task<Stock> DeleteAsync(Guid id);
}