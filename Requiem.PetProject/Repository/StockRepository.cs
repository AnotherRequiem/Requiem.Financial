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

    public async Task<List<Stock>> GetAllAsync(QueryObject query)
    {
        var stocks = _context.Stocks.Include(c => c.Comments).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.CompanyName))
            stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
        
        if (!string.IsNullOrWhiteSpace(query.Symbol))
            stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
            
            if (query.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                stocks = query.IsDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        
        return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(Guid id)
    {
        return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(s => s.Id == id);
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