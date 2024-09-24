using System;
using System.Collections.Generic;

namespace LHLapTrinhWeb.Models;

public partial class Ctdathang
{
    public int SoDh { get; set; }

    public int MaSach { get; set; }

    public int? SoLuong { get; set; }

    public decimal? DonGia { get; set; }

    public decimal? ThanhTien { get; set; }

    public virtual Sach MaSachNavigation { get; set; } = null!;

    public virtual Dondathang SoDhNavigation { get; set; } = null!;
}
