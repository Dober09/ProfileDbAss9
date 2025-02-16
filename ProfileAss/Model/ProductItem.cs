using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileAss.Model
{
    [Table("product")]
    public class ProductItem
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
