using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEST1.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public bool IsPublished { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Lütfen geçerli bir kategori seçin.")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
