using System;
using System.Collections.Generic;

namespace LHLapTrinhWeb.Models;

public partial class Admin
{
    public int MaAdmin { get; set; }

    public string? HoTenAdmin { get; set; }

    public string? DiaChiAdmin { get; set; }

    public string? DienThoaiAdmin { get; set; }

    public string? TenDnadmin { get; set; }

    public string? MatKhauAdmin { get; set; }

    public DateTime? NgaySinhAdmin { get; set; }

    public bool? GioiTinhAdmin { get; set; }

    public string? EmailAdmin { get; set; }

    public int? QuyenAdmin { get; set; }
}
