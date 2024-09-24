using System;
using System.Collections.Generic;

namespace LHLapTrinhWeb.Models;

public partial class Quangcao
{
    public int Stt { get; set; }

    public string TenCongTy { get; set; } = null!;

    public string? HinhMinhHoa { get; set; }

    public string? Href { get; set; }

    public DateTime? NgayBatDau { get; set; }

    public DateTime? NgayHetHan { get; set; }
}
