using System;
using System.Collections.Generic;
using CanvasPhase3.Entities;
using Microsoft.EntityFrameworkCore;

namespace CanvasPhase3.Context;

public partial class LMSContext : DbContext
{
    public LMSContext()
    {
    }

    public LMSContext(DbContextOptions<LMSContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<Assignmentcategory> Assignmentcategories { get; set; }

    public virtual DbSet<Assignmentsubmission> Assignmentsubmissions { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Professor> Professors { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=atr.eng.utah.edu;Database=LMS5;Username=u0294347;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.Uid).HasName("administrators_pkey");

            entity.ToTable("administrators");

            entity.Property(e => e.Uid)
                .ValueGeneratedNever()
                .HasColumnName("uid");

            entity.HasOne(d => d.UidNavigation).WithOne(p => p.Administrator)
                .HasForeignKey<Administrator>(d => d.Uid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("administrators_uid_fkey");
        });

        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => new { e.Assignmentid, e.Categoryid }).HasName("assignments_pkey");

            entity.ToTable("assignments");

            entity.HasIndex(e => e.Assignmentid, "unique_assignmentid").IsUnique();

            entity.Property(e => e.Assignmentid)
                .ValueGeneratedOnAdd()
                .HasColumnName("assignmentid");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Duedate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("duedate");
            entity.Property(e => e.Maxscore).HasColumnName("maxscore");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Score).HasColumnName("score");

            entity.HasOne(d => d.Category).WithMany(p => p.Assignments)
                .HasPrincipalKey(p => p.Categoryid)
                .HasForeignKey(d => d.Categoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assignments_categoryid_fkey");
        });

        modelBuilder.Entity<Assignmentcategory>(entity =>
        {
            entity.HasKey(e => new { e.Categoryid, e.Classid }).HasName("assignmentcategories_pkey");

            entity.ToTable("assignmentcategories");

            entity.HasIndex(e => e.Categoryid, "assignmentcategories_categoryid_key").IsUnique();

            entity.Property(e => e.Categoryid)
                .ValueGeneratedOnAdd()
                .HasColumnName("categoryid");
            entity.Property(e => e.Classid).HasColumnName("classid");
            entity.Property(e => e.Gradingweight).HasColumnName("gradingweight");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.Class).WithMany(p => p.Assignmentcategories)
                .HasPrincipalKey(p => p.Classid)
                .HasForeignKey(d => d.Classid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assignmentcategories_classid_fkey");
        });

        modelBuilder.Entity<Assignmentsubmission>(entity =>
        {
            entity.HasKey(e => new { e.Submissiontime, e.Studentid, e.Assignmentid }).HasName("assignmentsubmissions_pkey");

            entity.ToTable("assignmentsubmissions");

            entity.Property(e => e.Submissiontime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("submissiontime");
            entity.Property(e => e.Studentid).HasColumnName("studentid");
            entity.Property(e => e.Assignmentid).HasColumnName("assignmentid");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Score).HasColumnName("score");

            entity.HasOne(d => d.Assignment).WithMany(p => p.Assignmentsubmissions)
                .HasPrincipalKey(p => p.Assignmentid)
                .HasForeignKey(d => d.Assignmentid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assignmentsubmissions_assignmentid_fkey");

            entity.HasOne(d => d.Student).WithMany(p => p.Assignmentsubmissions)
                .HasForeignKey(d => d.Studentid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assignmentsubmissions_studentid_fkey");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => new { e.Classid, e.Catalogid }).HasName("classes_pkey");

            entity.ToTable("classes");

            entity.HasIndex(e => e.Classid, "unique_classid").IsUnique();

            entity.Property(e => e.Classid)
                .ValueGeneratedOnAdd()
                .HasColumnName("classid");
            entity.Property(e => e.Catalogid)
                .HasMaxLength(5)
                .HasColumnName("catalogid");
            entity.Property(e => e.Endtime).HasColumnName("endtime");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.Profid).HasColumnName("profid");
            entity.Property(e => e.Semesterterm)
                .HasMaxLength(6)
                .HasColumnName("semesterterm");
            entity.Property(e => e.Semesteryear).HasColumnName("semesteryear");
            entity.Property(e => e.Starttime).HasColumnName("starttime");

            entity.HasOne(d => d.Catalog).WithMany(p => p.Classes)
                .HasForeignKey(d => d.Catalogid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("classes_catalogid_fkey");

            entity.HasOne(d => d.Prof).WithMany(p => p.Classes)
                .HasForeignKey(d => d.Profid)
                .HasConstraintName("classes_profid_fkey");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Catalogid).HasName("courses_pkey");

            entity.ToTable("courses");

            entity.Property(e => e.Catalogid)
                .HasMaxLength(5)
                .HasColumnName("catalogid");
            entity.Property(e => e.Depid).HasColumnName("depid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Number).HasColumnName("number");

            entity.HasOne(d => d.Dep).WithMany(p => p.Courses)
                .HasForeignKey(d => d.Depid)
                .HasConstraintName("courses_depid_fkey");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Depid).HasName("departments_pkey");

            entity.ToTable("departments");

            entity.HasIndex(e => e.Subjabbrv, "departments_subjabbrv_key").IsUnique();

            entity.Property(e => e.Depid).HasColumnName("depid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Subjabbrv)
                .HasMaxLength(4)
                .HasColumnName("subjabbrv");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => new { e.Classid, e.Uid }).HasName("enrollment_pkey");

            entity.ToTable("enrollment");

            entity.Property(e => e.Classid).HasColumnName("classid");
            entity.Property(e => e.Uid).HasColumnName("uid");
            entity.Property(e => e.Grade)
                .HasMaxLength(2)
                .HasColumnName("grade");

            entity.HasOne(d => d.Class).WithMany(p => p.Enrollments)
                .HasPrincipalKey(p => p.Classid)
                .HasForeignKey(d => d.Classid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("enrollment_classid_fkey");

            entity.HasOne(d => d.UidNavigation).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.Uid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("enrollment_uid_fkey");
        });

        modelBuilder.Entity<Professor>(entity =>
        {
            entity.HasKey(e => e.Uid).HasName("professors_pkey");

            entity.ToTable("professors");

            entity.Property(e => e.Uid)
                .ValueGeneratedNever()
                .HasColumnName("uid");
            entity.Property(e => e.Employerdep).HasColumnName("employerdep");

            entity.HasOne(d => d.EmployerdepNavigation).WithMany(p => p.Professors)
                .HasForeignKey(d => d.Employerdep)
                .HasConstraintName("professors_employerdep_fkey");

            entity.HasOne(d => d.UidNavigation).WithOne(p => p.Professor)
                .HasForeignKey<Professor>(d => d.Uid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("professors_uid_fkey");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Uid).HasName("students_pkey");

            entity.ToTable("students");

            entity.Property(e => e.Uid)
                .ValueGeneratedNever()
                .HasColumnName("uid");
            entity.Property(e => e.Majordep).HasColumnName("majordep");

            entity.HasOne(d => d.MajordepNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Majordep)
                .HasConstraintName("students_majordep_fkey");

            entity.HasOne(d => d.UidNavigation).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.Uid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("students_uid_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Uid).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Uid).HasColumnName("uid");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
