using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TestFinal.Models;

namespace TestFinal.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CheckIn> CheckIns { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<ExamRoster> ExamRosters { get; set; }

    public virtual DbSet<SeatAssignment> SeatAssignments { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Violation> Violations { get; set; }

    public virtual DbSet<VwExamReportDetail> VwExamReportDetails { get; set; }

    public virtual DbSet<VwExamReportSummary> VwExamReportSummaries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=BRKAYS;Database=ExamSecurityDB;Trusted_Connection=True;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CheckIn>(entity =>
        {
            entity.HasKey(e => e.CheckInId).HasName("PK__CheckIns__E6497684D2A068CD");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Exam).WithMany(p => p.CheckIns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CheckIns_Exam");

            entity.HasOne(d => d.ProctorUser).WithMany(p => p.CheckIns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CheckIns_Proctor");

            entity.HasOne(d => d.Student).WithMany(p => p.CheckIns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CheckIns_Student");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.ExamId).HasName("PK__Exams__297521C76C1D64CD");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Exams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Exams_Users");
        });

        modelBuilder.Entity<ExamRoster>(entity =>
        {
            entity.HasKey(e => new { e.ExamId, e.StudentId }).HasName("PK__ExamRost__AA59737E0C6B3468");

            entity.Property(e => e.AddedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Exam).WithMany(p => p.ExamRosters).HasConstraintName("FK_Roster_Exam");

            entity.HasOne(d => d.Student).WithMany(p => p.ExamRosters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Roster_Student");
        });

        modelBuilder.Entity<SeatAssignment>(entity =>
        {
            entity.HasKey(e => new { e.ExamId, e.StudentId }).HasName("PK__SeatAssi__AA59737E6AC3066B");

            entity.Property(e => e.AssignedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.ExamRoster).WithOne(p => p.SeatAssignment).HasConstraintName("FK_SeatAssign_Roster");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52B99F60B839C");

            entity.Property(e => e.StudentId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C50EC9553");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Violation>(entity =>
        {
            entity.HasKey(e => e.ViolationId).HasName("PK__Violatio__18B6DC083D268685");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.CheckIn).WithMany(p => p.Violations).HasConstraintName("FK_Violations_CheckIn");
        });

        modelBuilder.Entity<VwExamReportDetail>(entity =>
        {
            entity.ToView("vw_ExamReportDetails");
        });

        modelBuilder.Entity<VwExamReportSummary>(entity =>
        {
            entity.ToView("vw_ExamReportSummary");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
