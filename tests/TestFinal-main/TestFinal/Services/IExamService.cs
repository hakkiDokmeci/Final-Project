using Microsoft.EntityFrameworkCore;
using TestFinal.Data;
using TestFinal.Dtos;
using TestFinal.Models;
using TestFinal.Services.Interfaces;

namespace TestFinal.Services.Implementations;

public class ExamService : IExamService
{
    private readonly AppDbContext _db;

    public ExamService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Exam> CreateExamAsync(int createdByUserId, ExamCreateDto dto)
    {
        var exam = new Exam
        {
            ExamName = dto.ExamName,
            Room = dto.Room,
            ExamDate = dto.ExamDate,
            ExamTime = dto.ExamTime,
            SeatRows = dto.SeatRows,
            SeatCols = dto.SeatCols,
            CreatedByUserId = createdByUserId,
            CreatedAt = DateTime.UtcNow
        };

        _db.Exams.Add(exam);
        await _db.SaveChangesAsync();
        return exam;
    }

    public async Task AddStudentToRosterAsync(int examId, int studentId)
    {
        // Roster unique (ExamId, StudentId) DB’de primary key
        var exists = await _db.ExamRosters.AnyAsync(r => r.ExamId == examId && r.StudentId == studentId);
        if (exists) return;

        _db.ExamRosters.Add(new ExamRoster { ExamId = examId, StudentId = studentId });
        await _db.SaveChangesAsync();
    }

    public async Task GenerateSeatingAsync(int examId, bool overwriteExisting)
    {
        var exam = await _db.Exams.FirstOrDefaultAsync(e => e.ExamId == examId);
        if (exam == null) throw new KeyNotFoundException("Exam not found.");

        if (overwriteExisting)
        {
            var old = await _db.SeatAssignments.Where(x => x.ExamId == examId).ToListAsync();
            if (old.Count > 0) _db.SeatAssignments.RemoveRange(old);
        }

        var roster = await _db.ExamRosters.Where(r => r.ExamId == examId).Select(r => r.StudentId).ToListAsync();
        if (roster.Count == 0) return;

        int capacity = exam.SeatRows * exam.SeatCols;
        if (roster.Count > capacity)
            throw new InvalidOperationException("Not enough seats for roster size.");

        // A1..B2 vs
        var seatCodes = new List<string>();
        for (int r = 0; r < exam.SeatRows; r++)
        {
            char rowLetter = (char)('A' + r);
            for (int c = 1; c <= exam.SeatCols; c++)
                seatCodes.Add($"{rowLetter}{c}");
        }

        for (int i = 0; i < roster.Count; i++)
        {
            _db.SeatAssignments.Add(new SeatAssignment
            {
                ExamId = examId,
                StudentId = roster[i],
                SeatCode = seatCodes[i]
            });
        }

        await _db.SaveChangesAsync();
    }
}
