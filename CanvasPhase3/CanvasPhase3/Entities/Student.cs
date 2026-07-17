using System;
using System.Collections.Generic;

namespace CanvasPhase3.CanvasPhase3.Entities;

public partial class Student
{
    public string Uid { get; set; } = null!;

    public int? Majordep { get; set; }

    public virtual ICollection<Assignmentsubmission> Assignmentsubmissions { get; set; } = new List<Assignmentsubmission>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Department? MajordepNavigation { get; set; }
}
