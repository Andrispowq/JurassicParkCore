using JurassicPark.Core.DataSchemas;
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
    
    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance(long gameId)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        
        var balance = await gameService.GetCurrentBalance(game.GetValueOrThrow());
        return balance.ToHttpResult();
    }

    [HttpPost("create-checkpoint")]
    public async Task<IActionResult> CreateCheckpoint(long gameId)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        
        var transactions = await gameService.CreateCheckpoint(game.GetValueOrThrow());
        return transactions.ToOkResult(t => new TransactionDto(t));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction(long gameId, [FromBody] CreateTransactionDto request)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();

        if (request.Type == TransactionType.Checkpoint)
        {
            return BadRequest("Cannot create a checkpoint transaction on this endpoint");
        }

        var gameR = game.GetValueOrThrow();

        var transaction = new Transaction
        {
            Type = request.Type,
            Amount = request.Amount,
            SavedGameId = gameR.Id,
        };

        var result = await gameService.CreateTransaction(gameR, transaction,
            request.CanLose);
        if (result.IsSome)
        {
            return result.ToHttpResult();
        }

        return Ok(new TransactionDto(transaction));
    }
}