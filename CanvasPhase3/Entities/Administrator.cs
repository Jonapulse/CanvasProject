using System;
using System.Collections.Generic;

namespace CanvasPhase3.Entities;

public partial class Administrator
{
    public int Uid { get; set; }

    public virtual User UidNavigation { get; set; } = null!;
}
