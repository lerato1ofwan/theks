using Azure;
using Microsoft.AspNetCore.Mvc;
using Theks.Identity.Application.DTOs;
using Theks.Identity.Application.Interfaces;

namespace Theks.Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(
    IUserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Response>> RegisterAsync(ApplicationUser applicationUser, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await userService.RegisterAsync(applicationUser, cancellationToken);

        return Ok(response);
    }

    [HttpPost("/login")]
    public async Task<ActionResult<Response>> LoginAsync(Login login, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await userService.LoginAsync(login, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Response>> GetUserAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await userService.GetUserAsync(id, cancellationToken);

        return Ok(response);
    }
}