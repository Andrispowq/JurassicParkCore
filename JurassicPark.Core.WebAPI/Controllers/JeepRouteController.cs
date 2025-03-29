using JurassicPark.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JurassicPark.Core.WebAPI.Controllers;

[ApiController]
[Route("games/{id:long}/jeep-routes")]
public class JeepRouteController(IGameService gameService) : ControllerBase
{
    
}