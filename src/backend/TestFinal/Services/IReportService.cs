using Microsoft.EntityFrameworkCore;
using TestFinal.Data;
using TestFinal.Services.Interfaces;

namespace TestFinal.Services.Implementations;

public class ReportService : IReportService
{
    private readonly AppDbContext _db;

    public ReportService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<object> GetSummaryAsync(int examId)
    {
        // View modelin scaffold ile geldiyse direkt oradan da çekebilirsin.
        // Burada basit “gerçek DB query” var.
        var total = await _db.ExamRosters.CountAsync(r => r.ExamId == examId);
        var checkedIn = await _db.CheckIns.CountAsync(c => c.ExamId == examId && c.Status == "CHECKED_IN");
        var violations = await _db.CheckIns.CountAsync(c => c.ExamId == examId && c.Status == "VIOLATION");

        return new
        {
            ExamId = examId,
            TotalStudents = total,
            CheckedIn = checkedIn,
            Violations = violations,
            NotArrived = total - (checkedIn + violations)
        };
    }

    public async Task<List<object>> GetDetailsAsync(int examId)
    {
        var list = await _db.CheckIns
            .Where(c => c.ExamId == examId)
            .Select(c => new
            {
                c.CheckInId,
                c.StudentId,
                c.Status,
                c.IdentityResult,
                c.IdentityMatchScore,
                c.SeatResult,
                c.ObservedSeatCode,
                c.CreatedAt
            })
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync<object>();

        return list;
    }
}
