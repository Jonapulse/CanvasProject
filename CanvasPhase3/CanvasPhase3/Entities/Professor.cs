using System;
using System.Collections.Generic;

namespace CanvasPhase3.CanvasPhase3.Entities;

public partial class Professor
{
    public string Uid { get; set; } = null!;

    public int? Employerdep { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual Department? EmployerdepNavigation { get; set; }
}
