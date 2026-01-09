using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestFinal.Dtos;
using TestFinal.Services.Implementations;
using TestFinal.Services.Interfaces;

namespace TestFinal.Controllers;

[ApiController]
[Route("api/exams")]
[Authorize(Roles = "Admin")]
public class ExamsController : ControllerBase
{
    private readonly IExamService _exam;

    public ExamsController(IExamService exam)
    {
        _exam = exam;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExamCreateDto dto)
    {
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId);
        var exam = await _exam.CreateExamAsync(userId, dto);
        return Ok(exam);
    }

    [HttpPost("{id}/roster")]
    public async Task<IActionResult> AddRoster(int id, [FromBody] RosterAddDto dto)
    {
        await _exam.AddStudentToRosterAsync(id, dto.StudentId);
        return Ok(new { message = "Added to roster" });
    }

    [HttpPost("{id}/seating/generate")]
    public async Task<IActionResult> GenerateSeating(int id, [FromBody] SeatingGenerateRequestDto dto)
    {
        await _exam.GenerateSeatingAsync(id, dto.OverwriteExisting);
        return Ok(new { message = "Seating generated" });
    }
}
