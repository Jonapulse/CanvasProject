using System;
using System.Collections.Generic;

namespace CanvasPhase3.CanvasPhase3.Entities;

public partial class Assignmentsubmission
{
    public DateTime Submissiontime { get; set; }

    public int? Score { get; set; }

    public string? Content { get; set; }

    public string Studentid { get; set; } = null!;

    public int Assignmentid { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
