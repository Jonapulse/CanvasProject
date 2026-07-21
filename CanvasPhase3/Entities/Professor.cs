using System;
using System.Collections.Generic;

namespace CanvasPhase3.Entities;

public partial class Professor
{
    public int Uid { get; set; }

    public int? Employerdep { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual Department? EmployerdepNavigation { get; set; }

    public virtual User UidNavigation { get; set; } = null!;
}
