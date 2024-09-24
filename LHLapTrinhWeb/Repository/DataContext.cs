using System;
using System.Collections.Generic;
using LHLapTrinhWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace LHLapTrinhWeb.Repository;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Chude> Chudes { get; set; }

    public virtual DbSet<Ctdathang> Ctdathangs { get; set; }

    public virtual DbSet<Ctthamdo> Ctthamdos { get; set; }

    public virtual DbSet<Dondathang> Dondathangs { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Nhaxuatban> Nhaxuatbans { get; set; }

    public virtual DbSet<Quangcao> Quangcaos { get; set; }

    public virtual DbSet<Sach> Saches { get; set; }

    public virtual DbSet<Tacgia> Tacgia { get; set; }

    public virtual DbSet<Thamdo> Thamdos { get; set; }

    public virtual DbSet<Vietsach> Vietsaches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=NGUYENQUAN\\SQLEXPRESS;Database=QLBanSach;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.MaAdmin).HasName("PK_Admin");

            entity.ToTable("ADMIN");

            entity.Property(e => e.DiaChiAdmin).HasMaxLength(50);
            entity.Property(e => e.DienThoaiAdmin)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.EmailAdmin)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GioiTinhAdmin).HasDefaultValue(true);
            entity.Property(e => e.HoTenAdmin).HasMaxLength(50);
            entity.Property(e => e.MatKhauAdmin)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.NgaySinhAdmin).HasColumnType("smalldatetime");
            entity.Property(e => e.QuyenAdmin).HasDefaultValue(1);
            entity.Property(e => e.TenDnadmin)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("TenDNAdmin");
        });

        modelBuilder.Entity<Chude>(entity =>
        {
            entity.HasKey(e => e.MaCd).HasName("PK_ChuDe");

            entity.ToTable("CHUDE");

            entity.Property(e => e.MaCd).HasColumnName("MaCD");
            entity.Property(e => e.TenChuDe).HasMaxLength(50);
        });

        modelBuilder.Entity<Ctdathang>(entity =>
        {
            entity.HasKey(e => new { e.SoDh, e.MaSach }).HasName("PK_CTDatHang");

            entity.ToTable("CTDATHANG");

            entity.Property(e => e.SoDh).HasColumnName("SoDH");
            entity.Property(e => e.DonGia).HasColumnType("decimal(9, 2)");
            entity.Property(e => e.ThanhTien)
                .HasComputedColumnSql("([SoLuong]*[DonGia])", false)
                .HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.Ctdathangs)
                .HasForeignKey(d => d.MaSach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTDatHang_Sach");

            entity.HasOne(d => d.SoDhNavigation).WithMany(p => p.Ctdathangs)
                .HasForeignKey(d => d.SoDh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTDatHang_DonDatHang");
        });

        modelBuilder.Entity<Ctthamdo>(entity =>
        {
            entity.HasKey(e => new { e.MaCh, e.Stt }).HasName("PK_CTThamDo");

            entity.ToTable("CTTHAMDO");

            entity.Property(e => e.MaCh).HasColumnName("MaCH");
            entity.Property(e => e.Stt).HasColumnName("STT");
            entity.Property(e => e.NoiDungTraLoi).HasMaxLength(255);
            entity.Property(e => e.SoLanBinhChon).HasDefaultValue(0);

            entity.HasOne(d => d.MaChNavigation).WithMany(p => p.Ctthamdos)
                .HasForeignKey(d => d.MaCh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTThamDo_ThamDo");
        });

        modelBuilder.Entity<Dondathang>(entity =>
        {
            entity.HasKey(e => e.SoDh).HasName("PK_DonDatHang");

            entity.ToTable("DONDATHANG");

            entity.Property(e => e.SoDh).HasColumnName("SoDH");
            entity.Property(e => e.DaGiao).HasDefaultValue(false);
            entity.Property(e => e.DiaChiNhan).HasMaxLength(50);
            entity.Property(e => e.DienThoaiNhan)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.HtgiaoHang)
                .HasDefaultValue(false)
                .HasColumnName("HTGiaoHang");
            entity.Property(e => e.HtthanhToan)
                .HasDefaultValue(false)
                .HasColumnName("HTThanhToan");
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.NgayDh)
                .HasColumnType("smalldatetime")
                .HasColumnName("NgayDH");
            entity.Property(e => e.NgayGiaoHang).HasColumnType("smalldatetime");
            entity.Property(e => e.TenNguoiNhan)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TriGia).HasColumnType("money");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Dondathangs)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK_DonDatHang_KhachHang");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK_KhachHang");

            entity.ToTable("KHACHHANG");

            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.DaDuyet).HasDefaultValue(false);
            entity.Property(e => e.DiaChiKh)
                .HasMaxLength(50)
                .HasColumnName("DiaChiKH");
            entity.Property(e => e.DienThoaiKh)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DienThoaiKH");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GioiTinh).HasDefaultValue(true);
            entity.Property(e => e.HoTenKh)
                .HasMaxLength(50)
                .HasColumnName("HoTenKH");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.NgaySinh).HasColumnType("smalldatetime");
            entity.Property(e => e.TenDn)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("TenDN");
        });

        modelBuilder.Entity<Nhaxuatban>(entity =>
        {
            entity.HasKey(e => e.MaNxb).HasName("PK_NhaXuatBan");

            entity.ToTable("NHAXUATBAN");

            entity.Property(e => e.MaNxb).HasColumnName("MaNXB");
            entity.Property(e => e.DiaChi).HasMaxLength(150);
            entity.Property(e => e.DienThoai).HasMaxLength(15);
            entity.Property(e => e.TenNxb)
                .HasMaxLength(100)
                .HasColumnName("TenNXB");
        });

        modelBuilder.Entity<Quangcao>(entity =>
        {
            entity.HasKey(e => e.Stt).HasName("PK_QuangCao");

            entity.ToTable("QUANGCAO");

            entity.Property(e => e.Stt).HasColumnName("STT");
            entity.Property(e => e.HinhMinhHoa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Href)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NgayBatDau).HasColumnType("smalldatetime");
            entity.Property(e => e.NgayHetHan).HasColumnType("smalldatetime");
            entity.Property(e => e.TenCongTy).HasMaxLength(200);
        });

        modelBuilder.Entity<Sach>(entity =>
        {
            entity.HasKey(e => e.MaSach).HasName("PK_Sach");

            entity.ToTable("SACH");

            entity.Property(e => e.DonGia).HasColumnType("money");
            entity.Property(e => e.DonViTinh)
                .HasMaxLength(50)
                .HasDefaultValue("cu?n");
            entity.Property(e => e.HinhMinhHoa)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MaCd).HasColumnName("MaCD");
            entity.Property(e => e.MaNxb).HasColumnName("MaNXB");
            entity.Property(e => e.MoTa).HasColumnType("ntext");
            entity.Property(e => e.NgayCapNhat).HasColumnType("smalldatetime");
            entity.Property(e => e.SoLanXem).HasDefaultValue(0);
            entity.Property(e => e.TenSach).HasMaxLength(100);

            entity.HasOne(d => d.MaCdNavigation).WithMany(p => p.Saches)
                .HasForeignKey(d => d.MaCd)
                .HasConstraintName("FK_Sach_ChuDe");

            entity.HasOne(d => d.MaNxbNavigation).WithMany(p => p.Saches)
                .HasForeignKey(d => d.MaNxb)
                .HasConstraintName("FK_Sach_NhaXuatBan");
        });

        modelBuilder.Entity<Tacgia>(entity =>
        {
            entity.HasKey(e => e.MaTg).HasName("PK_TacGia");

            entity.ToTable("TACGIA");

            entity.Property(e => e.MaTg).HasColumnName("MaTG");
            entity.Property(e => e.DiaChiTg)
                .HasMaxLength(100)
                .HasColumnName("DiaChiTG");
            entity.Property(e => e.DienThoaiTg)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("DienThoaiTG");
            entity.Property(e => e.TenTg)
                .HasMaxLength(50)
                .HasColumnName("TenTG");
        });

        modelBuilder.Entity<Thamdo>(entity =>
        {
            entity.HasKey(e => e.MaCh).HasName("PK_ThamDo");

            entity.ToTable("THAMDO");

            entity.Property(e => e.MaCh).HasColumnName("MaCH");
            entity.Property(e => e.NgayDang).HasColumnType("smalldatetime");
            entity.Property(e => e.NoiDungThamDo).HasMaxLength(255);
            entity.Property(e => e.TongSoBinhChon).HasDefaultValue(0);
        });

        modelBuilder.Entity<Vietsach>(entity =>
        {
            entity.HasKey(e => new { e.MaTg, e.MaSach }).HasName("PK_VietSach");

            entity.ToTable("VIETSACH");

            entity.Property(e => e.MaTg).HasColumnName("MaTG");
            entity.Property(e => e.VaiTro).HasMaxLength(30);

            entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.Vietsaches)
                .HasForeignKey(d => d.MaSach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VietSach_Sach");

            entity.HasOne(d => d.MaTgNavigation).WithMany(p => p.Vietsaches)
                .HasForeignKey(d => d.MaTg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VietSach_TacGia");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
