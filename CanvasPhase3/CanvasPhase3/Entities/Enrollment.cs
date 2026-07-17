using System;
using System.Collections.Generic;

namespace CanvasPhase3.CanvasPhase3.Entities;

public partial class Enrollment
{
    public int Classid { get; set; }

    public string Uid { get; set; } = null!;

    public string? Grade { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Student UidNavigation { get; set; } = null!;
}
