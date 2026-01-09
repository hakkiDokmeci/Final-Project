using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestFinal.Dtos;
using TestFinal.Services.Interfaces;

namespace TestFinal.Controllers;

[ApiController]
[Route("api/checkins")]
[Authorize(Roles = "Admin,Proctor")]
public class CheckInsController : ControllerBase
{
    private readonly ICheckInService _checkIn;

    public CheckInsController(ICheckInService checkIn)
    {
        _checkIn = checkIn;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CheckInCreateDto dto)
    {
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int proctorId);

        try
        {
            var (checkIn, violations) = await _checkIn.CreateCheckInAsync(proctorId, dto);
            return Ok(new { checkIn.CheckInId, checkIn.Status, checkIn.IdentityResult, checkIn.SeatResult, violationCount = violations.Count });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message }); // cannot check-in twice
        }
    }
}
