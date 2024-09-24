namespace LHLapTrinhWeb.Models;

public partial class Chude
{
    public int MaCd { get; set; }

    public string TenChuDe { get; set; } = null!;

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
