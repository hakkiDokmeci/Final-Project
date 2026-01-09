using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestFinal.Models;

[Keyless]
public partial class VwExamReportSummary
{
    public int ExamId { get; set; }

    [StringLength(200)]
    public string ExamName { get; set; } = null!;

    [StringLength(50)]
    public string Room { get; set; } = null!;

    public DateOnly ExamDate { get; set; }

    [Precision(0)]
    public TimeOnly ExamTime { get; set; }

    public int? TotalStudents { get; set; }

    public int? CheckedIn { get; set; }

    public int? Violations { get; set; }

    public int? NotArrived { get; set; }
}
