using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestFinal.Models;

public partial class Student
{
    [Key]
    public int StudentId { get; set; }

    [StringLength(150)]
    public string FullName { get; set; } = null!;

    [StringLength(150)]
    public string? Email { get; set; }

    [StringLength(300)]
    public string? FaceTemplateRef { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();

    [InverseProperty("Student")]
    public virtual ICollection<ExamRoster> ExamRosters { get; set; } = new List<ExamRoster>();
}
