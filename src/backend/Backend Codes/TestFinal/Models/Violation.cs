using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestFinal.Models;

[Index("ExamId", "ViolationType", "CreatedAt", Name = "IX_Violations_Exam_Type")]
public partial class Violation
{
    [Key]
    public int ViolationId { get; set; }

    public int CheckInId { get; set; }

    public int ExamId { get; set; }

    public int StudentId { get; set; }

    [StringLength(50)]
    public string ViolationType { get; set; } = null!;

    [StringLength(500)]
    public string? Details { get; set; }

    [StringLength(300)]
    public string? EvidenceRef { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("CheckInId")]
    [InverseProperty("Violations")]
    public virtual CheckIn CheckIn { get; set; } = null!;
}
