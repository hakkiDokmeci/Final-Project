using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using TestFinal.Data;
using TestFinal.Services; // CheckInService namespace burada olabilir
using TestFinal.Models;

namespace TestFinal.Tests;

[TestClass]
public class CheckInServiceTests
{
    private AppDbContext _db = default!;

    [TestInitialize]
    public void Init()
    {
        _db = TestDbFactory.CreateContext();
        TestSeed.SeedMinimal(_db);
    }

    [TestMethod]
    public async Task CreateCheckIn_Valid_Should_Save_CheckedIn_And_No_Violation()
    {
        // Arrange: face match OK
        var face = new StubFaceMatchService(match: true, score: 98);
        var service = new CheckInService(_db, face);

        // Act
        var (checkIn, violations) = await service.CreateCheckInAsync(
            examId: TestSeed.ExamId,
            studentId: TestSeed.StudentId,
            proctorUserId: TestSeed.ProctorUserId,
            observedSeatCode: TestSeed.AssignedSeat,
            capturedImageRef: "img-ok",
            notes: "ok"
        );

        // Assert (DB constraint uyumlu değerler)
        Assert.AreEqual("CHECKED_IN", checkIn.Status);
        Assert.AreEqual("MATCH", checkIn.IdentityResult);
        Assert.AreEqual("OK", checkIn.SeatResult);
        Assert.AreEqual(0, violations.Count);

        var count = await _db.CheckIns.CountAsync();
        Assert.AreEqual(1, count);
    }

    [TestMethod]
    public async Task CreateCheckIn_Duplicate_Should_Throw_And_Not_Add_Second_Record()
    {
        // Arrange: face match OK
        var face = new StubFaceMatchService(match: true, score: 90);
        var service = new CheckInService(_db, face);

        // First check-in
        await service.CreateCheckInAsync(
            examId: TestSeed.ExamId,
            studentId: TestSeed.StudentId,
            proctorUserId: TestSeed.ProctorUserId,
            observedSeatCode: TestSeed.AssignedSeat,
            capturedImageRef: "img-1",
            notes: "first"
        );

        // Act + Assert: second check-in should fail (business rule)
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
        {
            await service.CreateCheckInAsync(
                examId: TestSeed.ExamId,
                studentId: TestSeed.StudentId,
                proctorUserId: TestSeed.ProctorUserId,
                observedSeatCode: TestSeed.AssignedSeat,
                capturedImageRef: "img-2",
                notes: "second"
            );
        });

        var count = await _db.CheckIns.CountAsync();
        Assert.AreEqual(1, count);
    }

    [TestMethod]
    public async Task CreateCheckIn_WrongSeat_Should_Save_Violation_And_Log_WRONG_SEAT()
    {
        // Arrange: face match OK, seat wrong
        var face = new StubFaceMatchService(match: true, score: 95);
        var service = new CheckInService(_db, face);

        // Act (observed seat differs from assigned seat)
        var (checkIn, violations) = await service.CreateCheckInAsync(
            examId: TestSeed.ExamId,
            studentId: TestSeed.StudentId,
            proctorUserId: TestSeed.ProctorUserId,
            observedSeatCode: "B3",
            capturedImageRef: "img-seat-wrong",
            notes: "seat wrong test"
        );

        // Assert
        Assert.AreEqual("VIOLATION", checkIn.Status);
        Assert.AreEqual("MATCH", checkIn.IdentityResult);
        Assert.AreEqual("NOT_OK", checkIn.SeatResult);

        Assert.IsTrue(
            violations.Any(v => v.ViolationType == "WRONG_SEAT"),
            "Expected WRONG_SEAT violation type."
        );

        var vCount = await _db.Violations.CountAsync();
        Assert.AreEqual(1, vCount);
    }

    [TestMethod]
    public async Task CreateCheckIn_IdentityMismatch_Should_Save_Violation_And_Log_IDENTITY_MISMATCH()
    {
        // Arrange: face mismatch, seat correct
        var face = new StubFaceMatchService(match: false, score: 30);
        var service = new CheckInService(_db, face);

        // Act
        var (checkIn, violations) = await service.CreateCheckInAsync(
            examId: TestSeed.ExamId,
            studentId: TestSeed.StudentId,
            proctorUserId: TestSeed.ProctorUserId,
            observedSeatCode: TestSeed.AssignedSeat,
            capturedImageRef: "img-face-mismatch",
            notes: "face mismatch test"
        );

        // Assert
        Assert.AreEqual("VIOLATION", checkIn.Status);
        Assert.AreEqual("NO_MATCH", checkIn.IdentityResult);
        Assert.AreEqual("OK", checkIn.SeatResult);

        Assert.IsTrue(
            violations.Any(v => v.ViolationType == "IDENTITY_MISMATCH"),
            "Expected IDENTITY_MISMATCH violation type."
        );

        var vCount = await _db.Violations.CountAsync();
        Assert.AreEqual(1, vCount);
    }
}
