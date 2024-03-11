using Microsoft.EntityFrameworkCore;
using Requiem.PetProject.Data;
using Requiem.PetProject.DTOs.Stock;
using Requiem.PetProject.Helpers;
using Requiem.PetProject.Interfaces;
using Requiem.PetProject.Models;

namespace Requiem.PetProject.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _context;
    
    public StockRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Stock>> GetAllAsync(StockQueryObject stockQuery)
    {
        var stocks = _context.Stocks.Include(s => s.Comments).ThenInclude(c => c.AppUser).AsQueryable();

        if (!string.IsNullOrWhiteSpace(stockQuery.CompanyName))
            stocks = stocks.Where(s => s.CompanyName.Contains(stockQuery.CompanyName));
        
        if (!string.IsNullOrWhiteSpace(stockQuery.Symbol))
            stocks = stocks.Where(s => s.Symbol.Contains(stockQuery.Symbol));

        if (!string.IsNullOrWhiteSpace(stockQuery.SortBy))
        {
            if (stockQuery.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                stocks = stockQuery.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
            
            if (stockQuery.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                stocks = stockQuery.IsDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
        }

        var skipNumber = (stockQuery.PageNumber - 1) * stockQuery.PageSize;
        
        return await stocks.Skip(skipNumber).Take(stockQuery.PageSize).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(Guid id)
    {
        return await _context.Stocks.Include(c => c.Comments).ThenInclude(c => c.AppUser).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Stock?> GetBySymbolAsync(string symbol)
    {
        return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
    }

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();

        return stockModel;
    }

    public async Task<Stock> UpdateAsync(Guid id, UpdateStockRequestDto updateStockDto)
    {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
        
        if (stockModel == null) 
            return null;
        
        stockModel.Symbol = updateStockDto.Symbol;
        stockModel.CompanyName = updateStockDto.CompanyName;
        stockModel.Purchase = updateStockDto.Purchase;
        stockModel.LastDiv = updateStockDto.LastDiv;
        stockModel.Industry = updateStockDto.Industry;
        stockModel.MarketCap = updateStockDto.MarketCap;

        await _context.SaveChangesAsync();

        return stockModel;
    }

    public async Task<Stock> DeleteAsync(Guid id)
    {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

        if (stockModel == null) 
            return null;
        
        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync();

        return stockModel;
    }

    public Task<bool> StockExists(Guid id)
    {
        return _context.Stocks.AnyAsync(s => s.Id == id);
    }
}