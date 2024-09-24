using System;
using System.Collections.Generic;

namespace LHLapTrinhWeb.Models;

public partial class Ctthamdo
{
    public int MaCh { get; set; }

    public int Stt { get; set; }

    public string NoiDungTraLoi { get; set; } = null!;

    public int? SoLanBinhChon { get; set; }

    public virtual Thamdo MaChNavigation { get; set; } = null!;
}
