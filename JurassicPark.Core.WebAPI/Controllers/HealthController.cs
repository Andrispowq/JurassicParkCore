using JurassicPark.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JurassicPark.Core.WebAPI.Controllers;

[ApiController]
[Route("health")]
public class HealthController(IGameService service) : ControllerBase
{
     [HttpGet]
     public async Task<IActionResult> Get()
     {
          try
          {
               _ = await service.GetSavedGames();
               return Ok(new { status = "Healthy" });
          }
          catch (Exception e)
          {
               return BadRequest(new { status = "Unhealthy", error = e.Message });
          }
     }
}