
using System.ComponentModel.DataAnnotations.Schema;


namespace ProfileAss.Model
{
    [Table("basket_item")]
    public class BasketItem
    {
        public int Id { get; set; }

        // Link to Basket
        [ForeignKey("BasketId")]
        public int BasketId { get; set; }
        public Basket Basket { get; set; }

        // Link to Product
        [ForeignKey("ProductItemId")]
        public int ProductItemId { get; set; }
        public ProductItem ProductItem { get; set; }

        // Additional fields
        public int Quantity { get; set; } = 1;
  
    }
}
