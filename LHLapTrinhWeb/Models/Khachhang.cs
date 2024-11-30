    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    namespace LHLapTrinhWeb.Models;

    public partial class Khachhang
    {
        public int MaKh { get; set; }

        public string? HoTenKh { get; set; }

        public string? DiaChiKh { get; set; }

        public string? DienThoaiKh { get; set; }

        public string? TenDn { get; set; }

        public string? MatKhau { get; set; }

        public DateTime? NgaySinh { get; set; }

        public bool? GioiTinh { get; set; }

        public string? Email { get; set; }

        public bool? DaDuyet { get; set; }

        public virtual ICollection<Dondathang> Dondathangs { get; set; } = new List<Dondathang>();
    }
