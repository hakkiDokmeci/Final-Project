using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestFinal.Models;

[Index("Username", Name = "UQ__Users__536C85E4B9A22825", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(100)]
    public string Username { get; set; } = null!;

    [StringLength(200)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(20)]
    public string Role { get; set; } = null!;

    [StringLength(150)]
    public string DisplayName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("ProctorUser")]
    public virtual ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();

    [InverseProperty("CreatedByUser")]
    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
}
