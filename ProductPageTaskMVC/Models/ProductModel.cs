namespace ProductPageTaskMVC.Models
{
    public class ProductModel
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Amount { get; set; }

        public string Description { get; set; } = null!;

        //public IFormFile? imageDataFile { get; set; }
    }
}
