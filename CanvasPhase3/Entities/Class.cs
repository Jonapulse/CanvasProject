using System;
using System.Collections.Generic;

namespace CanvasPhase3.Entities;

public partial class Class
{
    public int Classid { get; set; }

    public short? Semesteryear { get; set; }

    public string? Semesterterm { get; set; }

    public string? Location { get; set; }

    public TimeOnly? Starttime { get; set; }

    public TimeOnly? Endtime { get; set; }

    public string Catalogid { get; set; } = null!;

    public int? Profid { get; set; }

    public virtual ICollection<Assignmentcategory> Assignmentcategories { get; set; } = new List<Assignmentcategory>();

    public virtual Course Catalog { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Professor? Prof { get; set; }
}
