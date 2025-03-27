using JurassicPark.Core.Services.Interfaces;
using JurassicPark.Core.WebAPI.Dto;
using JurassicPark.Core.WebAPI.Functional;
using Microsoft.AspNetCore.Mvc;

namespace JurassicPark.Core.WebAPI.Controllers;

[ApiController]
[Route("games/{gameId:long}/transactions")]
public class TransactionController(IGameService gameService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTransactions(long gameId)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        
        var transactions = await gameService.GetTransactionsFromLastCheckpoint(game.GetValueOrThrow());
        return Ok(transactions.Select(t => new TransactionDto(t)));
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllTransactions(long gameId)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        
        var transactions = await gameService.GetAllTransactions(game.GetValueOrThrow());
        return Ok(transactions.Select(t => new TransactionDto(t)));
    }

    [HttpPost("create-checkpoint")]
    public async Task<IActionResult> CreateCheckpoint(long gameId)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        
        var transactions = await gameService.CreateCheckpoint(game.GetValueOrThrow());
        return transactions.ToOkResult(t => new TransactionDto(t));
    }
}