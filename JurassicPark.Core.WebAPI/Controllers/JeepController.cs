using JurassicPark.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JurassicPark.Core.WebAPI.Controllers;

[ApiController]
[Route("games/{id:long}/jeeps")]
public class JeepController(IGameService gameService) : ControllerBase
{
    
}