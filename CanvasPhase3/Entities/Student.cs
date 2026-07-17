using System;
using System.Collections.Generic;

namespace CanvasPhase3.Entities;

public partial class Student
{
    public int Uid { get; set; }

    public int? Majordep { get; set; }

    public virtual ICollection<Assignmentsubmission> Assignmentsubmissions { get; set; } = new List<Assignmentsubmission>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Department? MajordepNavigation { get; set; }

    public virtual User UidNavigation { get; set; } = null!;
}
