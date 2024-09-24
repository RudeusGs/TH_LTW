using System;
using System.Collections.Generic;

namespace LHLapTrinhWeb.Models;

public partial class Vietsach
{
    public int MaTg { get; set; }

    public int MaSach { get; set; }

    public string? VaiTro { get; set; }

    public virtual Sach MaSachNavigation { get; set; } = null!;

    public virtual Tacgium MaTgNavigation { get; set; } = null!;
}
