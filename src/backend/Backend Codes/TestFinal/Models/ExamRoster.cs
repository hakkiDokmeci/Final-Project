using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestFinal.Models;

[PrimaryKey("ExamId", "StudentId")]
[Table("ExamRoster")]
public partial class ExamRoster
{
    [Key]
    public int ExamId { get; set; }

    [Key]
    public int StudentId { get; set; }

    public DateTime AddedAt { get; set; }

    [ForeignKey("ExamId")]
    [InverseProperty("ExamRosters")]
    public virtual Exam Exam { get; set; } = null!;

    [InverseProperty("ExamRoster")]
    public virtual SeatAssignment? SeatAssignment { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("ExamRosters")]
    public virtual Student Student { get; set; } = null!;
}
