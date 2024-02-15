﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Requiem.PetProject.Data;
using Requiem.PetProject.DTOs.Stock;
using Requiem.PetProject.Interfaces;
using Requiem.PetProject.Mappers;

namespace Requiem.PetProject.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IStockRepository _stockRepository;
    
    public StockController(ApplicationDbContext context, IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await _stockRepository.GetAllAsync();
        var stockDto = stocks.Select(s => s.ToStockDto());

        return Ok(stockDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var stock = await _stockRepository.GetByIdAsync(id);

        if (stock == null)
        {
            return NotFound();
        }
        
        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto createDto)
    {
        var stockModel = createDto.ToStockFromCreateDto();
        await _stockRepository.CreateAsync(stockModel);
        
        return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStockRequestDto updateDto)
    {
        var stockModel = await _stockRepository.UpdateAsync(id, updateDto);

        if (stockModel == null)
        {
            return NotFound();
        }

        return Ok(stockModel.ToStockDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var stockModel = await _stockRepository.DeleteAsync(id);

        if (stockModel == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}