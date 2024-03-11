using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Requiem.PetProject.DTOs.Stock;
using Requiem.PetProject.Helpers;
using Requiem.PetProject.Interfaces;
using Requiem.PetProject.Mappers;

namespace Requiem.PetProject.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IStockRepository _stockRepository;
    
    public StockController(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] StockQueryObject stockQuery)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        var stocks = await _stockRepository.GetAllAsync(stockQuery);
        
        var stockDto = stocks.Select(s => s.ToStockDto()).ToList();

        return Ok(stockDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        var stock = await _stockRepository.GetByIdAsync(id);

        if (stock == null)
            return NotFound();
        
        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto createDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        var stockModel = createDto.ToStockFromCreateDto();
        await _stockRepository.CreateAsync(stockModel);
        
        return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStockRequestDto updateDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        var stockModel = await _stockRepository.UpdateAsync(id, updateDto);

        if (stockModel == null)
            return NotFound();
        
        return Ok(stockModel.ToStockDto());
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        var stockModel = await _stockRepository.DeleteAsync(id);

        if (stockModel == null)
            return NotFound();

        return NoContent();
    }
}