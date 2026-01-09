using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestFinal.Services.Implementations;
using TestFinal.Services.Interfaces;

namespace TestFinal.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize(Roles = "Admin")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reports;

    public ReportsController(IReportService reports)
    {
        _reports = reports;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> Summary([FromQuery] int examId)
        => Ok(await _reports.GetSummaryAsync(examId));

    [HttpGet("details")]
    public async Task<IActionResult> Details([FromQuery] int examId)
        => Ok(await _reports.GetDetailsAsync(examId));
}
