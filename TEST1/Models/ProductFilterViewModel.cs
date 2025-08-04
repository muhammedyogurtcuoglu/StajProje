namespace TEST1.Models.ViewModels
{
    public class ProductFilterViewModel
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public bool? IsPublished { get; set; }

        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}
