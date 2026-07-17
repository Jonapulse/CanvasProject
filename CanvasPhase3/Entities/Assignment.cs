using System;
using System.Collections.Generic;

namespace CanvasPhase3.Entities;

public partial class Assignment
{
    public int Assignmentid { get; set; }

    public string? Name { get; set; }

    public int? Score { get; set; }

    public int? Maxscore { get; set; }

    public string? Content { get; set; }

    public DateTime? Duedate { get; set; }

    public int Categoryid { get; set; }

    public virtual ICollection<Assignmentsubmission> Assignmentsubmissions { get; set; } = new List<Assignmentsubmission>();

    public virtual Assignmentcategory Category { get; set; } = null!;
}
