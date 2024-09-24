using System;
using System.Collections.Generic;

namespace LHLapTrinhWeb.Models;

public partial class Sach
{
    public int MaSach { get; set; }

    public string TenSach { get; set; } = null!;

    public string? DonViTinh { get; set; }

    public decimal? DonGia { get; set; }

    public string? MoTa { get; set; }

    public string? HinhMinhHoa { get; set; }

    public int? MaCd { get; set; }

    public int? MaNxb { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public int? SoLuongBan { get; set; }

    public int? SoLanXem { get; set; }

    public virtual ICollection<Ctdathang> Ctdathangs { get; set; } = new List<Ctdathang>();

    public virtual Chude? MaCdNavigation { get; set; }

    public virtual Nhaxuatban? MaNxbNavigation { get; set; }

    public virtual ICollection<Vietsach> Vietsaches { get; set; } = new List<Vietsach>();
}
