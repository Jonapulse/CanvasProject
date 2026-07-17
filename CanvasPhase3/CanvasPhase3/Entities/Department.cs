using System;
using System.Collections.Generic;

namespace CanvasPhase3.CanvasPhase3.Entities;

public partial class Department
{
    public int Depid { get; set; }

    public string? Subjabbrv { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Professor> Professors { get; set; } = new List<Professor>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
