using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestFinal.Models;

[PrimaryKey("ExamId", "StudentId")]
public partial class SeatAssignment
{
    [Key]
    public int ExamId { get; set; }

    [Key]
    public int StudentId { get; set; }

    [StringLength(10)]
    public string SeatCode { get; set; } = null!;

    public DateTime AssignedAt { get; set; }

    [ForeignKey("ExamId, StudentId")]
    [InverseProperty("SeatAssignment")]
    public virtual ExamRoster ExamRoster { get; set; } = null!;
}
