
using System.ComponentModel.DataAnnotations.Schema;


namespace ProfileAss.Model
{
    [Table("basket")]
    public class Basket
    {
        public int Id { get; set; }

        // Link to Profile 
        [ForeignKey("ProfileId")]
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }

        // Products in the basket 
        public List<BasketItem> BasketItems { get; set; } = new();
    }
}
