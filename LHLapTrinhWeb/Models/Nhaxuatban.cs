using System;
using System.Collections.Generic;

namespace LHLapTrinhWeb.Models;

public partial class Nhaxuatban
{
    public int MaNxb { get; set; }

    public string TenNxb { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string? DienThoai { get; set; }

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
