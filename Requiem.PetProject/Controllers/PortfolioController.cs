using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Requiem.PetProject.Extensions;
using Requiem.PetProject.Interfaces;
using Requiem.PetProject.Models;

namespace Requiem.PetProject.Controllers;

[Route("api/controller")]
[ApiController]
public class PortfolioController : ControllerBase
{
    private readonly IStockRepository _stockRepository;
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly UserManager<AppUser> _userManager;
    
    public PortfolioController(UserManager<AppUser> userManager, 
        IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
    {
        _userManager = userManager;
        _stockRepository = stockRepository;
        _portfolioRepository = portfolioRepository;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolio()
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        
        var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

        return Ok(userPortfolio);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreatePortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);

        var stock = await _stockRepository.GetBySymbolAsync(symbol);
        if (stock == null)
            return BadRequest("Stock not found");

        var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

        if (userPortfolio.Any(s => s.Symbol.ToLower() == symbol.ToLower()))
            return BadRequest("Cannot add same stock to portfolio");

        var portfolioModel = new Portfolio
        {
            StockId = stock.Id,
            AppUserId = appUser.Id
        };

        await _portfolioRepository.CreateAsync(portfolioModel);

        if (portfolioModel == null)
            return StatusCode(500, "Could not create");
        else 
            return Created();
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);

        var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

        var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower());

        if (filteredStock.Count() == 1)
            await _portfolioRepository.DeletePortfolio(appUser, symbol);
        else
            return BadRequest("Stock not in your portfolio");

        return Ok();
    }
}