using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestFinal.Models;

[Index("ExamId", "Status", "CreatedAt", Name = "IX_CheckIns_Exam_Status")]
[Index("ExamId", "StudentId", Name = "UQ_CheckIn_Exam_Student", IsUnique = true)]
public partial class CheckIn
{
    [Key]
    public int CheckInId { get; set; }

    public int ExamId { get; set; }

    public int StudentId { get; set; }

    public int ProctorUserId { get; set; }

    [StringLength(10)]
    public string ObservedSeatCode { get; set; } = null!;

    public int? IdentityMatchScore { get; set; }

    [StringLength(20)]
    public string IdentityResult { get; set; } = null!;

    [StringLength(20)]
    public string SeatResult { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [StringLength(300)]
    public string? CapturedImageRef { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("ExamId")]
    [InverseProperty("CheckIns")]
    public virtual Exam Exam { get; set; } = null!;

    [ForeignKey("ProctorUserId")]
    [InverseProperty("CheckIns")]
    public virtual User ProctorUser { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("CheckIns")]
    public virtual Student Student { get; set; } = null!;

    [InverseProperty("CheckIn")]
    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
