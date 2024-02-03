﻿using Microsoft.AspNetCore.Mvc;
using Requiem.PetProject.Data;
using Requiem.PetProject.Mappers;

namespace Requiem.PetProject.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public StockController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var stocks = _context.Stocks.ToList()
            .Select(s => s.ToStockDto());

        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] Guid id)
    {
        var stock = _context.Stocks.Find(id);

        if (stock == null)
        {
            return NotFound();
        }
        
        return Ok(stock.ToStockDto());
    }
    
}