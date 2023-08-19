namespace ProductPageTaskMVC.DBModel;

public partial class Product
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Description { get; set; } = null!;

    public byte[]? Image { get; set; }
}
