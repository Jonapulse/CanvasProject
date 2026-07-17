using System;
using System.Collections.Generic;

namespace CanvasPhase3.CanvasPhase3.Entities;

public partial class User
{
    public string Uid { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public DateOnly Dob { get; set; }
}
