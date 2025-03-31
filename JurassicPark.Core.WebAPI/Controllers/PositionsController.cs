using JurassicPark.Core.Services.Interfaces;
using JurassicPark.Core.WebAPI.Dto;
using Microsoft.AspNetCore.Mvc;

namespace JurassicPark.Core.WebAPI.Controllers;

[ApiController]
[Route("positions")]
public class PositionsController(IGameService gameService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPositions()
    {
        var positions = await gameService.GetPositions();
        return Ok(positions.Select(p => new PositionDto(p)));
    }
}