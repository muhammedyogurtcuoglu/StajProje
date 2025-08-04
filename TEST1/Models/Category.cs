using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TEST1.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int MinStock { get; set; }

        public List<Product> Products { get; set; }
    }
}

