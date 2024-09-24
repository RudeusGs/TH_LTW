using System;
using System.Collections.Generic;

namespace LHLapTrinhWeb.Models;

public partial class Dondathang
{
    public int SoDh { get; set; }

    public int? MaKh { get; set; }

    public DateTime? NgayDh { get; set; }

    public decimal? TriGia { get; set; }

    public bool? DaGiao { get; set; }

    public DateTime? NgayGiaoHang { get; set; }

    public string? TenNguoiNhan { get; set; }

    public string? DiaChiNhan { get; set; }

    public string? DienThoaiNhan { get; set; }

    public bool? HtthanhToan { get; set; }

    public bool? HtgiaoHang { get; set; }

    public virtual ICollection<Ctdathang> Ctdathangs { get; set; } = new List<Ctdathang>();

    public virtual Khachhang? MaKhNavigation { get; set; }
}
