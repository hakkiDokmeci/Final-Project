using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestFinal.Models;

[Keyless]
public partial class VwExamReportDetail
{
    [Column("TimeUTC")]
    public DateTime TimeUtc { get; set; }

    public int ExamId { get; set; }

    public int StudentId { get; set; }

    [StringLength(150)]
    public string FullName { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [StringLength(20)]
    public string IdentityResult { get; set; } = null!;

    public int? IdentityMatchScore { get; set; }

    [StringLength(20)]
    public string SeatResult { get; set; } = null!;

    [StringLength(10)]
    public string? AssignedSeat { get; set; }

    [StringLength(10)]
    public string ObservedSeat { get; set; } = null!;

    [StringLength(50)]
    public string? ViolationType { get; set; }

    [StringLength(500)]
    public string? ViolationDetails { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
}
