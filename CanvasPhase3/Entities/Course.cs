using System;
using System.Collections.Generic;

namespace CanvasPhase3.Entities;

public partial class Course
{
    public string Catalogid { get; set; } = null!;

    public string? Name { get; set; }

    public short? Number { get; set; }

    public int? Depid { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual Department? Dep { get; set; }
}
