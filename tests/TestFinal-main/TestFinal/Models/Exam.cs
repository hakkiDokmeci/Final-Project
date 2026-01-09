using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestFinal.Models;

public partial class Exam
{
    [Key]
    public int ExamId { get; set; }

    [StringLength(200)]
    public string ExamName { get; set; } = null!;

    [StringLength(50)]
    public string Room { get; set; } = null!;

    public DateOnly ExamDate { get; set; }

    [Precision(0)]
    public TimeOnly ExamTime { get; set; }

    public int SeatRows { get; set; }

    public int SeatCols { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("Exam")]
    public virtual ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();

    [ForeignKey("CreatedByUserId")]
    [InverseProperty("Exams")]
    public virtual User CreatedByUser { get; set; } = null!;

    [InverseProperty("Exam")]
    public virtual ICollection<ExamRoster> ExamRosters { get; set; } = new List<ExamRoster>();
}
