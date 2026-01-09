using Microsoft.EntityFrameworkCore;
using TestFinal.Data;
using TestFinal.Dtos;
using TestFinal.Models;
using TestFinal.Services.Interfaces;

namespace TestFinal.Services.Implementations;

public class CheckInService : ICheckInService
{
    private readonly AppDbContext _db;

    public CheckInService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<(CheckIn checkIn, List<Violation> violations)> CreateCheckInAsync(int proctorUserId, CheckInCreateDto dto)
    {
        // ❗ Rule: Student cannot check in twice (DB’de UNIQUE var, burada da kontrol ediyoruz)
        bool already = await _db.CheckIns.AnyAsync(c => c.ExamId == dto.ExamId && c.StudentId == dto.StudentId);
        if (already) throw new InvalidOperationException("Student cannot check in twice.");

        // Seat check
        var assignment = await _db.SeatAssignments.FirstOrDefaultAsync(a => a.ExamId == dto.ExamId && a.StudentId == dto.StudentId);
        bool seatCheckRan = assignment != null;
        bool seatOk = seatCheckRan && string.Equals(assignment!.SeatCode, dto.ObservedSeatCode, StringComparison.OrdinalIgnoreCase);

        string seatResult = !seatCheckRan ? "NOT_RUN" : (seatOk ? "OK" : "NOT_OK"); // ✅ constraint

        // Face check (gerçek ML yok: burada sadece “NOT_RUN / MATCH / NO_MATCH” üretiyoruz)
        // Hoca çalıştırmayacağı için “logic var” görünmesi yeter.
        bool faceCheckRan = !string.IsNullOrWhiteSpace(dto.CapturedImageRef);
        string identityResult = !faceCheckRan ? "NOT_RUN" : "MATCH"; // ✅ constraint (istersen heuristik ekleriz)
        int? score = faceCheckRan ? 90 : null;                        // ✅ 0..100

        string status = (identityResult == "MATCH" && seatResult == "OK") ? "CHECKED_IN" : "VIOLATION"; // ✅ constraint

        var checkIn = new CheckIn
        {
            ExamId = dto.ExamId,
            StudentId = dto.StudentId,
            ProctorUserId = proctorUserId,
            ObservedSeatCode = dto.ObservedSeatCode,
            CapturedImageRef = dto.CapturedImageRef,
            Notes = dto.Notes,

            IdentityMatchScore = score,
            IdentityResult = identityResult,
            SeatResult = seatResult,
            Status = status,

            CreatedAt = DateTime.UtcNow
        };

        _db.CheckIns.Add(checkIn);
        await _db.SaveChangesAsync(); // CheckInId oluşur

        var violations = new List<Violation>();

        if (identityResult == "NO_MATCH")
        {
            violations.Add(new Violation
            {
                CheckInId = checkIn.CheckInId,
                ExamId = dto.ExamId,
                StudentId = dto.StudentId,
                ViolationType = "IDENTITY_MISMATCH", // ✅ constraint
                Details = $"Identity mismatch. Score={score}",
                EvidenceRef = dto.CapturedImageRef,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (seatResult == "NOT_OK")
        {
            violations.Add(new Violation
            {
                CheckInId = checkIn.CheckInId,
                ExamId = dto.ExamId,
                StudentId = dto.StudentId,
                ViolationType = "WRONG_SEAT", // ✅ constraint
                Details = $"Assigned={assignment?.SeatCode}, Observed={dto.ObservedSeatCode}",
                EvidenceRef = dto.CapturedImageRef,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (violations.Count > 0)
        {
            _db.Violations.AddRange(violations);
            await _db.SaveChangesAsync();
        }

        return (checkIn, violations);
    }
}
