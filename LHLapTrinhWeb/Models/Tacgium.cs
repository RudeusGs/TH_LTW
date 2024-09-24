using System;
using System.Collections.Generic;

namespace LHLapTrinhWeb.Models;

public partial class Tacgium
{
    public int MaTg { get; set; }

    public string TenTg { get; set; } = null!;

    public string? DiaChiTg { get; set; }

    public string? DienThoaiTg { get; set; }

    public virtual ICollection<Vietsach> Vietsaches { get; set; } = new List<Vietsach>();
}
