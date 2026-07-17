using System;
using System.Collections.Generic;

namespace CanvasPhase3.Entities;

public partial class Assignmentcategory
{
    public int Categoryid { get; set; }

    public string? Name { get; set; }

    public short? Gradingweight { get; set; }

    public int Classid { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual Class Class { get; set; } = null!;
}
