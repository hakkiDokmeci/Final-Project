using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestFinal.Data;
using TestFinal.Models;

namespace TestFinal.Controllers;

[ApiController]
[Route("api/exams/{examId:int}/seating")]
[Authorize(Roles = "Admin")]
public class SeatingController : ControllerBase
{
    private readonly AppDbContext _db;
    public SeatingController(AppDbContext db) => _db = db;

    // POST api/exams/{examId}/seating/generate
    [HttpPost("generate")]
    public async Task<IActionResult> Generate(int examId)
    {
        var exam = await _db.Exams.FirstOrDefaultAsync(e => e.ExamId == examId);
        if (exam == null) return NotFound(new { message = "Exam not found" });

        // roster list
        var rosterStudentIds = await _db.ExamRosters
            .Where(r => r.ExamId == examId)
            .Select(r => r.StudentId)
            .ToListAsync();

        if (rosterStudentIds.Count == 0)
            return BadRequest(new { message = "Roster is empty" });

        int capacity = exam.SeatRows * exam.SeatCols;
        if (capacity < rosterStudentIds.Count)
            return BadRequest(new { message = $"Not enough seats. Capacity={capacity}, Students={rosterStudentIds.Count}" });

        // önce eski seat assignments temizle (yeniden generate için)
        var old = await _db.SeatAssignments.Where(x => x.ExamId == examId).ToListAsync();
        if (old.Count > 0) _db.SeatAssignments.RemoveRange(old);

        // Seat code üret: A1..A{cols}, B1..B{cols}...
        var seatCodes = new List<string>();
        for (int r = 0; r < exam.SeatRows; r++)
        {
            char rowLetter = (char)('A' + r); // A, B, C...
            for (int c = 1; c <= exam.SeatCols; c++)
                seatCodes.Add($"{rowLetter}{c}");
        }

        // sırayla dağıt
        var assignments = new List<SeatAssignment>();
        for (int i = 0; i < rosterStudentIds.Count; i++)
        {
            assignments.Add(new SeatAssignment
            {
                ExamId = examId,
                StudentId = rosterStudentIds[i],
                SeatCode = seatCodes[i]
            });
        }

        _db.SeatAssignments.AddRange(assignments);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Seating generated", count = assignments.Count });
    }

    // GET api/exams/{examId}/seating
    [HttpGet]
    public async Task<IActionResult> Get(int examId)
    {
        var list = await _db.SeatAssignments
            .Where(x => x.ExamId == examId)
            .Join(_db.Students, a => a.StudentId, s => s.StudentId,
                (a, s) => new { s.StudentId, s.FullName, a.SeatCode })
            .OrderBy(x => x.SeatCode)
            .ToListAsync();

        return Ok(list);
    }
}
