using System;
using System.Collections.Generic;

namespace LHLapTrinhWeb.Models;

public partial class Thamdo
{
    public int MaCh { get; set; }

    public DateTime? NgayDang { get; set; }

    public string NoiDungThamDo { get; set; } = null!;

    public int? TongSoBinhChon { get; set; }

    public virtual ICollection<Ctthamdo> Ctthamdos { get; set; } = new List<Ctthamdo>();
}
